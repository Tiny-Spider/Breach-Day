using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour {
    public Transform spawnA;
    public Transform spawnB;

    public Transform playerHolder;
    
	void Start () {
        if (Network.isServer) {
            NetworkManager networkManager = NetworkManager.instance;

            foreach (KeyValuePair<NetworkPlayer, PlayerInfo> entry in networkManager.connectedPlayers) {
                bool teamA = entry.Value.team.Equals("teamA");
                Player player = Network.Instantiate(teamA ? PrefabManager.instance.teamA : PrefabManager.instance.teamB, teamA ? spawnA.position : spawnB.position, teamA ? spawnA.rotation : spawnB.rotation, 0) as Player;
                player.networkView.RPC("SetPlayer", RPCMode.AllBuffered, entry.Key);

                Debug.LogError("ADDED PLAYER: " + entry.Key);
            }
        }
	}
}
