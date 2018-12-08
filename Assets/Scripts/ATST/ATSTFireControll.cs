using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTFireControll : MonoBehaviour {

	public Rigidbody leftLaser;
	public Rigidbody rightLaser;

	private bool _fire;

	private Vector3 _leftLaserPosition = new Vector3(-0.00988f, 0.00062f, 0.01785f);
	private Vector3 _rightLaserPosition = new Vector3(-0.00987999f, 0.00062f, 0.01785f);


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateFireState();
		OnFire();
	}

	public void OnLaserEnter()
	{

	}

	private void UpdateFireState()
	{
		_fire = Input.GetButtonDown("Fire");
	}

	private void OnFire()
	{

	}
	
	//private GameObject MakeLasers()
	//{
	//	//GameObject.Instantiate()
	//	//Rigidbody bullet = Instantiate(prefab);

	//	//bullet.AddForce()
		
	//}
}
