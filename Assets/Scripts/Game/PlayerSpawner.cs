using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour {
    public Transform spawnA;
    public Transform spawnB;
    
	void Start () {
        if (Network.isServer) {
            NetworkManager networkManager = NetworkManager.instance;
            Player player;

            foreach (KeyValuePair<NetworkPlayer, PlayerInfo> entry in networkManager.connectedPlayers) {
                switch (entry.Value.team) {
                    case 1:
                        player = Network.Instantiate(PrefabManager.instance.teamA, spawnA.position, spawnA.rotation, 0) as Player;
                        player.networkView.RPC("SetPlayer", RPCMode.AllBuffered, entry.Key);
                        break;
                    case 2:
                        player = Network.Instantiate(PrefabManager.instance.teamB, spawnB.position, spawnB.rotation, 0) as Player;
                        player.networkView.RPC("SetPlayer", RPCMode.AllBuffered, entry.Key);
                        break;
                    default:
                        // Spectator
                        break;
                }
               
            }
        }
	}
}
