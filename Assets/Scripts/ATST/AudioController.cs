using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AudioController : MonoBehaviour {

	public ATSTAnimationController animationController;
	public ChairController chairController;
	public ATSTPowerController power;

	public AudioClip engineWorkAudio;
	public AudioClip powerOnAudio;
	public AudioClip powerOffAudio;

	public AudioSource lFootAudioSource;
	public AudioSource rFootAudioSource;
	public AudioSource engineWorkAudioSource;
	public AudioSource powerAudioSource;

	private bool _footSoundPlayed = false;
	private bool _engineSoundPlayed = false;

	private void Awake()
	{
		Assert.IsNotNull(animationController, "[AudioController]: Animation Controller is null");
		Assert.IsNotNull(chairController, "[AudioController]: Chair Controller is null");
		Assert.IsNotNull(power, "[AudioController]: Power Controller is null");
		Assert.IsNotNull(engineWorkAudio, "[AudioController]: Engine Work Audio is null");
		Assert.IsNotNull(powerOnAudio, "[AudioController]: Power On Audio is null");
		Assert.IsNotNull(powerOffAudio, "[AudioController]: Power Off Audio is null");
		Assert.IsNotNull(lFootAudioSource, "[AudioController]: Left Foot Audio Source is null");
		Assert.IsNotNull(rFootAudioSource, "[AudioController]: Right Foot Audio Source is null");
		Assert.IsNotNull(engineWorkAudioSource, "[AudioController]: Engine Work Audio Source is null");
		Assert.IsNotNull(powerAudioSource, "[AudioController]: Power Audio Source is null");
	}

	// Use this for initialization
	void Start ()
	{
		engineWorkAudioSource.loop = true;
		engineWorkAudioSource.Stop();
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
			powerAudioSource.Stop();
			powerAudioSource.clip = powerOnAudio;
			powerAudioSource.Play();
		}
		else if (!power.state && power.lastState)
		{
			powerAudioSource.Stop();
			powerAudioSource.clip = powerOffAudio;
			powerAudioSource.Play();
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

			if (dChair < 0 && !_footSoundPlayed)
			{
				// Play sound
				// ....
				if (xCurAngle > 0)
				{
					rFootAudioSource.Play();
				}
				else
				{
					lFootAudioSource.Play();
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
			engineWorkAudioSource.Stop();
			engineWorkAudioSource.loop = true;
			engineWorkAudioSource.clip = engineWorkAudio;
			engineWorkAudioSource.Play();
			_engineSoundPlayed = true;
		}
		else if (!power.state && _engineSoundPlayed)
		{
			engineWorkAudioSource.Stop();
			_engineSoundPlayed = false;
		}
	}
}
