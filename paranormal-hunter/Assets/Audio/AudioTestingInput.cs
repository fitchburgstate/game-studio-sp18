using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestingInput : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F)){
			PlayFootsteps();
		}
	}
	void PlayFootsteps(){
		Fabric.EventManager.Instance.PostEvent("Footstep");
	}
}
