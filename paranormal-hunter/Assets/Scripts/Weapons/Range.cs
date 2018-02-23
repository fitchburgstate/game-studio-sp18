using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hunter.Character
{
    public class Range : Weapon
    {
        /// <summary>
        /// Clip Size of the Weapon.
        /// </summary>
        public int clipSize;

        /// <summary>
        /// Reload Speed of the Weapon.
        /// </summary>
        public int reloadSpeed;

        /// <summary>
        /// Range of the Weapon (affects raycast length).
        /// </summary>
        public int weaponRange;

        /// <summary>
        /// Hold the weapon's total damage after calulation is complete.
        /// </summary>
        private float totalDamage;

        /// <summary>
        /// Holds the distance between the weapon and enemy (used for falloff calculation).
        /// </summary>
        private float distanceBetweenWeaponAndEnemy;

        /// <summary>
        /// Holds the fall of ratio to determine damage based on distance travelled.
        /// </summary>
        private float damageFalloffRatio;

        /// <summary>
        /// Variable for holding the amount of shoots fired.
        /// </summary>
        private int clipS = 0;

        /// <summary>
        /// Holds the distance between an enemy's "weapon" and the player (used for falloff calculation).
        /// </summary>
        private float playerRange;

        private Ray ray;

        /// <summary>
        /// Performs a raycast to the range of the weapon and then calls the damage calculation method if an enemy is hit,
        /// also plays the animation for shooting.
        /// </summary>
        public virtual void Shoot()
        {
            var player = UnityEngine.Object.FindObjectOfType<Player>();
            var hit = new RaycastHit();

            ray.origin = transform.position;
            ray.direction = player.transform.forward;

            if (Physics.Raycast(ray, out hit, weaponRange))
            {
                var character = hit.collider.GetComponent<Character>();
                if(character is Enemy)
                {
                    //Debug.Log(transform.position);
                    //Debug.Log("working");
                    character = character as Enemy;
                    distanceBetweenWeaponAndEnemy = Vector3.Distance(character.transform.position, transform.position);
                    damageFalloffRatio = weaponRange / distanceBetweenWeaponAndEnemy;
                    AttackEnemy(character, character.GetComponent<Enemy>().type, character.GetComponent<Enemy>().type.weakness, character.GetComponent<Enemy>().type.resistance1, character.GetComponent<Enemy>().type.resistance2, damageFalloffRatio);
                }
                if(character is Player)
                {
                    character = character as Player;
                    playerRange = Vector3.Distance(character.transform.position, transform.position);
                    damageFalloffRatio = weaponRange / playerRange;
                    AttackPlayer(character, playerRange);
                }
            }

            Ammo();
        }

        /// <summary>
        /// Damage function which can take a ratio float value and a bonus double value.
        /// </summary>
        /// <param name="falloffRatio">Ratio for damage fall off</param>
        /// <param name="elementDamageBonus">Bonus ratio based on the type of weapon and enemy</param>
        /// <returns></returns>
        public override int Damage(float falloffRatio, double elementDamageBonus)
        {
            //May Need to tweak formula in order to achieve balance
            if(falloffRatio > 1)
            {
                falloffRatio = 1;
            }
            totalDamage = baseDamage * atkSpeed * falloffRatio * (float)elementDamageBonus;
            return (int)totalDamage;
        }

        /// <summary>
        /// Increments clipS variable to determine when the reload happens.
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
        /// this attack method is used for attacking an enemy.
        /// </summary>
        /// <param name="e">Enemy variable</param>
        /// <param name="elementType">Enemy type variable</param>
        /// <param name="elementWeakness">Enemy weakness variable</param>
        /// <param name="elementResistance1">Enemy 1st resistence variable</param>
        /// <param name="elementResistance2">Enemy 2nd resistence</param>
        /// <param name="damageFalloff">Damage falloff</param>
        public void AttackEnemy(Character enemyCharacter, ElementType elementType, Type elementWeakness, Type elementResistance1, Type elementResistance2, float damageFalloff)
        {
            if (elementWeakness.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(damageFalloff, 2)), 2f, enemyCharacter);
            }
            else if (elementResistance1.Equals(type.GetType()) || elementResistance2.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(damageFalloff, 0.5)), 2f, enemyCharacter);
            }
            else if ((elementType.GetType()).Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(damageFalloff, 0)), 2f, enemyCharacter);
            }
            else
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(damageFalloff, 1)), 2f, enemyCharacter);
                
            }
        }

        /// <summary>
        /// Determines the damage done to the player when an enemy attacks the player.
        /// </summary>
        /// <param name="playerCharacter">Character variable</param>
        /// <param name="damageFalloff">Damage falloff</param>
        public void AttackPlayer(Character playerCharacter, float damageFalloff)
        {
            Critical(critPercent);
            Damaged(playerCharacter.health, (float)(playerCharacter.health - Damage(damageFalloff, 1)), 2f, playerCharacter);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(ray.origin, ray.direction * weaponRange);
        }
    }
}
