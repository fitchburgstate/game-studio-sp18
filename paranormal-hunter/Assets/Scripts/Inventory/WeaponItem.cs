using UnityEngine;

namespace Interactable
{
    [CreateAssetMenu(menuName = "Inventory/Weapon")]
    public class WeaponItem : ScriptableObject
    {
        public Sprite icon;
        public string nameOfWeapon;
        public string weaponDamage;
        public string weaponSpeed;
        [TextArea]
        public string weaponDescription;
    }
}
