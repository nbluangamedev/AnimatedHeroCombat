using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] Vector2 moveSpeed = new Vector2(3f, 0);
    [SerializeField] Vector2 knockback = new Vector2(0, 0);

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDamageable enemyDamageable = collision.GetComponent<EnemyDamageable>();
        if (enemyDamageable != null)
        {
            Destroy(gameObject);
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            enemyDamageable.Hit(damage, deliveredKnockback);            
        }
    }
}
