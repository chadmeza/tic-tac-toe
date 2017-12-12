using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This represents and controls each space on the game board.
 */
public class SquareSpace : MonoBehaviour {
	// Toggles clickable depending on if the space is available
	public Button button;
	// Holds the X or O for the space
	public Image image;
	// GameManager instance
	public GameManager gameManager;
	// X or O
	public string player;

	// Use this for initialization
	void Start () {
		player = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Assigns the space to a player, and ends the turn.
	 */
	public void SetSpace() {
		// Checks to make sure the space has not been selected yet
		if (player == "") {
			image.sprite = gameManager.GetPlayerSprite ();
			image.color = new Color32 (255, 255, 255, 255);
			player = gameManager.GetPlayerTurn ();
			button.interactable = false;
			gameManager.EndTurn ();
		}
	}

	/**
	 * Resets the space to empty and clickable.
	 */
	public void ResetSpace() {
		image.sprite = null;
		image.color = new Color32 (255, 255, 255, 0);
		player = "";
		button.interactable = true;
	}

	public void SetGameManagerReference(GameManager manager) {
		gameManager = manager;
	}

}
