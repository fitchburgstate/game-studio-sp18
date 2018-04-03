using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class DestroyBarricade : MonoBehaviour
    {
        private IEnumerator destroyThisCR;

        private void Start()
        {
            destroyThisCR = DestroyThis();
            StartCoroutine(destroyThisCR);
        }

        private IEnumerator DestroyThis()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}
