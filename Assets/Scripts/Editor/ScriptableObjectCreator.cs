using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScriptableObjectCreator {
    [MenuItem("Assets/Create/Level")]
    public static void CreateLevel() {
        LevelData asset = ScriptableObject.CreateInstance<LevelData>();

        AssetDatabase.CreateAsset(asset, "Assets/New Level.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Mode")]
    public static void CreateMode() {
        ModeData asset = ScriptableObject.CreateInstance<ModeData>();

        AssetDatabase.CreateAsset(asset, "Assets/New Mode.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
