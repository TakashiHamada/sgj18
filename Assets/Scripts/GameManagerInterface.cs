using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInterface : SingletonBehaviour <GameManagerInterface> {
	void Start () {}
	void Update () {
	}
	public GameState GetGameState () {
		return GameManager.Instance.GetGameState ();
	}
	public int GetScoreA () {
		return GameManager.Instance.GetFlagUpCount (0);
	}
	public int GetScoreB () {
		return GameManager.Instance.GetFlagUpCount (1);
	}
	public int GetLastPusedKey () {
		return GameManager.Instance.GetStoredKeyIndex ();
	}
}
