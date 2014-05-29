using UnityEngine;
using System.Collections;

public class CheckPlayerCollision : MonoBehaviour {

	public int playerLife = 100;
	public Transform explosionEffect;
	public AudioClip explosionClip;
	public GameObject player;
	public PlayerController playerContr;

	private AudioSource audioSource;
	private int lifeLeft;
	public int LifeLeft {get{return lifeLeft;}}

	void Start()
	{
		lifeLeft = playerLife;
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Laser" && tag != "Main Player")
		{
			lifeLeft -= 3;
		}
		if (other.tag ==  "Asteroid")
		{
			lifeLeft -= 20;
			if (!audioSource.isPlaying)
				audioSource.Play();
		}
		if (lifeLeft <= 0)
		{
			networkView.RPC("DestroyPlayer", RPCMode.AllBuffered);
		}

	}

	[RPC]
	void DestroyPlayer()
	{
		AudioSource.PlayClipAtPoint(explosionClip, transform.position);
		Instantiate(explosionEffect, transform.position, transform.rotation);
		player.transform.position = new Vector3(0,0,0);
		player.rigidbody.velocity = new Vector3(0,0,0);
		player.rigidbody.angularVelocity = new Vector3(0,0,0);
		player.transform.rotation = Quaternion.identity;
		playerContr.Acceleration = 0f;
		lifeLeft = playerLife;
	}

	void OnGUI()
	{
		if (networkView.isMine)
			GUI.TextArea(new Rect(Screen.width-50, Screen.height-70, 50, 50), lifeLeft + "%");
	}
}
