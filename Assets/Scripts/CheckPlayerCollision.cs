using UnityEngine;
using System.Collections;

public class CheckPlayerCollision : MonoBehaviour {

	public int playerLife = 100;
	public Transform explosionEffect;
	public AudioClip explosionClip;
	public GameObject player;
	public PlayerController playerContr;
	public ScoreWindow scoreWindow;

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
		if (networkView.isMine)
		{
			if (other.tag == "Laser")
			{
				networkView.RPC("InflictDamage", RPCMode.All, 3);
			}
			if (other.tag ==  "Asteroid")
			{
				networkView.RPC("InflictDamage", RPCMode.All, 20);
				if (!audioSource.isPlaying)
					audioSource.Play();
			}
			if (lifeLeft <= 0)
			{
				networkView.RPC("DestroyPlayer", RPCMode.AllBuffered);
				NetworkViewID netView = networkView.viewID;
				Debug.Log(netView.owner.ipAddress);
				scoreWindow.AddPoint(netView.owner);
			}
		}

	}

	[RPC]
	void InflictDamage(int damage)
	{
		lifeLeft -= damage;
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
		//if (networkView.isMine)
			//GUI.TextArea(new Rect(Screen.width-50, Screen.height-70, 50, 50), lifeLeft + "%");
	}
}
