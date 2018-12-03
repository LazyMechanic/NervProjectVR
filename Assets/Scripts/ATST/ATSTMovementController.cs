using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTMovementController : MonoBehaviour {

	public ATSTAnimationController animationController;

	public Transform transform;
	public Rigidbody rigidbody;
	public CapsuleCollider collider;

	public JoystickAxes joystick;

	public float forwardSpeed = 100.0f;
	public float strafeSpeed = 30.0f;
	public float roundSpeed = 30.0f;

	private readonly Vector3 _forward = new Vector3(-1, 0, 0);
	private readonly Vector3 _back = new Vector3(1, 0, 0);
	private readonly Vector3 _left = new Vector3(0, 0, -1);
	private readonly Vector3 _right = new Vector3(0, 0, 1);

	private bool _power = false;

	// Use this for initialization
	private void Start ()
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
		joystick.horizontal = Input.GetAxis("Horizontal");
		joystick.vertical = Input.GetAxis("Vertical");
		joystick.round = Input.GetAxis("Round");

		if (Input.GetButtonDown("Power"))
		{
			SwitchPower();
		}

		//Debug.Log("Horizontal = " + joystick.horizontal);
		//Debug.Log("Vertical = " + joystick.vertical);
		//Debug.Log("Round = " + joystick.round);

		animationController.AnimationUpdate(joystick.horizontal, joystick.vertical, joystick.round, _power);
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
			//if (animationController.IsWalkAndRotation ||
			//	animationController.IsWalkAndStrafe)
			rigidbody.velocity = (
				_forward * joystick.vertical * forwardSpeed +
				_right * joystick.horizontal * strafeSpeed
				) * Time.deltaTime;
		}
	}

	private void Rotate()
	{

	}

	private void SwitchPower()
	{
		_power = !_power;
	}
}
