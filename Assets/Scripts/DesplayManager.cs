using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesplayManager : MonoBehaviour {
	[SerializeField] private GameObject _title_root;
	[SerializeField] private GameObject _game_root;
	[SerializeField] private GameObject _result_root;
	void Start () {
		_title_root.SetActive  (true);
		_game_root.SetActive   (false);
		_result_root.SetActive (false);
	}
	void Update () {
		if (GameManagerInterface.Instance.GetGameState().ToString() == "Preparing"){
			_title_root.SetActive  (false);
			_game_root.SetActive   (true);
		} else if (GameManagerInterface.Instance.GetGameState().ToString() == "GameEnd"){
			_game_root.SetActive   (false);
			_result_root.SetActive (true);
		} else if (GameManagerInterface.Instance.GetGameState().ToString() == "Title"){
			_title_root.SetActive  (true);
			_game_root.SetActive   (false);
		}
	}
}
