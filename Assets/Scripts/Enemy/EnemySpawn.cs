using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //[SerializeField] List<GameObject> enemyList;
    //[SerializeField] GameObject bossEnemy;

    //int count = 0;

    //private void Update()
    //{
    //    CheckEnemy();
    //}

    //private void CheckEnemy()
    //{
    //    for (int i = 0; i < enemyList.Count; i++)
    //    {
    //        var enemy = enemyList[i].GetComponent<Damageable>();
    //        if( enemy != null )
    //        {
    //            if(enemy.Health == 0)
    //            {
    //                count++;
    //                Debug.Log("count: "+count);
    //            }
    //        }
    //    }

    //    if (count == enemyList.Count)
    //    {
    //        bossEnemy.SetActive(true);
    //    }
    //}

    [SerializeField] GameObject bossEnemy;

    public void BossSpawn()
    {
        bossEnemy.SetActive(true);
    }
}
