using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance { private set; get; }

    public LevelData[] levels { private set; get; }

    void Awake() {
        instance = this;

        LoadLevels();
    }

    private void LoadLevels() {
        levels = Resources.LoadAll<LevelData>("Levels");

        if (levels.Length == 0) {
            Debug.LogError("No levels loaded! Add a \"LevelData\" in a \"Resources/Levels/\" folder!");
        }
    }

    public LevelData GetDefault() {
        return levels[0];
    }

    public LevelData GetByName(string name) {
        foreach (LevelData level in levels) {
            if (level.levelName.EqualsIgnoreCase(name)) {
                return level;
            }
        }

        // TODO: we should accualy kick the player because
        // he could be using older build
        return levels[0];
    }
}
