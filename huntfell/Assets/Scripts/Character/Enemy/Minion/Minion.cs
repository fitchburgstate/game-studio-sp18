using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public abstract class Minion : Enemy
    {
        #region Variables
        public float invincibilityFrames = 5;

        public Image minionHealthBar;

        protected Transform minionHealthBarParent;
        #endregion

        protected override void Start()
        {
            base.Start();
            if (minionHealthBar != null)
            {
                minionHealthBarParent = minionHealthBar.transform.parent;
                minionHealthBarParent.gameObject.SetActive(false);
            }
        }

        #region SubtractHealthFromCharacter Function
        //TODO Move this into Effects Controller as an optional parameter that only minions take
        protected override IEnumerator SubtractHealthFromCharacter(int damage, bool isCritical)
        {
            minionHealthBarParent?.gameObject.SetActive(true);
            var targetHealth = CurrentHealth - damage;
            if (minionHealthBar != null)
            {
                minionHealthBar.fillAmount = targetHealth / totalHealth;
            }
            else { Debug.LogWarning($"{name} does not have a health bar set in the inspector."); }
            CurrentHealth = targetHealth;
            invincible = true;
            for (var i = 0; i < invincibilityFrames; i++)
            {
                yield return null;
            }
            invincible = false;
            yield return null;
        }
        #endregion
    }
}
