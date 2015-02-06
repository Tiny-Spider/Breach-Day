using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeEntry : MonoBehaviour {
    public Text name;
    public Text description;

    private GameObject panel;
    private ModeData modeData;

    public void SetPanel(GameObject panel) {
        this.panel = panel;
    }

    public void SetMode(ModeData modeData) {
        this.modeData = modeData;

        name.text = modeData.modeName;
        description.text = modeData.modeDescription;
    }

    public void OnSelect() {
        GameManager gameManager = GameManager.instance;

        gameManager.SetMode(modeData.modeName);

        panel.SetActive(false);
    }
}
