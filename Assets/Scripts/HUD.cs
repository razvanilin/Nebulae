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
	
	private Transform player;

	void Start()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for (int i=0; i<players.Length; i++)
		{
			if (players[i].networkView.isMine)
			{
				player = players[i].transform;
				break;
			}
		}
		ships = new Dictionary<GameObject, bool>();
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

						if (targetScreenPos.x <= cam.pixelWidth && targetScreenPos.x >= 0)
						{
							width = targetScreenPos.x;
						}
						if (targetScreenPos.y <= cam.pixelHeight && targetScreenPos.y >= 0)
						{
							height = targetScreenPos.y;
						}
						if (targetScreenPos.z < 0)
						{
							width = (targetScreenPos.x < cam.pixelWidth/2) ? 0.1f : cam.pixelWidth - 0.1f;
						}
						if (ships[ship]) {
							if (GUI.Button(
								new Rect(
									width-(hudTargetSize/2), cam.pixelHeight - height - (hudTargetSize/2), 
									hudTargetSize, hudTargetSize
									),
								""))
								isActive = false;


						}
					}
				}
			}
		}
	}	
}
