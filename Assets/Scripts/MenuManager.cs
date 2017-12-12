using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This controls the interactions on the Menu scene.
 */
public class MenuManager : MonoBehaviour {
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayVsPlayer() {
		gameController.PlayGame ("Player");
	}

	public void PlayVsBot() {
		gameController.PlayGame ("Bot");
	}

	public void ToggleBackgroundAudio() {
		gameController.ToggleBackgroundAudio ();
	}

	public void PlayClickSound() {
		gameController.PlayClickSoundAudio();
	}
}
