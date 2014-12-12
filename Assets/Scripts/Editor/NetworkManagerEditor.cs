using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(NetworkManager))]
public class NetworkManagerEditor : Editor {

    public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Connected Players:");

        foreach(KeyValuePair<NetworkPlayer, PlayerInfo> player in NetworkManager.instance.connectedPlayers) {
            EditorGUILayout.LabelField("Player ID: " + player.Key, player.Value.name);
        }

        EditorGUILayout.EndVertical();
	}
}
