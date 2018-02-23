using UnityEngine;
using UnityEngine.UI;

namespace Interactable
{
    public class WeaponDisplay : MonoBehaviour
    {
        public WeaponItem weapon;
        public string nameOfImageSlot;
        public string nameOfWeaponSlot;
        public string weaponDamageSlot;
        public string weaponSpeedSlot;
        public string weaponDescriptionSlot;

        private GameObject imageSlot;
        private GameObject weaponSlot;
        private GameObject damageSlot;
        private GameObject speedSlot;
        private GameObject descriptionSlot;

        private Image weaponImage;
        private Text weaponText;
        private Text damageText;
        private Text speedText;
        private Text descriptionText;

        private void FindSlots() // gets all the gameobjects for the weapon display
        {
            imageSlot = GameObject.Find(nameOfImageSlot);
            weaponSlot = GameObject.Find(nameOfWeaponSlot);
            damageSlot = GameObject.Find(weaponDamageSlot);
            speedSlot = GameObject.Find(weaponSpeedSlot);
            descriptionSlot = GameObject.Find(weaponDescriptionSlot);
        }

        private void FillSlots() // Fills the gameobject for the weapon display with information from the weaponItem class
        {
            weaponImage = imageSlot.GetComponent<Image>();
            weaponImage.sprite = weapon.icon;
            weaponText = weaponSlot.GetComponent<Text>();
            weaponText.text = weapon.nameOfWeapon;
            damageText = damageSlot.GetComponent<Text>();
            damageText.text = weapon.weaponDamage;
            speedText = speedSlot.GetComponent<Text>();
            speedText.text = weapon.weaponSpeed;
            descriptionText = descriptionSlot.GetComponent<Text>();
            descriptionText.text = weapon.weaponDescription;
        }

        private void OnEnable()
        {
            FindSlots();
            FillSlots();
        }
    }
}
