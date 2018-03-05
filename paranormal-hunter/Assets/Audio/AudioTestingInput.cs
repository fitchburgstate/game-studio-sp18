using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestingInput : MonoBehaviour {
	public KeyCode Footstep = KeyCode.F;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(Footstep)){
			PlayFootsteps();
		}
	}
	void PlayFootsteps(){
		Fabric.EventManager.Instance.PostEvent("Footstep");
	}
}
