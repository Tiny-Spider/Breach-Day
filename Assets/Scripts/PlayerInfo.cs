using UnityEngine;
using System.Collections;

public class PlayerInfo {
    public const string NAME = "name";
    public const string TEAM = "team";
    public const string PING = "ping";
    public const string LOADED = "loaded";

    public PlayerEntry playerEntry;

    public string name { private set; get; }
    public int team { private set; get; }
    public int ping { private set; get; }
    public bool loaded { private set; get; }

    public Player playerObject;

    public bool SetValue(string setting, string value) {
        bool succes;

        switch (setting) {
            case NAME:
                name = value;
                return true;
            case TEAM:
                int team = 0;
                succes = int.TryParse(value, out team);

                if (succes)
                    this.team = team;

                return succes;
            case PING:
                int ping = 0;
                succes = int.TryParse(value, out ping);
 
                if (succes) 
                    this.ping = ping;

                return succes;
            case LOADED:
                bool loaded = false;
                succes = bool.TryParse(value, out loaded);

                if (succes)
                    this.loaded = loaded;

                return succes;

            default:
                Debug.Log("Unknown Setting: \"" + setting + "\"");
                return false;
        }
    }
}
