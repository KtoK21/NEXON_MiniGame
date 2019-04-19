using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    public int AIChoice;
    float timer = 0;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /* 전투중이 아니라면, AI는 매 초마다 랜덤 기능을 실행한다.
         * 0~9 중 랜덤 숫자를 생성하고, 0~2일 경우 왼쪽, 3~5일 경우 가운데, 6~9일 경우 오른쪽에 병력을 충원한다. 
         * 9~10일 경우 아무것도 하지 않는다.
         */
        if (!gameManager.IsFightOn)
        {
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                AIChoice = Random.Range(0, 11);
                switch (AIChoice)
                {
                    case 0:
                    case 1:
                    case 2:
                        gameManager.LineAdd("Left_Enemy", false);
                        break;
                    case 3:
                    case 4:
                    case 5:
                        gameManager.LineAdd("Middle_Enemy", false);
                        break;
                    case 6:
                    case 7:
                    case 8:
                        gameManager.LineAdd("Right_Enemy", false);
                        break;
                    case 9:
                    case 10:
                        break;
                    /*
                     * case 11:
                     * gameManager.StartFight();
                     * break;
                     */
                }
                timer = 0.0f;
            }
        }
    }

}
