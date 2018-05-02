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
        public Image minionHealthBar;
        public Image minionWoundBar;

        protected Transform minionHealthBarParent;
        #endregion

        #region Properties
        public override float CurrentHealth
        {
            get
            {
                return base.CurrentHealth;
            }

            set
            {
                base.CurrentHealth = value;
                if (minionWoundBar != null)
                {
                    minionWoundBar.fillAmount = currentHealth / totalHealth;
                }
            }
        }

        public override float TargetHealth
        {
            get
            {
                return base.TargetHealth;
            }

            set
            {
                base.TargetHealth = value;
                if (minionHealthBar != null)
                {
                    minionHealthBar.fillAmount = targetHealth / totalHealth;
                }
            }
        }
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

        protected override IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            minionHealthBarParent?.gameObject.SetActive(true);
            return base.SubtractHealthFromCharacter(damage, isCritical);
        }

        protected override IEnumerator KillCharacter ()
        {
            minionHealthBarParent?.gameObject.SetActive(false);
            return base.KillCharacter();
        }
    }
}
