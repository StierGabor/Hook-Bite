using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellFish : MonoBehaviour
{
    public void SellAll()
    {
        if (GameManager.Instance.halak > 0)
        {

            int numberOfFish = GameManager.Instance.halak;
            int fishWorth = 2;
            int totalWorth = numberOfFish * fishWorth;
            GameManager.Instance.AddPenz(totalWorth);

            //majd reseteljük a halak számát
            GameManager.Instance.halak = 0;

            Debug.Log("Halak eladva!");
        }
        else
        {
            Debug.Log("Nincs eladni való hal!");
        }
    }
}
