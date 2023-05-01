using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePanel : MonoBehaviour
{
    public FixedJoystick fixedJoystick;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DelayLoad", 0.3f);
    }

    private void DelayLoad()
    {
        fixedJoystick.gameObject.SetActive(true);
    }

    public void OnSettingButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
            UIManager.Instance.ActiveMobilePanel(false);
        }
    }

    public void OnPauseButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActivePausePanel(true);
            UIManager.Instance.ActiveMobilePanel(false);
        }
    }
}
