﻿//———————————————————————–
// <copyright file=”TriggerListener.cs” game="KzzzZZZzzT!">
//     Copyright (c) Extreme Z7.  All rights reserved.
// </copyright>
//———————————————————————–
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CustomPropertyDrawers;

public class TriggerListener : MonoBehaviour
{
	//enums
	public enum ListenToActivation
	{
		Default,
		FirstFrameOnly}
	;

	//fields
	[Header ("Trigger Listener")]
	[Tooltip ("The Trigger Switches to listen to")]
	public List<TriggerSwitch> switches = new List<TriggerSwitch> ();
	[Tooltip ("The names will likely never be used, this is just for show")]
	public List<string> names = new List<string> ();

	[Space (10)]
	[Tooltip ("Default: Listen to any activation\n" +
	"First Frame Only: Listen only to the first frame of activation")]
	public ListenToActivation listenToActivation;
	[Tooltip ("Toggle this to also listen to the first frame of deactivation")]
	[ToggleLeft]
	public bool listenToFirstFrameOnDeactivate;
	[Tooltip ("This will reverse what the listener will tell the derived class" +
	" about the switch (i.e. On becomes Off, Off becomes On)")]
	[ToggleLeft]
	public bool flipActivation;

	ListenerRunner listener;
	//properties
	protected bool TriggerIsPaused { get; set; }

	protected Coroutine ListenerCoroutine { get; set; }

	protected bool Listening {
		get{ return listener != null && !TriggerIsPaused; }
	}

	protected ListenerRunner Listener {
		get {
			//If the listener has not been initialized, create a new instance
			if (listener == null) {
				listener = new ListenerRunner (this);
			}
			return listener;
		}
	}

	//methods
	void OnEnable ()
	{
		TriggerListenerManager.Instance.Register (this);
	}

	void OnDisable ()
	{
		TriggerListenerManager.Instance.Unregister (this);

		//I have to remove the instance of the listener here because disabling
		//the gameobject normally stops all coroutines. Because the listener starts
		// a coroutine in its constructor, a new instance needs to be created
		// when the Listener property is called again.
		listener = null;
	}

	public virtual void ManagedUpdate ()
	{
		if (Listening) {
			//For performance reasons, the listener is stopped
			//if this function is not overriden
			Listener.Stop ();
		}
	}

	/// <summary>This is normally called by the derived class on its OnValidate() MonoBehavior method</summary>
	///
	protected void ValidateTriggerListener ()
	{
		if (switches.Count <= 0) {
			switches = switches.Resize (1);
		}
            
		if (names.Count != switches.Count) {
			names = names.Resize (switches.Count);
		}

		for (int i = 0; i < switches.Count; i++) {
			if (switches [i] != null) {
				names [i] = switches [i].switchName;
			} else {
				foreach (TriggerSwitch triggerSwitch in GetComponents<TriggerSwitch>()) {
					if (!names [i].Equals ("") && names [i] == triggerSwitch.switchName) {
						switches [i] = triggerSwitch;
						names [i] = switches [i].switchName;
					}
				}
			}
		}
	}

	//nested types
	protected class ListenerRunner
	{
		//fields
		bool active;
		bool activatedOnCurrentFrame;
		bool deactivatedOnCurrentFrame;
		readonly TriggerListener listener;
		float pauseTime;

		//constructor
		public ListenerRunner (TriggerListener listener)
		{
			this.listener = listener;

			//Starts the coroutine that listens to the switches
			listener.ListenerCoroutine = (listener.StartCoroutine (CheckSwitches ()));
		}

		//properties
		public bool ActivatedOnCurrentFrame {
			get{ return activatedOnCurrentFrame; }
		}

		public bool DeactivatedOnCurrentFrame {
			get{ return deactivatedOnCurrentFrame; }
		}

		public bool IsActivated {
			get{ return (active || activatedOnCurrentFrame) != listener.flipActivation; }
		}

		/// <summary>Stops the running listener coroutine then drops the listener instance</summary>
		///
		public void Stop ()
		{ 
			listener.StopCoroutine (listener.ListenerCoroutine);
			listener.listener = null;
		}

		/// <summary>Pauses the listener instance for a certain number of seconds</summary>
		///
		public void Pause (float seconds)
		{
			if (seconds <= 0f) {
				throw new ArgumentException ("Seconds should be greater than 0!");
			}
			pauseTime = seconds;
		}

		/// <summary>Sets the bits that are set during the first frame of activation or deactivation</summary>
		/// <param name="active">Tells which bit to set</param>
		/// 
		void SetSingleFrameBits (bool active = true)
		{
			if (active) {
				activatedOnCurrentFrame = true;
			} else {
				deactivatedOnCurrentFrame = true;
			}
		}

		/// <summary>The coroutine that listens to the switches</summary>
		///
		IEnumerator CheckSwitches ()
		{
			for (;;) {
				foreach (TriggerSwitch triggerSwitch in listener.switches) {
					//Skip the elements that have not been set
					if (triggerSwitch == null) {
						continue;
					}

					//Ask if the listener has been asked to listen any activation
					if (listener.listenToActivation == ListenToActivation.Default) {
						active |= triggerSwitch.IsActivated;
					}

					//Ask if the listener has been asked to the first frame of activation
					if (listener.listenToActivation == ListenToActivation.FirstFrameOnly) {
						if (triggerSwitch.ActivatedOnCurrentFrame) {
							SetSingleFrameBits (!listener.flipActivation);
						}
					}

					//Ask if the listener has been asked to the first frame of deactivation
					if (listener.listenToFirstFrameOnDeactivate) {
						if (triggerSwitch.DeactivatedOnCurrentFrame) {
							SetSingleFrameBits (listener.flipActivation);
						}
					}
				}

				//Pause time happens here
				if (pauseTime > 0f) {
					listener.TriggerIsPaused = true;
					yield return new WaitForSeconds (pauseTime);
					listener.TriggerIsPaused = false;
					pauseTime = 0f;
				} else {
					yield return new WaitForEndOfFrame ();
				}

				//Reset all the bits before restarting the loop
				active = false;
				activatedOnCurrentFrame = false;
				deactivatedOnCurrentFrame = false;
			}
		}
	}
}