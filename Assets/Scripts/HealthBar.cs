using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text healthBarText;

    Damageable playerDamageable;

    public delegate void PlayerHealth(int playerHealth, int playerMaxHealth); //Dinh nghia ham delegate 
    public static PlayerHealth playerHealthDelegate; //Khai bao ham delegate
    private int currentplayerHealth=100, currentplayerMaxHealth=100;

    private void Awake()
    {
        playerDamageable = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
        
    }

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            currentplayerHealth = GameManager.Instance.PlayerHealth;
            currentplayerMaxHealth = GameManager.Instance.PlayerMaxHealth;
        }
    }

    private void OnEnable()
    {
        healthBar.value = CalculateSliderPercentage(currentplayerHealth, currentplayerMaxHealth);
        healthBarText.text = "HP: " + currentplayerHealth + " / " + currentplayerMaxHealth;
    }

    private void Update()
    {
        GameManager.Instance.UpdatePlayerHealth(currentplayerHealth, currentplayerMaxHealth);
        playerHealthDelegate(currentplayerHealth, currentplayerMaxHealth); //Broadcast event
    }

    //private void OnEnable()
    //{
    //    if(playerDamageable == null)
    //    {
    //        playerDamageable = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    //    }
    //    healthBar.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
    //    healthBarText.text = "HP: " + playerDamageable.Health + " / " + playerDamageable.MaxHealth;
    //    playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    //}

    //private void OnDisable()
    //{
    //    playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    //}

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
