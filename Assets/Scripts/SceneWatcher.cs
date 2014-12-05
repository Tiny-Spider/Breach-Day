using UnityEngine;
using System;
using System.Collections;

public class SceneWatcher : MonoBehaviour {
    public static string splashScreen = "Splash Screen";

    private static bool started = false;

    void Awake() {
        if (!started) {
            if (!Application.loadedLevelName.Equals(splashScreen, StringComparison.InvariantCultureIgnoreCase)) {
                Application.LoadLevel(splashScreen);
            }

            started = true;
        }
    }
}
