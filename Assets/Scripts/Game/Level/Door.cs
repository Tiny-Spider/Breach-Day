using UnityEngine;
using System.Collections;

// Should be placed on door object
public class Door : MonoBehaviour {
    public GameObject disabledDoor;
    public GameObject enabledDoor;

    public void SetDoorActive(bool enable) {
        disabledDoor.SetActive(!enable);
        enabledDoor.SetActive(enable);
    }
}
