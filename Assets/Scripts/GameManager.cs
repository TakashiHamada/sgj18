﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour <GameManager> {
	private readonly int MAX_TREASURE = 3; // 配置数
	[SerializeField] private AudioClip [] _clips;
	[SerializeField] private AudioClip [] _clips_key;
	private GameState _state = GameState.Title;
	private bool [] _flags_a = new bool [7];
	private bool [] _flags_b = new bool [7];
	private bool _last_trun_a_selected = false;
	private AudioSource _se;
	private int _result = -1;
	private int _sored_key_idx = -1;
	void Awake () {
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
	}
	void Start () {
		Debug.Log (_state);
		_se = GetComponent <AudioSource> ();
		Clear ();
	}
	void Update () {
		// -------------------------------------------
		// Title
		// -------------------------------------------
		if (_state == GameState.Title) {
			if (Input.GetKey (KeyCode.Space)) {
				_state = GameState.Preparing;
				Debug.Log ("Move to Preparing");
				PlaySe (17);
			}
		} else
		// -------------------------------------------
		// Preparing
		// -------------------------------------------
		if (_state == GameState.Preparing) {
			if (DownKeyCheck () == "") return;
			// key
			if (IsInRange (DownKeyCheck (), 0)) {
				PutTreasure (DownKeyCheck ());
			} else {
				PlaySe (0);
			}
			if (IsCompletedPreparation ()) {
				Debug.Log ("Move to GameStartWait");
				PlaySe (16);
				StartCoroutine ("GameStartWait");
			}
		} else
		// -------------------------------------------
		// GameTurnA
		// -------------------------------------------
		if (_state == GameState.GameTurnA) {
			_last_trun_a_selected = true;
			GameTurnUpdate ();
		} else
		// -------------------------------------------
		// GameTurnB
		// -------------------------------------------
		if (_state == GameState.GameTurnB) {
			_last_trun_a_selected = false;
			GameTurnUpdate ();
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
	// -------------------------------------------
	// GameStartWait
	// -------------------------------------------
	IEnumerator GameStartWait () {
		_state = GameState.GameStartWait;
		yield return new WaitForSeconds (3f);
		Debug.Log ("Move to GameTurnA");
		PlaySe (12);
		_state = GameState.GameTurnA;
	}
	// -------------------------------------------
	// GameReactionWait
	// -------------------------------------------
	IEnumerator GameReactionWait () {
		_state = GameState.GameReactionWait;
		yield return new WaitForSeconds (2f);
		PlaySe (2);
		// result
		if (_result == 0) {
			// 失敗
			PlaySe (0);
			Debug.Log ("NG");
			yield return new WaitForSeconds (1f);
		} else
		if (_result == 1) {
			// 成功
			PlaySe (7);
			Debug.Log ("GOOD");
			yield return new WaitForSeconds (1f);
		} else
		if (_result == 2){
			// 近く
			PlaySe (5);
			Debug.Log ("NEAR");
			yield return new WaitForSeconds (1f);
		}
		if (_last_trun_a_selected) {
			Debug.Log ("Move to GameTurnB");
			_state = GameState.GameTurnB;
			PlaySe (14);
		} else {
			Debug.Log ("Move to GameTurnA");
			_state = GameState.GameTurnA;
			PlaySe (12);
		}
	}
	// -------------------------------------------
	// GameTurn の主処理
	// -------------------------------------------
	private void GameTurnUpdate () {
		if (IsGameEnd ()) {
			_state = GameState.GameEnd;
			Debug.Log ("Move to End");
			StartCoroutine ("PlaySeJudgement");
		}
		if (DownKeyCheck () == "") return;
		// 何か押されたが範囲外？
		if (IsInRange (DownKeyCheck (), 0) == false) {
			Debug.Log ("OUT OF RANGE");
			PlaySe (0);
			return;
		}
		// 確認
		if (IsInRange (DownKeyCheck (), _last_trun_a_selected ? 1 : 2)) {
			// ヒットじゃだめ　結果のみ
			if (Check (DownKeyCheck ())) {
				Debug.Log ("Exsited");
				PlaySe (1);
			}
		} else
		// 攻撃
		if (IsInRange (DownKeyCheck (), _last_trun_a_selected ? 2 : 1)) {
			PrepareToAttack ();
			if (Hit (DownKeyCheck ())) {
				_result = 1; // 成功
			} else {
				if (IsNear (DownKeyCheck ())) {
					_result = 2; // 近い
				} else {
					_result = 0; // 失敗
				}
			}
			Debug.Log ("Move to GameReactionWait");
			StartCoroutine ("GameReactionWait");
		}
	}
	IEnumerator PlaySeJudgement () {
		PlaySe (2);
		yield return new WaitForSeconds (1f);
		// 1の勝ち
		if (GetFlagUpCount (1) == 0) {
			PlaySe (13);
		} else
		// 2の勝ち
		if (GetFlagUpCount (0) == 0) {
			PlaySe (15);
		}
	}
	private void PrepareToAttack () {
		int idx = GetKeyIndex (DownKeyCheck ());
		if (idx > 99) idx -= 93;
		PlaySeKey (idx);
		_sored_key_idx = idx; // 画像部連絡用
	}
	public int GetStoredKeyIndex () {
		return _sored_key_idx > 99 ? _sored_key_idx - 93 : _sored_key_idx;
	}
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
	private bool IsInRange (string key, int mask_type) {
		if (key == "") return false;
		string valid_keys = mask_type == 0 ? "RTDFGCVUIHJKNM" :
							mask_type == 1 ? "RTDFGCV" :
							mask_type == 2 ? "UIHJKNM" : "";
		for (int idx = 0; idx < valid_keys.Length; idx++) {
			if (valid_keys [idx] == key [0]) return true;
		}
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
		int idx  = ParseIndex (GetKeyIndex (key));
		var flag = GetFlag    (GetKeyIndex (key));
		// すでにUP?
		if (flag [idx]) {
			Debug.Log ("TAKE");
			PlaySe (10);
			flag [idx] = false;
		} else if (GetFlagUpCount (GetFlagInt (GetKeyIndex (key))) < MAX_TREASURE) {
			PlaySe (9);
			Debug.Log ("PUT");
			flag [idx] = true;
		} else {
			Debug.Log ("False:Max");
			PlaySe (4);
		}
	}
	private bool Hit (string key) {
		int idx  = ParseIndex (GetKeyIndex (key));
		var flag = GetFlag    (GetKeyIndex (key));
		if (flag [idx]) {
			// 成功
			flag [idx] = false;
			return true;
		}
		return false;
	}
	private bool Check (string key) {
		int idx  = ParseIndex (GetKeyIndex (key));
		var flag = GetFlag    (GetKeyIndex (key));
		if (flag [idx]) {
			// 成功
			return true;
		}
		return false;
	}
	private int ParseIndex (int raw_idx) {
		return raw_idx < 100 ? raw_idx : raw_idx - 100;
	}
	private bool [] GetFlag (int raw_idx) {
		return raw_idx < 100 ? _flags_a : _flags_b;
	}
	private int GetFlagInt (int raw_idx) {
		return raw_idx < 100 ? 0 : 1;
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
	public void Clear () {
		for (int idx = 0; idx < _flags_a.Length; idx++) {
			_flags_a [idx] = false;
		}
		for (int idx = 0; idx < _flags_b.Length; idx++) {
			_flags_a [idx] = false;
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
}
