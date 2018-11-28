using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RobotMovement : MonoBehaviour
{
	public AnimationCurve FootAnimationCurveLeft;
	public AnimationCurve FootAnimationCurveRight;

	public Rigidbody LeftFoot;
	public Rigidbody RightFoot;

	public float Speed = 10.0f;
	public float FootLength = 2.0f;
	public float FootHeight = 2.0f;

	private float _timer;

	private void Start()
	{
		Assert.IsNotNull(LeftFoot);
		Assert.IsNotNull(RightFoot);
	}

	private void Update()
	{
		FootAnimationCurveLeft.Evaluate(_timer);

		_timer = Mathf.Repeat(Time.deltaTime * _timer, 1.0f);
	}
}
