using UnityEngine;
using System.Collections;

public class LifeRemaining : MonoBehaviour {

	public CheckPlayerCollision player;

	// Update is called once per frame
	void Update () 
	{
		if (networkView.isMine)
			guiText.text = player.LifeLeft + "%";
	}
	
}
