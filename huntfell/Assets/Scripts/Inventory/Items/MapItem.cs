using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    [CreateAssetMenu(fileName = "NewMap", menuName = "Map Item", order = 0)]
    public class MapItem : InventoryItem
    {
        public Sprite mapSprite;
    }
}
