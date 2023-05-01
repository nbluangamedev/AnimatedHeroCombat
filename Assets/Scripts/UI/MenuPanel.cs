using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject howToPlayBtn;
    [SerializeField] GameObject howToPlayPanel;

    public void OnStartButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveMenuPanel(false);
            UIManager.Instance.ActiveLoadingPanel(true);
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.FadeOutBGM(2f);
            AudioManager.Instance.PlayBGM(AUDIO.BGM_GOOD_DAY_SO_FAR_PERCUSSION);
        }
        
        if(GameManager.HasInstance)
        {
            GameManager.Instance.UpdateScores(0);
            GameManager.Instance.UpdatePlayerLife(3);
        }
    }

    public void OnSettingButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
        }
    }
    public void OnQuitButtonClick()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }

    public void OnHowToPlayButtonClick()
    {
        howToPlayPanel.SetActive(true);
        howToPlayBtn.SetActive(false);
    }

    public void OnHowToPlayPanelClick()
    {
        howToPlayPanel.SetActive(false);
        howToPlayBtn.SetActive(true);
    }
}