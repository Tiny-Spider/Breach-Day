using UnityEngine;
using System.Collections;

public class PlayerInfo {
    public const string NAME = "name";
    public const string TEAM = "team";


    public string name { private set; get; }
    public string team { private set; get; }

    public bool SetValue(string setting, string value) {
        switch (setting) {
            case NAME:
                name = value;
                return true;
            case TEAM:
                team = value;
                return true;

            default:
                Debug.Log("Unknown Setting: \"" + setting + "\"");
                return false;
        }
    }
}
