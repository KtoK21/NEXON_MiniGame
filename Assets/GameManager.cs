using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Resource = 10;
    public int Health = 100;

    public int LeftLineCount = 0;
    public int MiddleLineCount = 0;
    public int RightLineCount = 0;

    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1.0f)
        {
            Resource++;
            timer = 0.0f;
        }
    }

    public int LineAdd(string line)
    {
        if (Resource <= 0)
            return -1;
        else
        {
            Resource--;

            switch (line)
            {
                case "Left":
                    return ++LeftLineCount;
                case "Middle":
                    return ++MiddleLineCount;
                case "Right":
                    return ++RightLineCount;
            }
            return -1;
        }
    }
}
