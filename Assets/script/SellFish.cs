using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellFish : MonoBehaviour
{
    public GameManager gameManager;

    public void SellAll()
    {
        if (gameManager.halak > 0)
        {

            int numberOfFish = gameManager.halak;
            int fishWorth = 2;
            int totalWorth = numberOfFish * fishWorth;
            gameManager.AddPenz(totalWorth);

            //majd reseteljük a halak számát
            gameManager.halak = 0;

            Debug.Log("Halak eladva!");
        }
        else
        {
            Debug.Log("Nincs eladni való hal!");
        }
    }
}
