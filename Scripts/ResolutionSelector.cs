using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSelector : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    struct ResOption
    {
        public int width;
        public int height;
        public string label;

        public ResOption(int w, int h, string l)
        {
            width = w;
            height = h;
            label = l;
        }
    }

List<ResOption> resolutions = new List<ResOption>()
{
    new ResOption(1280, 720,  "1280 x 720"),
    new ResOption(1600, 900,  "1600 x 900"),
    new ResOption(1920, 1080, "1920 x 1080"),
    new ResOption(2560, 1440, "2560 x 1440")
};

    void Start()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            options.Add(resolutions[i].label);

            if (Screen.width == resolutions[i].width &&
                Screen.height == resolutions[i].height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // Cargar guardado
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            currentIndex = PlayerPrefs.GetInt("ResolutionIndex");
        }

        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // FULLSCREEN
        bool isFullscreen = Screen.fullScreen;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        ApplyResolution(currentIndex);
    }

void ApplyResolution(int index)
{
    if (index < 0 || index >= resolutions.Count) return;

    var r = resolutions[index];
    Screen.SetResolution(r.width, r.height, fullscreenToggle.isOn);
}

    public void SetResolution(int index)
    {
        ApplyResolution(index);

        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        ApplyResolution(resolutionDropdown.value);

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}