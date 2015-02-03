using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(GunTransform))]
public class GunTransformEditor : Editor
{
    GunTransform gunTransform;

    public override void OnInspectorGUI() 
    {
        //feiko zei qwuak qwuak XD 13:20 15-1-2015
        //André vindt ons code kloppers. http://www.keukenboeren.nl/resources/klutser.jpg
        DrawDefaultInspector();

         if(GUILayout.Button("Set current transform")){

         }

    }
}
