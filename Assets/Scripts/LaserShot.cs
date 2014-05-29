using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

	public float speed;
	public float laserDistance;
	public ParticleEmitter destroyEffect;

	private Vector3 startPosition;
	private float distanceTraveled;
	
	void Start () 
	{
		startPosition = transform.position;
		rigidbody.velocity = transform.forward * Time.deltaTime * speed;
	}

	void Update()
	{
		distanceTraveled = Vector3.Distance(transform.position, startPosition);

		if (distanceTraveled >= laserDistance)
		{

			Destroy(gameObject);
			DestroyChildren();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Radar" && other.tag != "Main Player")
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
