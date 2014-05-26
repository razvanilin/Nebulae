using UnityEngine;
using System.Collections;

public class EnginePower : MonoBehaviour {

	public float engineEmission = 1000f;
	public float maxEmission = 10000f;

	public PlayerController player;
	private float acceleration ;
	public float Acceleration
	{
		get { return acceleration;}
		set { acceleration = value;}
	}
	
	void Start () 
	{
		particleEmitter.emit = false;
	}

	void Update () 
	{
		acceleration = player.Acceleration;
		//Debug.Log(particleEmitter.minEmission);
		if (acceleration > 0)
			particleEmitter.emit = true;
		EmitEngineParticles();
		/*if (acceleration != 0)
			networkView.RPC("EmitEngineParticles", RPCMode.All);*/
	}
	
	void EmitEngineParticles()
	{
		//if (particleEmitter.minEmission <= maxEmission || acceleration <= 0)
		//{
			particleEmitter.minEmission = acceleration * engineEmission;
			particleEmitter.maxEmission = acceleration * engineEmission;
		//}
	}
}
