using UnityEngine;
using System.Collections;

public class EngineHum : MonoBehaviour {

	private AudioSource engineHum;
	// Use this for initialization
	void Start () 
	{
		engineHum = GetComponent<AudioSource>();

		if (!networkView.isMine)
			engineHum.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
