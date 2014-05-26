using UnityEngine;
using System.Collections;

public class EffectLifetime : MonoBehaviour {
	
	public float effectLifetime = 1f;
	private float timePassed = 0f;

	void Update () {
		timePassed += Time.deltaTime;
		if (timePassed >= effectLifetime)
		{
			Destroy(gameObject);
		}
	}
}
