using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This controls the interactions on the Game scene.
 */
public class GameManager : MonoBehaviour {
	// Represents each space on the game board
	public Image[] squareList;
	// Represents each available space on the game board
	public Image[] availableSquareList;

	public Sprite squareX;
	public Sprite squareO;
	public Image[] winList;

	// Pop-up when the round is over
	public GameObject dialog;
	public Text dialogText;

	public Image playerX;
	public Image playerO;
	public Text playerXScoreText;
	public Text playerOScoreText;

	public Sprite playerXActive;
	public Sprite playerXInactive;
	public Sprite playerOActive;
	public Sprite playerOInactive;

	public AudioSource selectAudio;

	public string playerTurn;
	public int moveCount;
	private int playerXScore;
	private int playerOScore;
	private GameController gameController;
	public int botMove;
	public int specialBotMove;
	public int botHolder;

	/**
	 * Initializes the game and game board.
	 */
	void Awake() {
		SetGameManagerReferenceOnButtons ();
		moveCount = 0;
		playerXScore = 0;
		playerOScore = 0;
		specialBotMove = 999;
		availableSquareList = new Image[squareList.Length];
	}

	void Start () {
		playerTurn = "O";
		gameController = FindObjectOfType<GameController> ();
		UpdatePlayerScores ();
		ChangeSides ();
	}
	
	void Update () {
		
	}

	void SetGameManagerReferenceOnButtons() {
		for (int i = 0; i < squareList.Length; i++) {
			squareList [i].GetComponentInParent<SquareSpace>().SetGameManagerReference (this);
		}
	}

	public string GetPlayerTurn() {
		return playerTurn;
	}

	public Sprite GetPlayerSprite() {
		if (playerTurn == "X") {
			return squareX;
		} else {
			return squareO;
		}
	}

	/**
	 * Checks to see if there is a win condition, and ends the turn.
	 */
	public void EndTurn() {
		moveCount++; 

		if (squareList [0].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [1].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [2].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win on the top row
			winList [0].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (squareList [3].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [4].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [5].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win on the middle row
			winList [1].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (squareList [6].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [7].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [8].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win on the bottom row
			winList [2].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [3].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [6].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win on the first column
			winList [3].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (squareList [1].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [4].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [7].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win on the middle column
			winList [4].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (squareList [2].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [5].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [8].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win on the last column
			winList [5].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [4].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [8].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win diagnally from first column top row to last column bottom row
			winList [7].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (squareList [2].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [4].GetComponentInParent<SquareSpace>().player == playerTurn && squareList [6].GetComponentInParent<SquareSpace>().player == playerTurn) {
			// Win diagnally from last column top row to first column bottom row
			winList [6].enabled = true;
			StartCoroutine(GameOver(playerTurn));
		} else if (moveCount >= 9) {
			// If there are no more spaces, it is a draw
			StartCoroutine(GameOver ("draw"));
		} else {
			ChangeSides ();
		}
	}

	/**
	 * Displays the game results.
	 * @param winningPlayer Player who won the game, or "draw" 
	 */
	IEnumerator GameOver(string winningPlayer) {
		SetBoardInteractable (false);

		yield return new WaitForSeconds(0.5f);

		if (winningPlayer == "draw") {
			SetGameOverText ("It's a draw!");
		} else {
			if (playerTurn == "X") {
				playerXScore++;
			} else {
				playerOScore++;
			}

			SetGameOverText (winningPlayer + " Wins!");
		}
	}

	/**
	 * Changes turns.
	 */
	void ChangeSides() {
		if (playerTurn == "X") {
			playerTurn = "O";
			playerX.sprite = playerXInactive;
			playerO.sprite = playerOActive;

			if (gameController.GetVersus () == "Bot") {
				FreezeAvailableSquares ();
				StartCoroutine(SelectBotSquare ());
			}
		} else {
			playerTurn = "X";
			playerX.sprite = playerXActive;
			playerO.sprite = playerOInactive;

			if (gameController.GetVersus () == "Bot") {
				if (moveCount > 0) {
					UnfreezeAvailableSquares ();
				}
			}
		}
	}

	/**
	 * Sets the text to display in the game over dialog.
	 * @param value String to display
	 */
	void SetGameOverText(string value) {
		dialog.SetActive (true);
		dialogText.text = value + "\n" + "Score: " + playerXScore + "-" + playerOScore;
	}

	/**
	 * Resets the game board for a new round.
	 */
	public void NextRound() {
		moveCount = 0;
		playerTurn = "O";
		dialog.SetActive (false);
		ChangeSides ();
		UpdatePlayerScores ();
		botHolder = new int();
		botMove = new int();
		availableSquareList = new Image[squareList.Length];

		for (int i = 0; i < winList.Length; i++) {
			winList [i].enabled = false;
		}

		for (int i = 0; i < squareList.Length; i++) {
			squareList [i].GetComponentInParent<SquareSpace> ().ResetSpace ();
		}
	}

	/**
	 * Toggles whether the spaces on the game board can be selected.
	 * @param toggle True if spaces can now be interacted with
	 */
	void SetBoardInteractable(bool toggle) {
		for (int i = 0; i < squareList.Length; i++) {
			squareList [i].GetComponentInParent<Button>().interactable = toggle;
		}
	}

	/**
	 * Disables player interactions. Called when it's the bot's turn.
	 */
	void FreezeAvailableSquares() {
		SetBoardInteractable (false);
		availableSquareList = new Image[9-moveCount];
		int availableCount = 0;

		for (int i = 0; i < squareList.Length; i++) {
			if (availableCount < availableSquareList.Length) {
				if (squareList [i].GetComponentInParent<SquareSpace> ().player == "") {	
					availableSquareList [availableCount] = squareList [i];
					availableCount++;
				}
			}
		}
	}

	/**
	 * Enables player interactions. Called when the bot's turn is over.
	 */
	void UnfreezeAvailableSquares() {
		SetBoardInteractable (true);

		for (int i = 0; i < squareList.Length; i++) {
			if (squareList [i].GetComponentInParent<SquareSpace> ().player != "") {
				squareList [i].GetComponentInParent<Button> ().interactable = false;
			}
		}
	}
	
	void UpdatePlayerScores() {
		playerXScoreText.text = playerXScore.ToString();
		playerOScoreText.text = playerOScore.ToString();
	}

	public void ExitGame() {
		gameController.ExitGame ();
	}

	/**
	 * Decides the bot's move.
	 */
	IEnumerator SelectBotSquare() {
		int squareNumber = -1;

		// Check to see if a block is neccessary, or if a win is possible
		if (squareList [0].GetComponentInParent<SquareSpace>().player == "X" && squareList [1].GetComponentInParent<SquareSpace>().player == "X" && squareList [2].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 2;
		} else if (squareList [1].GetComponentInParent<SquareSpace>().player == "X" && squareList [2].GetComponentInParent<SquareSpace>().player == "X" && squareList [0].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 0;
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == "X" && squareList [2].GetComponentInParent<SquareSpace>().player == "X" && squareList [1].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 1;
		} else if (squareList [6].GetComponentInParent<SquareSpace>().player == "X" && squareList [7].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 8;
		} else if (squareList [7].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "X" && squareList [6].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 6;
		} else if (squareList [6].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "X" && squareList [7].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 7;
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 8;
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 4;
		} else if (squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "X" && squareList [0].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 0;
		} else if (squareList [3].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [5].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 5;
		} else if (squareList [3].GetComponentInParent<SquareSpace>().player == "X" && squareList [5].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 4;
		} else if (squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [5].GetComponentInParent<SquareSpace>().player == "X" && squareList [3].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 3;
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == "X" && squareList [3].GetComponentInParent<SquareSpace>().player == "X" && squareList [6].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 6;
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == "X" && squareList [6].GetComponentInParent<SquareSpace>().player == "X" && squareList [3].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 3;
		} else if (squareList [3].GetComponentInParent<SquareSpace>().player == "X" && squareList [6].GetComponentInParent<SquareSpace>().player == "X" && squareList [0].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 0;
		} else if (squareList [1].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [7].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 7;
		} else if (squareList [1].GetComponentInParent<SquareSpace>().player == "X" && squareList [7].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 4;
		} else if (squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [7].GetComponentInParent<SquareSpace>().player == "X" && squareList [1].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 1;
		} else if (squareList [2].GetComponentInParent<SquareSpace>().player == "X" && squareList [5].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 8;
		} else if (squareList [2].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "X" && squareList [5].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 5;
		} else if (squareList [5].GetComponentInParent<SquareSpace>().player == "X" && squareList [8].GetComponentInParent<SquareSpace>().player == "X" && squareList [2].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 2;
		} else if (squareList [2].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [6].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 6;
		} else if (squareList [2].GetComponentInParent<SquareSpace>().player == "X" && squareList [6].GetComponentInParent<SquareSpace>().player == "X" && squareList [4].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 4;
		} else if (squareList [4].GetComponentInParent<SquareSpace>().player == "X" && squareList [6].GetComponentInParent<SquareSpace>().player == "X" && squareList [2].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 2;
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == "O" && squareList [1].GetComponentInParent<SquareSpace>().player == "O" && squareList [2].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 2;
		} else if (squareList [1].GetComponentInParent<SquareSpace>().player == "O" && squareList [2].GetComponentInParent<SquareSpace>().player == "O" && squareList [0].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 0;
		} else if (squareList [0].GetComponentInParent<SquareSpace>().player == "O" && squareList [2].GetComponentInParent<SquareSpace>().player == "O" && squareList [1].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 1;
		} else if (squareList [6].GetComponentInParent<SquareSpace>().player == "O" && squareList [7].GetComponentInParent<SquareSpace>().player == "O" && squareList [8].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 8;
		} else if (squareList [7].GetComponentInParent<SquareSpace>().player == "O" && squareList [8].GetComponentInParent<SquareSpace>().player == "O" && squareList [6].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 6;
		} else if (squareList [6].GetComponentInParent<SquareSpace>().player == "O" && squareList [8].GetComponentInParent<SquareSpace>().player == "O" && squareList [7].GetComponentInParent<SquareSpace>().player == "") {
			botMove = specialBotMove;
			squareNumber = 7;
		} else {
			botMove = Random.Range (0, availableSquareList.Length);
		}

		yield return new WaitForSeconds (0.5f);

		if (botMove != specialBotMove) {
			availableSquareList [botMove].GetComponentInParent<SquareSpace> ().SetSpace ();
		} else if (squareNumber >= 0) {
			squareList [squareNumber].GetComponentInParent<SquareSpace> ().SetSpace ();
			botHolder = squareNumber;
		}
	}

	public void ToggleBackgroundAudio() {
		gameController.ToggleBackgroundAudio ();
	}

	public void PlayClickSound() {
		gameController.PlayClickSoundAudio();
	}

	public void PlaySelectAudio() {
		if (selectAudio.isPlaying) {
			selectAudio.Stop ();
			selectAudio.Play ();
		} else {
			selectAudio.Play ();
		}
	}
}
