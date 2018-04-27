using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private void FixedUpdate ()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
