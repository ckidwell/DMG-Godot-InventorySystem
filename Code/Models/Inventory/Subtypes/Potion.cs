using Godot;

namespace  DMGInventorySystem
{
    [System.Serializable]
    public partial class Potion : InventoryItem
    {
        public Potion(PotionTypes potionType)
        {
            this.potionType = potionType;
        }
        
        [Export]
        public PotionTypes potionType;
        
       [Export]
        public PotionStrengths potionStrength;

        private InventoryEvents _inventoryEvents;
        private int spellEffectID;
        public int SpellEffectID { get; set; }

        public override void _Ready()
        {
            _inventoryEvents = GetNode<InventoryEvents>("/root/InventoryEvents");
        }
        public void SetInventoryEvents(InventoryEvents inventoryEvents) => _inventoryEvents = inventoryEvents;
        public override void UseItem()
        {
            _inventoryEvents.EmitUseItem(
                new UseItemData()
                {
                    action = ActionTaken.USE_POTION,
                    item = this,
                    targets = null
                });
        }
    }
}