using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    PlayerController playerController;
    int clicked = 0;
    float clickTime = 0;
    float clickDelay = 0.3f;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clicked++;
        if(clicked == 1)
        {
            //playerController.SetJump(true);
        }

        if(clicked>1 && Time.time - clickTime < clickDelay)
        {
            //Double click detected
            clicked = 0;
            clickTime = 0;
            //playerController.SetDoubleJump(false);
        }
        else if (clicked>2|| Time.time-clickTime>1)
        {
            clicked = 0;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //playerController.SetJump(false);
    }
}
