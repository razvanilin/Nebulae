using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Texture2D cursorTexture;
	public int cursorSizeX = 20;
	public int cursorSizeY = 20;
	public float musicFadeSpeed = 0.5f;
	public GUISkin guiSkin;
	
	//public AudioClip backgroundMusic;
	private float delayTime = 0.5f;
	private float tempTime = 0f;
	private SceneFadeInOut sceneFadeIn;
	private AudioSource gameMusic;
	private State gameState;
	private bool musicIsFading;
	private bool hideTooltip = true;
	private GameObject tooltip;
	private float tooltipDelay = 0.5f;
	private bool showMenu = false;
	// Use this for initialization
	void Start () 
	{
		tooltip = GameObject.FindGameObjectWithTag("Tooltip");
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
			tempTime+=Time.deltaTime;
			if (Input.GetButton("Tooltip") && tempTime>tooltipDelay)
			{
				Debug.Log("Tooltip");
				tooltip.SetActive(!tooltip.activeInHierarchy);
				tempTime = 0f;
			}
			if (Input.GetButton("Sub-Menu") && tempTime>=tooltipDelay)
			{
				showMenu = !showMenu;
				tempTime = 0f;
			}
			FadeMusic();
			break;
		default:
			break;
		}
	}

	void OnGUI()
	{
		GUI.skin = guiSkin;
		GUI.depth = 0;
		GUI.DrawTexture (new Rect(Event.current.mousePosition.x-cursorSizeX/2, Event.current.mousePosition.y-cursorSizeY/2, cursorSizeX, cursorSizeY), cursorTexture);
		if (showMenu)
		{
			if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 50, 200, 100), "Exit"))
				Application.Quit();
			if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 150, 200, 100), "Leave the Nebula"))
			{
				Network.Disconnect();
				showMenu = false;
				State.GetInstance().GState = State.GameState.MENU;
				Application.LoadLevel(0);
			}
			if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 250, 200, 100), "Resume"))
				showMenu = false;
		}
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
