using System;

namespace Hunter.Elements
{
    /// <summary>
    /// Options Enum for the Elements that can be selected.
    /// </summary>
    public enum ElementOptions
    {
        Fire,
        Ice,
        Lightning,
        Nature,
        Silver
    }

    public abstract class Element
    {
        public Type weakness = null;
    }

    public class Fire : Element
    {
        public Fire()
        {
            weakness = typeof(Ice);
        }
    }

    public class Ice : Element
    {
        public Ice()
        {
            weakness = typeof(Fire);
        }
    }

    public class Lightning : Element
    {
        public Lightning()
        {
            weakness = typeof(Nature);
        }
    }

    public class Nature : Element
    {
        public Nature ()
        {
            weakness = typeof(Lightning);
        }
    }

    public class Silver : Element
    {
    }
}
