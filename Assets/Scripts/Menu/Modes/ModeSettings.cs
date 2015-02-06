using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ModeSettings {
    private const char splitChar_1 = '|';

    public List<Value> values = new List<Value>();

    public string ToDataString() {
        List<string> stringList = new List<string>();

        foreach (Value value in values) {
            Debug.Log("Added to send list: " + value.name + " | " + value.value);
            stringList.Add(value.value);
        }

        return String.Join(splitChar_1.ToString(), stringList.ToArray());
    }

    public void FromDataString(string data) {
        string[] splitData = data.Split(splitChar_1);

        for (int i = 0; i < splitData.Length; i++) {
            if (i < values.Count) {
                Debug.Log("SetFromDataString: " + values[i].name + " | " + splitData[i]);
                values[i].SetValue(splitData[i]);
            }
        }
    }

    public void SetValue(string valueName, string data) {
        Value valueToRemove = null;
        Value valueToAdd = null;

        foreach (Value value in values) {
            if (value.name.EqualsIgnoreCase(valueName)) {
                Debug.Log("SetValue: " + valueName + " | " + data);
                valueToRemove = value;
                value.SetValue(data);
                valueToAdd = value;
                break;
            }
        }

        values.Remove(valueToRemove);
        values.Add(valueToAdd);


        
        Debug.Log("Current Values:");
        foreach (Value value in values) {
            Debug.Log("Value: " + value.name + value.value);
        }
        Debug.Log("End Current Values");
    }
}
