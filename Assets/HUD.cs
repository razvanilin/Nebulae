using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
	public Transform target;
	public float hudTargetSize = 20f;

	private Camera cam;
	private Vector3 targetScreenPos;
	private float width = 0f, height = 0f;
	private GUIStyle currentStyle = null;

	void Start()
	{
		cam = GetComponent<Camera>();
	}

	void Update () 
	{
		targetScreenPos = cam.WorldToScreenPoint(target.position);
	}

	void OnGUI()
	{
		if (targetScreenPos.x <= cam.pixelWidth-50 && targetScreenPos.x >= 0)
		{
			width = targetScreenPos.x;
		}
		if (targetScreenPos.y <= cam.pixelHeight-50 && targetScreenPos.y >= 0)
		{
			height = targetScreenPos.y;
		}

		if (targetScreenPos.z >= 0)
		{
			InitStyles();
			GUI.Box(new Rect(
				width-(hudTargetSize/2), 
				cam.pixelHeight-height-(hudTargetSize/2), 
				hudTargetSize, hudTargetSize),
			        "",
			        currentStyle);
		}

	}

	private void InitStyles()	
	{
		if( currentStyle == null )	
		{			
			currentStyle = new GUIStyle( GUI.skin.box );			
			currentStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 1f, 0f, 0.5f ) );
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
