using System;
using UnityEngine;

namespace Hunter
{
    /// <summary>
    /// Options Enum for the Elements that can be selected.
    /// </summary>
    public enum ElementOption
    {
        None,
        Fire,
        Ice,
        Lightning,
        Nature,
        Silver
    }

    public abstract class Element
    {
        public Sprite elementHUDSprite;
        public Color elementColor = Color.white;
        protected Type weakness = null;
        
        public Type Weakness
        {
            get
            {
                return weakness;
            }
        }

        public class Fire : Element
        {
            public Fire ()
            {
                weakness = typeof(Ice);
                elementColor = Color.red;
            }
        }

        public class Ice : Element
        {
            public Ice ()
            {
                weakness = typeof(Fire);
                elementColor = Color.cyan;
            }
        }

        public class Lightning : Element
        {
            public Lightning ()
            {
                weakness = typeof(Nature);
                elementColor = Color.yellow;
            }
        }

        public class Nature : Element
        {
            public Nature ()
            {
                weakness = typeof(Lightning);
                elementColor = Color.green;
            }
        }

        public class Silver : Element
        {
            public Silver ()
            {
                elementColor = Color.gray;
            }
        }
    }
}
