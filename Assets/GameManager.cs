using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsFightOn = false;
    public int Resource_Player = 0;
    public int Health_Player = 100;

    public int Resource_Enemy = 0;
    public int Health_Enemy = 100;

    float timer = 0.0f;
    string Line_1 = null, Line_2 = null;

    Dictionary<string, int> LineCount = new Dictionary<string, int>();

    UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        LineCount.Add("Left_Player", 0);
        LineCount.Add("Middle_Player", 0);
        LineCount.Add("Right_Player", 0);
        LineCount.Add("Left_Enemy", 0);
        LineCount.Add("Middle_Enemy", 0);
        LineCount.Add("Right_Enemy", 0);

        uiManager.LineCountOnText(LineCount);
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseResource();
    }

    void IncreaseResource()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            Resource_Player++;
            Resource_Enemy++;
            timer = 0.0f;
        }
    }

    public void LineAdd(string line, bool IsPlayer)
    {
        //전투 진행중에는 공격로 충원 불가
        if (IsFightOn)
            return;

        if (IsPlayer)
        {
            if (Resource_Player <= 0)
                return;
            else
                Resource_Player--;
        }

        else
        {
            if (Resource_Enemy <= 0)
                return;
            else
                Resource_Enemy--;
        }

        ++LineCount[line];
        uiManager.LineCountOnText(LineCount);
    }

     public IEnumerator FightCalc()
    {

        IsFightOn = true;
        uiManager.ToggleEnemyLineInfoText(true);

        //양 플레이어에게 전투시마다 주어지는 이자 계산 및 지급
        GiveInterest();

        yield return StartCoroutine(DecreaseLineCount());

        //전투 계산이 끝난 후 데미지 계산 및 체력 계산 시작
        DamageCalc("Left_Player", "Left_Enemy");
        DamageCalc("Middle_Player", "Middle_Enemy");
        DamageCalc("Right_Player", "Right_Enemy");

        uiManager.HealthOnText();

        //양 플레이어의 승리 조건 확인
        FindWinner(false);

        uiManager.ToggleEnemyLineInfoText(false);
        IsFightOn = false;
    }

    IEnumerator DecreaseLineCount()
    {
        /*
         * 각 라인에서, 양 쪽 병력 모두 0이 아닐 경우, 각각 1씩 감소. 
         * 한 쪽이라도 병력이 0이라면 그 라인에서의 계산은 완료됨.
         */
        while (true)
        {
            bool IsLeftDone = false;
            bool IsMiddleDone = false;
            bool IsRightDone = false;

            if (LineCount["Left_Player"] != 0 && LineCount["Left_Enemy"] != 0)
            {
                --LineCount["Left_Player"];
                --LineCount["Left_Enemy"];
            }
            else
                IsLeftDone = true;

            if (LineCount["Middle_Player"] != 0 && LineCount["Middle_Enemy"] != 0)
            {
                --LineCount["Middle_Player"];
                --LineCount["Middle_Enemy"];
            }
            else
                IsMiddleDone = true;

            if (LineCount["Right_Player"] != 0 && LineCount["Right_Enemy"] != 0)
            {
                --LineCount["Right_Player"];
                --LineCount["Right_Enemy"];
            }
            else
                IsRightDone = true;

            uiManager.LineCountOnText(LineCount);

            if (IsLeftDone && IsMiddleDone && IsRightDone)
                break;

            yield return new WaitForSeconds(0.3f);
        }
    }

    void DamageCalc(string str1, string str2)
    {
        //각 라인에서 전투 후 남은 병력이 있는 플레이어는 그 병력의 숫자만큼의 데미지를 입힘.
        int Damage = LineCount[str1] - LineCount[str2];

        if (Damage > 0)
            Health_Enemy -= Damage;
        else
            Health_Player += Damage;

    }

    void GiveInterest()
    {
        //전투시마다 가지고 있던 자원 / 10 만큼의 이자 지불.
        Resource_Player += Resource_Player / 10;
        Resource_Enemy += Resource_Enemy / 10;
    }

    public void DoubleIncome(bool IsPlayer)
    {
        if (IsPlayer)
        {
            StartCoroutine(DoubleIncomeCoroutine(Resource_Player, IsPlayer));
            Resource_Player = 0;
        }
        else
        {
            StartCoroutine(DoubleIncomeCoroutine(Resource_Enemy, IsPlayer));
            Resource_Enemy = 0;
        }
    }

    IEnumerator DoubleIncomeCoroutine(int time, bool IsPlayer)
    {
        int i = 0;
        while (i < time * 2)
        {
            if (IsPlayer)
                Resource_Player++;
            else
                Resource_Enemy++;
            i++;
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator ResourceObserve()
    {
        if (Resource_Player >= 20)
        {
            Resource_Player -= 20;
            yield return StartCoroutine(ShowResource5Sec());
            uiManager.ToggleEnemyResourceInfoText(false);
        }
    }

    IEnumerator ShowResource5Sec()
    {
        uiManager.ToggleEnemyResourceInfoText(true);
        yield return new WaitForSeconds(5f);
    }

    public bool LineSwap(string line)
    {
        /* 처음으로 공격로를 선택하면 그 공격로를 Line_1에 등록하고 false를 리턴.
         * 두번째 공격로를 선택하면 xor swap을 진행하고 Line_1, Line_2를 null로 되돌린 다음
         * true를 리턴.
         */
        if (Line_1 == null)
        {
            Line_1 = line;
            return false;
        }
        else
        {
            Line_2 = line;
            LineCount[Line_1] = LineCount[Line_1] ^ LineCount[Line_2];
            LineCount[Line_2] = LineCount[Line_2] ^ LineCount[Line_1];
            LineCount[Line_1] = LineCount[Line_1] ^ LineCount[Line_2];
            Line_1 = Line_2 = null;
            uiManager.LineCountOnText(LineCount);
            return true;
        }
    }

    public void FindWinner(bool IsTimeOut)
    {
        if (IsTimeOut)
        {
            if (Health_Player > Health_Enemy)
                uiManager.ShowWinner("Player");
            else if (Health_Enemy > Health_Player)
                uiManager.ShowWinner("Enemy");
            else
                uiManager.ShowWinner("Player & Enemy");
        }
        else
        {
            if (Health_Player <= 0)
                uiManager.ShowWinner("Enemy");
            else if (Health_Enemy <= 0)
                uiManager.ShowWinner("Player");
        }

    }
}
