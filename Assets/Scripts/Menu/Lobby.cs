using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {
    public TeamDisplayer teamDisplayer;

	void Start () {
        NetworkManager.instance.OnJoin += PlayerJoin;
        PlayerJoin(Network.player);
        //NetworkManager.instance.UpdateMyself(PlayerInfo.TEAM, "1");
	}

    void OnDestroy() {
        NetworkManager.instance.OnJoin -= PlayerJoin;
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

    public void LoadMainMenu() {
        Application.LoadLevel(1);
    }

    public void OpenPopupPanel(GameObject panel) {
        panel.SetActive(true);
    }

    public void ClosePopupPanel(GameObject panel) {
        panel.SetActive(false);
    }

    
}
