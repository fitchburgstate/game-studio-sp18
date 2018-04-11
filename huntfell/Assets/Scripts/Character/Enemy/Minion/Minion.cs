using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.Character
{
    public abstract class Minion : Enemy
    {
        #region Variables
        /// <summary>
        /// This is the speed at which the character runs.
        /// </summary>
        [Range(0, 20)]
        [Tooltip("The running speed of the character when it is in combat.")]
        public float runSpeed = 5f;
        public float invincibilityFrames = 5;

        public Image minionHealthBar;
        protected Transform minionHealthBarParent;
        #endregion

        protected override void Start ()
        {
            base.Start();
            if (minionHealthBar != null)
            {
                minionHealthBarParent = minionHealthBar.transform.parent;
                minionHealthBarParent.gameObject.SetActive(false);
            }
        }

        //TODO Move this into Effects Controller as an optional parameter that only minions take
        protected override IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            minionHealthBarParent?.gameObject.SetActive(true);
            var targetHealth = CurrentHealth - damage;
            if(minionHealthBar != null) {
                minionHealthBar.fillAmount = targetHealth / totalHealth;
            }
            else { Debug.LogWarning($"{name} does not have a health bar set in the inspector."); }
            CurrentHealth = targetHealth;
            invinvible = true;
            for (int i = 0; i < invincibilityFrames; i++)
            {
                yield return null;
            }
            invinvible = false;
            yield return null;
        }
    }
}
