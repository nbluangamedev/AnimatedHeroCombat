using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawn : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject firePrefab;

    [SerializeField] int manaRequire = 20;

    public void Fire()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.Mana -= manaRequire;
            GameObject fire = Instantiate(firePrefab, spawnPoint.position, firePrefab.transform.rotation);
            Vector3 originScale = fire.transform.localScale;

            fire.transform.localScale = new Vector3(
                originScale.x * transform.localScale.x > 0 ? 1 : -1,
                originScale.y,
                originScale.z
                );
            Destroy(fire, 1f);

        }
    }
}
