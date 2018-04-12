using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;

namespace Hunter {
    [CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "Ranged Weapon Item", order = 0)]
    public class RangedWeaponItem : WeaponItem {

        [SerializeField]
        private Ranged rangedWeaponPrefab;
        public Ranged RangedWeaponPrefab
        {
            get
            {
                return rangedWeaponPrefab;
            }
        }

    }
}
