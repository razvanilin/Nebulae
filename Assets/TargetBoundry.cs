using UnityEngine;
using System.Collections;

public class TargetBoundry : MonoBehaviour {
	
	public LaserShot laser;

	private SphereCollider boundry;
	// Use this for initialization
	void Start () 
	{
		boundry = GetComponent<SphereCollider>();
		boundry.radius = laser.laserDistance;
	}

}
