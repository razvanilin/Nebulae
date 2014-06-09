using UnityEngine;
using System.Collections;

public class WeaponRotation : MonoBehaviour {

	public LaserShot laser;
	public GameObject[] player;

	private float hitdist;

	void Start()
	{
		hitdist = laser.laserDistance;
		player = GameObject.FindGameObjectsWithTag("Player");
	}

	void Update () 
	{
		if (networkView.isMine)
		{
			if (Input.GetButton("Turn"))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 targetPoint = ray.GetPoint(hitdist);
				networkView.RPC("Rotate", RPCMode.All, Quaternion.LookRotation(targetPoint - transform.position));
				//transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
				//transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
			}
			else
			{
				for (int i = 0; i<player.Length; i++)
				{
					if (player[i].networkView.isMine)
						networkView.RPC ("Rotate", RPCMode.All, player[i].transform.rotation);
						//transform.rotation = player[i].transform.rotation;
				}
			}
		}
	}

	[RPC]
	void Rotate(Quaternion targetRotation)
	{
		transform.rotation = targetRotation;
	}
}
