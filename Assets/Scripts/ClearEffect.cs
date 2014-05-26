using UnityEngine;
using System.Collections;

public class ClearEffect : MonoBehaviour {

	public float effectLife = 2f;
	private float timePassed;

	void Update () 
	{
		timePassed += Time.deltaTime;
		//if (timePassed >= effectLife)
			//Destroy(gameObject);
	}
}
