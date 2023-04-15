using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLife : MonoBehaviour
{
    [SerializeField] private Damageable enemyDamageable;

    private void OnEnable()
    {
        enemyDamageable.enemyHealChanged.AddListener(OnEnemyHealthChanged);
    }

    private void OnDisable()
    {
        enemyDamageable.enemyHealChanged.RemoveListener(OnEnemyHealthChanged);
    }

    private void OnEnemyHealthChanged(int scores)
    {
        scores += 5;
        if (GameManager.HasInstance)
        {
            GameManager.Instance.UpdateScores(scores);
        }
    }
}
