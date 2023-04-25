using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    [SerializeField] Toggle bgmMute;
    [SerializeField] Toggle seMute;

    private float bgmValue;
    private float seValue;
    private bool isBGMMute;
    private bool isSEMute;

    private void Awake()
    {
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
            isBGMMute = AudioManager.Instance.AttachBGMSource.mute;
            isSEMute = AudioManager.Instance.AttachSESource.mute;
            bgmMute.isOn = isBGMMute;
            seMute.isOn = isSEMute;
        }
    }

    private void OnEnable()
    {
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
            isBGMMute = AudioManager.Instance.AttachBGMSource.mute;
            isSEMute = AudioManager.Instance.AttachSESource.mute;
            bgmMute.isOn = isBGMMute;
            seMute.isOn = isSEMute;
        }
    }

    public void OnSliderChangeBGMValue(float v)
    {
        bgmValue = v;
    }

    public void OnSliderChangeSEValue(float v)
    {
        seValue = v;
    }

    public void OnChangeValueBGMMute(bool v)
    {
        isBGMMute = v;
    }

    public void OnChangeValueSEMute(bool v)
    {
        isSEMute = v;
    }

    public void OnCancelButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(false);
        }

        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.IsPlaying == false && !UIManager.Instance.MenuPanel.gameObject.activeSelf)
            {
                UIManager.Instance.ActivePausePanel(true);
            }
        }
    }

    public void OnSubmitButtonClick()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeBGMVolume(bgmValue);
            AudioManager.Instance.ChangeSEVolume(seValue);
            AudioManager.Instance.MuteBGM(isBGMMute);
            AudioManager.Instance.MuteSE(isSEMute);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(false);
        }

        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.IsPlaying == false && !UIManager.Instance.MenuPanel.gameObject.activeSelf)
            {
                UIManager.Instance.ActivePausePanel(true);
            }
        }
    }
}
