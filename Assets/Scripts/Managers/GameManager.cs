﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance { private set; get; }

    public string name = "Player";
    public string uniqueGameType = "breach_day";

    public Mode mode;
    public WorldSettings worldSettings = new WorldSettings("Map_1", true, 0, 10);

	void Awake () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
}
