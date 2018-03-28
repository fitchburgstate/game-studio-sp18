using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter.AI
{
    public class PassiveAreaDamage : MonoBehaviour
    {
        public int damageAmount = 1;

        public void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                var damageable = other.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.DealDamage(damageAmount, false);
                }
            }
        }
    }
}
