using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;
using System.Collections;

namespace Hunter
{
    class InteractableHealthRestore : InteractableInventoryItem
    {
        public int healthAmount = 20;

        public override void FireInteraction (Character characterTriggeringInteraction)
        {
            characterTriggeringInteraction.Heal(healthAmount, true);
            gameObject.SetActive(false);
        }
    }
}
