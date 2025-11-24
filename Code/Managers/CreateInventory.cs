using System;
using System.Linq;
using Godot;


namespace DMGInventorySystem
{
    public partial class CreateInventory : Node
    {
        public InventoryDatabase weapons;
        public InventoryDatabase equipment;
        public PotionDatabase potion;
        public Texture2D blankIcon;
        private RandomNumberManager _randomNumberManager;

        public override void _Ready()
        {
            base._Ready();
            _randomNumberManager = GetNode<RandomNumberManager>("/root/RandomNumberManager");
        }

        public InventoryItem CreateRandomItem()
        {
            // TODO hard coded percentages of equipment distribution 
            // either expose this in an editor or maybe parameterize it 
            var val = _randomNumberManager.GetRandomNumber(0,100);

            //if (val >= 15) return val < 75 ? CreateRandomEquipment() : CreateRandomPotion();

            if (val < 15)
            {
                return CreateRandomEquipment();
            }
            if (val < 75)
            {
                return CreateRandomPotion();
            }
            
            var slot = Requires.WEAPON_1H;
            var weapon = CreateRandomWeapon(slot);
            weapon._resourceData.itemType = ItemTypes.WEAPON;
            
            return weapon;
        }
        public InventoryItem CreateRandomWeapon(Requires slotRequirement)
        {
            var randomNumber = _randomNumberManager.GetRandomNumber(0, weapons.inventoryItems.Length);
            
            var resourceData = new InventoryData
            {
                itemType = ItemTypes.WEAPON,
                icon = weapons.inventoryItems[randomNumber].icon,
                itemName = weapons.inventoryItems[randomNumber].itemName,
                thisSlotRequires = slotRequirement
            };
            var newWeapon = new InventoryItem()
            {
                _resourceData = resourceData,
                itemName = resourceData.itemName,
            };
            

            return newWeapon;
        }
        public InventoryItem CreateRandomEquipment()
        {
            var values = Enum.GetValues(typeof(Requires));
            var requires = (Requires) values.GetValue(_randomNumberManager.GetRandomNumber(0, values.Length));
            return CreateEquipment(requires);
        }
        public InventoryItem CreateEquipment(Requires slotRequirement)
        {
            var dataItem = equipment.inventoryItems.Where((x) => x.thisSlotRequires == slotRequirement).ToList();
            if (dataItem.Count < 1)
            {
               
                var resourceData = new InventoryData()
                {
                    icon = blankIcon,
                    itemName = "",
                    description = "",
                    thisSlotRequires = Requires.NONE,
                    price = 0
                };
                return new InventoryItem()
                {
                    _resourceData = resourceData,
                    itemName = resourceData.itemName,
                };
            }
                
            var randomNumber = _randomNumberManager.GetRandomNumber(0, dataItem.Count);
            var newResourceData = new InventoryData
            {
                itemType = ItemTypes.EQUIPMENT,
                icon = dataItem[randomNumber].icon,
                thisSlotRequires = dataItem[randomNumber].thisSlotRequires,
                itemName = dataItem[randomNumber].itemName
            };
            
            return new InventoryItem()
            {
                _resourceData = newResourceData,
                itemName = newResourceData.itemName,
            };

        }
        public InventoryItem CreateRandomPotion()
        {
            var randomNumber = _randomNumberManager.GetRandomNumber(0, potion.potions.Length);
            var types =   Enum.GetValues(typeof(PotionTypes));
            var potionType = (PotionTypes) types.GetValue(randomNumber);
            
            var iconToUse = potion.potions[randomNumber]._resourceData.icon;
            
            var resourceData = new InventoryData(){
                stackable = true,
                itemType = ItemTypes.POTION,
                icon = iconToUse,
                thisSlotRequires = potion.potions[randomNumber]._resourceData.thisSlotRequires,
                itemName = potion.potions[randomNumber].itemName,
                quantity = 1,
            };
            
            return new InventoryItem()
            {
                _resourceData = resourceData,
                itemName = resourceData.itemName,
            };
        }
    }
}