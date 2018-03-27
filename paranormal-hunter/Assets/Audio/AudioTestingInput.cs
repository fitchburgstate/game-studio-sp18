﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestingInput : MonoBehaviour {
	
	[Header("Test Inputs - Player")]
	public KeyCode Footstep = KeyCode.F;
	public KeyCode PlayerHit = KeyCode.H;
	[Header("Test Inputs - Sword")]
	public KeyCode PlayerSwordSwing = KeyCode.S;
	[Header("Test Inputs - Luger")]
	public KeyCode PlayerDrawLuger = KeyCode.D;
	public KeyCode PlayerLugerShot = KeyCode.L;
	[Header("Test Inputs - Mob")]
	public KeyCode WolfAttack = KeyCode.W;
	
	void Update () {
		
		/* Player Events */
		if(Input.GetKeyDown(Footstep)){
			PlayFootsteps();
		}
		if(Input.GetKeyDown(PlayerHit)){
			PlayPlayerHit();
		}

		/* Sword Events */
		if(Input.GetKeyDown(PlayerSwordSwing)){
			PlayPlayerSwordSwing();
		}

		/* Ranged Weapon Events */
		if(Input.GetKeyDown(PlayerDrawLuger)){
			PlayDrawLuger();
		} 
		if(Input.GetKeyDown(PlayerLugerShot)){
			PlayLugerShot();
		}

		/* Mob Events */
		if(Input.GetKeyDown(WolfAttack)){
			PlayWolfAttack();
		}
	}
	
	/* All SFX should be posted with a reference to their parent object,
		So that Fabric can track their 3D position and instances.
		If this isn't possible, just remove the ", gameObject" */

	/* Player Events */
	// Player footsteps
	void PlayFootsteps(){
		Fabric.EventManager.Instance.PostEvent("Footstep", gameObject);
	}
	// For when the player gets hit
	void PlayPlayerHit(){
		Fabric.EventManager.Instance.PostEvent("Player Hit", gameObject);
	}

	/* Sword Events */
	void PlayPlayerSwordSwing(){
		Fabric.EventManager.Instance.PostEvent("Player Sword Swing", gameObject);
	}

	/* Ranged Weapon Events */
	// Luger Shot
	void PlayDrawLuger(){
		Fabric.EventManager.Instance.PostEvent("Player Draw Luger", gameObject);
	}
	void PlayLugerShot(){
		// Currently 1 shot pitch modulated, will fix later to be random container
		Fabric.EventManager.Instance.PostEvent("Player Luger Shot", gameObject);
	}

	/* Mob Events */
	void PlayWolfAttack(){
		// Currently has 1 attack modulated multiple times, will fix later
		Fabric.EventManager.Instance.PostEvent("Wolf Attack", gameObject);
	}
}
