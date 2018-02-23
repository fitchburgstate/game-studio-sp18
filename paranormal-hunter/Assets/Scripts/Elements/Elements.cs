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
                resistence = typeof(Silver);
                resistence2 = typeof(None);
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
                resistence = typeof(Blood);
                resistence2 = typeof(None);
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
                resistence = typeof(None);
                resistence2 = typeof(None);
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
                resistence = typeof(None);
                resistence2 = typeof(None);
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
                resistence = typeof(Ice);
                resistence2 = typeof(Mechanical);
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
                resistence = typeof(Stone);
                resistence2 = typeof(None);
            }
        }

        public class None : ElementType
        {
            public None()
            {

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
                resistence = typeof(Disease);
                resistence2 = typeof(None);
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
                resistence = typeof(Fire);
                resistence2 = typeof(Lightning);
            }
        }
    }
}

