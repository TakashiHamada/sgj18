using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour <GameManager> {
	[SerializeField] private AudioClip [] _clips;
	[SerializeField] private AudioClip [] _clips_key;
	private readonly int MAX_TREASURE = 3; // 配置数
	private GameState _state = GameState.Title;
	private bool [] _flags_a = new bool [7];
	private bool [] _flags_b = new bool [7];
	private bool _last_trun_a_selected = false;
	private int _result = -1; //
	private int _sored_key_idx = -1;
	public int GetStoredKeyIndex () {
		if (_sored_key_idx > 100) {
			return _sored_key_idx - 93;
		} else {
			return _sored_key_idx;
		}
	}
	private AudioSource _se;
	private Dictionary <string, string> _neighborhood_list  = new Dictionary<string, string> () {
        {"R", "DFT"},
        {"T", "RFG"},
		{"D", "RFC"},
		{"F", "RTDGCV"},
		{"G", "TFV"},
		{"C", "DFV"},
		{"V", "CFG"},

        {"U", "HJI"},
		{"I", "UJK"},
        {"H", "UJN"},
		{"J", "UHNMKI"},
		{"K", "IJM"},
		{"N", "HJM"},
		{"M", "NJK"},
    };
	private bool IsNear (string key) {
		var neighborhoods = GetNeighborhoods (key);
		bool flag = false;
		for (int idx = 0; idx < neighborhoods.Count; idx++) {
			int key_idx = GetKeyIndex (neighborhoods [idx]);
			if (key_idx < 100) {
				if (_flags_a [key_idx]) flag = true;
			} else {
				if (_flags_b [key_idx - 100]) flag = true;
			}
		}
		return flag;
	}
	private List <string> GetNeighborhoods (string key) {
		var answer = _neighborhood_list [key];
		var neighborhoods = new List <string> ();
		for (int idx = 0; idx < answer.Length; idx++) {
			neighborhoods.Add (answer [idx].ToString ());
		}
		return neighborhoods;
	}
	private bool IsInRange (string key) {
		if (key == "") return false;
		string valid_keys = "RTDFGCVUIHJKNM";
		for (int idx = 0; idx < valid_keys.Length; idx++) {
			if (valid_keys [idx] == key [0]) return true;
		}
		PlaySe (0);
		return false;
	}
	private void PlaySe (int se_idx) {
		_se.clip = _clips [se_idx];
		_se.Play ();
	}
	private void PlaySeKey (int se_idx) {
		_se.clip = _clips_key [se_idx];
		_se.Play ();
	}
	private void PutTreasure (string key) {
		int idx = GetKeyIndex (key);
		if (idx == -1) {
			Debug.Log ("ERROR");
		}
		if (idx < 100) {
			// すでにUP?
			if (_flags_a [idx]) {
				// se
				Debug.Log ("TAKE");
				PlaySe (1);
				_flags_a [idx] = false;
			} else if (GetFlagUpCount (0) < MAX_TREASURE) {
				// se
				PlaySe (1);
				Debug.Log ("PUT");
				_flags_a [idx] = true;
			} else {
				// se
				Debug.Log ("False:Max");
				PlaySe (0);
			}
		} else {
			// すでにUP?
			if (_flags_b [idx - 100]) {
				// se
				PlaySe (1);
				Debug.Log ("TAKE");
				_flags_b [idx - 100] = false;
			} else  if (GetFlagUpCount (1) < MAX_TREASURE) {
				// se
				PlaySe (1);
				Debug.Log ("PUT");
				_flags_b [idx - 100] = true;
			} else {
				// se
				Debug.Log ("False:Max");
				PlaySe (0);
			}
		}
	}
	private bool Hit (string key) {
		int idx = GetKeyIndex (key);
		if (idx == -1) {
			Debug.Log ("ERROR");
			return false;
		}
		_sored_key_idx = idx;
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
		if (key == "R") return 0;
		if (key == "T") return 1;
		if (key == "D") return 2;
		if (key == "F") return 3;
		if (key == "G") return 4;
		if (key == "C") return 5;
		if (key == "V") return 6;
		
		if (key == "U") return 100;
		if (key == "I") return 101;
		if (key == "H") return 102;
		if (key == "J") return 103;
		if (key == "K") return 104;
		if (key == "N") return 105;
		if (key == "M") return 106;
		return -1;
	}
	public GameState GetGameState () {
		return _state;
	}
	void Awake () {
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
	}
	void Start () {
		Debug.Log (_state);
		_se = GetComponent <AudioSource> ();
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
				PlaySe (2);
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
				Debug.Log ("Move to GameStartWait");
				PlaySe (2);

				StartCoroutine ("GameStartWait");
			}
		} else
		// -------------------------------------------
		// GameTurnA
		// -------------------------------------------
		if (_state == GameState.GameTurnA) {
			_last_trun_a_selected = true;
			// clear
			if (IsGameEnd ()) {
				_state = GameState.GameEnd;
				Debug.Log ("Move to End");
				PlaySe (2);
			}
			// key
			if (IsInRange (DownKeyCheck ())) {

				int idx = GetKeyIndex (DownKeyCheck ());
				if (idx > 100) idx -= 93;
				PlaySeKey (idx);

				if (Hit (DownKeyCheck ())) {
					// 成功
					_result = 1;
				} else {
					// 失敗
					// 近い判定
					if (IsNear (DownKeyCheck ())) {
						_result = 2;
					} else {
						_result = 0;
					}
				}
				Debug.Log ("Move to GameReactionWait");
				StartCoroutine ("GameReactionWait");
			}
		} else
		// -------------------------------------------
		// GameTurnB
		// -------------------------------------------
		if (_state == GameState.GameTurnB) {
			_last_trun_a_selected = false;
			// clear
			if (IsGameEnd ()) {
				_state = GameState.GameEnd;
				Debug.Log ("Move to End");
			}
			// key
			if (IsInRange (DownKeyCheck ())) {

				int idx = GetKeyIndex (DownKeyCheck ());
				if (idx > 100) idx -= 93;
				PlaySeKey (idx);
				
				if (Hit (DownKeyCheck ())) {
					// 成功
					_result = 1;
				} else {
					// 失敗
					// 近い判定
					if (IsNear (DownKeyCheck ())) {
						_result = 2;
					} else {
						_result = 0;
					}
				}
				Debug.Log ("Move to GameReactionWait");
				StartCoroutine ("GameReactionWait");
			}
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
		return GetFlagUpCount (0) == MAX_TREASURE && GetFlagUpCount (1) == MAX_TREASURE;
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
	IEnumerator GameStartWait () {
		_state = GameState.GameStartWait;
		yield return new WaitForSeconds (1f);
		Debug.Log ("Move to GameTurnA");
		PlaySe (2);
		_state = GameState.GameTurnA;
	}
	IEnumerator GameReactionWait () {
		_state = GameState.GameReactionWait;
		yield return new WaitForSeconds (3f);
		PlaySe (2);
		// result
		if (_result == 0) {
			// 失敗
			PlaySe (0);
			Debug.Log ("NG");
		} else
		if (_result == 1) {
			// 成功
			PlaySe (4);
			Debug.Log ("GOOD");
		} else
		if (_result == 2){
			// 近く
			PlaySe (3);
			Debug.Log ("NEAR");
		}
		if (_last_trun_a_selected) {
			Debug.Log ("Move to GameTurnB");
			_state = GameState.GameTurnB;
		} else {
			Debug.Log ("Move to GameTurnA");
			_state = GameState.GameTurnA;
		}
	}
}
