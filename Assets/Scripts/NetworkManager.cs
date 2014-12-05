using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    public string lobbyScene;

    #region On Connected
    void OnConnectedToServer() {
        Application.LoadLevel(lobbyScene);
    }

    void OnServerInitialized() {
        Application.LoadLevel(lobbyScene);
    }
    #endregion


    #region On Disconnect/Quit/Stop
    void OnDisconnectedFromServer(NetworkDisconnection info) {

    }
    #endregion

    #region Server
    void OnPlayerConnected(NetworkPlayer player) {
   
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        // Cleanup
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
    #endregion
}
