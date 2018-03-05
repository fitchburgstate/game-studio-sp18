using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

namespace Hunter.Character
{
    public abstract class Character : MonoBehaviour, IDamageable
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
            if (weapon != null)
            {
                currentWeapon = weapon;
            }
        }

        public void DealDamage (int damage, int timeOfDamage)
        {
            //float t = 0;
            //while (t < 1.0 && !isCritical)
            //{
            //    t += Time.deltaTime / time;
            //    c.health = (int)Mathf.Lerp(start, end, t);
            //    //Debug.Log(c.health);
            //}
            //if (isCritical)
            //{
            //    var damage = start - end;
            //    damage = damage + critDamage;
            //    Debug.Log("Total Damage: " + damage);
            //    c.health = c.health - (int)damage;
            //    isCritical = false;
            //}
        }
    }
}
