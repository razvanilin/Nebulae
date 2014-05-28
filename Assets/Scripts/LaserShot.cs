using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

	public float speed;
	public float life;
	public ParticleEmitter destroyEffect;

	private float timePassed;
	
	void Start () 
	{
		rigidbody.velocity = transform.forward * speed;

	}

	void Update()
	{
		timePassed += Time.deltaTime;

		if (timePassed >= life)
		{
			Destroy(gameObject);
			DestroyChildren();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Radar")
		{
			ParticleEmitter emitter = Instantiate(destroyEffect, transform.position, transform.rotation) as ParticleEmitter;
			//emitter.Emit();
			Destroy(gameObject);
			DestroyChildren();
		}
	}

	void DestroyChildren()
	{
		int childs = transform.childCount;
		
		for (int i = childs - 1; i > 0; i--)	
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}
	}
}
