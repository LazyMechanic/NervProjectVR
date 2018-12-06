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

		Vector3 curForwardAcceleration = curAcceleration.magnitude * _forward;
		Vector3 curStrafeAcceleration = curAcceleration.magnitude * _right;



		//Vector3 curLocalAcceleration = new Vector3()
  //      {
  //          x = localXAcceleration,
  //          y = localYAcceleration,
  //          z = localZAcceleration
  //      };

        Debug.Log("curAcceleration = " + curAcceleration);
        Debug.Log("curLocalAcceleration = " + curAcceleration);

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
