using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTMovementController : MonoBehaviour {
	
	public Transform atstTransform;
	public Rigidbody atstRigidbody;
	public CapsuleCollider atstCollider;

	public Transform rotationPoint;

	public ATSTAnimationController animationController;
	public ATSTPowerController power;

	public Vector3 lastVelocity { get; private set; }

	public float forwardSpeed = 100.0f;
	public float strafeSpeed = 30.0f;
	public float roundSpeed = 0.5f;

	private JoystickAxes _joystick;

	private Vector3 _forward	= new Vector3(-1, 0, 0);
	private Vector3 _back		= new Vector3(1, 0, 0);
	private Vector3 _left		= new Vector3(0, 0, -1);
    private Vector3 _right		= new Vector3(0, 0, 1);
    private Vector3 _up			= new Vector3(0, 1, 0);
    private Vector3 _down		= new Vector3(0, -1, 0);

	// Use this for initialization
	private void Start ()
	{
		for (int i = 0; i < Input.GetJoystickNames().Length; i++)
		{

			Debug.Log(Input.GetJoystickNames()[i]);
		}
	}
	
	// Update is called once per frame
	private void Update ()
	{
		UpdateNormals();

		UpdateJoystickAxes();

		//Debug.Log("Horizontal = " + joystick.horizontal);
		//Debug.Log("Vertical = " + joystick.vertical);
		//Debug.Log("Round = " + joystick.round);
		//Debug.Log("Traction = " + joystick.traction);

		//Debug.Log("HeadHorizontal = " + Input.GetAxis("HeadHorizontal"));
		//Debug.Log("HeadVertical   = " + Input.GetAxis("HeadVertical"));
	}

	private void FixedUpdate()
	{
		AtstMove();
		AtstRotate();
	}

	private void UpdateNormals()
	{
		_forward = -transform.right;
		_back = -_forward;
		_left = -transform.forward;
		_right = -_left;
		_up = transform.up;
		_down = -_up;
	}

	private void UpdateJoystickAxes()
	{
		_joystick.horizontal = Input.GetAxis("Horizontal");
		_joystick.vertical = Input.GetAxis("Vertical");
		_joystick.traction = Input.GetAxis("Traction");
		_joystick.round = Input.GetAxis("Round");

		// X-52 traction
		_joystick.traction = -(_joystick.traction - 1) * 0.5f;
	}

	private void AtstMove()
	{
		if (power.state)
		{
			Vector3 newVelocity = new Vector3(0, 0, 0);
			Vector3 direction = new Vector3(0, 0, 0);

			//float normalizedVertical = (Mathf.Abs(joystick.vertical) > 0 ? joystick.vertical / Mathf.Abs(joystick.vertical) : 0);
			//float normalizedHorizontal = (Mathf.Abs(joystick.horizontal) > 0 ? joystick.horizontal / Mathf.Abs(joystick.horizontal) : 0);


			// ---------------------------------------------------------------------------------------------------------------- //
			direction =
				_forward * _joystick.vertical +
				_right * _joystick.horizontal;

			newVelocity +=
				_forward * _joystick.vertical * forwardSpeed * Time.deltaTime +
				_right * _joystick.horizontal * strafeSpeed * Time.deltaTime;

			//newVelocity.x = direction.normalized.x * joystick.traction * forwardSpeed * Time.deltaTime;
			//newVelocity.z = direction.normalized.z * joystick.traction * strafeSpeed * Time.deltaTime;

			// Falling speed (G force)
			newVelocity.y = atstRigidbody.velocity.y;

			atstRigidbody.velocity = newVelocity;

			lastVelocity = atstRigidbody.velocity;

			// ---------------------------------------------------------------------------------------------------------------- //
		}
	}

	private void AtstRotate()
	{
        if (power.state)
        {
            // Rotation
            if (animationController.isWalkAndRotation ||
                animationController.isStrafeAndRotation)
            {
				//Quaternion q = Quaternion.AngleAxis(joystick.round * roundSpeed * Time.deltaTime, _up);
				//atstRigidbody.MovePosition(q * (atstRigidbody.transform.position - rotationPoint.position) + rotationPoint.position);
				//atstRigidbody.MoveRotation(atstRigidbody.transform.rotation * q);
				RotateAroundPoint(atstRigidbody, _joystick.round * roundSpeed * Time.deltaTime, _up, rotationPoint.position);
			}
        }
    }

	private void RotateAroundPoint(Rigidbody body, float angle, Vector3 axis, Vector3 origin)
	{
		Quaternion q = Quaternion.AngleAxis(angle, axis);
		body.MovePosition(q * (body.transform.position - origin) + origin);
		body.MoveRotation(body.transform.rotation * q);
	}
}
