using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hunter {
    public class HUDManager : MonoBehaviour {

        [HideInInspector]
        public static HUDManager instance;
        [SerializeField]
        private Canvas hudCanvas;
        [SerializeField]
        public Image healthBar;
        [SerializeField]
        public Image woundBar;
        [SerializeField]
        public Image staminaBar;
        [SerializeField]
        private Image activeWeapon;
        [SerializeField]
        private Image inactiveWeapon;
        [SerializeField]
        private Image activeElement;
        [SerializeField]
        private Sprite nullElementSprite;

        public GameObject damagePopUpPrefab;

        public void Awake ()
        {
            if (instance == null)
            {
                instance = this;
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

        public void UpdateWeaponImage(Sprite newSprite)
        {
            inactiveWeapon.sprite = activeWeapon.sprite;
            activeWeapon.sprite = newSprite;
        }

        public void UpdateElementImage (Sprite newSprite)
        {
            if(newSprite == null)
            {
                activeElement.sprite = nullElementSprite;
                return;
            }
            activeElement.sprite = newSprite;
        }
    }
}
