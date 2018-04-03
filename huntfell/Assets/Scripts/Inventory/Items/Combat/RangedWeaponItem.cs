using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter {
    [CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Ranged Weapon Item", order = 0)]
    public class RangedWeaponItem : WeaponItem {

        [SerializeField]
        private Range rangedWeaponPrefab;
        public Range RangedWeaponPrefab
        {
            get
            {
                return rangedWeaponPrefab;
            }
        }

    }
}
