using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

	public float speed;
	public float laserDistance;
	public ParticleEmitter destroyEffect;

	private Vector3 startPosition;
	private float distanceTraveled;
	private GameObject[] playerBody;
	
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
		if (networkView.isMine)
		{
		//playerBody = GameObject.FindGameObjectsWithTag("Player Body");
			if (other.tag != "Radar")
			{
				ParticleEmitter emitter = Instantiate(destroyEffect, transform.position, transform.rotation) as ParticleEmitter;
				//emitter.Emit();
				Destroy(gameObject);
				DestroyChildren();
			}
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
