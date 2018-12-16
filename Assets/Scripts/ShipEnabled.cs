using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipEnabled : MonoBehaviour {
	[SerializeField] private GameObject _ship1;
	[SerializeField] private GameObject _ship2;
	[SerializeField] private GameObject _ship3;
	[SerializeField] private GameObject _ship4;
	[SerializeField] private GameObject _ship5;
	[SerializeField] private GameObject _ship6;
	[SerializeField] private GameObject _ship7;
	[SerializeField] private GameObject _ship8;
	[SerializeField] private GameObject _ship9;
	[SerializeField] private GameObject _ship10;
	[SerializeField] private GameObject _ship11;
	[SerializeField] private GameObject _ship12;
	[SerializeField] private GameObject _ship13;
	[SerializeField] private GameObject _ship14;
	[SerializeField] private GameObject _player_a_normal;
	[SerializeField] private GameObject _player_a_happy;
	[SerializeField] private GameObject _player_a_shock;
	[SerializeField] private GameObject _player_b_normal;
	[SerializeField] private GameObject _player_b_happy;
	[SerializeField] private GameObject _player_b_shock;

	public int PlayerAScore;
	public int PlayerBScore;
	void Start () {
		PlayerAScore = 3;
		PlayerBScore = 3;
	}
	void Update () {
		if(PlayerAScore < GameManagerInterface.Instance.GetScoreA()){
			_player_a_normal.SetActive  (false);
			_player_a_happy.SetActive  (false);
			_player_a_shock.SetActive  (true);
			_player_b_normal.SetActive  (false);
			_player_b_happy.SetActive  (true);
			_player_b_shock.SetActive  (false);
			PlayerAScore = GameManagerInterface.Instance.GetScoreA();
		} else if (PlayerBScore < GameManagerInterface.Instance.GetScoreB()){
			_player_a_normal.SetActive  (false);
			_player_a_happy.SetActive  (true);
			_player_a_shock.SetActive  (false);
			_player_b_normal.SetActive  (false);
			_player_b_happy.SetActive  (false);
			_player_b_shock.SetActive  (true);
			PlayerBScore = GameManagerInterface.Instance.GetScoreB();
		}
		switch(GameManagerInterface.Instance.GetLastPushedKeyIndex ())
		{
			case 0:
				_ship1.SetActive  (true);
				break;
			case 1:
				_ship2.SetActive  (true);
				break;
			case 2:
				_ship3.SetActive  (true);
				break;
			case 3:
				_ship4.SetActive  (true);
				break;
			case 4:
				_ship5.SetActive  (true);
				break;
			case 5:
				_ship6.SetActive  (true);
				break;
			case 6:
				_ship7.SetActive  (true);
				break;
			case 7:
				_ship8.SetActive  (true);
				break;
			case 8:
				_ship9.SetActive  (true);
				break;
			case 9:
				_ship10.SetActive  (true);
				break;
			case 10:
				_ship11.SetActive  (true);
				break;
			case 11:
				_ship12.SetActive  (true);
				break;
			case 12:
				_ship13.SetActive  (true);
				break;
			case 13:
				_ship14.SetActive  (true);
				break;
		}
		// 例
		// StartCoroutine ("HideShip", _ship10);
	}
	// 一定時間後に船を隠す
	IEnumerator HideShip (GameObject ship) {
		yield return new WaitForSeconds (1.5f);
		ship.SetActive (false);
	}
}
