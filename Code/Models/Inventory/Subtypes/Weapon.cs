using Godot;

namespace DMGInventorySystem
{
    [System.Serializable]
    public partial class Weapon : InventoryItem
    {
 

        [Export] public WeaponTypes weaponType;
        
        [Export] public float weaponDamage = 1.0f;
        
        [Export] public float weaponSpeed = 1.0f;
        
        public float DPS => weaponSpeed <= 0 ? 0 : weaponDamage * weaponSpeed;
        
        [Export] public bool twoHanded;

        private InventoryEvents _inventoryEvents;
        
        public override void _Ready()
        {
            _inventoryEvents = GetNode<InventoryEvents>("/root/InventoryEvents");
        }

        public void SetInventoryEvents(InventoryEvents inventoryEvents) => _inventoryEvents = inventoryEvents;
        public override void UseItem()
        {
            Attack();
        }

        public void Attack()
        {
            _inventoryEvents.EmitUseItem(
                new UseItemData()
                {
                    action = ActionTaken.ATTACK,
                    item = this,
                    targets = null
                });
        }

        public Requires GetRequiredSlot(WeaponTypes type)
        {
            switch (type)
            {
                case WeaponTypes.SHIELD:
                    return Requires.WEAPON_SECONDARY;
                case WeaponTypes.TWO_HAND:
                case WeaponTypes.POLEARM:
                case WeaponTypes.STAFF:
                case WeaponTypes.BOW:
                    return Requires.WEAPON_2H;
                default:
                    return Requires.WEAPON_PRIMARY;
            }
        }

        public string GetDisplayName(WeaponTypes type)
        {
            return type switch
            {
                WeaponTypes.SHIELD => "Shield - Secondary Only",
                WeaponTypes.ONE_HAND => "One handed (1H)",
                WeaponTypes.ONE_HAND_PRIMARY => "One handed (1H) - Primary Only",
                WeaponTypes.TWO_HAND => "Two handed (2H)",
                WeaponTypes.POLEARM => "Polearm (2H)",
                WeaponTypes.DAGGER => "Dagger",
                WeaponTypes.STAFF => "Staff (2H)",
                WeaponTypes.BOW => "Bow (2H)",
                _ => "Weapon"
            };
        }
    }
}