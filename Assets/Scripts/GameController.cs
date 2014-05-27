using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	//public AudioClip backgroundMusic;
	private float delayTime = 0.5f;
	private float tempTime = 0f;
	// Use this for initialization
	void Start () 
	{
		/*Screen.lockCursor = true;
		Screen.showCursor = false;*/

		//AudioSource.PlayClipAtPoint(backgroundMusic, transform.position);
	}

	void Update()
	{
		tempTime += Time.deltaTime;
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
		}
	}

	void OnGUI()
	{
		if (!Screen.lockCursor && Screen.showCursor)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Exit"))
			{
				Application.Quit();
			}
		}
	}
}
