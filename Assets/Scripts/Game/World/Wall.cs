using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
    public GameObject disabledWall;
    public GameObject enabledWall;
    public GameObject a;

    public void SetWallActive(bool enable) {
        disabledWall.SetActive(!enable);
        enabledWall.SetActive(enable);
    }
}
