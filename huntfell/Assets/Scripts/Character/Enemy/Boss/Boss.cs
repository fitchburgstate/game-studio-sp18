namespace Hunter.Characters
{
    public class Boss : Enemy
    {
        public override float CurrentHealth
        {
            get
            {
                return base.CurrentHealth;
            }

            set
            {
                base.CurrentHealth = value;
                HUDManager.instance?.SetBossCurrentHealthBar(currentHealth / totalHealth);
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
                HUDManager.instance?.SetBossTargetHealthBar(targetHealth / totalHealth);
            }
        }
    }
}
