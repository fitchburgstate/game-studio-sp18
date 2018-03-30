using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon Item", order = 0)]
    public class WeaponItem : InventoryItem
    {
        [SerializeField]
        private Weapon  weaponPrefab;
        public Weapon WeaponPrefab
        {
            get
            {
                return weaponPrefab;
            }
        }
    }
}
