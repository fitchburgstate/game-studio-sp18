using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class TimedDestroy : MonoBehaviour
    {
        private IEnumerator destroyCR;
        public bool activateOnStart = true;
        public float destroyWaitTime = 3;

        private void Start()
        {
            if (!activateOnStart) { return; }
            InitDestroy();
        }

        public void InitDestroy ()
        {
            if(destroyCR != null) { return; }
            destroyCR = DestroyThis();
            StartCoroutine(destroyCR);
        }

        private IEnumerator DestroyThis()
        {
            yield return new WaitForSeconds(destroyWaitTime);
            Destroy(gameObject);
        }
    }
}
