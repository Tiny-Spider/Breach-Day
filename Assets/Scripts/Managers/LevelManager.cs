using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance { private set; get; }

    public Level[] levels;

    void Awake() {
        instance = this;
    }

    public Level GetByName(string name) {
        foreach (Level level in levels) {
            if (level.levelName.EqualsIgnoreCase(name)) {
                return level;
            }
        }

        // TODO: we should accualy kick the player because
        // he could be using older build
        return levels[0];
    }
}
