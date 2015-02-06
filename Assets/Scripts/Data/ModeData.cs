using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeData : ScriptableObject {
    public string modeName;
    public string modeDescription;
    public string modeClassName;
    //public int index;

    public ModeSettings modeSettings = new ModeSettings();

    public ModeSettings GetModeSettings() {
        return modeSettings;
    }
 }
