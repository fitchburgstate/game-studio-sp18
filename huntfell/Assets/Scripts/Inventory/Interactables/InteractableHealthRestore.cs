using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;
using System.Collections;

namespace Hunter
{
    class InteractableHealthRestore : InteractableInventoryItem
    {
        public int healthAmount = 20;

        public override void Interact (Character.Character characterTriggeringInteraction)
        {
            if (characterTriggeringInteraction is Player) { (characterTriggeringInteraction as Player).PlayPickupAnimation(transform); }
            StartCoroutine(RestoreHealthToCharacter(characterTriggeringInteraction));
        }

        private IEnumerator RestoreHealthToCharacter (Character.Character characterTriggeringInteraction)
        {
            yield return new WaitForSeconds(1.25f);
            characterTriggeringInteraction.RestoreHealthToCharacter(healthAmount);
            gameObject.SetActive(false);
            yield return null;
        }
    }
}
