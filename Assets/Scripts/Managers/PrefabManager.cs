using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {
    public static PrefabManager instance;

    public Player teamA;
    public Player teamB;

    void Awake() {
        instance = this;
    }
}
