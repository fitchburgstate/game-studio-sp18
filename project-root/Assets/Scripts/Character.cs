using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace CharacterScripts
{
    public abstract class Character : MonoBehaviour, IDamageable<float>
    {
        public virtual void TakeDamage(float amount)
        {

        }

        public virtual void DealDamage(float amount)
        {

        }
    }
}

