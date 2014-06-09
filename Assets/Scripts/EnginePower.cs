using UnityEngine;
using System.Collections;

public class EnginePower : MonoBehaviour {

	public float engineEmission = 1000f;
	public float maxEmission = 10000f;
	public float engineSoundFactor = 0.05f;
	public float enginePitchFactor = 0.2f;

	public PlayerController player;

	private AudioSource audioSource;
	private float acceleration ;
	public float Acceleration
	{
		get { return acceleration;}
		set { acceleration = value;}
	}
	
	void Start () 
	{
		particleEmitter.emit = false;
		audioSource = GetComponent<AudioSource>();
	}

	void Update () 
	{
		acceleration = player.Acceleration;
		//Debug.Log(particleEmitter.minEmission);
		if (acceleration > 0)
			particleEmitter.emit = true;
		//EmitEngineParticles();
		if (networkView.isMine)
		{
			if (audioSource.volume<0.3f)
				audioSource.volume = engineSoundFactor * acceleration;
			audioSource.pitch = enginePitchFactor * (acceleration);
		}
		if (acceleration >= 0)
			networkView.RPC("EmitEngineParticles", RPCMode.All);
	}

	[RPC]
	void EmitEngineParticles()
	{
		//if (particleEmitter.minEmission <= maxEmission || acceleration <= 0)
		//{
		particleEmitter.minEmission = acceleration * engineEmission;
		particleEmitter.maxEmission = acceleration * engineEmission;

	}
}
