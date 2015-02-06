using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeValueDisplayer : MonoBehaviour {
    public Text text;
    public ValueEntry valueEntry;
    public VerticalLayoutGroup valueHolder;

    void Start() {
        GameManager.instance.OnModeUpdate += OnModeUpdate;
        //GameManager.instance.CallModeEvent();
    }

    void OnDestroy() {
        GameManager gameManager = GameManager.instance;

        if (gameManager) {
            gameManager.OnModeUpdate -= OnModeUpdate;
        }
    }

    public void OnModeUpdate(ModeData modeData, ModeSettings modeSettings) {
        text.text = modeData.modeName;

        foreach (Transform child in valueHolder.transform) {
            Destroy(child.gameObject);
        }

        foreach (Value value in modeSettings.values) {
            ValueEntry valueEntry = Instantiate(this.valueEntry) as ValueEntry;

            valueEntry.SetValue(value);
            valueEntry.transform.SetParent(valueHolder.transform);
        }
    }
}
