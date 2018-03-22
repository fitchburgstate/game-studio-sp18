using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEditor;

namespace Hunter.Character
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
//#if UNITY_EDITOR
//        public virtual List<string> SerialzedPropertyNamesList
//        {
//            get
//            {
//                return null;
//            }
//        }
//#endif
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
        [SerializeField]
        protected Melee melee;

        /// <summary>
        /// Current Ranged Weapon Equipped on the Character
        /// </summary>
        [SerializeField]
        protected Range range;

        protected Weapon currentWeapon;

        private Transform rotationTransform;
        public const string ROTATION_TRANSFORM_TAG = "Rotation Transform";


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

        public Transform RotationTransform
        {
            get
            {
                if (rotationTransform == null)
                {
                    foreach (Transform child in transform)
                    {
                        if (child.tag == ROTATION_TRANSFORM_TAG) { rotationTransform = child; }
                    }
                    //Fallback for if the tag isn't set
                    if (rotationTransform == null)
                    {
                        Debug.LogWarning("GameObject: " + gameObject.name + " has no rotational transform set. Check the tag of the first childed GameObject underneath this GameObject.", gameObject);
                        rotationTransform = transform.GetChild(0);
                    }
                }
                return rotationTransform;
            }

        }


        public void SwitchWeapon()
        {
            if (CurrentMeleeWeapon)
            {
                melee.gameObject.SetActive(false);
                range.gameObject.SetActive(true);
                SetCurrentWeapon(range);
            }
            else if (CurrentRangeWeapon)
            {
                range.gameObject.SetActive(false);
                melee.gameObject.SetActive(true);
                SetCurrentWeapon(melee);
            }
        }

        public void SetCurrentWeapon(Weapon weapon)
        {
            if (weapon != null)
            {
                currentWeapon = weapon;
                currentWeapon.characterHoldingWeapon = this;
            }
        }

        public void DealDamage(int damage, bool isCritical)
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

        private IEnumerator SubtractHealthFromCharacter(int damage, bool isCritical)
        {
            //float t = 0;
            //while (t < 1.0 && !isCritical)
            //{
            //    t += Time.deltaTime / time;
            //    health = (int)Mathf.Lerp(start, end, t);
            //    //Debug.Log(c.health);
            //}
            //if (isCritical)
            //{
            //    damage = start - end;
            //    damage = damage + critDamage;
            //    Debug.Log("Total Damage: " + damage);
            //    health = health - (int)damage;
            //    isCritical = false;
            //}
            health -= damage;
            yield return null;
        }
    }
}
