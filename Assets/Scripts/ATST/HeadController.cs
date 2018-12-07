using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour {

	public Rigidbody head;
	public Transform rotationOrigin;
	public Animator animator;

	public float rotationSpeed = 1.0f;

	public float rotationY { get; private set; }
	public float rotationX { get; private set; }

	private bool _movingToOrigin = false;

	private JoystickAxes _joystick;
	
	private Vector3 _forward = new Vector3(-1, 0, 0);
	private Vector3 _back = new Vector3(1, 0, 0);
	private Vector3 _left = new Vector3(0, 0, -1);
	private Vector3 _right = new Vector3(0, 0, 1);
	private Vector3 _up = new Vector3(0, 1, 0);
	private Vector3 _down = new Vector3(0, -1, 0);

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateNormals();

		_joystick.headHorizontal = Input.GetAxis("HeadHorizontal");
		_joystick.headVertical = Input.GetAxis("HeadVertical");


		Debug.Log("HeadHorizontal = " + _joystick.headHorizontal);
		Debug.Log("HeadVertical   = " + _joystick.headVertical);
	}

	void FixedUpdate()
	{
		HeadRotate();
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

	void HeadRotate()
	{
		if (animator.GetBool("Power"))
		{
			//RotateAroundPoint(head, _joystick.headVertical * rotationSpeed, _right, movementOrigin.position);
			//head.MovePosition(movementOrigin.position);

			//RotateAroundPoint(head, _joystick.headHorizontal * rotationSpeed, _up, movementOrigin.position);
			//head.MovePosition(movementOrigin.position);

			//if (head.transform.rotation.eulerAngles.)

			if (Input.GetButtonDown("ResetHeadRotation"))
			{
				rotationY = 0;
				rotationX = 0;
				_movingToOrigin = true;
			}

			if (_movingToOrigin)
			{
				MoveToOriginRotation();

				if (head.rotation == new Quaternion(0, rotationOrigin.transform.rotation.y, 0, head.transform.rotation.w))
				{
					_movingToOrigin = false;
				}

				return;
			}

			rotationY += _joystick.headHorizontal * rotationSpeed;
			rotationX += _joystick.headVertical * rotationSpeed;

			if (rotationY > -30 && rotationY < 30)
			{
				head.transform.RotateAround(head.transform.position, rotationOrigin.up, _joystick.headHorizontal * rotationSpeed);
			}
			else
			{
				rotationY -= _joystick.headHorizontal * rotationSpeed;
			}

			if (rotationX > -30 && rotationX < 30)
			{
				head.transform.RotateAround(head.transform.position, _right, _joystick.headVertical * rotationSpeed);
			}
			else
			{
				rotationX -= _joystick.headVertical * rotationSpeed;
			}
		}
		else
		{
			MoveToOriginRotation();
			_movingToOrigin = false;
		}
	}

	private void MoveToOriginRotation()
	{
		head.transform.rotation = Quaternion.Lerp(head.transform.rotation, rotationOrigin.transform.rotation, 5 * rotationSpeed * Time.deltaTime);
	}
}
