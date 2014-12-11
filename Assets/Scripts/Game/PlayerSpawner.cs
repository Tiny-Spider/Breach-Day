using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
    // TODO: fix
	void Start () {
        if (NetworkManager.instance.GetMyInfo().team == "teamA") {
            //Network.Instantiate()
        }
	}
}
