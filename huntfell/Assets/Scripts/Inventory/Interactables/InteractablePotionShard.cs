using System;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;
using System.Collections;

namespace Hunter
{
    class InteractablePotionShard : InteractableInventoryItem
    {

        public int shardCount = 1;
        public float flySpeed = 4;
        private Character characterToFlyTowards = null;

        public override void FireInteraction (Character characterTriggeringInteraction)
        {
            if(characterTriggeringInteraction is Player)
            {
                for (int i = 0; i < shardCount; i++)
                {
                    (characterTriggeringInteraction as Player).PotionShardCount += shardCount;
                }
                Destroy(gameObject);
            }
        }

        private void Start ()
        {
            characterToFlyTowards = FindObjectOfType<Player>();
        }

        private void Update ()
        {
            if (interactableItemCollider.enabled && characterToFlyTowards != null)
            {
                //var direction = (characterToFlyTowards.EyeLineTransform.position - transform.position);
                //transform.Translate(direction * (Time.deltaTime * flySpeed), Space.World);
                transform.position = Vector3.MoveTowards(transform.position, characterToFlyTowards.EyeLineTransform.position, (Time.deltaTime * flySpeed));
            }
        }
    }
}
