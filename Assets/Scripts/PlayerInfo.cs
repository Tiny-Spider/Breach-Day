﻿using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
    public const string NAME = "name";


    public string name;

    public bool SetValue(string setting, string value) {
        switch (setting) {
            case NAME:
                name = value;
                return true;

            default:
                Debug.Log("Unknown Setting: \"" + setting + "\"");
                return false;
        }
    }
}
