using Hunter.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class PlayerRadius : MonoBehaviour
    {
        #region Unity Functions
        private void OnTriggerEnter(Collider other)
        {
            var minion = other.GetComponent<Minion>();
            if (minion != null)
            {
                GameManager.instance?.AddMinionToRadius(minion);
                return;
            }

            var boss = other.GetComponent<Boss>();
            if(boss != null)
            {
                GameManager.instance?.AddBossToRadius(boss);
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var minion = other.GetComponent<Minion>();
            if (minion != null)
            {
                GameManager.instance?.RemoveMinionFromRadius(minion);
                return;
            }

            var boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                //GameManager.instance?.RemoveBossFromRadius(boss);
                return;
            }
        }
        #endregion
    }
}
