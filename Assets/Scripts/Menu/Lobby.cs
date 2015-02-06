using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {
    public TeamDisplayer teamDisplayer;

    public Image levelImage;
    public Text levelName;
    public Text levelTime;
    public Text levelDoors;
    public Text levelWalls;

    public ValueEntry valueEntry;
    public Text modeText;
    public VerticalLayoutGroup modeHolder;

	void Start () {
        NetworkManager.instance.OnJoin += PlayerJoin;
        GameManager.instance.OnLevelUpdate += OnLevelUpdate;

        PlayerJoin(Network.player);

        GameManager.instance.UpdateLevel();
        GameManager.instance.UpdateMode();
	}

    void OnDestroy() {
        NetworkManager networkManager = NetworkManager.instance;
        GameManager gameManager = GameManager.instance;

        if (networkManager && gameManager) {
            networkManager.OnJoin -= PlayerJoin;
            gameManager.OnLevelUpdate -= OnLevelUpdate;
        }
    }

    public void OnLevelUpdate(LevelData levelData, LevelSettings levelSettings) {
        levelImage.sprite = levelData.levelImage;
        levelName.text = levelData.levelName;
        levelTime.text = levelSettings.time + "";
        levelDoors.text = levelSettings.doors + "/" + levelData.maxDoorAmount;
        levelWalls.text = levelSettings.walls + "/" + levelData.maxWallAmount;
    }

    public void OnModeUpdate(ModeData modeData) {
        modeText.text = modeData.modeName;
    }

    void PlayerJoin(NetworkPlayer networkPlayer) {
        // Not really required, since OnJoin is only called on a server
        // But it's always good to be safe
        if (Network.isServer) {
            int aCount = teamDisplayer.teamA.transform.childCount;
            int bCount = teamDisplayer.teamB.transform.childCount;

            if (bCount > aCount) {
                NetworkManager.instance.UpdatePlayer(networkPlayer, PlayerInfo.TEAM, "1");
            }
            else if (aCount > bCount) {
                NetworkManager.instance.UpdatePlayer(networkPlayer, PlayerInfo.TEAM, "2");
            }
            else {
                int team = Random.Range(1, 2);
                NetworkManager.instance.UpdatePlayer(networkPlayer, PlayerInfo.TEAM, team.ToString());
            }
        }
    }

    public void StartGame() {
        // Start the game trough NetworkManager, so everyone get's the update
        GameManager.instance.StartGame();
    }
}
