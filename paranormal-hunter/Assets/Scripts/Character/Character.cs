using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

namespace Hunter.Character
{
    public abstract class Character : MonoBehaviour
    {
        /// <summary>
        /// Name of the Player
        /// </summary>
        public string playerName;
        /// <summary>
        /// Health of the Character Object
        /// </summary>
        public int health;
        /// <summary>
        /// Current Melee Weapon Equipped on the Character
        /// </summary>
        public Melee melee;
        /// <summary>
        /// Current Ranged Weapon Equipped on the Character
        /// </summary>
        public Range range;

        public Melee CurrentMeleeWeapon
        {
            get
            {
                return currentWeapon as Melee;
            }
        }

        public Range CurrentRangeWeapon
        {
            get
            {
                return currentWeapon as Range;
            }
        }

        private Weapon currentWeapon;

        private bool swap = false;


        public void SwitchWeapon()
        {
            swap = true;
            if (CurrentMeleeWeapon && swap == true)
            {
                melee.gameObject.SetActive(false);
                range.gameObject.SetActive(true);
                SetCurrentWeapon(range);
                swap = false;
            }
            if (CurrentRangeWeapon && swap == true)
            {
                range.gameObject.SetActive(false);
                melee.gameObject.SetActive(true);
                SetCurrentWeapon(melee);
                swap = false;
            }
        }

        public void SetCurrentWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
        }
    }
}
