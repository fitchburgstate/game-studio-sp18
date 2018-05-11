using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;

namespace Hunter
{
    [CreateAssetMenu(fileName = "NewBestiaryEntry", menuName = "Bestiary Entry Item", order = 0)]
    public class BestiaryItem : BookItem
    {
        public Sprite entrySketch;
        public EnemyType enemyType;
        [Range(1, 3)]
        public int entryOrder = 1;
    }
}
