using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEntry : MonoBehaviour {
    public Text nameText;

    public void SetName(string name) {
        nameText.text = name;
    }
}
