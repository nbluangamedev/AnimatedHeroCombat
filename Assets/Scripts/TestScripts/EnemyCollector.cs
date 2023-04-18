using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollector : MonoBehaviour
{
    Damageable enemyDamageable;
    private int s = 0;

    private void Awake()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyDamageable = enemy.GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.HasInstance)
        {
            int s = GameManager.Instance.Scores;
        }
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
        s += score;
        GameManager.Instance.UpdateScores(s);
        ItemCollector.collectDiamondDelegate(s);
    }
}
