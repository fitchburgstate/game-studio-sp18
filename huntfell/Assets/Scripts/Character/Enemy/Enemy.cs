﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Hunter.Characters
{
    public abstract class Enemy : Character
    {
        /// <summary>
        /// Element Type of the Enemy
        /// </summary>
        public Element elementType;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown
        /// </summary>
        public ElementOption enemyElementOption;

        protected override void Start ()
        {
            elementType = Utility.ElementOptionToElement(enemyElementOption);
        }

    }
}
