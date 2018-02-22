using System.Collections;
using UnityEngine;

namespace Interactable
{
    [RequireComponent(typeof(Item))]
    public class InteractableObject : MonoBehaviour
    {
        [Header("Inventory item for this object")]
        public Item item;
        [Header("Speed of the object")]
        public float objectSpeed;

        public float bounceSpeed;
        public float bounceHeight;
        public float maxDistance;

        private float objectOffset;

        [Header("Animation the object plays in curve")]
        [SerializeField]
        private AnimationCurve curve;
        [SerializeField]
        private MeshRenderer mesh;
        private Collider objectCollider;
        private Vector3 targetPosition;

        private NavPosition navPosition = new NavPosition();

        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            objectCollider = GetComponent<Collider>();

            objectOffset = objectCollider.bounds.extents.y;

            if(navPosition.RandomPoint(transform.position, maxDistance, out targetPosition))
            {
                targetPosition.y += objectOffset;
            }

            objectCollider.enabled = false;
            StartCoroutine(PlayAnim());
        }

        private void OnMouseDown()
        {
            AddItemToInventory();
        }

        public void AddItemToInventory()
        {
            //TODO move inventory into Player
            var added = Inventory.instance.AddItem(item);

            if(added == true)
            {
                DisableMesh();
            }
        }

        private void DisableMesh()
        {
            mesh.enabled = false;
        }

        private void EnableCollider()
        {
            objectCollider.enabled = true;
        }

        private IEnumerator PlayAnim()
        {
            var inRange = 0.1f;
            float bounceTime = 0;

            while (Vector3.Distance(transform.position, targetPosition) > inRange)
            {
                bounceTime += bounceSpeed * Time.deltaTime;
                var bounceAmount = curve.Evaluate(bounceTime);
                var bouncePosition = new Vector3(0,0,0);
                bouncePosition.y = (bounceHeight * bounceAmount);
                var currentPosition = new Vector3(targetPosition.x, bouncePosition.y + objectOffset, targetPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, currentPosition, objectSpeed * Time.deltaTime);
                yield return null;
            }
            
            EnableCollider();

        }
    }
}
