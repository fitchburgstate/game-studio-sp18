using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hunter
{
    /// <summary>
    /// Options Enum for the Elements that can be selected.
    /// </summary>
    public enum OPTIONS
    {
        Fire,
        Ice,
        Disease,
        Silver,
        Blood,
        Lightning,
        Mechanical,
        Stone
    }
    //[CreateAssetMenu(fileName = "New Element", menuName = "Element")]
    public abstract class ElementType
    {
        /// <summary>
        /// Weakness of the type.
        /// </summary>
        public Type weakness;

        /// <summary>
        /// Resistence one of the type, if it has any.
        /// </summary>
        public Type resistance1;

        /// <summary>
        /// Resistence two of the type, if it has any.
        /// </summary>
        public Type resistance2;
    }
}

