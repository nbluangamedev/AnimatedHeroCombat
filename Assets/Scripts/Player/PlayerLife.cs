using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    GameObject player;

    [SerializeField] int playerLive = 3;
    [SerializeField] TMP_Text playerLiveText;
    [SerializeField] Transform checkpointPosition;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        damageable = player.GetComponent<Damageable>();
        playerLiveText.text = "Lives: " + playerLive;
    }

    private void Start()
    {

    }

    private void Update()
    {
        playerLiveText.text = "Lives: " + playerLive;

        if (damageable.Health <= 0)
        {
            Die();
        }
    }

    public void UpdateCheckpoint(Transform newPosition)
    {
        checkpointPosition = newPosition;
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
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_DEATH);
        }
        //rb.bodyType = RigidbodyType2D.Static;
        damageable.IsAlive = false;
    }

    //This function reference in animator
    private void Restart()
    {
        transform.position = checkpointPosition.position;
        rb.bodyType = RigidbodyType2D.Dynamic;
        animator.Rebind();
        damageable.Health = damageable.MaxHealth;
        playerLive -= 1;
        if (playerLive <= 0 && GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
            UIManager.Instance.ActiveLosePanel(true);
        }
    }
}
