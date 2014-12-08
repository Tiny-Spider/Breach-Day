﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public Mode mode;
    public WorldSettings worldSettings;

	void Awake () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
}
