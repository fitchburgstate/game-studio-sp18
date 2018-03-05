using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestingInput : MonoBehaviour {
	[Header("Test Inputs")]
	public KeyCode Footstep = KeyCode.F;
	public KeyCode PlayerHit = KeyCode.H;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(Footstep)){
			PlayFootsteps();
		}
		if(Input.GetKeyDown(PlayerHit)){
			PlayPlayerHit();
		}
	}
	// Player footsteps
	/* In the future will create overrides within character controller
		for differing stuff. Also do enemies need steps? */
	void PlayFootsteps(){
		Fabric.EventManager.Instance.PostEvent("Footstep");
	}
	// For when the player gets hit
	void PlayPlayerHit(){
		Fabric.EventManager.Instance.PostEvent("Player Hit");
	}
}
