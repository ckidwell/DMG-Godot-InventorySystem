using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace DMGInventorySystem
{
    [Serializable]
    public partial class InventoryManager : Node
    {
        [Export] private PackedScene inventoryPrefab;
        [Export] private PackedScene equipmentPrefab;
        [Export] private PackedScene inspectorPanel;
        [Export] private InventoryResource inventoryResource;
        
        private InventoryContainer mainHandContainer;
        private InventoryContainer offHandContainer;
    
        private InventoryExample inventoryInstance;
        private EquipmentExample equipmentInstance;
        private InspectorHelper inspectorHelper;
        private Attributes attributes;
        private InventoryEvents _inventoryEvents;
        private RandomNumberManager _randomNumberManager;
        
        [Export] public Texture2D blankSprite;

        private Dictionary<PackedScene,InventoryContainer> inventory = new Dictionary<PackedScene, InventoryContainer>();
        private Dictionary<Requires, InventoryContainer> equipment = new Dictionary<Requires, InventoryContainer>();
       
        [Export] private int maximumNumberOfAttributesOnItems = 3;

        public override void _Ready()
        {
            base._Ready();
            inventoryInstance = inventoryPrefab.Instantiate() as InventoryExample;
            equipmentInstance = equipmentPrefab.Instantiate() as EquipmentExample;
            inspectorHelper = inspectorPanel.Instantiate() as InspectorHelper;
            
            attributes = GetNode<Attributes>("/root/inventory_main/inventory_manager/attributes");
            _randomNumberManager = GetNode<RandomNumberManager>("/root/RandomNumberManager");
            _inventoryEvents = GetNode<InventoryEvents>("/root/InventoryEvents");
            _inventoryEvents.ShowInventory += OnShowInventory;
            _inventoryEvents.ShowInspector += OnShowInspector;
            _inventoryEvents.ShowEquipment += OnShowEquipment;
            _inventoryEvents.CloseInspector += OnCloseInspector;
   
            
            AddChild(equipmentInstance);
            AddChild(inventoryInstance);
            AddChild(inspectorHelper);

            CallDeferred(nameof(HideUIOnStartup));

        }

        private void HideUIOnStartup()
        {
            inspectorHelper?.ShowInspector();
            OnShowInventory();
            OnShowEquipment();
        }
        private void OnShowInspector(InspectorDataVariant inspectorData)
        {
            inspectorHelper.ShowInspector();
            inspectorHelper.SetData(inspectorData);
        }
        private void OnCloseInspector()
        {
            inspectorHelper.CloseInspector();
        }

        public void OnShowInventory()
        {
            inventoryInstance.ShowInventory();
        }
        private void OnShowEquipment()
        {
            equipmentInstance.ShowEquipment();
        }

        public int MaximumNumberOfAttributesOnItems
        {
            get
            { 
                var actualMaxAttributes = Enum.GetNames(typeof(Attributes.AttributeTypes)).Length;
                
                var max =  maximumNumberOfAttributesOnItems  > actualMaxAttributes
                    ? actualMaxAttributes
                    : maximumNumberOfAttributesOnItems;
               return max;
            }
            set => maximumNumberOfAttributesOnItems = value;
        }

         public InventoryItem[] GetWeapons()
            {
                var weaponData = inventoryResource.inventoryData.Where(i => i.itemType == ItemTypes.WEAPON).ToArray();
                var weaponItems = new InventoryItem[weaponData.Length];

                for (var i = 0; i < weaponData.Length; i++)
                {
                    var item = weaponData[i];
                    var weapon = new Weapon()
                    {
                        _resourceData = item,
                        itemName = item.itemName,
                        attributes = attributes.GetNewRandomAttributes(Attributes.GetNumAttributesForQualityLevel(item.quality), item.quality)
                    };
                    weapon.SetInventoryEvents(_inventoryEvents);
                    weaponItems[i] = weapon;
                }

                return weaponItems;
            }
            public InventoryItem[] GetRandomItems(int numberOfItems)
            {
                var count = inventoryResource.inventoryData.Length;
            
            var randomItems = new InventoryItem[numberOfItems];
            var randomData = new InventoryData[numberOfItems];
            
            for (var i = 0; i < numberOfItems; i++)
            {
                var randomIndex = GD.Randi() % count;
                var data = inventoryResource.inventoryData[randomIndex];
                randomData[i] = data;
            }

            var index = 0;
            foreach (var item in randomData)
            {
                if (item.itemType == ItemTypes.WEAPON)
                {
                    var weapon = new Weapon()
                    {
                        _resourceData = item,
                        itemName = item.itemName,
                        attributes = attributes.GetNewRandomAttributes(Attributes.GetNumAttributesForQualityLevel(item.quality), item.quality) 
                    };
                    weapon.SetInventoryEvents(_inventoryEvents);
                    randomItems[index] = weapon;
                    
                }
                else if (item.itemType == ItemTypes.POTION)
                {

                    var potion = new Potion(PotionTypes.HEALTH)
                    {
                        _resourceData = item,
                        itemName = item.itemName,
                        attributes = []
                    };
                     potion.SetInventoryEvents(_inventoryEvents);
                     randomItems[index] = potion;
                } 
                else
                {
                    randomItems[index] = new InventoryItem()
                    {
                        _resourceData = item,
                        itemName = item.itemName,
                        attributes = attributes.GetNewRandomAttributes(Attributes.GetNumAttributesForQualityLevel(item.quality), item.quality) 
                    };
                }
               
                index++;
            }
            return randomItems;
        }
        
        private InventoryData CreateEquipmentForSlotType(Requires slotRequirement)
        {
            var matchingItems = inventoryResource.inventoryData
                .Where(item => item.thisSlotRequires == slotRequirement)
                .ToArray();
            
            // If no matching items found, return null
            if (matchingItems.Length == 0)
                return null;

            var randomIndex = GD.Randi() % matchingItems.Length;
            return matchingItems[randomIndex];
        }

        private bool AddToStackIfPossible(InventoryContainer destinationItem, InventoryContainer droppedItem)
        {
            //do this later
            
            // if (!destinationItem.equipment.itemType.Equals(droppedItem.equipment.itemType)
            //     || !destinationItem.equipment.stackable) return false;
            //
            // // this is not a redundant check to the above because were checking the item.equipment version item.itemType above
            // if (droppedItem.equipment.GetType() == typeof(Potion) && destinationItem.equipment.GetType() == typeof(Potion)) 
            // {
            //     Potion dropped = (Potion)droppedItem.equipment;
            //     Potion destination  = (Potion)destinationItem.equipment;
            //
            //     if (!dropped.potionType.Equals(destination.potionType)) return false;
            // }
            // destinationItem.equipment.quantity += droppedItem.equipment.quantity;
            // destinationItem.stackText.text = destinationItem.equipment.quantity.ToString();
            //
            // SetBlank(droppedItem);
            return true;
        }
        
    }
}