using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSTFireControll : MonoBehaviour {

	[SerializeField] private Rigidbody _leftLaserPrefab;
	[SerializeField] private Rigidbody _rightLaserPrefab;

	public Transform originTransform;

	private bool _fire;

	private Vector3 _leftLaserPosition = new Vector3(6e-05f, 0.00222f, -0.00054f);
	private Vector3 _rightLaserPosition = new Vector3(8e-05f, 0.00222f, 0.00054f);


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		InputFireState();

	}

	private void InputFireState()
	{
		_fire = Input.GetButtonDown("Fire");
	}

	private void OnFire()
	{

	}

	public void OnLaserEnter()
	{

	}

	//private GameObject MakeLasers()
	//{
	//	//GameObject.Instantiate()
	//	//Rigidbody bullet = Instantiate(prefab);

	//	//bullet.AddForce()

	//}
}
