using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class Elements : MonoBehaviour
    {
        public class Blood : ElementType
        {
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Disease,
            /// defines the first resistence of the type to Silver,
            /// & defines the second resistence of the type to None
            /// </summary>
            public Blood()
            {
                weakness = typeof(Disease);
                resistance1 = typeof(Silver);
                resistance2 = typeof(None);
            }
        }

        public class Disease : ElementType
        {
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Silver,
            /// defines the first resistence of the type to Blood,
            /// & defines the second resistence of the type to None
            /// </summary>
            public Disease()
            {
                weakness = typeof(Silver);
                resistance1 = typeof(Blood);
                resistance2 = typeof(None);
            }
        }

        public class Fire : ElementType
        {
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Ice,
            /// defines the first resistence of the type to None,
            /// & defines the second resistence of the type to None
            /// </summary>
            public Fire()
            {
                weakness = typeof(Ice);
                resistance1 = typeof(None);
                resistance2 = typeof(None);
            }
        }

        public class Ice : ElementType
        {
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Fire,
            /// defines the first resistence of the type to None,
            /// & defines the second resistence of the type to None
            /// </summary>
            public Ice()
            {
                weakness = typeof(Fire);
                resistance1 = typeof(None);
                resistance2 = typeof(None);
            }
        }

        public class Lightning : ElementType
        {
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Stone,
            /// defines the first resistence of the type to Ice,
            /// & defines the second resistence of the type to Mechanical
            /// </summary>
            public Lightning()
            {
                weakness = typeof(Stone);
                resistance1 = typeof(Ice);
                resistance2 = typeof(Mechanical);
            }
        }

        public class Mechanical : ElementType
        {
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Lightning,
            /// defines the first resistence of the type to Stone,
            /// & defines the second resistence of the type to None
            /// </summary>
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
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Blood,
            /// defines the first resistence of the type to Disease,
            /// & defines the second resistence of the type to None
            /// </summary>
            public Silver()
            {
                weakness = typeof(Blood);
                resistance1 = typeof(Disease);
                resistance2 = typeof(None);
            }
        }

        public class Stone : ElementType
        {
            /// <summary>
            /// Constructor Method for the Blood Element Type which
            /// defines the weakness of the type to Mechanical,
            /// defines the first resistence of the type to Fire,
            /// & defines the second resistence of the type to Lightning
            /// </summary>
            public Stone()
            {
                weakness = typeof(Mechanical);
                resistance1 = typeof(Fire);
                resistance2 = typeof(Lightning);
            }
        }
    }
}

