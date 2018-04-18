using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class TitleManager : MonoBehaviour
    {
        private DeviceManager myDeviceManager;
        public GameObject titlePanel;
        public GameObject tutorialPanel;

        private bool acceptingInput = true;

        private void Start ()
        {
            myDeviceManager = DeviceManager.Instance;

        }

        private void Update ()
        {
            if (acceptingInput & (myDeviceManager.PressedConfirm || myDeviceManager.PressedCancel))
            {
                if (titlePanel.activeSelf)
                {
                    Fabric.EventManager.Instance?.PostEvent("UI Navigation Blip");
                    titlePanel.SetActive(false);
                    tutorialPanel.SetActive(true);
                }
                else
                {
                    Fabric.EventManager.Instance?.PostEvent("UI Start Game");
                    GameManager.instance.LoadNewScene("PAXLevelScene", false);
                    acceptingInput = false;
                }
            }
        }
    }
}
