﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
    public Text pingText;

	void Start() {
        NetworkManager.instance.OnUpdate += OnUpdate;
	}

    void OnDestroy() {
        NetworkManager.instance.OnUpdate -= OnUpdate;
    }

    void OnUpdate(NetworkPlayer player, PlayerInfo playerInfo, string setting) {
        if (player.Equals(Network.player)) {
            if (setting.Equals(PlayerInfo.PING)) {
                pingText.text = "Ping: " + playerInfo.ping + "ms";
            }
        }
    }
}
