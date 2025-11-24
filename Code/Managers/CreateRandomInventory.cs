using System.Collections.Generic;
using Godot;

namespace DMGInventorySystem
{
    public partial class CreateRandomInventory : Node
    {
        private CreateInventory _createInventory;
        public PackedScene inventoryPrefab;
        private RandomNumberManager _randomNumberManager;


        public override void _Ready()
        {
            base._Ready();
            _randomNumberManager = GetNode<RandomNumberManager>("/root/RandomNumberManager");
            _createInventory = GetNode<CreateInventory>("CreateInventory");
        }


        public Dictionary<Node, InventoryContainer> CreateSomeRandomItems(int count, int MaximumNumberOfAttributesOnItems)
        {
            var items = new Dictionary<Node, InventoryContainer>();

            if (count <= 0)
            {
                count = 21;
            }
            
            for (var i = 0; i < count; i++)
            {
                var equipment = _createInventory.CreateRandomItem();
                
                var tscn = inventoryPrefab.Instantiate();
                
                var inventoryItem = tscn as  InventoryContainer;
                if (inventoryItem == null) continue;
                
                inventoryItem.SetData(equipment);
                tscn.Name = equipment.itemName;

                var magicAttribute = new Attributes();
           
                var numAttributes = _randomNumberManager.GetRandomNumber(0, MaximumNumberOfAttributesOnItems);
                var quality = _randomNumberManager.GetRandomNumber(0, 4);

                if (inventoryItem.GetData()._resourceData.itemType.Equals(ItemTypes.POTION))
                {
                    quality = 0;
                    numAttributes = 0;
                }

                // if (numAttributes > 0)
                //     inventoryItem.equipment.attributes = magicAttribute.newAttributes(numAttributes);

                var qualityLevel = (ItemQuality) quality;
                inventoryItem.GetData()._resourceData.quality = qualityLevel;
                items.Add(tscn, inventoryItem);
            }

            return items;
        }
    }
}