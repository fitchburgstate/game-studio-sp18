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
            SetElementType(elementType);
        }

        private void Update()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        // ----------- Sound Event Methods ----------- \\
        void PlayWolfAttack(){
		    Fabric.EventManager.Instance.PostEvent("Wolf Attack", gameObject);
	    }
        // -------------------------------------------------- \\

    }
}
