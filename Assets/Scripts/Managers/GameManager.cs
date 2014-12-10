using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public string name = "Player";

    public Mode mode;
    public WorldSettings worldSettings;

    public string uniqueGameType = "breach_day";

	void Awake () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
}
