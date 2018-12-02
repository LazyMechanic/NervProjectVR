using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTMovementController : MonoBehaviour {

	public Animator Animator;

	public Transform Transform;
	public Rigidbody Rigidbody;
	public CapsuleCollider Collider;

	public JoystickAxes Joystick;

	public float MovementSpeed = 30.0f;

	// Use this for initialization
	private void Start ()
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
		Joystick.Horizontal = Input.GetAxis("Horizontal");
		Joystick.Vertical = Input.GetAxis("Vertical");
		Joystick.Round = Input.GetAxisRaw("Round");

		Debug.Log("Horizontal = " + Joystick.Horizontal);
		Debug.Log("Vertical = " + Joystick.Vertical);
		Debug.Log("Round = " + Joystick.Round);
		//Debug.Log(Input.GetJoystickNames().);

		
	}

	private void FixedUpdate()
	{
		Rigidbody.velocity = new Vector3(-Joystick.Vertical, 0, Joystick.Horizontal) * Time.deltaTime * MovementSpeed;
	}
}
