using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeManager : MonoBehaviour {
    public static ModeManager instance { private set; get; }

    public ModeData[] modes { private set; get; }

    void Awake() {
        instance = this;
        LoadModes();
    }

    private void LoadModes() {
        modes = Resources.LoadAll<ModeData>("Modes");

        if (modes.Length == 0) {
            Debug.LogError("No modes loaded! Add a \"ModeData\" in a \"Resources/Modes/\" folder!");
        }
    }

    public ModeData GetDefault() {
        return modes[0];
    }

    public ModeData GetByName(string name) {
        foreach (ModeData mode in modes) {
            if (mode.modeName.EqualsIgnoreCase(name)) {
                return mode;
            }
        }

        // TODO: we should accualy kick the player because
        // he could be using older build

        Debug.Log("MODE NOT FOUND: " + name);

        return modes[0];
    }
}
