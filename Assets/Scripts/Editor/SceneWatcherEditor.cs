using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SceneWatcher))]
public class SceneWatcherEditor : Editor {

    public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls(); 
        EditorGUILayout.BeginVertical();

        SceneWatcher.splashScreen = EditorGUILayout.TextField("Splash Screen Scene: ", SceneWatcher.splashScreen);

        EditorGUILayout.EndVertical();
    }
}
