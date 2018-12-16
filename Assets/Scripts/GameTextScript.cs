using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTextScript : MonoBehaviour {

	Text myText;
	// Use this for initialization
	void Start () {
		myText = GetComponentInChildren<Text>();
	}

	// Update is called once per frame
	void Update () {
		if (GameManagerInterface.Instance.GetGameState().ToString() == "GameTurnA"){
			myText.text = "1Playerのターン";
		}else if (GameManagerInterface.Instance.GetGameState().ToString() == "GameTurnB"){
			myText.text = "2Playerのターン";
		}
	}
}
