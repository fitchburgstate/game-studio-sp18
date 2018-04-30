using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class TimedDestroy : MonoBehaviour
    {
        private IEnumerator destroyAction;

        [Tooltip("The object to destroy after a given time. Leave this null to default to the gameObject this component is attached to.")]
        public Object destroyTarget;
        public float destroyWaitTime = 3;
        public bool activateOnStart = true;

        private void Start()
        {
            if (!activateOnStart) { return; }
            InitiateDestroy();
        }

        public void InitiateDestroy ()
        {
            if(destroyAction != null) { return; }

            if(destroyTarget == null) { destroyTarget = gameObject; }
            destroyAction = DestroyAction();
            StartCoroutine(destroyAction);
        }

        public void InitiateDestroy (Object target, float waitTime)
        {
            if (destroyAction != null) { return; }

            if(destroyTarget == null)
            {
                Debug.LogWarning("Cannot destroy a null target.", gameObject);
                return;
            }
            else
            {
                destroyTarget = target;
                destroyWaitTime = waitTime;
            }
            destroyAction = DestroyAction();
            StartCoroutine(destroyAction);
        }

        private IEnumerator DestroyAction()
        {
            yield return new WaitForSeconds(destroyWaitTime);
            Destroy(destroyTarget);
        }
    }
}
