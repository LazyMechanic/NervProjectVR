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

	public void AnimationUpdate(float horizontal, float vertical, float round, bool power)
	{
		animator.SetFloat("Round", round);
		animator.SetFloat("Vertical", vertical);
		animator.SetFloat("Horizontal", horizontal);
		animator.SetBool("Power", power);

		animator.speed = 1.0f;

		// If mech turns off then no animate other animation
		if (!power)
		{
			return;
		}

		// Idle
		if (Mathf.Abs(round) < 0.01f &&
			Mathf.Abs(vertical) < 0.01f &&
			Mathf.Abs(horizontal) < 0.01f)
		{
			IsWalkAndStrafe = false;
			IsWalkAndRotation = false;
			IsStrafeAndRotation = false;
		}
		// Walk and strafe || walk
		else if (
			Mathf.Abs(round) < 0.01f &&
			Mathf.Abs(vertical) > 0.01f &&
			Mathf.Abs(horizontal) > 0.01f ||
			Mathf.Abs(round) < 0.01f &&
			Mathf.Abs(vertical) > 0.01f &&
			Mathf.Abs(horizontal) < 0.01f)
		{
			animator.speed = Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(horizontal));
		
			IsWalkAndStrafe = true;
			IsWalkAndRotation = false;
			IsStrafeAndRotation = false;
		}
		// Walk and rotation || rotation
		else if (
			Mathf.Abs(round) > 0.01f &&
			Mathf.Abs(vertical) > 0.01f &&
			Mathf.Abs(horizontal) < 0.01f ||
			Mathf.Abs(round) > 0.01f &&
			Mathf.Abs(vertical) < 0.01f &&
			Mathf.Abs(horizontal) < 0.01f)
		{
			animator.speed = Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(round));

			IsWalkAndStrafe = false;
			IsWalkAndRotation = true;
			IsStrafeAndRotation = false;
		}
		// Strafe and rotation || strafe
		else if (
			Mathf.Abs(round) > 0.01f &&
			Mathf.Abs(vertical) < 0.01f &&
			Mathf.Abs(horizontal) > 0.01f ||
			Mathf.Abs(round) < 0.01f &&
			Mathf.Abs(vertical) < 0.01f &&
			Mathf.Abs(horizontal) > 0.01f)
		{
			animator.speed = Mathf.Max(Mathf.Abs(round), Mathf.Abs(horizontal));

			IsWalkAndStrafe = false;
			IsWalkAndRotation = false;
			IsStrafeAndRotation = true;
		}

		animator.SetBool("IsWalkAndStrafe", IsWalkAndStrafe);
		animator.SetBool("IsWalkAndRotation", IsWalkAndRotation);
		animator.SetBool("IsStrafeAndRotation", IsStrafeAndRotation);
	}
}
