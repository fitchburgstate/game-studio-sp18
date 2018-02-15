using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

namespace CharacterScripts
{
    public abstract class Character : MonoBehaviour, IDamageable<int, int>, IHealth<int>
    {
        public int StartingHealth
        {
            get
            {
                return startingHealth;
            }

            set
            {
                startingHealth = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }

            set
            {
                maxHealth = value;
            }
        }

        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }

            set
            {
                currentHealth = value;
            }
        }

        private int startingHealth;
        private int maxHealth;
        private int currentHealth;

        public virtual void SetStartingHealth(int amount)
        {
            startingHealth = amount;
        }

        public virtual void SetMaxHealth(int amount)
        {
            maxHealth = amount;
        }

        public virtual void SetCurrentHealth(int amount)
        {
            currentHealth = amount;
        }

        public virtual void DealDamage(int targetHealthValue, int amount)
        {
            targetHealthValue -= amount;
        }

        public virtual void TakeDamage(int healthValue, int amount)
        {
            healthValue -= amount;
        }
    }
}
