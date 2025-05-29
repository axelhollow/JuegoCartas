using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] musicClips;
    public List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> sfxDict;

    void Awake()
    {
        // Singleton: sólo una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Eliminar duplicados
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persistir entre escenas
        InitSFXDict();
    }

    private void Start()
    {
        Instance.PlayMusic(0);
    }
    void InitSFXDict()
    {
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
        {
            if (!sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
        }
    }

    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sfxDict[name]);
        }
        else
        {
            Debug.LogWarning("SFX no encontrado: " + name);
        }
    }

    public void SetMusicVolume(float volume) => musicSource.volume = volume;

    public void SetSFXVolume(float volume) => sfxSource.volume = volume;

    public void StopMusic() => musicSource.Stop();
}
