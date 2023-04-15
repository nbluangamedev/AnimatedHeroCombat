using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Damageable damageable;
    private GameObject player;

    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private int playerLive = 3;
    [SerializeField] private TMP_Text playerLiveText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        damageable = player.GetComponent<Damageable>();
        playerLiveText.text = "Lives: " + playerLive;
    }

    private void Update()
    {
        playerLiveText.text = "Lives: " + playerLive;

        if (damageable.Health <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (playerLive > 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        //if (AudioManager.HasInstance)
        //{
        //    AudioManager.Instance.PlaySE(AUDIO.SE_DEATH);
        //}
        //rb.bodyType = RigidbodyType2D.Static;
        //animator.SetTrigger("Death");
        damageable.IsAlive = false;
    }

    //This function reference in animator
    private void Restart()
    {
        this.transform.position = playerSpawnPoint.position;
        rb.bodyType = RigidbodyType2D.Dynamic;
        animator.Rebind();
        damageable.Health = 100;
        playerLive -= 1;
        if (playerLive <= 0 && GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
            UIManager.Instance.ActiveLosePanel(true);
        }
    }
}
