using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Texture2D cursorTexture;
	public int cursorSizeX = 20;
	public int cursorSizeY = 20;
	public float musicFadeSpeed = 0.5f;
	public GUISkin guiSkin;
	public string nebulaName = "Maya's Horizon Nebula";
	
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
	private bool showNebula = true;
	private string nebulaGUI;
	private float nebulaTime = 15f;
	private float tempNebulaTime = 0f;
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
				tooltip.SetActive(!tooltip.activeInHierarchy);
				tempTime = 0f;
			}
			if (Input.GetButton("Sub-Menu") && tempTime>=tooltipDelay)
			{
				showMenu = !showMenu;
				tempTime = 0f;
			}

			if (showNebula)
			{
				StartCoroutine(AnimateText(nebulaName));
				showNebula = false;
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

		tempNebulaTime += Time.deltaTime;
		if (State.GetInstance().GState == State.GameState.PLAY && tempNebulaTime <= nebulaTime)
		{
			GUI.TextArea(new Rect(Screen.width/3, Screen.height/4, 400, 50), nebulaGUI);
		}
	}

	IEnumerator AnimateText(string strComplete){
		int i = 0;
		nebulaGUI = "";
		while( i < strComplete.Length ){
			nebulaGUI += strComplete[i++];
			yield return new WaitForSeconds(0.05F);
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
