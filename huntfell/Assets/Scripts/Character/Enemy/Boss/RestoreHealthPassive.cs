using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Characters
{
    public class RestoreHealthPassive : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// A boolean to determine whether the silver debuff has been applied to the boss.
        /// </summary>
        public bool silverDebuff = false;

        /// <summary>
        /// The interval time between each health tick.
        /// </summary>
        public float secondsBetweenHeal = 1;

        /// <summary>
        /// The amount of health that will be restored to the boss every interval.
        /// </summary>
        public int amountToHeal = 2;

        /// <summary>
        /// Determines whether the HealthDrain Coroutine can be played or not.
        /// </summary>
        private IEnumerator healthDrainCR;

        /// <summary>
        /// Used to reference the Character script attached to the boss.
        /// </summary>
        private Character characterToHeal;
        #endregion

        #region Properties
        public Character CharacterToHeal
        {
            get
            {
                if (characterToHeal == null) { characterToHeal = GetComponent<Character>(); }
                return characterToHeal;
            }
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            if (healthDrainCR == null)
            {
                healthDrainCR = HealthDrain();
                StartCoroutine(healthDrainCR);
            }
        }
        #endregion

        #region Health Drain Functions
        private IEnumerator HealthDrain()
        {
            while (!silverDebuff || CharacterToHeal.CurrentHealth < CharacterToHeal.totalHealth)
            {
                CharacterToHeal.RestoreHealthToCharacter(amountToHeal);
                yield return new WaitForSeconds(secondsBetweenHeal);
            }
        }
        #endregion
    }
}
