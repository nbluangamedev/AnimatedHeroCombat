using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    [SerializeField] Joystick joyStick;

    public void OnSettingButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveShowOnMobilePanel(false);
            UIManager.Instance.ActiveSettingPanel(true);
        }
    }

    public void OnPauseButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveShowOnMobilePanel(false);
            UIManager.Instance.ActivePausePanel(true);
        }
    }
}
