using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public class Bat : Minion, IUtilityBasedAI
    {
        #region Properties
        public override int CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                if (health <= 0 && !isDying)
                {
                    //TODO Change this to reflect wether the death anim should be cinematic or not later
                    StartCoroutine(KillBat(true));
                    isDying = true;
                }
                health = value;
            }
        }
        #endregion

        #region Variables
        bool isDying = false;
        #endregion

        public void Idle()
        {
            if (isDying) { return; }
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
            agent.speed = runSpeed / 2;
            agent.destination = target;
        }

        private IEnumerator KillBat(bool isCinematic)
        {
            agent.speed = 0;
            agent.destination = transform.position;
            anim.SetTrigger(isCinematic ? "cinDeath" : "death");
            //TODO Change this later to reflect the animation time
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }
}
