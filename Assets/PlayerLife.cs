using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Damageable damageable;

    [SerializeField]
    private Transform playerSpawnPoint;
    [SerializeField] private int playerLive = 3;
    [SerializeField] TMP_Text playerLiveText;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
        playerLiveText.text = "Lives: " + playerLive;
    }

    private void Update()
    {
        playerLiveText.text = "Lives: " + playerLive;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (playerLive > 0)
            {
                damageable.IsAlive = false;
                playerLive-=1;
            }
            if(playerLive ==0 ) GameManager.Instance.RestartGame();
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

    private void Restart()
    {
        this.transform.position = playerSpawnPoint.position;
        rb.bodyType = RigidbodyType2D.Dynamic;
        animator.Rebind();
    }
}
