using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {
    public static PrefabManager instance { private set; get; }

    public Player teamA;
    public Player teamB;
    public Player teamFFA;

    void Awake() {
        instance = this;
    }
}
