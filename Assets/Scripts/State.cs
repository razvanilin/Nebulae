using UnityEngine;
using System.Collections;

public class State : MonoBehaviour {

	public enum GameState
	{
		MENU,
		PLAY,
		EXIT
	}

	private GameState gameState;
	public GameState GState
	{
		get{return gameState;}
		set{gameState = value;}
	}

	private static State state = new State();

	private State()
	{
		gameState = GameState.MENU;
	}

	public static State GetInstance()
	{
		return state;
	}
}