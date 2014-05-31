using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Texture2D cursorTexture;
	public int cursorSizeX = 20;
	public int cursorSizeY = 20;
	public float musicFadeSpeed = 0.5f;
	
	//public AudioClip backgroundMusic;
	private float delayTime = 0.5f;
	private float tempTime = 0f;
	private SceneFadeInOut sceneFadeIn;
	private AudioSource gameMusic;
	private State gameState;
	private bool musicIsFading;
	// Use this for initialization
	void Start () 
	{
		gameState = State.GetInstance();
		musicIsFading = false;
		Screen.showCursor = false;
		sceneFadeIn = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneFadeInOut>();
		gameMusic = GameObject.FindGameObjectWithTag("Game Music").GetComponent<AudioSource>();
	}

	void Update()
	{
		switch(gameState.GState)
		{
		case State.GameState.MENU:
			break;
		case State.GameState.PLAY:
			FadeMusic();
			break;
		default:
			break;
		}
	}

	void OnGUI()
	{
		GUI.DrawTexture (new Rect(Event.current.mousePosition.x-cursorSizeX/2, Event.current.mousePosition.y-cursorSizeY/2, cursorSizeX, cursorSizeY), cursorTexture);
	}

	void FadeMusic()
	{
		if (!musicIsFading)
		{
			audio.volume = Mathf.Lerp (audio.volume, 0f, musicFadeSpeed * Time.deltaTime);
			gameMusic.volume = Mathf.Lerp(gameMusic.volume, 1f, musicFadeSpeed * Time.deltaTime);
			if (gameMusic.volume == 1f)
				musicIsFading = true;
		}
	}
	
}
