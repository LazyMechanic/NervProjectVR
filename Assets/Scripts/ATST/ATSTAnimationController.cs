using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTAnimationController : MonoBehaviour {

	public Animator animator;
	
	public bool IsWalkAndStrafe { get; private set; }
	public bool IsWalkAndRotation { get; private set; }
	public bool IsStrafeAndRotation { get; private set; }

	// Use this for initialization
	void Start () {
		
	}

	public void AnimationUpdate(JoystickAxes joystick, bool power)
	{
		animator.SetFloat("Round", joystick.round);
		animator.SetFloat("Vertical", joystick.vertical);
		animator.SetFloat("Horizontal", joystick.horizontal);
		animator.SetBool("Power", power);

		animator.speed = 1.0f;

		// Idle
		if (Mathf.Abs(joystick.round) < 0.01f &&
			Mathf.Abs(joystick.vertical) < 0.01f &&
			Mathf.Abs(joystick.horizontal) < 0.01f ||
			!power)
		{
			IsWalkAndStrafe = false;
			IsWalkAndRotation = false;
			IsStrafeAndRotation = false;
		}
		// Walk and strafe || walk
		else if (
			Mathf.Abs(joystick.round) < 0.01f &&
			Mathf.Abs(joystick.vertical) > 0.01f &&
			Mathf.Abs(joystick.horizontal) > 0.01f ||
			Mathf.Abs(joystick.round) < 0.01f &&
			Mathf.Abs(joystick.vertical) > 0.01f &&
			Mathf.Abs(joystick.horizontal) < 0.01f)
		{
			animator.speed = Mathf.Max(Mathf.Abs(joystick.vertical), Mathf.Abs(joystick.horizontal));

			IsWalkAndStrafe = true;
			IsWalkAndRotation = false;
			IsStrafeAndRotation = false;
		}
		// Walk and rotation || rotation
		else if (
			Mathf.Abs(joystick.round) > 0.01f &&
			Mathf.Abs(joystick.vertical) > 0.01f &&
			Mathf.Abs(joystick.horizontal) < 0.01f ||
			Mathf.Abs(joystick.round) > 0.01f &&
			Mathf.Abs(joystick.vertical) < 0.01f &&
			Mathf.Abs(joystick.horizontal) < 0.01f)
		{
			animator.speed = Mathf.Max(Mathf.Abs(joystick.vertical), Mathf.Abs(joystick.round));

			IsWalkAndStrafe = false;
			IsWalkAndRotation = true;
			IsStrafeAndRotation = false;
		}
		// Strafe and rotation || strafe
		else if (
			joystick.traction > 0.01f && (
			Mathf.Abs(joystick.round) > 0.01f &&
			Mathf.Abs(joystick.vertical) < 0.01f &&
			Mathf.Abs(joystick.horizontal) > 0.01f ||
			Mathf.Abs(joystick.round) < 0.01f &&
			Mathf.Abs(joystick.vertical) < 0.01f &&
			Mathf.Abs(joystick.horizontal) > 0.01f))
		{
			animator.speed = Mathf.Max(Mathf.Abs(joystick.round), Mathf.Abs(joystick.horizontal));

			IsWalkAndStrafe = false;
			IsWalkAndRotation = false;
			IsStrafeAndRotation = true;
		}

		animator.SetBool("IsWalkAndStrafe", IsWalkAndStrafe);
		animator.SetBool("IsWalkAndRotation", IsWalkAndRotation);
		animator.SetBool("IsStrafeAndRotation", IsStrafeAndRotation);
	}
}
