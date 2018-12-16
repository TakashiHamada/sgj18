using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour {
	[SerializeField] private GameObject _1pchar;
	[SerializeField] private GameObject _1plogo;
	[SerializeField] private GameObject _2pchar;
	[SerializeField] private GameObject _2plogo;
	// Use this for initialization
	void Start () {
		if (GameManagerInterface.Instance.GetScoreA() == 0){
			_2pchar.SetActive  (true);
			_2plogo.SetActive  (true);
		} else {
			_1pchar.SetActive  (true);
			_1plogo.SetActive  (true);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
