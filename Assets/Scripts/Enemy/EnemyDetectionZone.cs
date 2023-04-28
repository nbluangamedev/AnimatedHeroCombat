using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDetectionZone : MonoBehaviour
{
    public UnityEvent noEnemyColliderRemain;
    public UnityEvent noBossEnemyColliderRemain;

    public List<Collider2D> detectedEnemyColliders = new List<Collider2D>();
    public List<Collider2D> detectedBossColliders = new List<Collider2D>();

    //Collider2D col;

    //private void Awake()
    //{
    //    col = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Collider2D>();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            detectedEnemyColliders.Add(collision);
        }

        if (collision.CompareTag("BossEnemy"))
        {
            detectedBossColliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            detectedEnemyColliders.Remove(collision);
            if (detectedEnemyColliders.Count <= 0)
            {
                noEnemyColliderRemain.Invoke();
            }
        }        

        if (collision.CompareTag("BossEnemy"))
        {
            detectedBossColliders.Remove(collision);
            if (detectedBossColliders.Count <= 0)
            {
                noBossEnemyColliderRemain.Invoke();
            }
        }
    }
}
