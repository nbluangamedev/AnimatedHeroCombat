using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyList;

    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject bossEnemy;

    int count = 0;

    private void Update()
    {
        if (count > 3)
        {
            return;
        }
        else
        {
            CheckEnemy();
        }
    }

    private void CheckEnemy()
    {
        if (enemyList.Any())
        {
            foreach (var enemy in enemyList)
            {
                if (enemy == null)
                {
                    enemyList.Remove(enemy);
                    count++;
                }
            }
        }
        else
        {
            bossEnemy.SetActive(true);
            count++;
        }
    }
}
