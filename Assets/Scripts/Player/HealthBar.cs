using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text healthBarText;

    void Start()
    {
        if (GameManager.HasInstance)
        {
            healthBar.value = CalculateSliderPercentage(GameManager.Instance.Health, GameManager.Instance.MaxHealth);
            healthBarText.text = "HP: " + GameManager.Instance.Health + " / " + GameManager.Instance.MaxHealth;
        }
    }

    private void OnEnable()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.healthChanged.AddListener(OnPlayerHealthChanged);
        }
    }

    private void OnDisable()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.healthChanged.RemoveListener(OnPlayerHealthChanged);
        }
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthBar.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP: " + newHealth + " / " + maxHealth;
    }
}
