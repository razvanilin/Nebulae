using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour 
{
	public float hudTargetSize = 20f;
	public int maximumTargets = 50;
	public Camera cam;

	private Vector3 targetScreenPos;
	private float width = 0f, height = 0f;
	private GUIStyle currentStyle = null;
	private Dictionary<GameObject, bool> ships;
	private Color inRangeCol = new Color(0f, 1f, 0f, 0.5f);
	private Color outRangeCol = new Color(1f, 0f, 0f, 0.5f);

	void Start()
	{
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
			bool found = true;
			if (ships != null)
			{
				foreach(GameObject ship in ships.Keys)
				{
					if (ship != null)
					{
						//Debug.Log(ships[ship]);
						targetScreenPos = cam.WorldToScreenPoint(ship.transform.position);
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
								GUI.Box(new Rect(
									targetScreenPos.x-(hudTargetSize/2), 
									cam.pixelHeight-targetScreenPos.y-(hudTargetSize/2), 
									hudTargetSize, hudTargetSize),
								        "",
								        currentStyle);
							}
						}
					}
				}
			}
		}
	}

	private void InitStyles(Color col)	
	{
		if( currentStyle == null )	
		{			
			currentStyle = new GUIStyle( GUI.skin.box );			
			currentStyle.normal.background = MakeTex( 2, 2, col);
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
