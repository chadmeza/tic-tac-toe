using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This Game Controller persists across all scenes.
 * It is used to navigate through the game's scenes,
 * and handle game-wide interactions.
 */
public class GameController : MonoBehaviour {
	// Background music
	public AudioSource backgroundAudio;
	// Click sound effect
	public AudioSource clickSoundAudio;

	private string versus;
	private bool playAudio;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		playAudio = true;
		SceneManager.LoadScene ("Splash");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame() {
		SceneManager.LoadScene ("Menu");
	}

	public void PlayGame(string opponent) {
		versus = opponent;
		SceneManager.LoadScene ("Game");
	}

	public void ExitGame() {
		SceneManager.LoadScene ("Splash");
	}

	public string GetVersus() {
		return versus;
	}

	/**
	 * Toggles the background music when the
	 * audio button in the lower right corner
	 * is clicked.
	 */
	public void ToggleBackgroundAudio() {
		if (playAudio == true && backgroundAudio.isPlaying) {
			playAudio = false;
			backgroundAudio.Stop();
		} else if (playAudio == false && !backgroundAudio.isPlaying) {
			playAudio = true;
			backgroundAudio.Play();
		}
	}

	public void PlayClickSoundAudio() {
		if (clickSoundAudio.isPlaying) {
			clickSoundAudio.Stop ();
			clickSoundAudio.Play ();
		} else {
			clickSoundAudio.Play ();
		}
	}
}
