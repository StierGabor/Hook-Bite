using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Worm : MonoBehaviour
{
    public List<Button> holeButtons;   // 9 UI gomb
    public Sprite wormSprite;
    public Sprite emptySprite;

    public TMP_Text scoreText;   // TMP UI eleme
    private int wormCount = 0;
    private int activeHole = -1;

    void Start()
    {
        // Gombokra kattintás figyelése
        for (int i = 0; i < holeButtons.Count; i++)
        {
            int index = i;
            holeButtons[i].onClick.AddListener(() => OnHoleClicked(index));
        }

        UpdateScoreUI();
        SpawnWorm();
    }

    void SpawnWorm()
    {
        // Minden gomb üres legyen
        for (int i = 0; i < holeButtons.Count; i++)
        {
            holeButtons[i].GetComponent<Image>().sprite = emptySprite;
        }

        // Random hely kiválasztása
        activeHole = Random.Range(0, holeButtons.Count);

        // Giliszta megjelenítése
        holeButtons[activeHole].GetComponent<Image>().sprite = wormSprite;
    }

    void OnHoleClicked(int index)
    {
        if (index == activeHole)
        {
            Debug.Log("Giliszta elkapva!");

            wormCount++;
            if (wormCount % 10 == 0)
                GameManager.Instance.lure+=1;                 // ⬅️ pont növelése

            UpdateScoreUI();         // ⬅️ UI frissítés
            SpawnWorm();
        }
        else
        {
            Debug.Log("Üres lyuk!");
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Lures: " + GameManager.Instance.lure;
    }
}