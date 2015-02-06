using UnityEngine;
using System.Collections;

public class PlayerSettings {
    public string name;

    // Load
    public PlayerSettings() {
        name = PlayerPrefs.GetString("playerName");
    }

    // Save
    public void Save() {
        PlayerPrefs.SetString("playerName", name);

        PlayerPrefs.Save();
    }
}
