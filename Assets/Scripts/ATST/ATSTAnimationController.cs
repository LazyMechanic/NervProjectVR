using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTAnimationController : MonoBehaviour {

	public Animator animator;

	public ATSTPowerController power;
	
	public bool isWalkAndStrafe { get; private set; }
	public bool isWalkAndRotation { get; private set; }
	public bool isStrafeAndRotation { get; private set; }
	public bool isIdle { get; private set; }

	private JoystickAxes _joystick;
	private JoystickAxes _lastJoystick = new JoystickAxes()
	{
		round = 0,
		vertical = 0,
		horizontal = 0,
		traction = 0,
		headVertical = 0,
		headHorizontal = 0,
	};

	private float _lastAnimationRound = 0.0f;
	private float _lastAnimationVertical = 0.0f;
	private float _lastAnimationHorizontal = 0.0f;

	// Use this for initialization
	void Start ()
	{
	}

	private void Update()
	{
		UpdateJoystickAxes();

		AnimationUpdate();
	}

	private void UpdateJoystickAxes()
	{
		_joystick.round = Input.GetAxis("Round");
		_joystick.vertical = Input.GetAxis("Vertical");
		_joystick.horizontal = Input.GetAxis("Horizontal");
		_joystick.traction = Input.GetAxis("Traction");
	}

	public void AnimationUpdate()
	{
		float curAnimationRound = _joystick.round;
		float curAnimationVertical = _joystick.vertical;
		float curAnimationHorizontal = _joystick.horizontal;
		
		// Add condition "car burn out"
		// ---------------------------------------------------------------------------------------------------------------- //
		if (false)
		{
			float normalizedRound = (Mathf.Abs(_joystick.round) > 0 ? Mathf.Sign(_joystick.round) : 0);
			float normalizedVertical = (Mathf.Abs(_joystick.vertical) > 0 ? Mathf.Sign(_joystick.vertical) : 0);
			float normalizedHorizontal = (Mathf.Abs(_joystick.horizontal) > 0 ? Mathf.Sign(_joystick.horizontal) : 0);

			float targetRound = normalizedRound * _joystick.traction;
			float targetVertical = normalizedVertical * _joystick.traction;
			float targetHorizontal = normalizedHorizontal * _joystick.traction;

			curAnimationRound = Mathf.SmoothStep(animator.GetFloat("Round"), targetRound, Time.deltaTime);
			curAnimationVertical = Mathf.SmoothStep(animator.GetFloat("Vertical"), targetHorizontal, Time.deltaTime);
			curAnimationHorizontal = Mathf.SmoothStep(animator.GetFloat("Horizontal"), targetHorizontal, Time.deltaTime);
		}
		// ---------------------------------------------------------------------------------------------------------------- //

		//animator.SetFloat("Round", _joystick.round);
		//animator.SetFloat("Vertical", _joystick.vertical);
		//animator.SetFloat("Horizontal", _joystick.horizontal);
		bool localIsIdle = false;

		animator.SetFloat("Round", curAnimationRound);
		animator.SetFloat("Vertical", curAnimationVertical);
		animator.SetFloat("Horizontal", curAnimationHorizontal);
		animator.SetBool("Power", power.state);

		animator.speed = 1.0f;

		// Idle
		if (Mathf.Abs(_joystick.round) < 0.01f &&
			Mathf.Abs(_joystick.vertical) < 0.01f &&
			Mathf.Abs(_joystick.horizontal) < 0.01f ||
			!power.state)
		{
			isWalkAndStrafe = false; // main idle animation
			isWalkAndRotation = false;
			isStrafeAndRotation = false;
			localIsIdle = true;
		}
		// Walk and strafe || walk
		else if (
			Mathf.Abs(_joystick.round) < 0.01f &&
			Mathf.Abs(_joystick.vertical) > 0.01f &&
			Mathf.Abs(_joystick.horizontal) > 0.01f ||
			Mathf.Abs(_joystick.round) < 0.01f &&
			Mathf.Abs(_joystick.vertical) > 0.01f &&
			Mathf.Abs(_joystick.horizontal) < 0.01f)
		{
			animator.speed = Mathf.Max(Mathf.Abs(_joystick.vertical), Mathf.Abs(_joystick.horizontal));
			//animator.speed = _joystick.traction;

			isWalkAndStrafe = true;
			isWalkAndRotation = false;
			isStrafeAndRotation = false;
		}
		// Walk and rotation || rotation
		else if (
			Mathf.Abs(_joystick.round) > 0.01f &&
			Mathf.Abs(_joystick.vertical) > 0.01f &&
			Mathf.Abs(_joystick.horizontal) < 0.01f ||
			Mathf.Abs(_joystick.round) > 0.01f &&
			Mathf.Abs(_joystick.vertical) < 0.01f &&
			Mathf.Abs(_joystick.horizontal) < 0.01f)
		{
			animator.speed = Mathf.Max(Mathf.Abs(_joystick.vertical), Mathf.Abs(_joystick.round));
			//animator.speed = _joystick.traction;

			isWalkAndStrafe = false;
			isWalkAndRotation = true;
			isStrafeAndRotation = false;
		}
		// Strafe and rotation || strafe
		else if (
			_joystick.traction > 0.01f && (
			Mathf.Abs(_joystick.round) > 0.01f &&
			Mathf.Abs(_joystick.vertical) < 0.01f &&
			Mathf.Abs(_joystick.horizontal) > 0.01f ||
			Mathf.Abs(_joystick.round) < 0.01f &&
			Mathf.Abs(_joystick.vertical) < 0.01f &&
			Mathf.Abs(_joystick.horizontal) > 0.01f))
		{
			animator.speed = Mathf.Max(Mathf.Abs(_joystick.round), Mathf.Abs(_joystick.horizontal));
			//animator.speed = _joystick.traction;

			isWalkAndStrafe = false;
			isWalkAndRotation = false;
			isStrafeAndRotation = true;
		}

		animator.SetBool("IsWalkAndStrafe", isWalkAndStrafe);
		animator.SetBool("IsWalkAndRotation", isWalkAndRotation);
		animator.SetBool("IsStrafeAndRotation", isStrafeAndRotation);

		isIdle = localIsIdle;

		_lastJoystick = _joystick;

		_lastAnimationRound = curAnimationRound;
		_lastAnimationVertical = curAnimationVertical;
		_lastAnimationHorizontal = curAnimationHorizontal;
	}
}
