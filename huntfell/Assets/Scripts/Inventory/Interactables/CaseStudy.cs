using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;
using System.Collections;

namespace Hunter
{
    class CaseStudy : InteractableInventoryItem
    {
        public override void FireInteraction (Character characterTriggeringInteraction)
        {
            if (characterTriggeringInteraction.tag == "Player" && GameManager.instance != null)
            {
                GameManager.instance.LoadNewScene("UI_End", false);
            }
        }
    }
}
