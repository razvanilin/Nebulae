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
		}
		if (other.tag == "Player")
		{
			asteroidLife -= 5;
		}

		if (asteroidLife <= 0)
		{
			networkView.RPC("DestroyAsteroid", RPCMode.AllBuffered);
			//networkView.RPC("AsteroidEffects", RPCMode.All);
		}
	}

	[RPC]
	void DestroyAsteroid()
	{
		Instantiate(explosion, transform.position, transform.rotation);
		AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
		Destroy(asteroid);
	}

	/*[RPC]
	void AsteroidEffects()
	{
		Instantiate(explosion, transform.position, transform.rotation);
		AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);
	}*/
	
}
