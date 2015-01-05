using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(NetworkManager))]
public class NetworkManagerEditor : Editor {

    public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();


        if (NetworkManager.instance != null && NetworkManager.instance.connectedPlayers != null) {
            EditorGUILayout.LabelField("Connected Players:");

             EditorGUILayout.BeginVertical();

            foreach (KeyValuePair<NetworkPlayer, PlayerInfo> player in NetworkManager.instance.connectedPlayers) {
                EditorGUILayout.LabelField("Player:       ID: " + player.Key, "Name: " + player.Value.name);
            }

            EditorGUILayout.EndVertical();
        }
        else {
            EditorGUILayout.LabelField("Game is not running!");
        }

	}
}
