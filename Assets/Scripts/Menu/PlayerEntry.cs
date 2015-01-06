using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEntry : MonoBehaviour {
    public Text name;

    public void SetName(string name) {
        this.name.text = name;
    }
}
