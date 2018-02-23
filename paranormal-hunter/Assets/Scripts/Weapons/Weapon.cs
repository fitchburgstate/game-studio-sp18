using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using static Hunter.Elements;

namespace Hunter.Character
{
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Attack Speed of the Weapon.
        /// </summary>
        public int atkSpeed;

        /// <summary>
        /// Recovery Speed of the Weapon a.k.a how fast before you can attack again.
        /// </summary>
        public int recSpeed;

        /// <summary>
        /// Base Damage number of the weapon.
        /// </summary>
        public int baseDamage;

        /// <summary>
        /// Element type of the weapon.
        /// </summary>
        public ElementType type;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown.
        /// </summary>
        public OPTIONS elementType; 

        /// <summary>
        /// Critical Percentage Given to the Weapon.
        /// </summary>
        public int critPercent;

        /// <summary>
        /// Boolean that gets set true when a critical hit happens.
        /// </summary>
        public bool isCritical = false;

        /// <summary>
        /// Ratio that holds critical percentage for hit calculation.
        /// </summary>
        private float critDamage;

        void Update()
        {
            SetElementType(elementType);
        }

        /// <summary>
        /// Damage function which can take a ratio float value and a bonus double value, can be override by
        /// classes that inherit the Weapon class.
        /// </summary>
        /// <param name="ratio">Variable for damage falloff</param>
        /// <param name="bonus">Variable for damage multiplier</param>
        /// <returns>Damage based on falloff and multiplier</returns>
        public virtual int Damage(float ratio, double bonus)
        {
            var dam = (int)(ratio * bonus);
            return dam;
        }

        /// <summary>
        /// Sets the element type of the weapon based upon the given options variable.
        /// </summary>
        /// <param name="elementType">Option for the Element Type</param>
        private void SetElementType(OPTIONS elementType)
        {
            switch (elementType)
            {
                case OPTIONS.Fire:
                    type = new Fire();
                    break;
                case OPTIONS.Ice:
                    type = new Ice();
                    break;
                case OPTIONS.Disease:
                    type = new Disease();
                    break;
                case OPTIONS.Silver:
                    type = new Silver();
                    break;
                case OPTIONS.Blood:
                    type = new Blood();
                    break;
                case OPTIONS.Lightning:
                    type = new Lightning();
                    break;
                case OPTIONS.Mechanical:
                    type = new Mechanical();
                    break;
                case OPTIONS.Stone:
                    type = new Stone();
                    break;
            }
        }

        /// <summary>
        /// Calculates damage to be applied against an enemy and be lerped if hit is not critical, if the hit is critical the 
        /// damage is applied instantly.
        /// </summary>
        /// <param name="start">Starting Health value</param>
        /// <param name="end">Ending Health value</param>
        /// <param name="time">Time to lerp variables</param>
        public void Damaged(float start, float end, float time, Enemy e)
        {
            float t = 0;
            while (t < 1.0 && !isCritical)
            {
                t += Time.deltaTime / time;
                e.health = (int)Mathf.Lerp(start, end, t);
                //Debug.Log(e.health);
            }
            if (isCritical)
            {
                var damage = start - end;
                damage = (damage * critDamage) + damage;
                e.health = e.health - (int)damage;
                isCritical = false;
            }
        }
        
        /// <summary>
        /// Calculates damage to be applied against the player and be lerped if hit is not critical, if hit is critical the 
        /// damage is applied instantly.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="time"></param>
        /// <param name="c"></param>
        public void Damaged(float start, float end, float time, Character c)
        {
            float t = 0;
            while (t < 1.0 && !isCritical)
            {
                t += Time.deltaTime / time;
                c.health = (int)Mathf.Lerp(start, end, t);
                //Debug.Log(c.health);
            }
            if(isCritical)
            {
                var damage = start - end;
                damage = damage + critDamage;
                Debug.Log("Total Damage: " + damage);
                c.health = c.health - (int)damage;
                isCritical = false;
            }
        }

        /// <summary>
        /// Calculates whether or not the player crits based on crit percentage that is given to the function.
        /// </summary>
        /// <param name="percent"></param>
        public void Critical(int percent)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[4];

            rng.GetBytes(buffer);

            System.Random r = new System.Random();
            var num = r.Next(1, 100);
            if (num >= (100 - percent))
            {
                isCritical = true;
                //ratio = (float)num / 20;
                critDamage = r.Next(baseDamage + 1, baseDamage * 3);
                Debug.Log("Crit Damage Amount: " + critDamage);
            }
        }
    }
}
