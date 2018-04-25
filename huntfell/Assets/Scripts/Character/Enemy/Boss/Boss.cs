using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hunter.Characters
{
    public class Boss : Enemy
    {
        #region Variables
        /// <summary>
        /// Determines what in the scene is the bosses' health bar GUI.
        /// </summary>
        [Header("Health Bar Options")]
        public Image bossHealthBar;

        /// <summary>
        /// Determines for how many frames the boss should be invincible after taking damage.
        /// </summary>
        public float invincibilityFrames = 5;

        /// <summary>
        /// Determines what is the parent of the bosses' health bar GUI.
        /// </summary>
        protected Transform bossHealthBarParent;
        #endregion

        #region Unity Functions
        protected override void Start()
        {
            base.Start();
            if (bossHealthBar != null)
            {
                bossHealthBarParent = bossHealthBar.transform.parent;
                bossHealthBarParent.gameObject.SetActive(false);
            }
        }
        #endregion

        #region SubtractHealthFromCharacter Function
        /// <summary>
        /// Subtracts health from the character as well as adjusting the health bar accordingly.
        /// </summary>
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
