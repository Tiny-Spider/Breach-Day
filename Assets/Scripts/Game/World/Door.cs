using UnityEngine;
using System.Collections;

// Should be placed on door object
public class Door : MonoBehaviour {
    public GameObject noDoorObject;

    public void SetActive(bool enable) {
        noDoorObject.SetActive(!enable);
        gameObject.SetActive(enable);
    }
}
