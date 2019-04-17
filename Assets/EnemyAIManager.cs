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
        if (!gameManager.IsFightOn)
        {
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                AIChoice = Random.Range(0, 10);
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
                    case 11:
                        //gameManager.StartFight();
                        break;
                }
                timer = 0.0f;
            }
        }
    }

}
