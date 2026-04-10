using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Worm : MonoBehaviour
{
    public List<Button> holeButtons;   // 9 UI buttons
    public Sprite wormSprite;
    public Sprite emptySprite;

    public TMP_Text scoreText;   // TMP UI element
    private int wormCount = 0;
    private int activeHole = -1;
    private int lastLureCount = -1;

    void Start()
    {
        // Listen for button clicks
        for (int i = 0; i < holeButtons.Count; i++)
        {
            int index = i;
            holeButtons[i].onClick.AddListener(() => OnHoleClicked(index));
        }

        UpdateScoreUI();
        SpawnWorm();
    }

    void Update()
    {
        // Update UI if the lure count changed from outside
        if (GameManager.Instance != null && GameManager.Instance.lure != lastLureCount)
        {
            lastLureCount = GameManager.Instance.lure;
            UpdateScoreUI();
        }
    }

    void SpawnWorm()
    {
        // Make all buttons empty
        for (int i = 0; i < holeButtons.Count; i++)
        {
            holeButtons[i].GetComponent<Image>().sprite = emptySprite;
        }

        // Choose a random location
        activeHole = Random.Range(0, holeButtons.Count);

        // Show worm
        holeButtons[activeHole].GetComponent<Image>().sprite = wormSprite;
    }

    void OnHoleClicked(int index)
    {
        if (index == activeHole)
        {
            Debug.Log("Worm caught!");

            wormCount++;
            if (wormCount % 10 == 0)
                GameManager.Instance.lure += 1;                 // ⬅️ increase score

            UpdateScoreUI();         // ⬅️ UI update
            SpawnWorm();
        }
        else
        {
            Debug.Log("Empty hole!");
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Lures: " + GameManager.Instance.lure;
    }
}