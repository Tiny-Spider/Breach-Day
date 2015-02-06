using UnityEngine;
using System.Collections;

[System.Serializable]
public class Value {
    public string name;
    public string displayName;
    public ValueType valueType;
    public string value;// { private set; get; }

    public bool SetValue(string value) {
        bool succes = false;

        switch (valueType) {
            case ValueType.String:
                succes = true;
                break;
            case ValueType.Int:
                int _int;
                succes = int.TryParse(value, out _int);
                break;
            case ValueType.Double:
                double _double;
                succes = double.TryParse(value, out _double);
                break;
            case ValueType.Boolean:
                bool _bool;
                succes = bool.TryParse(value, out _bool);
                break;
        }

        if (succes)
            this.value = value;

        //Debug.Log("Value set!");

        return succes;
    }

    public override string ToString() {
        return value;
    }
}

public enum ValueType {
    String,
    Int,
    Double,
    Boolean
}
