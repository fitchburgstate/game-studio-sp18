using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterfaceScripts
{
    public class CameraFollow : MonoBehaviour
    {

        [Tooltip("This is the current object the camera is following.")]
        public Transform target;

        [Tooltip("Determines how much the camera is smoothing.")]
        public float smoothing = 5f;

        private Vector3 offset;
        private string deviceChangeText = ("Press Escape on the keyboard to change device inputs!"); // For debugging purposes

        void Start()
        {
            offset = transform.position - target.position;
        }

        void LateUpdate()
        {
            var targetCamPos = target.position + offset;
            
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }

        private void OnGUI() // For debugging purposes
        {
            deviceChangeText = GUI.TextField(new Rect(10, 10, 350, 20), deviceChangeText, 100);
        }

    }
}
