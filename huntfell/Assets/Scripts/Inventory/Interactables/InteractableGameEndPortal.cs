using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;
using System.Collections;

namespace Hunter
{
    public class InteractableGameEndPortal : InteractableInventoryItem
    {
        public string sceneName;

        public override void FireInteraction (Character characterTriggeringInteraction)
        {
            if (characterTriggeringInteraction.tag == "Player" && GameManager.instance != null)
            {
                GameManager.instance.DeviceManager.PauseInputEnabled = false;
                GameManager.instance.DeviceManager.GameInputEnabled = false;
                GameManager.instance.LoadNewScene(sceneName, false);
            }
        }
    }
}
