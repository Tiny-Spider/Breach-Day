using UnityEngine;
using System.Collections;

public class TextData : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        MenuManager.instance.LoadTextData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
