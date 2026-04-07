using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellFish : MonoBehaviour
{
    public void SellAll()
    {
        int totalGoldToAdd = 0;

        if (GameManager.Instance.bream > 0)
        {
            totalGoldToAdd += GameManager.Instance.bream * 2;
            GameManager.Instance.bream = 0;
            Debug.Log("Bream sold!");
        }
        
        if (GameManager.Instance.catfish > 0)
        {
            totalGoldToAdd += GameManager.Instance.catfish * 5;
            GameManager.Instance.catfish = 0;
            Debug.Log("Catfish sold!");
        }

        if (GameManager.Instance.ray > 0)
        {
            totalGoldToAdd += GameManager.Instance.ray * 10;
            GameManager.Instance.ray = 0;
            Debug.Log("Ray sold!");
        }

        if (GameManager.Instance.octopus > 0)
        {
            totalGoldToAdd += GameManager.Instance.octopus * 100;
            GameManager.Instance.octopus = 0;
            Debug.Log("Octopus sold!");
        }

        if (totalGoldToAdd > 0)
        {
            GameManager.Instance.Addgold(totalGoldToAdd);
            Debug.Log("All fish sold for " + totalGoldToAdd + " gold!");
        }
        else
        {
            Debug.Log("Nincs eladni valo hal!");
        }
    }
}
