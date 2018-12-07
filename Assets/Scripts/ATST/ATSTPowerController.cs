using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTPowerController : MonoBehaviour {

	public bool state { get; private set; }

	// Use this for initialization
	void Start ()
	{
		state = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("PowerOn"))
		{
			state = !state;
		}
	}
}
