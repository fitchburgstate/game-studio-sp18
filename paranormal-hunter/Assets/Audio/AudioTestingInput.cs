using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestingInput : MonoBehaviour {
	
	[Header("Test Inputs")]
	public KeyCode Footstep = KeyCode.F;
	public KeyCode PlayerHit = KeyCode.H;
	
	void Update () {
		if(Input.GetKeyDown(Footstep)){
			PlayFootsteps();
		}
		if(Input.GetKeyDown(PlayerHit)){
			PlayPlayerHit();
		}
	}
	
	/* All Player SFX should be posted with a reference to the player object,
		So that Fabric can track their 3D position and instances.
		If this isn't possible, just remove the ", gameObject" */
	
	// Player footsteps
	void PlayFootsteps(){
		Fabric.EventManager.Instance.PostEvent("Footstep", gameObject);
	}
	// For when the player gets hit
	void PlayPlayerHit(){
		Fabric.EventManager.Instance.PostEvent("Player Hit", gameObject);
	}
}
