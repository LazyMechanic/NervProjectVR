using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ATSTMovementController : MonoBehaviour {
	
	public Rigidbody atstRigidbody;

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

	private void Awake()
	{
		Assert.IsNotNull(atstRigidbody, "[ATSTMovementController]: Atst Rigidbody is null");
		Assert.IsNotNull(rotationPoint, "[ATSTMovementController]: Rotation Point is null");
		Assert.IsNotNull(animationController, "[ATSTMovementController]: Animation Controller is null");
		Assert.IsNotNull(power, "[ATSTMovementController]: Power Controller is null");
	}

	// Use this for initialization
	private void Start ()
	{

	}
	
	// Update is called once per frame
	private void Update ()
	{
		UpdateNormals();
		UpdateJoystickAxes();
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
			atstRigidbody.transform.RotateAround(rotationPoint.position, _up, _joystick.round * roundSpeed * Time.deltaTime);
        }
    }
}
