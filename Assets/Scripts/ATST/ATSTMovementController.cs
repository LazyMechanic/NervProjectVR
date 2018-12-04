using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTMovementController : MonoBehaviour {

	public ATSTAnimationController animationController;

	public Transform atstTransform;
	public Rigidbody atstRigidbody;
	public CapsuleCollider atstCollider;

	public Transform rotationPoint;

	public JoystickAxes joystick;

	public float forwardSpeed = 100.0f;
	public float strafeSpeed = 30.0f;
	public float roundSpeed = 0.5f;

	private Vector3 _forward	= new Vector3(-1, 0, 0);
	private Vector3 _back		= new Vector3(1, 0, 0);
	private Vector3 _left		= new Vector3(0, 0, -1);
    private Vector3 _right		= new Vector3(0, 0, 1);
    private Vector3 _up			= new Vector3(0, 1, 0);
    private Vector3 _down		= new Vector3(0, -1, 0);

    private bool _power = true;

	// Use this for initialization
	private void Start ()
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
		_forward = -atstRigidbody.transform.right;
		_back = -_forward;
		_left = -atstRigidbody.transform.forward;
		_right = -_left;
		_up = atstRigidbody.transform.up;
		_down = -_up;
		
		joystick.horizontal = Input.GetAxis("Horizontal");
		joystick.vertical = Input.GetAxis("Vertical");
		joystick.traction = -(Input.GetAxis("Traction") - 1) * 0.5f;
		joystick.round = Input.GetAxis("Round");

		if (Input.GetButtonDown("PowerOn"))
		{
			_power = !_power;
		}

		//Debug.Log("Horizontal = " + joystick.horizontal);
		//Debug.Log("Vertical = " + joystick.vertical);
		//Debug.Log("Round = " + joystick.round);

		animationController.AnimationUpdate(joystick, _power);
	}

	private void FixedUpdate()
	{
		Move();
		Rotate();
	}

	private void Move()
	{
		if (_power)
		{
			Vector3 newVelocity = new Vector3(0, 0, 0);
			//Vector3 direction = new Vector3(0, 0, 0);

			//float normalizedVertical = (Mathf.Abs(joystick.vertical) > 0 ? joystick.vertical / Mathf.Abs(joystick.vertical) : 0);
			//float normalizedHorizontal = (Mathf.Abs(joystick.horizontal) > 0 ? joystick.horizontal / Mathf.Abs(joystick.horizontal) : 0);

			newVelocity +=
				_forward * joystick.vertical * forwardSpeed * Time.deltaTime +
				_right * joystick.horizontal * strafeSpeed * Time.deltaTime;

			// Falling speed (G force)
			newVelocity.y = atstRigidbody.velocity.y;

			atstRigidbody.velocity = newVelocity;
		}
	}

	private void Rotate()
	{
        if (_power)
        {
            // Rotation
            if (animationController.IsWalkAndRotation ||
                animationController.IsStrafeAndRotation)
            {
				Quaternion q = Quaternion.AngleAxis(joystick.round * roundSpeed * Time.deltaTime, _up);
				atstRigidbody.MovePosition(q * (atstRigidbody.transform.position - rotationPoint.position) + rotationPoint.position);
				atstRigidbody.MoveRotation(atstRigidbody.transform.rotation * q);
			}
        }
    }
}
