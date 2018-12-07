using ChairControl.ChairWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour {

    public float pitch = 0;
    public float roll = 0;
    public int portNumber = 6;

	public Transform chairBone;

	public HeadController headController;

    private Vector3 _lastPosition;
    private Vector3 _lastVelocity;

	private Vector3 _curLocalAcceleration = new Vector3(0, 0, 0);
	private Vector3 _curLocalVelocity = new Vector3(0, 0, 0);

	private const float _chairMaxVelocity = 9.8f;

	private float _horizontalAcceleration = 0.0f;
	private float _verticalAcceleration = 0.0f;

    private FutuRiftController _chairController;

    private Vector3 _forward = new Vector3(-1, 0, 0);
    private Vector3 _back = new Vector3(1, 0, 0);
    private Vector3 _left = new Vector3(0, 0, -1);
    private Vector3 _right = new Vector3(0, 0, 1);
    private Vector3 _up = new Vector3(0, 1, 0);
    private Vector3 _down = new Vector3(0, -1, 0);

    void Start()
    {
        _lastPosition = transform.position;
        _lastVelocity = new Vector3(0, 0, 0);

        _chairController = new FutuRiftController(portNumber);

		UpdateChairRotation();
        _chairController.Start();
    }

    void Update()
    {
		UpdateNormals();
		//CalculateAcceleration();
		//CalculateVelocity();
		CalculateRotation();
		UpdateChairRotation();
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

	private void CalculateVelocity()
	{
		Vector3 curPosition = transform.position;
		Vector3 curVelocity = (curPosition - _lastPosition) / Time.deltaTime;

		_curLocalVelocity = GetLocalVelocity(curVelocity);

		_lastPosition = curPosition;
		_lastVelocity = curVelocity;
	}

	private Vector3 GetLocalVelocity(Vector3 globalVelocity)
	{
		Matrix4x4 A = new Matrix4x4();
		A.SetColumn(0, new Vector4(_forward.x, _forward.y, _forward.z, 0));
		A.SetColumn(1, new Vector4(_right.x, _right.y, _right.z, 0));
		A.SetColumn(2, new Vector4(_up.x, _up.y, _up.z, 0));
		A.SetColumn(3, new Vector4(0, 0, 0, 1));

		A = A.inverse;

		return A.MultiplyPoint3x4(globalVelocity);
	} 

	private void CalculateAcceleration()
	{
		Vector3 curPosition = transform.position;
		Vector3 curVelocity = (curPosition - _lastPosition) / Time.deltaTime;
		Vector3 curAcceleration = (curVelocity - _lastVelocity) / Time.deltaTime;

		// X - right/left,
		// Y - up/down
		// Z - forward/back
		_curLocalAcceleration = GetLocalAcceleration(curAcceleration);

		_lastPosition = curPosition;
		_lastVelocity = curVelocity;
	}

	private Vector3 GetLocalAcceleration(Vector3 globalAcceleration)
	{
		Matrix4x4 A = new Matrix4x4();
		A.SetColumn(0, new Vector4(_forward.x, _forward.y, _forward.z, 0));
		A.SetColumn(1, new Vector4(_right.x, _right.y, _right.z, 0));
		A.SetColumn(2, new Vector4(_up.x, _up.y, _up.z, 0));
		A.SetColumn(3, new Vector4(0, 0, 0, 1));

		A = A.inverse;

		return A.MultiplyPoint3x4(globalAcceleration);
	}

	private void CalculateRotation()
	{
		//float AB = _curLocalVelocity.magnitude * Mathf.Cos(Vector3.Angle(_curLocalVelocity, _forward));
		//float BC = _curLocalVelocity.magnitude * Mathf.Cos(Vector3.Angle(_curLocalVelocity, _right));
		//float BD = Mathf.Sqrt(9.8f * 9.8f - _curLocalVelocity.magnitude * _curLocalVelocity.magnitude);

		//pitch = Mathf.Atan2(BD, AB) * Mathf.Sign(_curLocalVelocity.x) * Mathf.Rad2Deg / 3;
		//roll = Mathf.Atan2(BD, BC) * Mathf.Sign(_curLocalVelocity.z) * Mathf.Rad2Deg / 3;

		float tempPitch = chairBone.eulerAngles.z + Mathf.Clamp(headController.rotationX, -4, 4);
		float tempRoll = chairBone.eulerAngles.x;

		if (tempPitch > 180)
		{
			tempPitch = -(360.0f - tempPitch);
		}

		if (tempRoll > 180)
		{
			tempRoll = -(360.0f - tempRoll);
		}

		//Debug.Log(chairBone.eulerAngles);

		pitch = -tempPitch; // set minus because opposite angle
		roll = tempRoll;
	}

	private void UpdateChairRotation()
	{
		_chairController.Pitch = pitch;
		_chairController.Roll = roll;
	}

    void OnApplicationQuit()
    {
        _chairController.Stop();
    }
}
