using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DancingBats : MonoBehaviour {

	private void Start () {
        GetComponent<Animator>().SetTrigger("dance");
	}
}
