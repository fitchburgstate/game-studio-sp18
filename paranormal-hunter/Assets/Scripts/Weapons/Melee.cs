using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * TODO: 
 */
namespace Hunter.Character
{
    public class Melee : Weapon
    {
        private float totalDamage;
        public BoxCollider bc;

        /// <summary>
        /// Play Swing Animation of the Weapon
        /// </summary>
        public virtual void Swing()
        {
            
        }

        void Start()
        {
            bc = GetComponent<BoxCollider>();
            bc.enabled = false;
        }

        void OnTriggerEnter(Collider target)
        {
            Debug.Log("HI!");
            var character = target.GetComponent<Character>();
            if (character is Enemy)
            {
                character = character as Enemy;
                Attack(character, character.GetComponent<Enemy>().type, character.GetComponent<Enemy>().type.weakness, character.GetComponent<Enemy>().type.resistence, character.GetComponent<Enemy>().type.resistence2);
            }
            if(character is Player)
            {
                character = character as Player;
                Attack(character);
            }
        }

        void OnTriggerExit(Collider other)
        {
            
        }

        /// <summary>
        /// Calculates damage base upon weapon base damage and given bonus multiplier and attack speed
        /// </summary>
        /// <param name="ratio">Damage Falloff</param>
        /// <param name="bonus">Damage Multiplier</param>
        /// <returns></returns>
        public override int Damage(float ratio, double bonus)
        {
            totalDamage = baseDamage * atkSpeed * ratio * (float)bonus;
            return (int)totalDamage;
        }

        /// <summary>
        /// Determines which damage calculation formula is based on the given enemy type, weakness, and resistence
        /// </summary>
        /// <param name="e">Enemy variable</param>
        /// <param name="et">Enemy type variable</param>
        /// <param name="weak">Enemy weakness variable</param>
        /// <param name="res">Enemy 1st resistence variable</param>
        /// <param name="res2">Enemy 2nd resistence</param>
        /// 
        public void Attack(Character c, ElementType et, Type weak, Type res, Type res2)
        {
            if (weak.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(1, 2)), 2f, c);
            }
            else if (res.Equals(type.GetType()) || res2.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(1, 0.5)), 2f, c);
            }
            else if ((et.GetType()).Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(1, 0)), 2f, c);

            }
            else
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(1, 1)), 2f, c);
            }
        }

        /// <summary>
        /// Determines the damage done to the player when an enemy attack the player
        /// </summary>
        /// <param name="c">Character variable</param>
        /// <param name="e">Enemy variable</param>
        public void Attack(Character c)
        {
            Critical(critPercent);
            Damaged(c.health, (float)(c.health - Damage(1, 1)), 2f, c);
        }

        public void EnableHitbox()
        {
            bc.enabled = true;
        }

        public void DisableHitbox()
        {
            bc.enabled = false;
        }

        //public void ToggleHitBox(bool state)
        //{
        //    bc.enabled = state;
        //}
    }
}
