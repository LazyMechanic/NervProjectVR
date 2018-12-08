using ChairControl.ChairWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChairController : MonoBehaviour {

    public float pitch = 0;
    public float roll = 0;
    public int portNumber = 6;

	public Transform chairBone;
	public HeadController headController;
	public ATSTMovementController atstController;

	public Vector3 lastChairBoneRotation;

	private const float _chairMaxVelocity = 9.8f;

	private float _horizontalAcceleration = 0.0f;
	private float _verticalAcceleration = 0.0f;

    private FutuRiftController _chairController;

	private bool _firstFootSoundPlayed = false;

    private Vector3 _forward = new Vector3(-1, 0, 0);
    private Vector3 _back = new Vector3(1, 0, 0);
    private Vector3 _left = new Vector3(0, 0, -1);
    private Vector3 _right = new Vector3(0, 0, 1);
    private Vector3 _up = new Vector3(0, 1, 0);
    private Vector3 _down = new Vector3(0, -1, 0);

	private void Awake()
	{
		Assert.IsNotNull(chairBone, "[ChairController]: Chair Bone is null");
		Assert.IsNotNull(headController, "[ChairController]: Head Controller is null");
		Assert.IsNotNull(atstController, "[ChairController]: Atst Controller is null");
	}

	private void Start()
    {
		_chairController = new FutuRiftController(portNumber);

		UpdateChairRotation();
        _chairController.Start();
    }

	private void Update()
    {
		UpdateNormals();
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

	private void CalculateRotation()
	{
		float zAngle = chairBone.localEulerAngles.z;
		float xAngle = chairBone.localEulerAngles.x;

		if (zAngle > 180)
		{
			zAngle = -(360.0f - zAngle);
		}

		if (xAngle > 180)
		{
			xAngle = -(360.0f - xAngle);
		}

		// Bringin pitch from -14 to 14
		float tempPitch = zAngle + headController.rotationX * (7 / headController.maxRotationY);
		float tempRoll = xAngle;

		// If start from idle then sets slow acceleration to
		// opposite direction than velocity direction
		if (atstController.atstRigidbody.velocity != atstController.lastVelocity &&
			atstController.lastVelocity == Vector3.zero)
		{			
			// Projects
			tempPitch -= 3 * (Vector3.Dot(atstController.atstRigidbody.velocity, -atstController.atstRigidbody.transform.right) / (-atstController.atstRigidbody.transform.right.magnitude));
			tempRoll -= 3 * (Vector3.Dot(atstController.atstRigidbody.velocity, atstController.atstRigidbody.transform.forward) / (atstController.atstRigidbody.transform.forward.magnitude));
		}

		//Debug.Log(chairBone.eulerAngles);

		pitch = -tempPitch; // set minus because opposite angle
		roll = tempRoll;
	}

	private void UpdateChairRotation()
	{
		_chairController.Pitch = pitch;
		_chairController.Roll = roll;

		lastChairBoneRotation = chairBone.localEulerAngles;
	}

    void OnApplicationQuit()
    {
        _chairController.Stop();
    }
}
