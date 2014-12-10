using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkView))]
public class NetworkManager : MonoBehaviour {
    public static NetworkManager instance;

    public delegate void OnPlayerJoined(NetworkPlayer player);
    public event OnPlayerJoined OnJoin = delegate { };

    public delegate void OnPlayerDisconnect(NetworkPlayer player);
    public event OnPlayerDisconnect OnDisconnect = delegate { };

    public Dictionary<NetworkPlayer, PlayerInfo> connectedPlayers = new Dictionary<NetworkPlayer, PlayerInfo>();

    public string menuScene;
    public string lobbyScene;

    void Awake() {
        instance = this;
    }

    #region Connecting

    // Whenever you connect, server or client we should load the lobby.
    void OnConnectedToServer() {
        OnConnect();
    }

    void OnServerInitialized() {
        OnConnect();
    }

    void OnConnect() {
        Application.LoadLevel(lobbyScene);
        //UpdatePlayer(PlayerInfo.NAME, GameManager.instance.name);
    }

    #endregion


    #region Disconnecting

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
        networkView.RPC("AddPlayer", RPCMode.AllBuffered, player);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        networkView.RPC("RemovePlayer", RPCMode.AllBuffered, player);
    }

    #endregion


    [RPC]
    public void AddPlayer(NetworkPlayer player) {
        connectedPlayers.Add(player, new PlayerInfo());
        OnJoin(player);
    }

    [RPC]
    public void RemovePlayer(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);

        OnDisconnect(player);
        connectedPlayers.Remove(player);
    }

    [RPC]
    public void UpdatePlayer(NetworkPlayer player, string setting, string value) {
        if (networkView.isMine)
        {
            networkView.RPC("UpdatePlayer", RPCMode.OthersBuffered, player, setting, value);
        }
        else
        {
            PlayerInfo playerInfo = connectedPlayers[player];

            if (playerInfo != null)
            {
                playerInfo.SetValue(setting, value);
            }
        }
    }

    [RPC]
    public void UpdatePlayer(string setting, string value) {
        UpdatePlayer(Network.player, setting, value);
    }

    [RPC]
    public void StartGame() {
        networkView.RPC("_StartGame", RPCMode.AllBuffered);
    }

    [RPC]
    public void _StartGame() {
        Application.LoadLevel("Map 1");
    }
}
