using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class Interactable : MonoBehaviour
    {
        [Header("Iventroy item")]
        public Item item;
        [HideInInspector]
        public bool spawnedFromProp = false; // equal true if object is spawned from an interactable prop
        public bool spawnedFromDestruct = false;
        

        //TODO Math for the Anim function
        public float propSpeed;
        public float bounceSpeed;
        public float bounceHeight;
        public float maxDistance;   
        
        [Tooltip("Animation the prop plays")]
        [SerializeField]
        private AnimationCurve curve;
        private MeshRenderer mesh;
        private Collider propCollider;
        private float propHeightOffset;
        private Vector3 targetPosition;
        private NavPosition navPosition = new NavPosition();

        public void AddItemToInventory() // adds this item the the inventroy script item list and disable mesh
        {
            //TODO move inventory into Player
            var added = Inventory.instance.AddItem(this);

            if (added == true)
            {
                mesh.enabled = false;
                propCollider.enabled = false;
            }
        }

        private void SpawnFromProp() // when object is spawned from an interactable prop
        {         
            propHeightOffset = propCollider.bounds.extents.y; // get half the height of the object

            if (navPosition.RandomPoint(transform.position, maxDistance, out targetPosition)) // gets random position on a nav mesh + hald the height of the object on the y axis
            {
                targetPosition.y += propHeightOffset;
            }

            propCollider.enabled = false; //  disable collider when item spawns
            StartCoroutine(PlayAnim()); // plays animation curve
        }

        private void OnMouseDown()
        {
            AddItemToInventory();
        }

        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            propCollider = GetComponent<Collider>();

            if (spawnedFromProp == true)
            {
                SpawnFromProp();
            }
            else if(spawnedFromDestruct == true)
            {
                SpawnOnGround();
            }
        }

        private bool CheckForWall()
        {
            var hit = Physics.Raycast(transform.position, transform.forward, 1);
            return hit;
        }

        private void SpawnOnGround()
        {
            var groundPosition = new Vector3(transform.position.x, targetPosition.y, targetPosition.z);
            transform.position = groundPosition;
        }

        private IEnumerator PlayAnim() // plays animation for object to move to point on navmesh
        {
            var inRange = 0.1f; // range for how close object has to get to destination
            float bounceTime = 0; // bounce speed

            while (Vector3.Distance(transform.position, targetPosition) > inRange)
            {
                bounceTime += bounceSpeed * Time.deltaTime;
                var bounceAmount = curve.Evaluate(bounceTime); // how big is the bounce
                var bouncePosition = new Vector3(0, 0, 0)
                {
                    y = (bounceHeight * bounceAmount) // the bounch changing the y axis of the object
                };
                var currentPosition = new Vector3(targetPosition.x, bouncePosition.y + propHeightOffset, targetPosition.z); // the nav mesh position x and z axis and the bounce's y axis
                transform.position = Vector3.MoveTowards(transform.position, currentPosition, propSpeed * Time.deltaTime); // moves toawrds that current position
                yield return null;
            }

            propCollider.enabled = true; // enables collider when reaches current position
        }     
    }
}
