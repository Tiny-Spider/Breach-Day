using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance { private set; get; }

    public string splashScene = "Splash Screen";
    public string menuScene = "Menu";
    public string lobbyScene = "Lobby";
 
    public string name = "Player";
    public string uniqueGameType = "breach_day";

    public string gamemode = "";

    public string levelName;
    public int levelTime;
    public int levelDoors;
    public int levelWalls;

    public bool inGame { private set; get; }

	void Awake () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}


    // Loading

    void OnLevelWasLoaded(int level) {
        string loadedLevel = Application.loadedLevelName;

        if (loadedLevel.EqualsIgnoreCase(splashScene) || loadedLevel.EqualsIgnoreCase(menuScene) || loadedLevel.EqualsIgnoreCase(lobbyScene)) {
            OnGameLeave();
            inGame = false;
        } else {
            OnGameLoaded();
            inGame = true;
        }
    }

    void OnGameLoaded() {
        gameObject.AddComponent(gamemode);

        NetworkManager.instance.UpdateMyself(PlayerInfo.LOADED, true.ToString());
    }

    void OnGameLeave() {
        Destroy(gameObject.GetComponent(gamemode));
    }

    // Data sync

    [RPC]
    private void _SetGamemode(string gamemode) {
        this.gamemode = gamemode;
    }

    public void SetGamemode(string gamemode) {
        if (Network.isServer) {
            networkView.RPC("_SetGamemode", RPCMode.AllBuffered, gamemode);
        }
    }

    [RPC]
    private void _SetLevelInfo(string levelName, int levelTime, int levelDoors, int levelWalls) {
        this.levelName = levelName;
        this.levelTime = levelTime;
        this.levelDoors = levelDoors;
        this.levelWalls = levelWalls;
    }

    public void SetLevelInfo(string levelName, int levelTime, int levelDoors, int levelWalls) {
        if (Network.isServer) {
            networkView.RPC("_SetLevelInfo", RPCMode.AllBuffered, levelName, levelTime, levelDoors, levelWalls);
        }
    }

    [RPC]
    private void _StartGame() {
        Level level = LevelManager.instance.GetByName(levelName);

        Application.LoadLevel(level.sceneName);
    }

    public void StartGame() {
        if (Network.isServer) {
            networkView.RPC("_StartGame", RPCMode.AllBuffered);
        }
    }
}
