using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float sensitivity;
	public float maxSpeed = 50.0f;
	public float minSpeed = -10.0f;
	public float accelerationDamp = 0.1f;
	public float tiltSpeed = 1f;
	public float strafeSpeed = 1f;

	public ParticleEmitter particles;
	public float engineEmission = 1000f;
	public float maxEmission = 100000f;

	public Transform laser;
	public Transform gun1;
	public Transform gun2;
	public float laserSpeed = 1.0f;
	public float fireRate = 0.5f;
	public AudioClip shotClip;

	private float acceleration = 0f;
	private float nextFire = 0f;
	private bool haltFlag;

	void Start()
	{
		particles.emit = false;
		haltFlag = false;
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}

	void FixedUpdate ()
	{
		// Mouse Rotation
		transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * Time.deltaTime * sensitivity);

		// Keyboard Inputs
		/*float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);*/
		float tempAcc = 0f;
		if (!haltFlag)
			tempAcc = acceleration;

		if (Input.GetButton("Acceleration") && acceleration <= maxSpeed)
		{
			tempAcc ++;
			haltFlag = false;
			particles.emit = true;
		}
		if (Input.GetButton("Deacceleration") && acceleration >= minSpeed)
		{
			tempAcc--;
			haltFlag = false;
		}
		if (Input.GetButton("Halt"))
		{
			tempAcc = 0f;
			haltFlag = true;
		}

		acceleration = Mathf.Lerp(acceleration, tempAcc, Time.deltaTime * accelerationDamp);
		//Engine particle effects
		EngineEmission(tempAcc);

		rigidbody.velocity = (transform.forward * acceleration) 
			+ (transform.up * Input.GetAxis("Vertical") * strafeSpeed)
				+ (transform.right * Input.GetAxis("Horizontal") * strafeSpeed);

		//rigidbody.velocity = transform.forward * Input.GetAxis("Horizontal") * strafeSpeed;
		//rigidbody.velocity += transform.up * Input.GetAxis("Vertical") * strafeSpeed;

		rigidbody.angularVelocity = transform.forward * Input.GetAxis("Tilt") * tiltSpeed;

	}

	void Update()
	{
		nextFire += Time.deltaTime;
		// Shooting lasers
		if (Input.GetButton("Fire1") && nextFire >= fireRate)
		{
			Instantiate(laser, gun1.transform.position, gun1.transform.rotation);
			Instantiate(laser, gun2.transform.position, gun2.transform.rotation);
			nextFire = 0f;
			AudioSource.PlayClipAtPoint(shotClip, transform.position);
		}
	}

	void EngineEmission(float acc)
	{
		if (particles.minEmission <= maxEmission || acc <= 0)
		{
			particles.minEmission = acc * engineEmission;
			particles.maxEmission = acc * engineEmission;
		}
	}

}
