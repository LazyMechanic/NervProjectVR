using System;
using UnityEngine;
using UnityEngine.Assertions;

public class TestMechMovementController : MonoBehaviour {

	public Rigidbody LeftFoot;
	public Rigidbody RightFoot;

	public Rigidbody Waist;

	public Rigidbody LeftHip;
	public Rigidbody RightHip;

	public Transform LeftFootCheckGround;
	public Transform RightFootCheckGround;

	public LayerMask GroundMask;

	public float StepForce = 3000.0f;
	public float StepSpeed = 3.0f;
	public float StepAmplitude = 3500.0f;

	public bool IsForwardMove = true;

	public float HitDistance = 0.0001f;

	public bool UseFootMass = false;

	[SerializeField] private float _footOriginMass;

	[SerializeField] private bool _isLeftGrounded = false;
	[SerializeField] private bool _isRightGrounded = false;

	[SerializeField] private float _timeLine = 0.0f;
	[SerializeField] private float _sin = 0.0f;
	[SerializeField] private float _deltaSin = 0.0f;
	[SerializeField] private float _cos = 1.0f;
	private float _prevSin = 0.0f;

	private Quaternion _curCockpitRotation;
	
	// Use this for initialization
	void Start () {
		Assert.IsNotNull(LeftFoot, "LeftFoot Rigidbody is null.");
		Assert.IsNotNull(RightFoot, "RightFoot Rigidbody is null.");

		Assert.IsNotNull(Waist, "Waist Rigidbody is null.");

		Assert.IsNotNull(LeftHip, "LeftHip Rigidbody is null.");
		Assert.IsNotNull(RightHip, "RightHip Rigidbody is null.");

		Assert.IsNotNull(LeftFootCheckGround, "LeftFootCheckGround is null.");
		Assert.IsNotNull(RightFootCheckGround, "RightFootCheckGround is null.");

		Assert.IsNotNull(RightFootCheckGround, "GroundMask is null.");

		_footOriginMass = LeftFoot.mass;
	}

	// Update is called once per frame
	void Update()
	{
		_isLeftGrounded = Physics.Raycast(LeftFootCheckGround.transform.position, LeftFootCheckGround.transform.TransformDirection(Vector3.down) * HitDistance, GroundMask);
		_isRightGrounded = Physics.Raycast(RightFootCheckGround.transform.position, RightFootCheckGround.transform.TransformDirection(Vector3.down) * HitDistance, GroundMask);

		if (IsForwardMove)
		{
			MoveForward();
		}
	}

	void MoveForward()
	{
		_prevSin = _sin;
		_sin = Mathf.Sin(_timeLine) * StepAmplitude;
		_deltaSin = Mathf.Abs(_prevSin) - Mathf.Abs(_sin);

		_cos = Mathf.Cos(_timeLine);

		Vector3 frontDirection = Vector3.left * Time.deltaTime * StepForce;

		if (_sin > 0)
		{
			UpdatePosition(RightFoot, frontDirection);
		}
		else if (_sin < 0)
		{
			UpdatePosition(LeftFoot, frontDirection);
		}

		//LeftHip.transform.rotation = Waist.transform.rotation;
		//RightHip.transform.rotation = Waist.transform.rotation;

		_timeLine += Time.deltaTime * StepSpeed;
	}

	void UpdatePosition(Rigidbody body, Vector3 direction)
	{
		body.AddForce(Vector3.up * _deltaSin);
		body.AddForce(direction);
		//body.gameObject.transform.position += Vector3.up * _dSin;

		if (UseFootMass)
		{
			//body.mass = Mathf.Clamp(_footOriginMass * Mathf.Abs(_cos), 1, _footOriginMass);
		}
	}
}
