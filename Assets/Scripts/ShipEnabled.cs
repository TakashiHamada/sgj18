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
	public int KeyIndex;
	void Start () {
		PlayerAScore = 3;
		PlayerBScore = 3;
		KeyIndex = GameManagerInterface.Instance.GetLastPushedKeyIndex ();
		_ship1.GetComponent <Image>().enabled = false;
		_ship2.GetComponent <Image>().enabled = false;
		_ship3.GetComponent <Image>().enabled = false;
		_ship4.GetComponent <Image>().enabled = false;
		_ship5.GetComponent <Image>().enabled = false;
		_ship6.GetComponent <Image>().enabled = false;
		_ship7.GetComponent <Image>().enabled = false;
		_ship8.GetComponent <Image>().enabled = false;
		_ship9.GetComponent <Image>().enabled = false;
		_ship10.GetComponent <Image>().enabled = false;
		_ship11.GetComponent <Image>().enabled = false;
		_ship12.GetComponent <Image>().enabled = false;
		_ship13.GetComponent <Image>().enabled = false;
		_ship14.GetComponent <Image>().enabled = false;
	}
	void Update () {
		if(GameManagerInterface.Instance.GetLastPushedKeyIndex () != KeyIndex){
			switch(GameManagerInterface.Instance.GetLastPushedKeyIndex ())
			{
				case 0:
					_ship1.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship1);
					break;
				case 1:
					_ship2.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship2);
					break;
				case 2:
					_ship3.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship3);
					break;
				case 3:
					_ship4.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship4);
					break;
				case 4:
					_ship5.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship5);
					break;
				case 5:
					_ship6.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship6);
					break;
				case 6:
					_ship7.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship7);
					break;
				case 7:
					_ship8.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship8);
					break;
				case 8:
					_ship9.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship9);
					break;
				case 9:
					_ship10.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship10);
					break;
				case 10:
					_ship11.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship11);
					break;
				case 11:
					_ship12.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship12);
					break;
				case 12:
					_ship13.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship13);
					break;
				case 13:
					_ship14.GetComponent <Image>().enabled = true;
					StartCoroutine ("HideShip", _ship14);
					break;
				default:
					KeyIndex = GameManagerInterface.Instance.GetLastPushedKeyIndex();
					break;
			}
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
