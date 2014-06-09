using UnityEngine;
using System.Collections;

public class LifeRemaining : MonoBehaviour {

	public CheckPlayerCollision player;

	// Update is called once per frame
	void Update () 
	{
		if (networkView.isMine)
		{
			Debug.Log(player.LifeLeft);
			guiText.text = player.LifeLeft + "%";
		}
	}
	
}
