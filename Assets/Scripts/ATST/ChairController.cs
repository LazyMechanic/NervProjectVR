using ChairControl.ChairWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour {

    public float pitch = 0;
    public float roll = 0;
    public int portNumber = 3;

    private Vector3 _lastPosition;
    private Vector3 _lastVelocity;

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

        _chairController = new FutuRiftController(portNumber)
        {
            Pitch = pitch,
            Roll = roll
        };
        //_chairController.Start();
    }

    void Update()
    {
        _forward = -transform.right;
        _back = -_forward;
        _left = -transform.forward;
        _right = -_left;
        _up = transform.up;
        _down = -_up;

        Vector3 curPosition = transform.position;
        Vector3 curVelocity = (curPosition - _lastPosition) / Time.deltaTime;
        Vector3 curAcceleration = (curVelocity - _lastVelocity) / Time.deltaTime;

		Matrix4x4 A = new Matrix4x4();
		A.SetColumn(0, new Vector4(_forward.x,	_forward.y,	_forward.z,	0));
		A.SetColumn(1, new Vector4(_right.x,	_right.y,	_right.z,	0));
		A.SetColumn(2, new Vector4(_up.x,		_up.y,		_up.z,		0));
		A.SetColumn(3, new Vector4(0,			0,			0,			1));

		A = A.inverse;

		// X - right/left,
		// Y - up/down
		// Z - forward/back
		Vector3 curLocalAcceleration = A.MultiplyPoint3x4(curAcceleration);

		//Debug.Log("|curLocalAcceleration| = " + curLocalAcceleration.magnitude);
		//Debug.Log("curLocalAcceleration   = " + curLocalAcceleration);
		//Debug.Log("|curAcceleration| = " + curAcceleration.magnitude);
		//Debug.Log("curAcceleration   = " + curAcceleration);

		//Debug.DrawLine(curPosition, curPosition + curAcceleration, Color.red);

		_chairController.Pitch = pitch;
        _chairController.Roll = roll;

        _lastPosition = curPosition;
        _lastVelocity = curVelocity;
    }    

    void OnApplicationQuit()
    {
        _chairController.Stop();
    }
}
