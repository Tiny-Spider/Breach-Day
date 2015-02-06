using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelData : ScriptableObject {
    public string levelName = "";
    public string levelDescription = "";
    public Sprite levelImage;

    public int maxPlayers;
    public string[] avalibleGamemodes;

    public int maxDoorAmount;
    public int maxWallAmount;

    public string sceneName;
}