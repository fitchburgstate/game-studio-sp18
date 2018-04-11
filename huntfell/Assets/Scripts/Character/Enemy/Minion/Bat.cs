using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public class Bat : Minion, IUtilityBasedAI
    {
        #region Properties
        public override float CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0 && !isDying)
                {
                    //TODO Change this to reflect wether the death anim should be cinematic or not later
                    StartCoroutine(KillBat(true));
                    isDying = true;
                }
            }
        }
        #endregion

        #region Variables


        #endregion

        protected override void Start()
        {
            base.Start();
            Fabric.EventManager.Instance.PostEvent("Bat Start Wing Loop", gameObject);
        }

        public void Idle()
        {
            if (isDying) { return; }
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
            agent.speed = speed / 2;
            agent.destination = target;
        }

        public void Turn(Transform target)
        {

        }

        private IEnumerator KillBat(bool isCinematic)
        {
            agent.speed = 0;
            agent.destination = transform.position;
            anim.SetTrigger("death");
            Fabric.EventManager.Instance.PostEvent("Bat Stop Wing Loop", gameObject);
            minionHealthBarParent?.gameObject.SetActive(false);
            //TODO Change this later to reflect the animation time
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }
}
