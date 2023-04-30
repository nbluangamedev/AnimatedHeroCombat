using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerLife playerLife;
    SpriteRenderer spriteRenderer;
    [SerializeField] Transform spawnPoint;
    public Sprite active;
    Collider2D coll;

    private void Awake()
    {
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerLife.UpdateCheckpoint(spawnPoint);
            spriteRenderer.sprite = active;
            coll.enabled = false;
            if(AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_013_CONFIRM);
            }
        }
    }
}
