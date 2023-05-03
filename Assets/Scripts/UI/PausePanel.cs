using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void OnClickedResumeButton()
    {
        if (GameManager.HasInstance && UIManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
            UIManager.Instance.ActivePausePanel(false);
            UIManager.Instance.ActiveMobilePanel(true);
        }
    }

    public void OnClickedSettingButton()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
            UIManager.Instance.ActivePausePanel(false);
        }
    }

    public void OnClickedRestartButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.RestartGame();
            UIManager.Instance.ActivePausePanel(false);
        }
    }

    public void OnClickedQuitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}