using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeValueSetDisplayer : MonoBehaviour {
    public ValueSetEntry valueSetEntry;
    public VerticalLayoutGroup valueSetHolder;

    void Start() {
        foreach (Value value in GameManager.instance.modeSettings.values) {
            ValueSetEntry valueSetEntry = Instantiate(this.valueSetEntry) as ValueSetEntry;

            valueSetEntry.SetValue(value);
            valueSetEntry.transform.SetParent(valueSetHolder.transform);
        }
    }

    public void OnClose() {
        GameManager.instance.UpdateMode();
    }
}
