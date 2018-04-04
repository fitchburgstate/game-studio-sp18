using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class CombatMusicTrigger : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (gameObject.tag == "StartCombatMusic")
            {
                Fabric.EventManager.Instance.PostEvent("Expo to Combat Music");
                Destroy(gameObject);
            }
            else if (gameObject.tag == "EndCombatMusic")
            {
                Fabric.EventManager.Instance.PostEvent("Combat to Expo Music");
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            if (gameObject.tag == "StartCombatMusic")
            {
                Gizmos.color = new Color(0, 1, 0, .25f);
                Gizmos.DrawCube(transform.position, transform.localScale);
            }
            if (gameObject.tag == "EndCombatMusic")
            {
                Gizmos.color = new Color(1, 0, 0, .25f);
                Gizmos.DrawCube(transform.position, transform.localScale);
            }
        }
    }
}
