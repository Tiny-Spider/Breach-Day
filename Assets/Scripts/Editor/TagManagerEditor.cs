using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TagManager))]
public class TagManagerEditor : Editor {

    public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();
        EditorGUILayout.BeginVertical();

        TagManager.wallPartTag = EditorGUILayout.TextField("Wall Tag: ", TagManager.wallPartTag);

        EditorGUILayout.EndVertical();
	}
}
