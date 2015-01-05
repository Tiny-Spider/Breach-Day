using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {

    public static Lobby instance;
    public GameObject playerEntry;
    
    public GameObject team1;
    public GameObject team2;



	void Start () {
        instance = this;
        NetworkManager.instance.OnJoin += PlayerJoin;
        NetworkManager.instance.OnUpdate += OnPlayerUpdate;
	}

    void PlayerJoin(NetworkPlayer networkPlayer) {
        // Not really required, since OnJoin is only called on a server
        // But it's always good to be safe
        if (Network.isServer)
        {
            NetworkManager.instance.UpdateInfo(networkPlayer, PlayerInfo.TEAM, "1");
        }
    }

    #region Lobby
    public void CreatePlayerCard(PlayerInfo playerInfo) {
        print("CreatePlayercard");
        GameObject temp = (GameObject)Instantiate(playerEntry);
        PlayerEntry tempPlayerEntry = temp.GetComponent<PlayerEntry>();
        tempPlayerEntry.gameObject.transform.parent = GetAutoBalanceTeam().transform;
        tempPlayerEntry.name.text = playerInfo.name;

    }

    public GameObject GetAutoBalanceTeam() {
        int team1Count = 0;
        int team2Count = 0;

        foreach (Transform child in team1.transform)
        {
            team1Count++;
        }

        foreach (Transform child in team2.transform)
        {
            team2Count++;
        }

        if (team1Count >= team2Count)
            return team1;
        else return team2;
    }

    #endregion

    public void OnPlayerUpdate(NetworkPlayer player, string setting, string value, bool set) {
        if (set)
        {
            if (setting.Equals(PlayerInfo.NAME))
            {
                
            }
        }
    }

    public void StartGame() {
        // Start the game trough NetworkManager, so everyone get's the update
        NetworkManager.instance.StartGame();
    }

    public void LoadMainMenu() {
        Application.LoadLevel(1);
    }

    public void OpenPopupPanel(GameObject panel) {
        panel.SetActive(true);
    }

    public void ClosePopupPanel(GameObject panel) {
        panel.SetActive(false);
    }

    public void UpdateTeams() {

    }

    public void UpdatePlayer(NetworkPlayer player, string setting, string value, bool set) {

    }
}
