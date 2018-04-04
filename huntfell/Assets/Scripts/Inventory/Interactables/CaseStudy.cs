using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter
{
    class CaseStudy : InteractableInventoryItem
    {
        protected override void OnTriggerEnter (Collider other)
        {
            if (other.gameObject.tag == "Player" && GameManager.instance != null)
            {
                GameManager.instance.LoadNewScene("UI_End", false);
            }
        }
    }
}
