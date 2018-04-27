using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;

namespace Hunter {
    [CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Melee Weapon Item", order = 0)]
    public class MeleeWeaponItem : WeaponItem {

        [SerializeField]
        private Melee meleeWeaponPrefab;
        public Melee MeleeWeaponPrefab
        {
            get
            {
                return meleeWeaponPrefab;
            }
        }

    }
}
