using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetBoundary : MonoBehaviour {

	public float hudTargetSize = 20f;
	public LaserShot laser;
	public GUISkin guiSkin;
	public Camera cam;
	public GameObject laserObj;

	private Vector3 targetScreenPos;
	private Vector3 interceptScreenPos;
	private float width = 0f, height = 0f;
	private SphereCollider boundry;
	private Dictionary<GameObject, bool> ships = new Dictionary<GameObject, bool>();
	private Transform player;
	private float shotSpeed;
	// Use this for initialization
	void Start () 
	{
		boundry = GetComponent<SphereCollider>();
		boundry.radius = laser.laserDistance;

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for (int i=0; i<players.Length; i++)
		{
			if (players[i].networkView.isMine)
			{
				player = players[i].transform;
				break;
			}
		}
		shotSpeed = laserObj.GetComponent<LaserShot>().speed * Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		
		if (networkView.isMine && other.tag == "Player")
		{
			bool existFlag = false;
			if (ships != null)
			{
				foreach (GameObject ship in ships.Keys)
				{
					if (ship.GetInstanceID() == other.gameObject.GetInstanceID())
					{
						existFlag = true;
						ships[ship] = true;
						break;
					}
				}
				if (!existFlag)
					ships.Add(other.gameObject, true);
			}
			else
				ships.Add(other.gameObject, true);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			if (ships.ContainsKey(other.gameObject))
			{
				ships[other.gameObject] = false;
			}
		}
	}

	bool isActive = false;
	void OnGUI()
	{
		if (networkView.isMine)
		{
			GUI.skin = guiSkin;
			bool found = true;
			if (ships != null)
			{
				foreach(GameObject ship in ships.Keys)
				{
					if (ship != null && ships[ship])
					{
						targetScreenPos = cam.WorldToScreenPoint(ship.transform.position);
						
						interceptScreenPos = cam.WorldToScreenPoint(
							FirstOrderIntercept(
							player.position,
							player.rigidbody.velocity,
							shotSpeed,
							ship.transform.position,
							ship.rigidbody.velocity)
							);

						if (targetScreenPos.z >= 0)
									GUI.Box(new Rect(
										interceptScreenPos.x-(hudTargetSize/4), 
										cam.pixelHeight-interceptScreenPos.y-(hudTargetSize/4), 
										hudTargetSize/2, hudTargetSize/2),
									        "");
							

					}
				}
			}
		}
	}
	
	//first-order intercept using absolute target position
	public static Vector3 FirstOrderIntercept
		(
			Vector3 shooterPosition,
			Vector3 shooterVelocity,
			float shotSpeed,
			Vector3 targetPosition,
			Vector3 targetVelocity
		)  {
		Vector3 targetRelativePosition = targetPosition - shooterPosition;
		Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
		float t = FirstOrderInterceptTime
			(
				shotSpeed,
				targetRelativePosition,
				targetRelativeVelocity
				);
		return targetPosition + t*(targetRelativeVelocity);
	}
	//first-order intercept using relative target position
	public static float FirstOrderInterceptTime
		(
			float shotSpeed,
			Vector3 targetRelativePosition,
			Vector3 targetRelativeVelocity
		) {
		float velocitySquared = targetRelativeVelocity.sqrMagnitude;
		if(velocitySquared < 0.001f)
			return 0f;
		
		float a = velocitySquared - shotSpeed*shotSpeed;
		
		//handle similar velocities
		if (Mathf.Abs(a) < 0.001f)
		{
			float t = -targetRelativePosition.sqrMagnitude/
				(
					2f*Vector3.Dot
					(
					targetRelativeVelocity,
					targetRelativePosition
					)
					);
			return Mathf.Max(t, 0f); //don't shoot back in time
		}
		
		float b = 2f*Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
		float c = targetRelativePosition.sqrMagnitude;
		float determinant = b*b - 4f*a*c;
		
		if (determinant > 0f) { //determinant > 0; two intercept paths (most common)
			float	t1 = (-b + Mathf.Sqrt(determinant))/(2f*a),
			t2 = (-b - Mathf.Sqrt(determinant))/(2f*a);
			if (t1 > 0f) {
				if (t2 > 0f)
					return Mathf.Min(t1, t2); //both are positive
				else
					return t1; //only t1 is positive
			} else
				return Mathf.Max(t2, 0f); //don't shoot back in time
		} else if (determinant < 0f) //determinant < 0; no intercept path
			return 0f;
		else //determinant = 0; one intercept path, pretty much never happens
			return Mathf.Max(-b/(2f*a), 0f); //don't shoot back in time
	}


}
