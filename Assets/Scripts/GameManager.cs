using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
	private GameState _state = GameState.Title;
	private bool [] _flags_a = new bool [3];
	private bool [] _flags_b = new bool [3];
	void Start () {
		Clear ();
		TempSet ();

		ChangeKeyState ("A", false);
		ChangeKeyState ("S", false);
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
	private void ChangeKeyState (string key, bool flag) {
		if (key == "A") _flags_a [0] = flag;
		if (key == "S") _flags_a [1] = flag;
		if (key == "D") _flags_a [2] = flag;

		if (key == "Q") _flags_b [0] = flag;
		if (key == "W") _flags_b [1] = flag;
		if (key == "E") _flags_b [2] = flag;
	}

	
	// Update is called once per frame
	void Update () {
		if (_state == GameState.Title) {
			if (Input.GetKey (KeyCode.Space)) {
				_state = GameState.Preparing;
			}
		} else
		if (_state == GameState.Preparing) {
			// if ()

		} else
		if (_state == GameState.Title) {

		} else
		if (_state == GameState.Title) {

		} else
		if (_state == GameState.Title) {

		}

		
		
		Debug.Log (GetFlagUpCount (0));



		DownKeyCheck ();

		if (_state == GameState.GameTurnA) {
			if (IsGameEnd ()) _state = GameState.GameEnd;
		}
	}
	private bool IsGameEnd () {
		return GetFlagUpCount (0) == 0 || GetFlagUpCount (1) == 0;
	}
	public int GetFlagUpCount (int player) {
		var flags = player == 0 ? _flags_a : _flags_b;
		int count = 0;
		for (int idx = 0; idx < flags.Length; idx ++) {
			if (flags [idx]) count ++;
		}
		return count;
	}
	private string DownKeyCheck(){
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
