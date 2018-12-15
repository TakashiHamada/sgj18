using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private GameState _state = GameState.Title;
	private bool [] _flags_a = new bool [3];
	private bool [] _flags_b = new bool [3];
	void Start () {
		Debug.Log (_state);
		Clear ();
		// TempSet ();

		// ChangeKeyState ("A", false);
		// ChangeKeyState ("S", false);
	}
	public void Clear () {
		for (int idx = 0; idx < _flags_a.Length; idx++) {
			_flags_a [idx] = false;
		}
		for (int idx = 0; idx < _flags_b.Length; idx++) {
			_flags_a [idx] = false;
		}
	}
	public void TempSet () {
		_flags_a [0] = true;
		_flags_a [1] = true;

		_flags_b [0] = true;
		_flags_b [1] = true;

	}
	private bool ChangeKeyState (string key, bool flag) {
		if (key == "A") {_flags_a [0] = flag; return true;}
		if (key == "S") {_flags_a [1] = flag; return true;}
		if (key == "D") {_flags_a [2] = flag; return true;}

		if (key == "Q") {_flags_b [0] = flag; return true;}
		if (key == "W") {_flags_b [1] = flag; return true;}
		if (key == "E") {_flags_b [2] = flag; return true;}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		// -------------------------------------------
		// Title
		// -------------------------------------------
		if (_state == GameState.Title) {
			if (Input.GetKey (KeyCode.Space)) {
				_state = GameState.Preparing;
				Debug.Log ("Move to Preparing");
			}
		} else
		// -------------------------------------------
		// Preparing
		// -------------------------------------------
		if (_state == GameState.Preparing) {
			// key
			if (Input.anyKeyDown) {
				ChangeKeyState (DownKeyCheck (), true);
				Debug.Log (DownKeyCheck ());
			}
			if (IsCompletedPreparation ()) {
				_state = GameState.GameStartWait;
				Debug.Log ("Move to GameStartWait");
			}
		} else
		// -------------------------------------------
		// GameStartWait
		// -------------------------------------------
		if (_state == GameState.GameStartWait) {
			_state = GameState.GameTurnA;
			Debug.Log ("Move to GameTurnA");
		} else
		// -------------------------------------------
		// GameTurnA
		// -------------------------------------------
		if (_state == GameState.GameTurnA) {
			// clear
			if (IsGameEnd ()) {
				_state = GameState.GameEnd;
				Debug.Log ("Move to End");
			}
			// key
			if (Input.anyKeyDown) {
				if (ChangeKeyState (DownKeyCheck (), false)) {
					_state = GameState.GameTurnB;
					Debug.Log ("Move to GameTurnB");
					Debug.Log (DownKeyCheck ());
				}
			}
		} else
		// -------------------------------------------
		// GameTurnB
		// -------------------------------------------
		if (_state == GameState.GameTurnB) {
			// clear
			if (IsGameEnd ()) {
				_state = GameState.GameEnd;
				Debug.Log ("Move to End");
			}
			// key
			if (Input.anyKeyDown) {
				if (ChangeKeyState (DownKeyCheck (), false)) {
					_state = GameState.GameTurnA;
					Debug.Log ("Move to GameTurnA");
					Debug.Log (DownKeyCheck ());
				}
			}
		} else
		// -------------------------------------------
		// GameReactionWait
		// -------------------------------------------
		if (_state == GameState.GameReactionWait) {
		} else
		// -------------------------------------------
		// GameEnd
		// -------------------------------------------
		if (_state == GameState.GameEnd) {
			if (Input.GetKey (KeyCode.Space)) {
				SceneManager.LoadScene (0);
			}
		}
	}
	private bool IsGameEnd () {
		return GetFlagUpCount (0) == 0 || GetFlagUpCount (1) == 0;
	}
	private bool IsCompletedPreparation () {
		return GetFlagUpCount (0) == 3 && GetFlagUpCount (1) == 3;
	}
	public int GetFlagUpCount (int player) {
		var flags = player == 0 ? _flags_a : _flags_b;
		int count = 0;
		for (int idx = 0; idx < flags.Length; idx ++) {
			if (flags [idx]) count ++;
		}
		return count;
	}
	private string DownKeyCheck (){
        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown (code)) {
                  return code.ToString ();
                }
            }
        }
		return "";
	}
}
