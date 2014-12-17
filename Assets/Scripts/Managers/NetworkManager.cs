using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkView))]
public class NetworkManager : MonoBehaviour {
    public static NetworkManager instance { private set; get; }

    public delegate void OnServerNotification(string message);
    public event OnServerNotification OnNote = delegate { };

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

    public PlayerInfo GetMyInfo() {
        return connectedPlayers[Network.player];
    }

    #region Connecting

    // Whenever you connect, server or client we should load the lobby.
    void OnConnectedToServer() {
        Application.LoadLevel(lobbyScene);
    }

    void OnServerInitialized() {
        Application.LoadLevel(lobbyScene);
        networkView.RPC("AddPlayer", RPCMode.AllBuffered, Network.player);
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
        Debug.Log("Added new player \"" + player.ToString() + "\"");
        
        // Senpai server noticed me, let's send him our name
        if (player.Equals(Network.player)) {
            Debug.Log("Added myself! Updating my name: " + Network.player.ToString() + " | " + player.ToString());
            StartCoroutine(SendInfo());
        }

        OnJoin(player);
    }

    IEnumerator SendInfo() {
        yield return new WaitForSeconds(0.1F);
        UpdateMyInfo(PlayerInfo.NAME, GameManager.instance.name);
    }

    [RPC]
    public void RemovePlayer(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);

        OnDisconnect(player);
        connectedPlayers.Remove(player);
    }

    // Can only be called over RPC due to networkmessageinfo parameter
    [RPC]
    public void UpdatePlayer(NetworkPlayer player, string setting, string value, bool set, NetworkMessageInfo info) {
        //Debug.Log("UpdatePlayer");

        // Sender is server, server may always update
        if (set) {
            Debug.Log("Update from server: " + info.sender.ToString() + " | " + player.ToString() + " | " + setting + " | " + value);

            // Update the profile of the person
            PlayerInfo playerInfo = connectedPlayers[player];

            if (playerInfo != null) {
                playerInfo.SetValue(setting, value);
            }
            else {
                Debug.Log("Player \"" + player.ToString() + "\" not found!");
            }

        } else {
            // Recived an update from client, do some checks before sending it to others
            if (Network.isServer) {
                Debug.Log("Update from client: " + info.sender.ToString() + " | " + player.ToString() + " | " + setting + " | " + value);

                switch (setting) {
                    case PlayerInfo.NAME:
                        // Check for invalid name, for example swearing
                        /*
                        if (value.Contains("fuck")) {
                            networkView.RPC("ServerNotification", player, "Your name is inappropiate, you have been kicked!");
                            Network.CloseConnection(player, true);
                            return;
                        }

                        HashSet<string> names = new HashSet<string>();
                        foreach (PlayerInfo playerInfo in connectedPlayers.Values) {
                            names.Add(playerInfo.name.ToLower());
                        }

                        if (names.Contains(value.ToLower())) {
                            networkView.RPC("ServerNotification", player, "Your name is already used! Please connect using a different name.");
                            Network.CloseConnection(player, true);
                            return;
                        }
                        */

                        // All is good, send the others information
                        networkView.RPC("UpdatePlayer", RPCMode.AllBuffered, player, setting, value, true);

                        break;
                    case PlayerInfo.TEAM:

                        // All is good, send the others information
                        networkView.RPC("UpdatePlayer", RPCMode.AllBuffered, player, setting, value, true);

                        break;
                    default:
                        networkView.RPC("ServerNotification", player, "Invalid data send! This can be caused by outdated server/client, please update your game!");
                        Network.CloseConnection(player, true);
                        break;
                }
            }
        }

        string connected = "";
        foreach (PlayerInfo a in connectedPlayers.Values) {
            connected += "(" + a.name + ") ";
        }

        Debug.Log("Connected Players: " + connected);
    }

    public void UpdateInfo(NetworkPlayer player, string setting, string value) {
        networkView.RPC("UpdatePlayer", Network.isServer ? RPCMode.AllBuffered : RPCMode.Server, player, setting, value, Network.isServer);
    }

    public void UpdateMyInfo(string setting, string value) {
        networkView.RPC("UpdatePlayer", Network.isServer ? RPCMode.AllBuffered : RPCMode.Server, Network.player, setting, value, Network.isServer);
    }

    [RPC]
    public void ServerNotification(string message) {
        OnNote(message);
    }

    [RPC]
    public void StartGame() {
        if (Network.isServer)
        networkView.RPC("_StartGame", RPCMode.AllBuffered);
    }

    [RPC]
    public void _StartGame() {
        Application.LoadLevel("Map_1");
    }
}
