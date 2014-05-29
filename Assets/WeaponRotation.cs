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
		if (Input.GetButton("Turn"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 targetPoint = ray.GetPoint(hitdist);
			transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
			//transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
		}
		else
		{
			for (int i = 0; i < player.Length; i++)
				if (player[i].networkView.isMine)
					transform.rotation = player[i].transform.rotation;
		}
	}
}
