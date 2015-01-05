using UnityEngine;
using System.Collections;

public class Level : ScriptableObject {
    public string levelName;
    public string levelDescription;
    public int maxPlayers;
    public string[] avalibleGamemodes;

    public int maxDoorAmount;
    public int maxWallAmount;

    public string sceneName;
}
