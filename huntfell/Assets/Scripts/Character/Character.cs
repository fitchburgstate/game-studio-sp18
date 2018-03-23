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
        /// Name of the Player, to be set in the inspector
        /// </summary>
        public readonly string PlayerName;

        /// <summary>
        /// How much health the character has
        /// </summary>
        private int health;
        public int CurrentHealth
        {
            get
            {
                return health;
            }
        }

        private Weapon currentWeapon;
        public Weapon CurrentWeapon
        {
            get
            {
                return currentWeapon;
            }
        }

        //Variables for handeling character rotation
        public const string ROTATION_TRANSFORM_TAG = "Rotation Transform";
        private Transform rotationTransform;
        public Transform RotationTransform
        {
            get
            {
                if(rotationTransform == null)
                {
                    foreach(Transform child in transform)
                    {
                        if(child.tag == ROTATION_TRANSFORM_TAG) { rotationTransform = child; }
                    }
                    //Fallback for if the tag isn't set
                    if(rotationTransform == null) {
                        Debug.LogWarning("GameObject: " + gameObject.name + " has no rotational transform set. Check the tag of the first childed GameObject underneath this GameObject.", gameObject);
                        rotationTransform = transform.GetChild(0);
                    }
                }
                return rotationTransform;
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

        public void DealDamage (int damage, bool isCritical)
        {
            //This is also where we'll do the damage number pop up
            StartCoroutine(SubtractHealthFromCharacter(damage, isCritical));
        }

        private IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            //TODO: Refactor this so the health subtration lerp works
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
