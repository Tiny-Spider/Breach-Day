using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkView))]
public class NetworkManager : MonoBehaviour {
    public PlayerInfo myPlayerInfo = new PlayerInfo();
    public Dictionary<NetworkPlayer, PlayerInfo> connectedPlayers = new Dictionary<NetworkPlayer, PlayerInfo>();

    public string menuScene;
    public string lobbyScene;

    #region On Connected

    // Whenever you connect, server or client we should load the lobby.
    void OnConnectedToServer() {
        Application.LoadLevel(lobbyScene);
    }

    void OnServerInitialized() {
        Application.LoadLevel(lobbyScene);
    }

    #endregion


    #region On Disconnect/Quit/Stop

    // Works for client and server, whenever you disconnect we should clear connected players
    // and close all connections to prevent dead connections, and load the menu
    void OnDisconnectedFromServer(NetworkDisconnection info) {
        Debug.Log("Disconnected: " + info.ToString());
        Application.LoadLevel(menuScene);
        connectedPlayers.Clear();
    }

    #endregion


    #region Server

    // Only called on a server, send RPC's to change the player amount on all clients
    void OnPlayerConnected(NetworkPlayer player) {
        networkView.RPC("ChangePlayer", RPCMode.AllBuffered, player, true);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        networkView.RPC("ChangePlayer", RPCMode.AllBuffered, player, false);
    }

    #endregion


    [RPC]
    public void ChangePlayer(NetworkPlayer player, bool add) {
        if (add) {
            connectedPlayers.Add(player, new PlayerInfo());
        } else {
            connectedPlayers.Remove(player);
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
        }
    }

    [RPC]
    public void UpdatePlayer(NetworkPlayer player, string setting, string value) {
        PlayerInfo playerInfo = connectedPlayers[player];

        if (playerInfo != null) {
            playerInfo.SetValue(setting, value);
        }
    }
}
