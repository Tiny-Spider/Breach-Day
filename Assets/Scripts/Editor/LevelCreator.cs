using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevelScriptableObject {
    [MenuItem("Assets/Create/Level")]
    public static void CreateLevel() {
        Level asset = ScriptableObject.CreateInstance<Level>();

        AssetDatabase.CreateAsset(asset, "Assets/New Level.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
