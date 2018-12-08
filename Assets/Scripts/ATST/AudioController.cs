using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public ATSTAnimationController animationController;
	public ChairController chairController;
	public ATSTPowerController power;

	public AudioClip engineWork;
	public AudioClip powerOn;
	public AudioClip powerOff;

	public AudioSource lFoot;
	public AudioSource rFoot;
	public AudioSource engine;
	public AudioSource enginePower;

	private bool _footSoundPlayed = false;
	private bool _engineSoundPlayed = false;

	// Use this for initialization
	void Start ()
	{
		engine.loop = true;
		engine.Stop();
	}
	
	// Update is called once per frame
	void Update ()
	{
		PlaySound();
	}

	private void PlaySound()
	{
		Power();

		Engine();

		FootStep();
	}

	private void Power()
	{
		if (power.state && !power.lastState)
		{
			enginePower.Stop();
			enginePower.clip = powerOn;
			enginePower.Play();
		}
		else if (!power.state && power.lastState)
		{
			enginePower.Stop();
			enginePower.clip = powerOff;
			enginePower.Play();
		}
	}

	private void FootStep()
	{
		if (power.state && !animationController.isIdle)
		{
			float zCurAngle = chairController.chairBone.localEulerAngles.z;
			float xCurAngle = chairController.chairBone.localEulerAngles.x;

			float zLastAngle = chairController.lastChairBoneRotation.z;
			float xLastAngle = chairController.lastChairBoneRotation.x;

			float dChair = Mathf.Abs(zCurAngle) - Mathf.Abs(zLastAngle);

			Debug.Log(dChair);

			if (dChair < 0 && !_footSoundPlayed)
			{
				// Play sound
				// ....
				if (xCurAngle > 0)
				{
					rFoot.Play();
				}
				else
				{
					lFoot.Play();
				}

				_footSoundPlayed = true;
			}
			else if (dChair >= 0)
			{
				_footSoundPlayed = false;
			}
		}
	}

	private void Engine()
	{
		if (power.state && !_engineSoundPlayed)
		{
			engine.Stop();
			engine.loop = true;
			engine.clip = engineWork;
			engine.Play();
			_engineSoundPlayed = true;
		}
		else if (!power.state && _engineSoundPlayed)
		{
			engine.Stop();
			_engineSoundPlayed = false;
		}
	}
}
