using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public delegate void CollectDiamond(int diamond); //Dinh nghia ham delegate 
    public static CollectDiamond collectDiamondDelegate; //Khai bao ham delegate
    private int diamonds = 0;

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            diamonds = GameManager.Instance.Scores;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            //if (AudioManager.HasInstance)
            //{
            //    AudioManager.Instance.PlaySE(AUDIO.SE_COLLECT);
            //}
            Destroy(collision.gameObject);
            diamonds++;
            GameManager.Instance.UpdateScores(diamonds);
            collectDiamondDelegate(diamonds); //Broadcast event
        }
    }
}
