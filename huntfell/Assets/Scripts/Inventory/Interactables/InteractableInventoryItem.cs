using System.Collections;
using UnityEngine;

namespace Hunter
{
    public class InteractableInventoryItem : MonoBehaviour
    {
        //All the relevent data about this item
        public InventoryItem itemData;

        //Math variables for the Anim function
        public float propSpeed = 2;
        public float bounceSpeed = 1;
        public float bounceHeight = 3;
        public float maxDistance = 10;   
        
        [Tooltip("Animation the prop plays")]
        [SerializeField]
        private AnimationCurve propCurve;

        private Collider interactableItemCollider;
        private float propHeightOffset; // half the height of the object
        private Vector3 targetPosition;

        private void Awake()
        {
            interactableItemCollider = GetComponent<Collider>();
        }

        public void AddItemToInventory()
        {
            if (InventoryManager.instance.TryAddItem(itemData, this))
            {
                transform.SetParent(InventoryManager.instance.transform);
                gameObject.SetActive(false);
            }
        }

        public void SpawnFromProp() // when object is spawned from an interactble prop
        {         
            propHeightOffset = interactableItemCollider.bounds.extents.y; // get half the height of the object so it doesnt go in the ground

            //Making it so it always spawns somewhere near the player and not behind the prop it came from
            if (Utility.RandomNavMeshPoint(GameObject.FindGameObjectWithTag("Player").transform.position, maxDistance, out targetPosition)) // gets random position on a nav mesh + hald the hieght of the object on the y axis
            {
                targetPosition.y += propHeightOffset;
            }

            interactableItemCollider.enabled = false; //  disable colider when item spawns
            StartCoroutine(PlaySpawnAnimation()); // plays anination curve
        }

        private IEnumerator PlaySpawnAnimation() // plays animation for object to move to point on navmesh
        {
            //TODO this whole thing needs to be refactored
            var inRange = 0.2f; // range for how close object has to get to destination
            float bounceTime = 0; // bounce speed

            while (Vector3.Distance(transform.position, targetPosition) > inRange)
            {
                //Debug.Log(Vector3.Distance(transform.position, targetPosition) + " -- " + inRange);
                bounceTime += bounceSpeed * Time.deltaTime;
                var bounceAmount = propCurve.Evaluate(bounceTime); // how big is the bounce
                var bouncePosition = new Vector3(0, 0, 0)
                {
                    y = (bounceHeight * bounceAmount) // the bounch changing the y axis of the object
                };
                var currentPosition = new Vector3(targetPosition.x, bouncePosition.y + propHeightOffset, targetPosition.z); // the nav mesh position x and z axis and the bounce's y axis
                transform.position = Vector3.MoveTowards(transform.position, currentPosition, propSpeed * Time.deltaTime); // moves towards that current position
                yield return null;
            }

            interactableItemCollider.enabled = true; // enables collider when reaches current position
        }

        public void SpawnOnGround()
        {
            // spawn the interactable on the ground below
            var groundPosition = new Vector3(transform.position.x, targetPosition.y, targetPosition.z);
            transform.position = groundPosition;
            // change to give it small radius
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Something is in the prop.");
            if(other.gameObject.tag == "Player")
            {
                AddItemToInventory();
            }
        }
    }
}
