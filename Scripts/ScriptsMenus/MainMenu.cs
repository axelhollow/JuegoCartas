using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        // Cargar volumen guardado
        AudioManager.Instance.SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        AudioManager.Instance.SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
