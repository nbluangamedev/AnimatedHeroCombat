using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberOfDiamond;
    [SerializeField] TextMeshProUGUI timeText;
    //[SerializeField] VariableJoystick joystick;

    public TextMeshProUGUI NumberOfDiamond => numberOfDiamond;
    private float timeRemaining;
    private bool timerIsRunning = false;

    private void Awake()
    {
        SetTimeRemain(600);
    }

    private void OnEnable()
    {
        SetTimeRemain(600);
        timerIsRunning = true;
        ItemCollector.collectDiamondDelegate += OnPlayerCollect;
    }

    private void OnDisable()
    {
        ItemCollector.collectDiamondDelegate -= OnPlayerCollect;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                if (UIManager.HasInstance && GameManager.HasInstance && AudioManager.HasInstance)
                {
                    //AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
                    GameManager.Instance.PauseGame();
                    UIManager.Instance.ActiveLosePanel(true);
                    UIManager.Instance.ActiveShowOnMobilePanel(false);
                }
            }
        }
    }

    private void OnPlayerCollect(int value)
    {
        numberOfDiamond.SetText(value.ToString());
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetTimeRemain(float v)
    {
        timeRemaining = v;
    }
}