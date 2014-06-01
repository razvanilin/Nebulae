using UnityEngine;
using System.Collections;

public class Tooltip : MonoBehaviour {

	private bool hideTooltipAtStart = false;
	private bool hideTooltip = true;

	void Start()
	{

	}

	// Update is called once per frame
	void Update () 
	{
		/*if (State.GetInstance().GState == State.GameState.PLAY && !hideTooltipAtStart)
		{
			gameObject.SetActive(false);
			hideTooltip = true;
		}*/
	}
}
