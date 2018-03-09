using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class Elements : MonoBehaviour
    {
        public class Blood : ElementType
        {
            public Blood()
            {
                weakness = typeof(Disease);
                resistance1 = typeof(Silver);
                resistance2 = typeof(None);
            }
        }

        public class Disease : ElementType
        {
            public Disease()
            {
                weakness = typeof(Silver);
                resistance1 = typeof(Blood);
                resistance2 = typeof(None);
            }
        }

        public class Fire : ElementType
        {
            public Fire()
            {
                weakness = typeof(Ice);
                resistance1 = typeof(None);
                resistance2 = typeof(None);
            }
        }

        public class Ice : ElementType
        {
            public Ice()
            {
                weakness = typeof(Fire);
                resistance1 = typeof(None);
                resistance2 = typeof(None);
            }
        }

        public class Lightning : ElementType
        {
            public Lightning()
            {
                weakness = typeof(Stone);
                resistance1 = typeof(Ice);
                resistance2 = typeof(Mechanical);
            }
        }

        public class Mechanical : ElementType
        {
            public Mechanical()
            {
                weakness = typeof(Lightning);
                resistance1 = typeof(Stone);
                resistance2 = typeof(None);
            }
        }

        public class None : ElementType
        {
            public None()
            {
                // Empty
            }
        }

        public class Silver : ElementType
        {
            public Silver()
            {
                weakness = typeof(Blood);
                resistance1 = typeof(Disease);
                resistance2 = typeof(None);
            }
        }

        public class Stone : ElementType
        {
            public Stone()
            {
                weakness = typeof(Mechanical);
                resistance1 = typeof(Fire);
                resistance2 = typeof(Lightning);
            }
        }
    }
}
