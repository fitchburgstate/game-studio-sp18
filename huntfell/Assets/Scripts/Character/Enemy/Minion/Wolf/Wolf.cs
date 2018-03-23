using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public sealed class Wolf : Minion
    {
        private void Start()
        {
            SetElementType(inspectorElementType);
        }

        private void Update()
        {
            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
