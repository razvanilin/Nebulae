using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public Transform target;
	public float distance = 2.0f;
	public float height;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

	private Quaternion oldRotation;
	private Quaternion targetRotation;
	private Vector3 relTargetPos;

	// Use this for initialization
	void Start () {
		oldRotation = target.rotation;
	}

	void FixedUpdate()
	{
		if (!target)
			return;

		targetRotation = target.rotation;
		Quaternion currentRotation = Quaternion.Lerp(oldRotation, targetRotation, rotationDamping * Time.deltaTime); 
		oldRotation = currentRotation;
		transform.position = target.transform.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		transform.LookAt(target, target.TransformDirection(Vector3.up));
	}

}
