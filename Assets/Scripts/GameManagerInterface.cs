using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInterface : SingletonBehaviour <GameManagerInterface> {
	void Start () {}
	void Update () {}
	public GameState GetGameState () {
		return GameState.Title;
	}
	public int GetScoreA () {
		return 0;
	}
	public int GetScoreB () {
		return 0;
	}
}
