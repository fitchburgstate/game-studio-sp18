using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hunter.Character
{
    public class Melee : Weapon
    {
        private float totalDamage;
        public BoxCollider weaponBoxCollider;

        /// <summary>
        /// Play Swing Animation of the Weapon.
        /// </summary>
        public virtual void Swing()
        {
            // Empty
        }

        void Start()
        {
            weaponBoxCollider = GetComponent<BoxCollider>();
            weaponBoxCollider.enabled = false;
        }

        void OnTriggerEnter(Collider target)
        {
            //Debug.Log("HI!");
            var playerCharacter = target.GetComponent<Character>();

            if (playerCharacter is Enemy)
            {
                playerCharacter = playerCharacter as Enemy;
                Attack(playerCharacter, playerCharacter.GetComponent<Enemy>().type, playerCharacter.GetComponent<Enemy>().type.weakness, playerCharacter.GetComponent<Enemy>().type.resistance1, playerCharacter.GetComponent<Enemy>().type.resistance2);
            }
            if (playerCharacter is Player)
            {
                playerCharacter = playerCharacter as Player;
                Attack(playerCharacter);
            }
        }

        /// <summary>
        /// Calculates damage base upon weapon base damage and given bonus multiplier and attack speed.
        /// </summary>
        /// <param name="ratio">Damage Falloff</param>
        /// <param name="elementDamageBonus">Damage Multiplier</param>
        /// <returns></returns>
        public override int Damage(float ratio, double elementDamageBonus)
        {
            totalDamage = baseDamage * atkSpeed * ratio * (float)elementDamageBonus;
            return (int)totalDamage;
        }

        /// <summary>
        /// Determines which damage calculation formula is based on the given enemy type, weakness, and resistance.
        /// </summary>
        /// <param name="e">Enemy variable</param>
        /// <param name="elementType">Enemy type variable</param>
        /// <param name="elementWeakness">Enemy weakness variable</param>
        /// <param name="elementResistance1">Enemy 1st resistence variable</param>
        /// <param name="elementResistance2">Enemy 2nd resistence</param>
        /// 
        public void Attack(Character enemyCharacter, ElementType elementType, Type elementWeakness, Type elementResistance1, Type elementResistance2)
        {
            if (elementWeakness.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(1, 2)), 2f, enemyCharacter);
            }
            else if (elementResistance1.Equals(type.GetType()) || elementResistance2.Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(1, 0.5)), 2f, enemyCharacter);
            }
            else if ((elementType.GetType()).Equals(type.GetType()))
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(1, 0)), 2f, enemyCharacter);
            }
            else
            {
                Critical(critPercent);
                Damaged(enemyCharacter.health, (float)(enemyCharacter.health - Damage(1, 1)), 2f, enemyCharacter);
            }
        }

        /// <summary>
        /// Determines the damage done to the player when an enemy attack the player.
        /// </summary>
        /// <param name="playerCharacter">Character variable</param>
        /// <param name="e">Enemy variable</param>
        public void Attack(Character playerCharacter)
        {
            Critical(critPercent);
            Damaged(playerCharacter.health, (float)(playerCharacter.health - Damage(1, 1)), 2f, playerCharacter);
        }

        /// <summary>
        /// Enables the hitbox of the equipped melee weapon.
        /// </summary>
        public void EnableHitbox()
        {
            weaponBoxCollider.enabled = true;
        }

        /// <summary>
        /// Disables the hitbox of the equipped melee weapon.
        /// </summary>
        public void DisableHitbox()
        {
            weaponBoxCollider.enabled = false;
        }
    }
}
