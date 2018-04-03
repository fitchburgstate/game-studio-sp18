using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hunter {
    public class HUDManager : MonoBehaviour {

        [HideInInspector]
        public static HUDManager instance;

        public Canvas hudCanvas;
        public Image healthBar;
        public Image woundBar;
        public Image staminaBar;

        public GameObject damagePopUpPrefab;

        public void Awake ()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            if(hudCanvas == null)
            {
                Debug.LogWarning("There is no HUD Cavnas set in the HUD Manager inspector. Please make sure to set this field. Defaulting to finding any Canvas.");
                hudCanvas = FindObjectOfType<Canvas>();
            }
        }

        
    }
}
