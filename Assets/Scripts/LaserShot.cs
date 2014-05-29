using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

	public float speed;
	public float laserDistance;
	public ParticleEmitter destroyEffect;

	private Vector3 startPosition;
	private float distanceTraveled;
	private GameObject[] playerBody;
	private Vector3 shipVelocity = Vector3.zero;

	public Vector3 ShipVelocity
	{
		get{return shipVelocity;}
		set{shipVelocity = value;}
	}

	void GetShipVelocity(Vector3 velocity)
	{
		shipVelocity = velocity;
	}

	void Start () 
	{
		startPosition = transform.position;
		rigidbody.velocity = shipVelocity + (transform.forward * Time.deltaTime * speed);
	}

	void Update()
	{
		//Debug.Log(shipVelocity);
		distanceTraveled = Vector3.Distance(transform.position, startPosition);

		if (distanceTraveled >= laserDistance)
		{
			Destroy(gameObject);
			//networkView.RPC("DestroyLaser", RPCMode.All);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//playerBody = GameObject.FindGameObjectsWithTag("Player Body");
		if (other.tag != "Radar" && other.tag != "Laser")
		{
			ParticleEmitter emitter = Instantiate(destroyEffect, transform.position, transform.rotation) as ParticleEmitter;
			Destroy(gameObject);
		}
	}
}
