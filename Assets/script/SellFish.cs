using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellFish : MonoBehaviour
{
    public void SellAll()
    {
        if (GameManager.Instance.bream > 0)
        {

            int numberOfFish = GameManager.Instance.bream;
            int fishWorth = 2;
            int totalWorth = numberOfFish * fishWorth;
            GameManager.Instance.Addgold(totalWorth);

            //majd reseteljük a bream számát
            GameManager.Instance.bream = 0;

            Debug.Log("bream eladva!");
        }
        else
        {
            Debug.Log("Nincs eladni való hal!");
        }
    }
}
