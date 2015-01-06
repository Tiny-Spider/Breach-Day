using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {
    GameObject playerEntry;
    
    GameObject team1;
    GameObject team2;



	void Start () {
        NetworkManager.instance.OnJoin += PlayerJoin;
        NetworkManager.instance.UpdateMyself(PlayerInfo.TEAM, "1");
	}

    void PlayerJoin(NetworkPlayer networkPlayer) {
        // Not really required, since OnJoin is only called on a server
        // But it's always good to be safe
        if (Network.isServer) {
            NetworkManager.instance.UpdatePlayer(networkPlayer, PlayerInfo.TEAM, "1");
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
