using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject howToPlayBtn;

    [SerializeField] GameObject howToPlayImage;

    public void OnStartButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveMenuPanel(false);
            UIManager.Instance.ActiveLoadingPanel(true);
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.FadeOutBGM(1f);
            AudioManager.Instance.PlayBGM(AUDIO.BGM_GOOD_DAY_SO_FAR_PERCUSSION);
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
        howToPlayImage.SetActive(true);
        howToPlayBtn.SetActive(false);
    }

    public void OnHowToPlayButtonImgClick()
    {
        howToPlayImage.SetActive(false);
        howToPlayBtn.SetActive(true);
    }
}