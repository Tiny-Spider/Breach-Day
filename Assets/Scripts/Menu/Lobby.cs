using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {
    GameObject playerEntry;
    

    GameObject team1;
    GameObject team2;

    void Awake() {

    }

	// Use this for initialization
	void Start () {
	    NetworkManager.instance.OnJoin += PlayerJoin;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnConnected() {

    }

    void PlayerJoin(NetworkPlayer networkPlayer) {




    }

    public void LoadMainMenu() {
        Application.LoadLevel(1);
    }
}
