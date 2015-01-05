using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamDisplayer : MonoBehaviour {
    public GameObject playerEntry;
    public GameObject team1;
    public GameObject team2;

	// Use this for initialization
	void Start () {
	    NetworkManager.instance.OnJoin += OnPlayerJoin;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPlayerJoin(NetworkPlayer networkPlayer) {
        GameObject go = (GameObject)GameObject.Instantiate(playerEntry);
        go.transform.parent = GetTeamAutoJoin().transform;

    }

    GameObject GetTeamAutoJoin(){
        int team1Count = 0;
        int team2Count = 0;
        foreach (GameObject playerEntry in team1.transform)
        {
            team1Count++;
        }
        foreach(GameObject playerEntry in team2.transform){
            team2Count++;
        }

        if (team1Count >= team2Count)
        {
            return team1;
        }
        else
        {
            return team2;
        }

    }
}
