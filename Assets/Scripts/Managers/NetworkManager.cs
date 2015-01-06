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

    public delegate void OnPlayerUpdate(NetworkPlayer player, PlayerInfo playerInfo, string setting);
    public event OnPlayerUpdate OnUpdate = delegate { };

    public Dictionary<NetworkPlayer, PlayerInfo> connectedPlayers = new Dictionary<NetworkPlayer, PlayerInfo>();

    public float pingUpdateSpeed = 0.5F;

    void Awake() {
        instance = this;
    }

    void Start() {
        if (Network.isServer)
            StartCoroutine(PingTask());
    }

    public PlayerInfo GetMyInfo() {
        return connectedPlayers[Network.player];
    }

    #region Connecting

    // Whenever you connect, server or client we should load the lobby.
    void OnConnectedToServer() {
        Application.LoadLevel(GameManager.instance.lobbyScene);
    }

    void OnServerInitialized() {
        Application.LoadLevel(GameManager.instance.lobbyScene);
        networkView.RPC("_AddPlayer", RPCMode.AllBuffered, Network.player);
    }

    #endregion


    #region Disconnecting

    // Works for client and server, whenever you disconnect we should clear connected players
    // and close all connections to prevent dead connections, and load the menu
    void OnDisconnectedFromServer(NetworkDisconnection info) {
        Debug.Log("Disconnected: " + info.ToString());
        Application.LoadLevel(GameManager.instance.menuScene);
        connectedPlayers.Clear();
    }

    #endregion


    #region Server

    // Only called on a server, send RPC's to change the player amount on all clients
    void OnPlayerConnected(NetworkPlayer player) {
        networkView.RPC("_AddPlayer", RPCMode.AllBuffered, player);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        networkView.RPC("_RemovePlayer", RPCMode.AllBuffered, player);
    }

    #endregion


    [RPC]
    private void _AddPlayer(NetworkPlayer player) {
        connectedPlayers.Add(player, new PlayerInfo());
        Debug.Log("Added new player \"" + player.ToString() + "\"");
        
        // Senpai server noticed me, let's send him our name
        if (player.Equals(Network.player)) {
            Debug.Log("Added myself, Updating my info! (" + Network.player.ToString() + " | " + player.ToString() + ")");
            StartCoroutine(SendInfo());
        }

        OnJoin(player);
    }

    // This has to be delayed, blame Unity
    IEnumerator SendInfo() {
        yield return new WaitForEndOfFrame();
        UpdateMyself(PlayerInfo.NAME, GameManager.instance.name);
    }

    [RPC]
    private void _RemovePlayer(NetworkPlayer player) {
        OnDisconnect(player);

        if (Network.isServer) {
            Player playerObject = connectedPlayers[player].playerObject;

            if (playerObject)
                Network.Destroy(playerObject.gameObject);
        }

        Network.RemoveRPCs(player);
        connectedPlayers.Remove(player);
    }

    [RPC]
    private void _UpdatePlayer(NetworkPlayer player, string setting, string value, bool set, NetworkMessageInfo info) {
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

            OnUpdate(player, playerInfo, setting);

        } else {
            // Recived an update from client, do some checks before sending it to others
            if (Network.isServer) {
                Debug.Log("Update from client: " + info.sender.ToString() + " | " + player.ToString() + " | " + setting + " | " + value);

                switch (setting) {
                    case PlayerInfo.NAME:
                        // Check for invalid name, for example swearing
                        if (value.Contains("fuck")) {
                            ServerNotification(player, "Your name is inappropiate, you have been kicked!");
                            Network.CloseConnection(player, true);
                            return;
                        }

                        if (connectedPlayers.Values.ContainsName(name)) {
                            ServerNotification(player, "Your name is already used! Please connect using a different name.");
                            Network.CloseConnection(player, true);
                            return;
                        }

                        // All is good, send the others information
                        UpdatePlayer(player, setting, value);

                        break;
                    case PlayerInfo.TEAM:

                        // All is good, send the others information
                        UpdatePlayer(player, setting, value);

                        break;
                    case PlayerInfo.LOADED:

                        // All is good, send the others information
                        UpdatePlayer(player, setting, value);

                        break;
                    default:
                        ServerNotification(player, "Invalid data send! This can be caused by outdated server/client, please update your game!");
                        Network.CloseConnection(player, true);
                        break;
                }
            }
        }
    }

    public void UpdatePlayer(NetworkPlayer player, string setting, string value) {
        networkView.RPC("_UpdatePlayer", Network.isServer ? RPCMode.AllBuffered : RPCMode.Server, player, setting, value, Network.isServer);
    }

    public void UpdateMyself(string setting, string value) {
        networkView.RPC("_UpdatePlayer", Network.isServer ? RPCMode.AllBuffered : RPCMode.Server, Network.player, setting, value, Network.isServer);
    }

    [RPC]
    private void _ServerNotification(string message) {
        OnNote(message);
    }

    public void ServerNotification(NetworkPlayer player, string message) {
        networkView.RPC("_ServerNotification", player, message);
    }

    

    IEnumerator PingTask() {
        while (true) {
            Debug.Log("ping");
            if (GameManager.instance.inGame) {
                Debug.Log("ingame");
                foreach (NetworkPlayer connection in Network.connections) {
                    Debug.Log("ping: " + connection.ToString());
                    UpdatePlayer(connection, PlayerInfo.PING, Network.GetLastPing(connection).ToString());
                }
            }

            yield return new WaitForSeconds(pingUpdateSpeed);
        }
    }
}
