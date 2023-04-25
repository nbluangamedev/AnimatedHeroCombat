using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject bossEnemy;

    [SerializeField] List<GameObject> enemyList;

    private void Start()
    {
        ActiveBoss(false);
    }

    private void FixedUpdate()    
    {
        if (enemyList != null)
        {
            foreach (var enemy in enemyList)
            {
                if (enemy == null)
                {
                    enemyList.Remove(enemy);
                }
            }
        }

        if (!enemyList.Any())
        {
            ActiveBoss(true);
        }
    }

    public void ActiveBoss(bool active)
    {
        bossEnemy.SetActive(active);
    }
}
