using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Toggle vSyncToggle;
    public TextMeshProUGUI fpsNumText;
    public Slider fpsNumSlider;
    public Toggle lockFPSToggle;
    public Toggle AnisotropicFiltering;

    void Start()
    {
        fpsNumSlider.wholeNumbers = true;
        fpsNumSlider.minValue = 20;
        fpsNumSlider.maxValue = 565;
    }

    void Update()
    {
        fps();
        GraphicsSettings();
    }

    void GraphicsSettings()
    {
        if (AnisotropicFiltering.isOn)
        {
            QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.ForceEnable;
        }
        else
        {
            QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Disable;
        }
    }

    void fps()
    {
        if (vSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
            lockFPSToggle.interactable = false;
            fpsNumSlider.interactable = false;
            fpsNumText.text = "VSync";
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            lockFPSToggle.interactable = true;
            fpsNumSlider.interactable = true;

            if (lockFPSToggle.isOn)
            {
                int fps = (int)fpsNumSlider.value;
                Application.targetFrameRate = fps;
                fpsNumText.text = fps.ToString();
            }
            else
            {
                Application.targetFrameRate = -1;
                fpsNumText.text = "Unlimited";
            }
        }
    }
}
