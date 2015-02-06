using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeDisplayer : MonoBehaviour {
    public GameObject panel;
    public ModeEntry modeEntry;
    public VerticalLayoutGroup modeHolder;

    void Start() {
        foreach (ModeData modeData in ModeManager.instance.modes) {
            ModeEntry modeEntry = Instantiate(this.modeEntry) as ModeEntry;

            modeEntry.SetPanel(panel);
            modeEntry.SetMode(modeData);
            modeEntry.transform.SetParent(modeHolder.transform);
        }
    }

    public void OnClose() {
       // GameManager.instance.CallModeEvent();
    }
}
