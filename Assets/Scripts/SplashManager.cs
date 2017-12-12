using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This controls the interactions on the Splash scene.
 */
public class SplashManager : MonoBehaviour {
	// Button to toggle the background music
	public GameObject audioButton;
	// Button to start the game
	public GameObject playButton;

	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame() {
		gameController.StartGame ();
	}

	public void ToggleBackgroundAudio() {
		gameController.ToggleBackgroundAudio ();
	}

	public void PlayClickSound() {
		gameController.PlayClickSoundAudio();
	}
}
