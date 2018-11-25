using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class AngleLimit
{
	public float Min = 0.0f;
	public float Max = 360.0f;
}

public class IKController : MonoBehaviour {

	public bool IsIKEnable = true;

	#region Transforms
	public Transform TargetIK;
	public Transform ElbowIK;

	public Transform RootBone;
	public Transform EndBone;
	#endregion

	#region Angle limits
	public bool HasXRotation = true;
	public bool HasYRotation = true;
	public bool HasZRotation = true;

	public AngleLimit XAngleLimit = new AngleLimit();
	public AngleLimit YAngleLimit = new AngleLimit();
	public AngleLimit ZAngleLimit = new AngleLimit();
	#endregion

	private void Start ()
	{
		
	}

	private void Update ()
	{
		if (IsIKEnable)
		{
			CalculateIK();
		}
	}

	private void CalculateIK()
	{
		EndBone.rotation = TargetIK.rotation;
	}
}
