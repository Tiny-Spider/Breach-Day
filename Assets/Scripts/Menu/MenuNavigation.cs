using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public class MenuNavigation : MonoBehaviour {
    private List<GameObject> panels = new List<GameObject>();

    void Start() {
        InitializePanels();
    }

    void InitializePanels() {
        foreach (Transform go in gameObject.transform) {
            if (go.gameObject.name.Contains("Panel")) {
                panels.Add(go.gameObject);
                go.gameObject.SetActive(false);
            }
        }

        panels[0].SetActive(true);
    }

    public void SwitchPanel(GameObject panel) {
        foreach (GameObject gameObject in panels) {
            gameObject.SetActive(false);
        }

        panel.SetActive(true);
    }

    public void OpenPanel(GameObject panel) {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel) {
        panel.SetActive(false);
    }
}
