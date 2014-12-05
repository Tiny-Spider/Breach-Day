using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour {

    // Lock on start
    void Start() {
        LockCursor(true);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0.0F) {
            LockCursor(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            LockCursor(!Screen.lockCursor);
        }
    }

    public void LockCursor(bool lockCursor) {
        Screen.lockCursor = lockCursor;
    }
}
