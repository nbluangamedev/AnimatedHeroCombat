using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] RectTransform[] buttonList;
    [SerializeField] RectTransform indicator;
    
    int indicatorPosition = 0;
    
    void Update()
    {        
        indicator.localPosition = buttonList[indicatorPosition].localPosition;
    }

    public void HoverOnButton(int buttonPos)
    {
        indicatorPosition = buttonPos;
        AudioManager.Instance.PlaySE(AUDIO.SE_WOODBLOCK1);
    }

    public void PressedOnButton()
    {
        AudioManager.Instance.PlaySE(AUDIO.SE_51_FLEE);
    }
}
