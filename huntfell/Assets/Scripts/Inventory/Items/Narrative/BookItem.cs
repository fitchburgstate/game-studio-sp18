using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public abstract class BookItem : InventoryItem
    {
        [TextArea(3, 20)]
        public string bookContents;
    }
}
