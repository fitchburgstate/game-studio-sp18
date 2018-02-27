﻿using System.Collections;
using UnityEngine;

namespace Interactable
{
    public class InteractableObject : MonoBehaviour
    {
        [Header("Inventory item for this object")]
        public Item item;
        [Header("Speed of the object")]
        public float objectSpeed;
        [Header("Speed of the bounce")]
        public float bounceSpeed;
        [Header("Hieght of the bounce")]
        public float bounceHeight;
        [Header("Distance the object can go out")]
        public float maxDistance;

        private float objectOffset;
        [Header("Animation the object plays in curve")]
        [SerializeField]
        private AnimationCurve curve;
        private MeshRenderer mesh;
        private Collider objectCollider;
        private Vector3 targetPosition;
        private NavPosition navPosition = new NavPosition();

        public void AddItemToInventory() // adds this item the the inventroy script item list and disable mesh
        {
            //TODO move inventory into Player
            var added = Inventory.instance.AddItem(item);

            if (added == true)
            {
                DisableMesh();
            }
        }

        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            objectCollider = GetComponent<Collider>();
            objectOffset = objectCollider.bounds.extents.y; // get half the height of the object

            if(navPosition.RandomPoint(transform.position, maxDistance, out targetPosition)) // gets random position on a nav mesh + hald the height of the object on the y axis
            {
                targetPosition.y += objectOffset;
            }

            objectCollider.enabled = false; //  disable collider when item spawns
            StartCoroutine(PlayAnim()); // plays animation curve
        }

        private void OnMouseDown()
        {
            AddItemToInventory();
        }

        private void DisableMesh()
        {
            mesh.enabled = false;
        }

        private void EnableCollider()
        {
            objectCollider.enabled = true;
        }

        private IEnumerator PlayAnim() // plays animation for object to move to point on navmesh
        {
            var inRange = 0.1f; // range for how close object has to get to destination
            float bounceTime = 0; // bounce speed

            while (Vector3.Distance(transform.position, targetPosition) > inRange)
            {
                bounceTime += bounceSpeed * Time.deltaTime;
                var bounceAmount = curve.Evaluate(bounceTime); // how big is the bounce
                var bouncePosition = new Vector3(0,0,0);
                bouncePosition.y = (bounceHeight * bounceAmount); // the bounch changing the y axis of the object
                var currentPosition = new Vector3(targetPosition.x, bouncePosition.y + objectOffset, targetPosition.z); // the nav mesh position x and z axis and the bounce's y axis
                transform.position = Vector3.MoveTowards(transform.position, currentPosition, objectSpeed * Time.deltaTime); // moves toawrds that current position
                yield return null;
            }     
            
            EnableCollider(); // enables collider when reaches current position
        }
    }
}
