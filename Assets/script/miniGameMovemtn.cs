using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public Transform bar;        // A teljes csík
    public GameObject FishingPanel;        // A panel referenciája
    public Image fishImage;      // A hal képe (UI Image)
    public Sprite[] fishImages;  // A 4 hal képe (Unityben a UI Image forrása Sprite típusú)
    public int[] fishRarityWeights = new int[] { 55, 33, 10, 2 }; // Esélyek aránya (Gyakori, Ritka, Epikus, Legendás stb.)

    [Header("Fish Movement Settings")]
    public bool enableFishMovement = false; // "Developer button" to toggle movement
    public float[] minSpeeds = new float[] { 1f, 1.5f, 0.5f, 3f }; // Minimum sebesség ritkaság szerint
    public float[] maxSpeeds = new float[] { 2f, 2.5f, 1.2f, 4f }; // Maximum sebesség ritkaság szerint
    public float edgeOffset = -0.1f;

    private float currentSpeed = 2f;
    private int direction = -1; // -1, hogy ellentétes irányba induljon, mint a pointer (ami 1)
    private bool wasFishingPanelActive = false;

    void OnEnable()
    {
        // Ha a scriptet tartalmazó object aktív lesz (pl. megnyílik a panel), akkor generál egy új pozíciót.
        if (bar != null)
        {
            SetRandomPosition();
        }
    }

    void Start()
    {
        SetRandomPosition();
    }

    void Update()
    {
        if (FishingPanel != null)
        {
            bool isFishingPanelActive = FishingPanel.activeInHierarchy;

            if (isFishingPanelActive != wasFishingPanelActive)
            {
                SetRandomPosition();
            }

            wasFishingPanelActive = isFishingPanelActive;
        }

        if (enableFishMovement)
        {
            MoveFish();
        }
    }

    private void MoveFish()
    {
        float halfWidth = 0f;
        RectTransform rt = bar.GetComponent<RectTransform>();
        if (rt != null)
        {
            halfWidth = (rt.rect.width * rt.localScale.x) / 2f;
        }
        else
        {
            halfWidth = bar.localScale.x / 2f;
        }
        
        halfWidth += edgeOffset;

        transform.localPosition += new Vector3(currentSpeed * direction * Time.deltaTime, 0, 0);

        if (transform.localPosition.x >= halfWidth)
        {
            transform.localPosition = new Vector3(halfWidth, transform.localPosition.y, transform.localPosition.z);
            direction = -1;
        }
        else if (transform.localPosition.x <= -halfWidth)
        {
            transform.localPosition = new Vector3(-halfWidth, transform.localPosition.y, transform.localPosition.z);
            direction = 1;
        }
    }

    void SetRandomPosition()
    {
        float halfWidth = 0f;
        RectTransform rt = bar.GetComponent<RectTransform>();
        if (rt != null)
        {
            halfWidth = (rt.rect.width * rt.localScale.x) / 2f;
        }
        else
        {
            halfWidth = bar.localScale.x / 2f;
        }
        
        halfWidth += edgeOffset;

        float randomX = Random.Range(-halfWidth, halfWidth);
        transform.localPosition = new Vector3(randomX, transform.localPosition.y, transform.localPosition.z);
        
        if (fishImage != null && fishImages != null && fishImages.Length > 0)
        {
            int totalWeight = 0;
            // Összeszámoljuk az összes esélyt
            for (int i = 0; i < fishImages.Length; i++)
            {
                if (i < fishRarityWeights.Length)
                    totalWeight += fishRarityWeights[i];
            }

            // Sorsolunk egy véletlen számot 0 és az összsúly között
            int randomValue = Random.Range(0, totalWeight);
            int selectedIndex = 0;

            // Megkeressük, melyik halhoz tartozik a sorsolt szám
            for (int i = 0; i < fishImages.Length; i++)
            {
                if (i < fishRarityWeights.Length)
                {
                    if (randomValue < fishRarityWeights[i])
                    {
                        selectedIndex = i;
                        break;
                    }
                    randomValue -= fishRarityWeights[i];
                }
            }

            fishImage.sprite = fishImages[selectedIndex];

            // Speed assignment based on rarity
            if (selectedIndex < minSpeeds.Length && selectedIndex < maxSpeeds.Length)
            {
                currentSpeed = Random.Range(minSpeeds[selectedIndex], maxSpeeds[selectedIndex]);
            }
            
            // Randomize starting direction or make it strictly opposite
            // We set it to -1 (left) or 1 (right) randomly for a bit of unpredictability
            direction = Random.Range(0, 2) == 0 ? 1 : -1;
        }
    }
}
