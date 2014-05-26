using UnityEngine;
using System.Collections;

public class RotateCrosshair : MonoBehaviour 
{
	public float rotationSpeed = 10f;

	void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
	}
}
