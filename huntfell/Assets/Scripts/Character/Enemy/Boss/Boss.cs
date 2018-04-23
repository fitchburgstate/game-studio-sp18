using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.Characters
{
    public class Boss : Enemy
    {
        #region Variables
        [Header("Health Bar Options")]
        public Image bossHealthBar;
        public float invincibilityFrames = 5;

        protected Transform bossHealthBarParent;
        #endregion

        protected override void Start()
        {
            base.Start();
            if (bossHealthBar != null)
            {
                bossHealthBarParent = bossHealthBar.transform.parent;
                bossHealthBarParent.gameObject.SetActive(false);
            }
        }

        #region SubtractHealthFromCharacter Function
        //TODO Move this into Effects Controller as an optional parameter that only minions take
        protected override IEnumerator SubtractHealthFromCharacter(int damage, bool isCritical)
        {
            bossHealthBarParent?.gameObject.SetActive(true);
            var targetHealth = CurrentHealth - damage;
            if (bossHealthBar != null)
            {
                bossHealthBar.fillAmount = targetHealth / totalHealth;
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
