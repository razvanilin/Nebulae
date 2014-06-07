using UnityEngine;
using System.Collections;

public class CheckPlayerCollision : MonoBehaviour {

	public int playerLife = 100;
	public float immunityTime = 2.0f;
	public Transform explosionEffect;
	public AudioClip explosionClip;
	public GameObject player;
	public PlayerController playerContr;
	public ScoreWindow scoreWindow;

	private float tempTime = 0f;
	private bool isImmune = false;
	private AudioSource audioSource;
	private int lifeLeft;
	public int LifeLeft {get{return lifeLeft;}}

	void Start()
	{
		lifeLeft = playerLife;
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (networkView.isMine)
		{
			if (isImmune)
			{
				/*player.rigidbody.velocity = new Vector3(0,0,0);
				player.rigidbody.angularVelocity = new Vector3(0,0,0);*/
				playerContr.Acceleration = 0f;
				player.rigidbody.isKinematic = true;
				tempTime += Time.deltaTime;
				if (tempTime >= immunityTime)
				{
					networkView.RPC("ResetPlayer", RPCMode.All);
					isImmune = false;
					tempTime = 0f;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (networkView.isMine)
		{
			if (other.tag == "Laser" && !isImmune)
			{
				networkView.RPC("InflictDamage", RPCMode.All, 3);
			}
			if (other.tag == "Asteroid" && !isImmune)
			{
				networkView.RPC("InflictDamage", RPCMode.All, 20);
				if (!audioSource.isPlaying)
					audioSource.Play();
			}
			if (lifeLeft <= 0)
			{
				networkView.RPC("DestroyPlayer", RPCMode.AllBuffered);
			}
		}

	}

	[RPC]
	void InflictDamage(int damage)
	{
		lifeLeft -= damage;
		if (lifeLeft < 0)
			lifeLeft = 0;
	}


	[RPC]
	void DestroyPlayer()
	{
		AudioSource.PlayClipAtPoint(explosionClip, transform.position);
		Instantiate(explosionEffect, transform.position, transform.rotation);
		/*player.transform.position = new Vector3(0,0,0);
		player.rigidbody.velocity = new Vector3(0,0,0);
		player.rigidbody.angularVelocity = new Vector3(0,0,0);
		player.transform.rotation = Quaternion.identity;
		playerContr.Acceleration = 0f;*/
		player.renderer.enabled = false;
		lifeLeft = playerLife;
		NetworkViewID netView = networkView.viewID;
		Debug.Log(netView.owner.ipAddress);
		scoreWindow.AddPoint(netView.owner);
		
		isImmune = true;
	}

	[RPC]
	void ResetPlayer()
	{
		player.rigidbody.isKinematic = false;
		player.transform.position = new Vector3(0,0,0);
		player.transform.rotation = Quaternion.identity;
		player.renderer.enabled = true;
	}
}
