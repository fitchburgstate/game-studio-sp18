using System.Collections;

namespace Hunter.Characters
{
    public abstract class Minion : Enemy
    {
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
                effectsModule?.SetWoundBarFill(currentHealth / totalHealth);

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
                effectsModule?.SetHealthBarFill(targetHealth / totalHealth);
            }
        }
        #endregion

        #region Combat Related Functions
        protected override IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            effectsModule?.EnableHealthBars();
            return base.SubtractHealthFromCharacter(damage, isCritical);
        }

        protected override IEnumerator KillCharacter ()
        {
            effectsModule?.DisableHealthBars();
            GameManager.instance?.RemoveMinionFromRadius(this);
            return base.KillCharacter();
        }
        #endregion
    }
}
