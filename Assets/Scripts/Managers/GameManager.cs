using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

	void Awake () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}

	void Update () {
	
	}
}
