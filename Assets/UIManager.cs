using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public Text ResourceText;
    public Text HealthText;
    public Text LeftLineText;
    public Text MiddleLineText;
    public Text RightLineText;
    public Text TimeText;
    public float remainTime = 300.0f;

    public Button LeftLine;
    public Button MiddleLine;
    public Button RightLine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeDisplay();
        ResourceDisplay();
        HealthDisplay();
    }

    public void OnLinePress(string line)
    {
        int Count = GameObject.Find("GameManager").GetComponent<GameManager>().LineAdd(line);
        
        //Exception
        if(Count == -1)
        {
            return;
        }

        switch (line)
        {
            case "Left":
                LeftLineText.text = Count.ToString();
                break;
            case "Middle":
                MiddleLineText.text = Count.ToString();
                break;
            case "Right":
                RightLineText.text = Count.ToString();
                break;
        }            
    }

    void TimeDisplay()
    {
        remainTime -= Time.deltaTime;
        int sec = Mathf.FloorToInt(remainTime % 60);

        if (sec < 10)
            TimeText.text = "Time : " + Mathf.FloorToInt(remainTime / 60) + ":0" + sec;
        else
            TimeText.text = "Time : " + Mathf.FloorToInt(remainTime / 60) + ":" + sec;
    }

    void ResourceDisplay()
    {
        ResourceText.text = "Resource : " + GameObject.Find("GameManager").GetComponent<GameManager>().Resource.ToString();
    }

    void HealthDisplay()
    {
        HealthText.text = "Health : " + GameObject.Find("GameManager").GetComponent<GameManager>().Health.ToString();
    }



}
