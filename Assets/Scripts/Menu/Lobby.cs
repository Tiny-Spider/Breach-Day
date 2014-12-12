﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {
    GameObject playerEntry;
    
    GameObject team1;
    GameObject team2;

	void Start () {
        NetworkManager.instance.OnJoin += PlayerJoin;
        NetworkManager.instance.UpdateMyInfo(PlayerInfo.TEAM, "teamA");
	}

    void PlayerJoin(NetworkPlayer networkPlayer) {
        // Not really required, since OnJoin is only called on a server
        // But it's always good to be safe
        if (Network.isServer) {
            NetworkManager.instance.UpdateInfo(networkPlayer, PlayerInfo.TEAM, "teamA");
        }
    }

    public void StartGame() {
        // Start the game trough NetworkManager, so everyone get's the update
        NetworkManager.instance.StartGame();
    }

    public void LoadMainMenu() {
        Application.LoadLevel(1);
    }
}