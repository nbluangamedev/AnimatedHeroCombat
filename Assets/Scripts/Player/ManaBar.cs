using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] Slider manaBar;
    [SerializeField] TMP_Text manaBarText;

    private float currentMana;
    private float maxMana;

    void Start()
    {
        if (GameManager.HasInstance)
        {
            manaBar.value = CalculateSliderPercentage(GameManager.Instance.Mana, GameManager.Instance.MaxMana);
            manaBarText.text = "HP " + GameManager.Instance.Mana + " / " + GameManager.Instance.MaxMana;
        }
    }

    private void Update()
    {
        if (GameManager.HasInstance)
        {
            currentMana = GameManager.Instance.Mana;
            maxMana = GameManager.Instance.MaxMana;
            if (currentMana > maxMana)
            {
                GameManager.Instance.Mana = maxMana;
            }
            else
            {
                GameManager.Instance.Mana = Mathf.MoveTowards(currentMana / maxMana, 1f, Time.deltaTime * 0.01f) * maxMana;
            }
        }
    }

    private void OnEnable()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.manaChanged.AddListener(OnPlayerManaChanged);
        }
    }

    private void OnDisable()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.manaChanged.RemoveListener(OnPlayerManaChanged);
        }
    }

    private float CalculateSliderPercentage(float currentMana, float maxMana)
    {
        return currentMana / maxMana;
    }

    private void OnPlayerManaChanged(float newMana, float maxMana)
    {
        manaBar.value = CalculateSliderPercentage(newMana, maxMana);
        manaBarText.text = "HP " + Mathf.FloorToInt(newMana) + " / " + maxMana;
    }
}
