using UnityEngine;

namespace Intereractable
{
    public class OpenInventory : MonoBehaviour
    {
        public GameObject inventory; // opens and closes the inventory used for testing purposes need work
        private bool openInventory = false; // if the inventroy is open or not

        void Update()
        {
            //TODO change funtion for controller support
            if (Input.GetKeyDown(KeyCode.I) && openInventory == false)
            {
                inventory.SetActive(true);
                openInventory = true;
            }
            else if (Input.GetKeyDown(KeyCode.I) && openInventory == true)
            {
                inventory.SetActive(false);
                openInventory = false;
            }
        }
    }
}
