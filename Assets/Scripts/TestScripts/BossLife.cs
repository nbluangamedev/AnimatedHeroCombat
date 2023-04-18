using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLife : MonoBehaviour
{
    Damageable enemyDamageable;

    private void Awake()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyDamageable = enemy.GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        enemyDamageable.enemyKillPoint.AddListener(OnEnemyDeath);
    }

    private void OnDisable()
    {
        enemyDamageable.enemyKillPoint.RemoveListener(OnEnemyDeath);
    }

    private void OnEnemyDeath(int score)
    {
        if (GameManager.HasInstance)
        {
            int s = GameManager.Instance.Scores;
            s += score;
            GameManager.Instance.UpdateScores(s);
            ItemCollector.collectDiamondDelegate(s);
        }
    }
}
