using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    public float soundVolume;
    public float musicVolume;

    void Awake() {
        instance = this;
    }

    void Start() {
        InitializeVolume();
    }

    void InitializeVolume() {
        soundVolume = PlayerPrefs.GetFloat("soundVolume", 1);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
    }

    public void SaveSettings() {
        PlayerPrefs.SetFloat("soundVolume", soundVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }
}
