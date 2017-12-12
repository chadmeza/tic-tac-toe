using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used as a component for the background audio and the
 * click sound effect. It persists them across the different
 * scenes.
 */
public class PersistentAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
