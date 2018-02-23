using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Hunter.Character
{
    public class Range : Weapon
    {
        /// <summary>
        /// Clip Size of the Weapon
        /// </summary>
        public int clipSize;

        /// <summary>
        /// Reload Speed of the Weapon
        /// </summary>
        public int reloadSpeed;

        /// <summary>
        /// Range of the Weapon (affects raycast length)
        /// </summary>
        public int range;

        /// <summary>
        /// Hold the weapon's total damage after calulation is complete
        /// </summary>
        private float totalDamage;

        /// <summary>
        /// Holds the distance between the weapon and enemy (used for falloff calculation)
        /// </summary>
        private float enemyRange;

        /// <summary>
        /// Holds the fall of ratio to determine damage based on distance travelled
        /// </summary>
        private float damageFallRatio;

        /// <summary>
        /// Variable for holding the amount of shoots fired
        /// </summary>
        private int clipS = 0;

        /// <summary>
        /// Holds the distance between an enemy's "weapon" and the player (used for falloff calculation)
        /// </summary>
        private float playerRange;

        private Ray ray;

        /// <summary>
        /// Performs a raycast to the range of the weapon and then calls the damage calculation method if an enemy is hit,
        /// also plays the animation for shooting
        /// </summary>
        public virtual void Shoot()
        {
            Vector3 fwd = transform.TransformDirection(Vector3.up);
            var hit = new RaycastHit();
            ray.direction = transform.up;
            ray.origin = transform.position;
            if(Physics.Raycast(ray, out hit, range))
            {
                var character = hit.collider.GetComponent<Character>();
                if(character is Enemy)
                {
                    Debug.Log(transform.position);
                    Debug.Log("working");
                    character = character as Enemy;
                    enemyRange = Vector3.Distance(character.transform.position, transform.position);
                    damageFallRatio = range / enemyRange;
                    Attack(character, character.GetComponent<Enemy>().type, character.GetComponent<Enemy>().type.weakness, character.GetComponent<Enemy>().type.resistence, character.GetComponent<Enemy>().type.resistence2, damageFallRatio);
                }
                if(character is Player)
                {
                    character = character as Player;
                    playerRange = Vector3.Distance(character.transform.position, transform.position);
                    damageFallRatio = range / playerRange;
                    Attack(character, playerRange);
                }
            }
            Ammo();
            
        }
        /// <summary>
        /// Damage function which can take a ratio float value and a bonus double value
        /// </summary>
        /// <param name="ratio">Ratio for damage fall off</param>
        /// <param name="bonus">Bonus ratio based on the type of weapon and enemy</param>
        /// <returns></returns>
        public override int Damage(float ratio, double bonus)
        {
            //May Need to tweak formula in order to achieve balance
            if(ratio > 1)
            {
                ratio = 1;
            }
            totalDamage = baseDamage * atkSpeed * ratio * (float)bonus;
            return (int)totalDamage;
        }
        /// <summary>
        /// Increments clipS variable to determine when the reload happens
        /// </summary>
        public void Ammo()
        {
            clipS++;
            if(clipS == clipSize)
            {
                //reload animation
                //anim.Play(clip2.name);
                clipS = 0;
            }
        }
        /// <summary>
        /// Determines which damage calculation formula is based on the given 
        /// this attack method is used for attacking an enemy
        /// </summary>
        /// <param name="e">Enemy variable</param>
        /// <param name="et">Enemy type variable</param>
        /// <param name="weak">Enemy weakness variable</param>
        /// <param name="res">Enemy 1st resistence variable</param>
        /// <param name="res2">Enemy 2nd resistence</param>
        /// <param name="damF">Damage falloff</param>
        public void Attack(Character c, ElementType et, Type weak, Type res, Type res2, float damF)
        {
            if (weak.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(damF, 2)), 2f, c);
            }
            else if (res.Equals(type.GetType()) || res2.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(damF, 0.5)), 2f, c);
            }
            else if ((et.GetType()).Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(damF, 0)), 2f, c);
            }
            else
            {
                Critical(critPercent);
                Damaged(c.health, (float)(c.health - Damage(damF, 1)), 2f, c);
                
            }
        }
        /// <summary>
        /// Determines the damage done to the player when an enemy attack the player
        /// </summary>
        /// <param name="c">Character variable</param>
        /// <param name="damF">Damage falloff</param>
        public void Attack(Character c, float damF)
        {
            Critical(critPercent);
            Damaged(c.health, (float)(c.health - Damage(damF, 1)), 2f, c);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(ray.origin, ray.direction * range);
        }
    }
}
