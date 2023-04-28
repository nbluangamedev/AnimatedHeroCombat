using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class MoveStickController : MonoBehaviour, IDragHandler
{
    [SerializeField] OnScreenStick onScreenStick;
    PlayerController player;

    private void Update()
    {
        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.IsPlaying == true)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();                
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.IsPointerMoving())
        {
            player.runSpeed = 15f;
        }
    }
}
