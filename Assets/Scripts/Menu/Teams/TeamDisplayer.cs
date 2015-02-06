using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TeamDisplayer : MonoBehaviour {
    public PlayerEntry playerEntry;
    public VerticalLayoutGroup teamA;
    public VerticalLayoutGroup teamB;
    public VerticalLayoutGroup teamFFA;

    private Dictionary<NetworkPlayer, PlayerEntry> connectedPlayers = new Dictionary<NetworkPlayer, PlayerEntry>();

	void Start () {
        NetworkManager networkManager = NetworkManager.instance;

        networkManager.OnUpdate += OnPlayerUpdate;
        networkManager.OnDisconnect += OnPlayerDisconnect;
	}

    void OnDestroy() {
        NetworkManager networkManager = NetworkManager.instance;

        if (networkManager) {
            networkManager.OnUpdate -= OnPlayerUpdate;
            networkManager.OnDisconnect -= OnPlayerDisconnect;
        }
    }

    void OnPlayerDisconnect(NetworkPlayer player) {
        Destroy(connectedPlayers[player]);
    }

    void OnPlayerUpdate(NetworkPlayer player, PlayerInfo playerInfo, string setting) {
        if (setting.EqualsIgnoreCase(PlayerInfo.TEAM)) {
            PlayerEntry playerEntry;

            if (connectedPlayers.ContainsKey(player)) {
                playerEntry = connectedPlayers[player];
            }
            else {
                playerEntry = Instantiate(this.playerEntry) as PlayerEntry;
                connectedPlayers.Add(player, playerEntry);
            }

            switch (playerInfo.team) {
                case 0:
                    playerEntry.transform.SetParent(teamFFA.transform);
                    break;
                case 1:
                    playerEntry.transform.SetParent(teamA.transform);
                    break;
                case 2:
                    playerEntry.transform.SetParent(teamB.transform);
                    break;
                default:
                    // Spectator
                    break;
            }

            playerEntry.SetName(playerInfo.name);
        }
    }
}
