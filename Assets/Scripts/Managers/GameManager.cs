using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance { private set; get; }

    #region Events
    public delegate void OnModeChange(ModeData modeData, ModeSettings modeSettings);
    public event OnModeChange OnModeUpdate = delegate { };

    public delegate void OnLevelChange(LevelData levelData, LevelSettings levelSettings);
    public event OnLevelChange OnLevelUpdate = delegate { };
    #endregion

    public string splashScene = "Splash Screen";
    public string menuScene = "Menu";
    public string lobbyScene = "Lobby";

    public ModeData modeData { private set; get; }
    public ModeSettings modeSettings { private set; get; }
    public LevelData levelData { private set; get; }
    public LevelSettings levelSettings { private set; get; }
 
    public string playerName = "Player";

    public bool inGame { private set; get; }

	void Awake () {
        instance = this;

        DontDestroyOnLoad(gameObject);
	}

    void Start() {
        SetMode(ModeManager.instance.GetDefault().modeName);
        SetLevel(LevelManager.instance.GetDefault().levelName);

        levelData = LevelManager.instance.GetDefault();
        levelSettings = new LevelSettings();
    }

    // ==== Loading ====

    void OnLevelWasLoaded(int level) {
        string loadedLevel = Application.loadedLevelName;

        if (loadedLevel.EqualsIgnoreCase(splashScene) || loadedLevel.EqualsIgnoreCase(menuScene) || loadedLevel.EqualsIgnoreCase(lobbyScene)) {
            if (inGame) {
                OnGameLeave();
                inGame = false;
            }
        } else {
            if (!inGame) {
                OnGameLoaded();
                inGame = true;
            }
        }
    }

    void OnGameLoaded() {
        gameObject.AddComponent(modeData.modeClassName);

        NetworkManager.instance.UpdateMyself(PlayerInfo.LOADED, true.ToString());
    }

    void OnGameLeave() {
        Destroy(gameObject.GetComponent(modeData.modeClassName));
    }

    // ==== MODE ====

    [RPC]
    private void _SetMode(string mode, string data) {
        Debug.Log("_SetMode: " + mode);

        modeData = ModeManager.instance.GetByName(mode);
        modeSettings = modeData.GetModeSettings();
        modeSettings.FromDataString(data);

        CallModeEvent();
    }

    public void SetMode(string mode) {
        Debug.Log("SetMode: " + mode);

        modeData = ModeManager.instance.GetByName(mode);
        modeSettings = modeData.GetModeSettings();

        CallModeEvent();
    }

    public void UpdateMode() {
        if (Network.isServer) {
            Debug.Log("Sending over network: " + modeSettings.ToDataString());
            networkView.RPC("_SetMode", RPCMode.AllBuffered, modeData.modeName, modeSettings.ToDataString());
        }
        else if (Network.peerType == NetworkPeerType.Disconnected) {
            CallModeEvent();
        }
    }

    private void CallModeEvent() {
        Debug.Log("CallModeEvent: " + modeData.modeName);
        OnModeUpdate(modeData, modeSettings);
    }

    // ==== LEVEL ====

    [RPC]
    private void _SetLevel(string level, string data) {
        Debug.Log("Setting Level: " + level);

        levelData = LevelManager.instance.GetByName(level);
        levelSettings = LevelSettings.FromDataString(data);

        CallLevelEvent();
    }

    public void SetLevel(string level) {
        Debug.Log("Setting Level: " + level);

        levelData = LevelManager.instance.GetByName(level);
        levelSettings = new LevelSettings();

        CallLevelEvent();
    }

    public void UpdateLevel() {
        if (Network.isServer) {
            networkView.RPC("_SetLevel", RPCMode.AllBuffered, levelData.levelName, levelSettings.ToDataString());
        }
        else if (Network.peerType == NetworkPeerType.Disconnected) {
            CallLevelEvent();
        }
    }

    private void CallLevelEvent() {
        Debug.Log("Level: " + levelData.levelName);
        OnLevelUpdate(levelData, levelSettings);
    }

    // ==== START ====

    [RPC]
    private void _StartGame() {
        Application.LoadLevel(levelData.sceneName);
    }

    public void StartGame() {
        if (Network.isServer) {
            networkView.RPC("_StartGame", RPCMode.AllBuffered);
        }
    }
}
