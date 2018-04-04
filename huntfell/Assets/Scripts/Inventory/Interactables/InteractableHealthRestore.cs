using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter
{
    class InteractableHealthRestore : InteractableInventoryItem
    {
        public int healthAmount = 20;

        protected override void OnTriggerEnter (Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Player>()?.RestoreHealth(healthAmount);
                Destroy(gameObject);
            }
        }
    }
}
