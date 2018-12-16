using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAnimation : MonoBehaviour {
	private Vector3 _init_pos;
	void Start () {
		_init_pos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = _init_pos + Vector3.up * (Mathf.Sin (Time.time * 5f) * 15f);
	}
}
