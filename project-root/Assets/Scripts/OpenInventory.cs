using UnityEngine;

namespace Intereractable
{
    public class OpenInventory : MonoBehaviour
    {
        public GameObject inventory;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventory.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                inventory.SetActive(false); 
            }

        }
    }
}
