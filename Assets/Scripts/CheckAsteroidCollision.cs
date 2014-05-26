using UnityEngine;
using System.Collections;

public class CheckAsteroidCollision : MonoBehaviour {

	public int asteroidLife = 3;
	public Transform explosion;
	public GameObject asteroid;
	public AudioClip explosionClip;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Laser")
		{
			asteroidLife--;

			if (asteroidLife <= 0)
			{
				Instantiate(explosion, transform.position, transform.rotation);
				AudioSource.PlayClipAtPoint(explosionClip, transform.position);
				Destroy(asteroid);
			}
		}
	}
	
}
