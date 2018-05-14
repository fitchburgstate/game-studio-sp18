using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;
using System.Collections;

namespace Hunter
{
    class InteractableDecanter : InteractableInventoryItem
    {
        public override void FireInteraction (Character characterTriggeringInteraction)
        {
            if (characterTriggeringInteraction is Player)
            {
                (characterTriggeringInteraction as Player).PotionCount++;
                if (HUDManager.instance != null) { HUDManager.instance.ShowItemPickupPrompt(itemData.itemName, itemData.icon); }
                gameObject.SetActive(false);
            }
        }
    }
}
