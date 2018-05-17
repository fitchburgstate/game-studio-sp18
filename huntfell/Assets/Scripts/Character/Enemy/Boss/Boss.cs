namespace Hunter.Characters
{
    public class Boss : Enemy
    {
        public InteractableGameEndPortal endPortal;

        public override float CurrentHealth
        {
            get
            {
                return base.CurrentHealth;
            }

            set
            {
                base.CurrentHealth = value;
                if (HUDManager.instance != null)
                {
                    HUDManager.instance?.SetBossCurrentHealthBar(currentHealth / totalHealth);
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
                if (HUDManager.instance != null)
                {
                    HUDManager.instance?.SetBossTargetHealthBar(targetHealth / totalHealth);
                }
            }
        }

        protected override void Start ()
        {
            base.Start();
            endPortal?.gameObject.SetActive(false);
        }
    }
}
