using UnityEngine;
using System.Collections;

public class PlayerInfo {
    public const string NAME = "name";
    public const string TEAM = "team";
    public const string PING = "ping";


    public string name { private set; get; }
    public string team { private set; get; }
    public int ping { private set; get; }

    public bool SetValue(string setting, string value) {
        switch (setting) {
            case NAME:
                name = value;
                return true;
            case TEAM:
                team = value;
                return true;
            case PING:
                int ping = 0;
                bool succes = int.TryParse(value, out ping);
 
                if (succes) 
                    this.ping = ping;

                return succes;

            default:
                Debug.Log("Unknown Setting: \"" + setting + "\"");
                return false;
        }
    }
}
