using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimEvent : MonoBehaviour {

	public void DestroyParent ()
    {
        Destroy(transform.parent.gameObject);
    }
}
