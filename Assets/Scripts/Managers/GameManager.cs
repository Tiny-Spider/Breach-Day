using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public string name = "Player";
    public string uniqueGameType = "breach_day";

    public Mode mode;
    public WorldSettings worldSettings;

	void Awake () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
}
