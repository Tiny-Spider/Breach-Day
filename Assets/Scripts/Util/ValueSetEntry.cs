using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ValueSetEntry : MonoBehaviour {
    public Text displayName;
    public InputField input;
    public Toggle toggle;

    private Value value;

    public void SetValue(Value value) {
        this.value = value;

        displayName.text = value.displayName;

        input.gameObject.SetActive(false);
        toggle.gameObject.SetActive(false);

        switch (value.valueType) {
            case ValueType.String:
                input.gameObject.SetActive(true);
                input.contentType = InputField.ContentType.Name;
                input.text = value.value;
                break;
            case ValueType.Int:
                input.gameObject.SetActive(true);
                input.contentType = InputField.ContentType.IntegerNumber;
                input.text = value.value;
                break;
            case ValueType.Double:
                input.gameObject.SetActive(true);
                input.contentType = InputField.ContentType.DecimalNumber;
                input.text = value.value;
                break;
            case ValueType.Boolean:
                toggle.gameObject.SetActive(true);
                toggle.isOn = bool.Parse(value.value);
                break;
        }
    }

    public void OnStringUpdate(string data) {
        Debug.Log("OnStringUpdate: " + data);
        GameManager.instance.modeSettings.SetValue(this.value.name, data);
    }

    public void OnBoolUpdate(bool boolean) {
        value.SetValue(boolean.ToString());
    }
}
