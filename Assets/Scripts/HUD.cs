using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour 
{
	public float hudTargetSize = 20f;
	public int maximumTargets = 50;
	public Camera cam;
	public Texture2D targetTexture;
	public GameObject laser;
	public GUISkin guiSkin;

	private Vector3 targetScreenPos;
	private Vector3 interceptScreenPos;
	private float width = 0f, height = 0f;
	private GUIStyle currentStyle = null;
	private Dictionary<GameObject, bool> ships;
	private Color inRangeCol = new Color(0f, 1f, 0f, 0.5f);
	private Color outRangeCol = new Color(1f, 0f, 0f, 0.5f);

	private float shotSpeed;
	private Transform player;

	void Start()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for (int i=0; i<players.Length; i++)
		{
			if (players[i].networkView.isMine)
				player = players[i].transform;
		}
		shotSpeed = laser.GetComponent<LaserShot>().speed;
		ships = new Dictionary<GameObject, bool>();
	}

	void Update () 
	{
/*		Dictionary<GameObject, bool> removeList = ships;

		foreach(GameObject ship in removeList.Keys)
		{
			if (ship == null)
			{
				ships.Remove(ship);
			}
		}*/
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
		if (networkView.isMine && other.tag == "Player")
		{
			if (ships.ContainsKey(other.gameObject))
			{
				ships[other.gameObject] = false;
			}
		}
	}

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
					if (ship != null)
					{
						//Debug.Log(ships[ship]);
						targetScreenPos = cam.WorldToScreenPoint(ship.transform.position);
						float realShotSpeed = Vector3.Magnitude((laser.rigidbody.velocity + player.rigidbody.velocity)* shotSpeed * Time.deltaTime);

						interceptScreenPos = cam.WorldToScreenPoint(
							FirstOrderIntercept(
							player.position,
							player.rigidbody.velocity,
							realShotSpeed,
							ship.transform.position,
							ship.rigidbody.velocity)
							);
						if (targetScreenPos.x <= cam.pixelWidth && targetScreenPos.x >= 0)
						{
							width = targetScreenPos.x;
						}
						if (targetScreenPos.y <= cam.pixelHeight && targetScreenPos.y >= 0)
						{
							height = targetScreenPos.y;
						}

						if (targetScreenPos.z >= 0)
						{
							if (ships[ship]) {
								InitStyles(inRangeCol);
								if (GUI.Button(new Rect(
									targetScreenPos.x-(hudTargetSize/2), 
									cam.pixelHeight-targetScreenPos.y-(hudTargetSize/2), 
									hudTargetSize, hudTargetSize),
								        ""))
									Debug.Log("pressed!");

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

	private void InitStyles(Color col)	
	{
		if( currentStyle == null )	
		{			
			currentStyle = new GUIStyle( GUI.skin.box );			
			currentStyle.normal.background = targetTexture;
		}
	}

	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; i++)	
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		
		result.Apply();
		
		return result;
	}
}
