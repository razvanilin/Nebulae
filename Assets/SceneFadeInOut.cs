using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour 
{
	public float fadeSpeed = 1.5f;
	
	private bool sceneStarting = true;
	
	void Awake()
	{
		guiTexture.pixelInset = new Rect (0f, 0f, Screen.width, Screen.height);
	}
	
	void Update()
	{
		if (sceneStarting) {
			StartScreen ();
		}
	}
	
	void FadeToClear()
	{
		guiTexture.color = Color.Lerp (guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	void FadeToBlack()
	{
		guiTexture.color = Color.Lerp (Color.clear, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	public void StartScreen()
	{
		FadeToClear ();
		
		if (guiTexture.color.a <= 0.05f) {
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			sceneStarting = false;
		}
	}
	
	public void EndScene()
	{
		guiTexture.enabled = true;
		FadeToBlack ();
		Debug.Log("Here");
		/*if (guiTexture.color.a >= 0.95f) {

			Application.LoadLevel(1);		
		}*/
	}
}