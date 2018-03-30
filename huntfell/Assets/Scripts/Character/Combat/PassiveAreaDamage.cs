using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
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
                    //Currently this is passing null as the Element but we could set it so this does elemental damage if we wanted
                    damageable.TakeDamage(damageAmount, false, null);
                }
            }
        }
    }
}
