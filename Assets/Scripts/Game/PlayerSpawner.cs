using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour {
	public Transform spawnpointsA;
	public Transform spawnpointsB;
	public Transform spawnpointsFFA;
	
	void Start () {
		if (Network.isServer) {
			NetworkManager networkManager = NetworkManager.instance;
			Player player;
			Transform position;

			int ffaCount = 0;
			int aCount = 0;
			int bCount = 0;

            var e = networkManager.connectedPlayers.GetEnumerator();
             while (e.MoveNext()) {
                 NetworkPlayer networkPlayer = e.Current.Key;

                 switch (e.Current.Value.team) {
					case 0:
						if (ffaCount + 1 >= spawnpointsFFA.childCount) {
							NetworkManager.instance.ServerNotification(networkPlayer, "Server Error: No avalible spawn point found!");
							Network.CloseConnection(networkPlayer, true);
						}
						else {
							position = spawnpointsFFA.GetChild(ffaCount);

							player = Network.Instantiate(PrefabManager.instance.teamFFA, position.position, position.rotation, 0) as Player;
							player.networkView.RPC("SetPlayer", RPCMode.AllBuffered, networkPlayer);
                            e.Current.Value.playerObject = player;

							ffaCount++;
						}
						break;
					case 1:
						if (aCount + 1 >= spawnpointsA.childCount) {
                            NetworkManager.instance.ServerNotification(networkPlayer, "Server Error: No avalible spawn point found!");
							Network.CloseConnection(networkPlayer, true);
						}
						else {
							position = spawnpointsA.GetChild(aCount);

							player = Network.Instantiate(PrefabManager.instance.teamA, position.position, position.rotation, 0) as Player;
							player.networkView.RPC("SetPlayer", RPCMode.AllBuffered, networkPlayer);
                            e.Current.Value.playerObject = player;

							aCount++;
						}
						break;
					case 2:
						if (bCount + 1 >= spawnpointsB.childCount) {
                            NetworkManager.instance.ServerNotification(networkPlayer, "Server Error: No avalible spawn point found!");
							Network.CloseConnection(networkPlayer, true);
						}
						else {
							position = spawnpointsB.GetChild(aCount);

							player = Network.Instantiate(PrefabManager.instance.teamB, position.position, position.rotation, 0) as Player;
							player.networkView.RPC("SetPlayer", RPCMode.AllBuffered, networkPlayer);
                            e.Current.Value.playerObject = player;

							bCount++;
						}
						break;
					default:
						// Spectator
						break;
				}
			}
		}
	}
}
