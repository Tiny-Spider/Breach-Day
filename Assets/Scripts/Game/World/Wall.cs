using UnityEngine;
using System.Collections;

// Should be placed on a disabled wall
public class Wall : MonoBehaviour {
    public GameObject[] pieces;

    public void SetActive(bool enable) {
        SetActive(!enable);

        if (enable) {
            foreach (GameObject piece in pieces) {
                piece.SetActive(true);
            }
        }
    }
}
