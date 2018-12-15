using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour <GameManager> {
	private GameState _state = GameState.Title;
	private bool [] _flags_a = new bool [3];
	private bool [] _flags_b = new bool [3];
	private Dictionary <string, string> _neighborhood_list  = new Dictionary<string, string> () {
        {"A", "QWS"},
        {"S", "AWED"},
		{"D", "SERF"},

        {"Q", "AW"},
		{"W", "QASE"},
        {"E", "WSDR"},
    };
	private bool IsInRange (string key) {
		if (key == "") return false;
		string valid_keys = "QWEASD";
		for (int idx = 0; idx < valid_keys.Length; idx++) {
			if (valid_keys [idx] == key [0]) return true;
		}
		return false;
	}
	private void ChangeKeyState (string key, bool flag) {
		if (key == "A") _flags_a [0] = flag;
		if (key == "S") _flags_a [1] = flag;
		if (key == "D") _flags_a [2] = flag;
		
		if (key == "Q") _flags_b [0] = flag;
		if (key == "W") _flags_b [1] = flag;
		if (key == "E") _flags_b [2] = flag;
	}
	private void PutTreasure (string key) {
		int idx = GetKeyIndex (key);
		// Debug.Log (idx);
		if (idx < 100) {
			// すでにUP?
			if (_flags_a [idx]) {
				// se
				_flags_a [idx] = false;
			} else if (GetFlagUpCount (0) < 2) {
				// se
				_flags_a [idx] = true;
			} else {
				// se
				Debug.Log ("MAx");
			}
		} else {
			// すでにUP?
			if (_flags_b [idx - 100]) {
				// se
				_flags_b [idx - 100] = false;
			} else  if (GetFlagUpCount (1) < 2) {
				// se
				_flags_b [idx - 100] = true;
			} else {
				// se
				Debug.Log ("MAx");
			}
		}
	}
	private bool Hit (string key) {
		int idx = GetKeyIndex (key);
		if (idx < 100) {
			if (_flags_a [idx]) {
				// 成功
				_flags_a [idx] = false;
				return true;
			} else {
				// 何もない
			}
		} else {
			// すでにUP?
			if (_flags_b [idx - 100]) {
				// 成功
				_flags_b [idx - 100] = false;
				return true;
			} else {
				// 何もない
			}
		}
		return false;
	}
	private int GetKeyIndex (string key) {
		if (key == "A") return 0;
		if (key == "S") return 1;
		if (key == "D") return 2;
		
		if (key == "Q") return 100;
		if (key == "W") return 101;
		if (key == "E") return 102;
		return -1;
	}
	public List <string> GetNeighborhoods (string key) {
		var answer = _neighborhood_list [key];
		var neighborhoods = new List <string> ();
		for (int idx = 0; idx < answer.Length; idx++) {
			neighborhoods.Add (answer [idx].ToString ());
		}
		return neighborhoods;
	}
	public GameState GetGameState () {
		return _state;
	}
	void Awake () {
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
	}
	void Start () {
		// var hoge = GetNeighborhoods ();
		// for (int idx = 0; idx < hoge.Count; idx++) {
		// 	Debug.Log (hoge [idx]);
		// }

		Debug.Log (_state);
		Clear ();
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
			if (IsInRange (DownKeyCheck ())) {
				PutTreasure (DownKeyCheck ());
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
			if (IsInRange (DownKeyCheck ())) {
				if (Hit (DownKeyCheck ())) {
					// 成功
				}
				_state = GameState.GameTurnB;
				Debug.Log ("Move to GameTurnB");
				Debug.Log (DownKeyCheck ());
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
			if (IsInRange (DownKeyCheck ())) {
				if (Hit (DownKeyCheck ())) {
					// 成功
				}
				_state = GameState.GameTurnA;
				Debug.Log ("Move to GameTurnA");
				Debug.Log (DownKeyCheck ());
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
		return GetFlagUpCount (0) == 2 && GetFlagUpCount (1) == 2;
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
