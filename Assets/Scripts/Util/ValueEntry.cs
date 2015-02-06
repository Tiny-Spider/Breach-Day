using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ValueEntry : MonoBehaviour {
    public Text displayName;
    public Text valueText;

    private Value value;

    public void SetValue(Value value) {
        this.value = value;

        Debug.Log("MODE: " + value.displayName + " - VALUE: " + value.value);
        displayName.text = value.displayName;
        valueText.text = value.value;
    }
}
