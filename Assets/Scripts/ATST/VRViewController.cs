using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRViewController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InputTracking.Recenter();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("ResetVRCamera"))
		{
			InputTracking.Recenter();
		}
	}
}
