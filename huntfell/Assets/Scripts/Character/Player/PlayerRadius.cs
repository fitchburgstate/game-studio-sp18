using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter {

    public class PlayerRadius : MonoBehaviour {

        public List<Transform> interactableInRadiusTransform = new List<Transform>();

        private void OnTriggerEnter (Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactable.IsImportant() && interactableInRadiusTransform == null)
            {
                interactableInRadiusTransform.Add(other.transform);
            }
        }
    }
}
