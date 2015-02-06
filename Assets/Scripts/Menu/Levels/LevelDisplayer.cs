using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelDisplayer : MonoBehaviour {
    public GameObject panel;
    public LevelEntry levelEntry;
    public HorizontalLayoutGroup levelHolder;

    void Start() {
        foreach (LevelData levelData in LevelManager.instance.levels) {
            LevelEntry levelEntry = Instantiate(this.levelEntry) as LevelEntry;

            levelEntry.SetPanel(panel);
            levelEntry.SetLevel(levelData);
            levelEntry.transform.SetParent(levelHolder.transform);
        }
    }

    public void OnClose() {
        //GameManager.instance.CallLevelEvent();
    }
}
