using UnityEngine;
using System.Collections;

public class NetworkUI : MonoBehaviour {
    public GameObject[] serverOnly = new GameObject[0];
    public GameObject[] clientOnly = new GameObject[0];

    void OnEnable() {
        if (!Network.isServer) {
            foreach (GameObject gameObject in serverOnly) {
                gameObject.SetActive(false);
            }
        }

        if (!Network.isClient) {
            foreach (GameObject gameObject in clientOnly) {
                gameObject.SetActive(false);
            }
        }
    }
}
