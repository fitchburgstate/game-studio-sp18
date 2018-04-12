using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;
using System.Collections;

namespace Hunter
{
    class CaseStudy : InteractableInventoryItem
    {
        public override void Interact (Character.Character characterTriggeringInteraction)
        {
            if (characterTriggeringInteraction.tag == "Player" && GameManager.instance != null)
            {
                if (characterTriggeringInteraction is Player) { (characterTriggeringInteraction as Player).PlayPickupAnimation(transform); }
                StartCoroutine(EndGame());
            }
        }

        private IEnumerator EndGame ()
        {
            GameManager.instance.LoadNewScene("UI_End", false);
            yield return null;
        }
    }
}
