using UnityEngine;
using System.Collections;

public class TargetBoundary : MonoBehaviour {
	
	public LaserShot laser;

	private SphereCollider boundry;
	// Use this for initialization
	void Start () 
	{
		boundry = GetComponent<SphereCollider>();
		boundry.radius = laser.laserDistance;
	}

}
