using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Texture2D cursorTexture;
	public int cursorSizeX = 20;
	public int cursorSizeY = 20;
	
	//public AudioClip backgroundMusic;
	private float delayTime = 0.5f;
	private float tempTime = 0f;
	private SceneFadeInOut sceneFadeIn;
	// Use this for initialization
	void Start () 
	{
		Screen.showCursor = false;
		sceneFadeIn = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneFadeInOut>();
	}

	void Update()
	{
		/*tempTime += Time.deltaTime;
		if (Input.GetButton("Lock Mouse") && Screen.lockCursor && !Screen.showCursor && tempTime >= delayTime)
		{
			Screen.lockCursor = false;
			Screen.showCursor = true;
			tempTime = 0f;
		}
		if (Input.GetButton("Lock Mouse") && !Screen.lockCursor && Screen.showCursor && tempTime >= delayTime)
		{
			Screen.lockCursor = true;
			Screen.showCursor = false;
			tempTime = 0f;
		}*/
	}

	void OnGUI()
	{
		GUI.DrawTexture (new Rect(Event.current.mousePosition.x-cursorSizeX/2, Event.current.mousePosition.y-cursorSizeY/2, cursorSizeX, cursorSizeY), cursorTexture);
	}
	
}
