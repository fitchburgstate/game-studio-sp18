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

        public void SpawnDamageText (int damage, Transform targetLocation, Element element)
        {
            //Create the text pop-up from a prefab
            var damageTextRoot = Instantiate(damagePopUpPrefab, hudCanvas.transform, false);

            //Set its position to be relative to the character that got hurt
            var worldPos = Camera.main.WorldToScreenPoint(targetLocation.position);
            damageTextRoot.transform.position = worldPos;

            //Set the text to the proper color and damage amount
            var damageText = damageTextRoot.GetComponentInChildren<TextMeshPro>();
            damageText.SetText(damage.ToString());
            if(element != null) { damageText.color = element.elementColor; }
        }
    }
}
