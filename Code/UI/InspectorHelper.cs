using System;
using System.Collections.Generic;
using Godot;

namespace DMGInventorySystem
{
    public partial class InspectorHelper : Node2D
    {
        [Export] PanelContainer panelContainer;
        [Export] private int inspectorYOffset = 100;
        
        public PackedScene itemTextPrefab;
        public PackedScene attributeScrollViewParent;
        private InventoryItem _inspectorItem;
        
        private InventoryItem _inspectorBaseInventory;
 
        [Export] private Color commonQualityColor = new Color(50, 50, 50, 1);
        [Export] private Color rareQualityColor = new Color(0, 255, 0, 1);
        [Export] private Color superiorQualityColor = new Color(0, 0, 255);
        [Export] private Color eliteQualityColor = new Color(153, 51, 255, 1);
        
        [Export] private TextureRect itemImage;
        [Export] private Label itemName;
        [Export] private Label itemType;
        
        private List<PackedScene> _statList = new List<PackedScene>();
        [Export] private Label itemArmor;
        [Export] private Label itemValue;
        [Export] private Label chargesValue;

        [Export] private Label attribute_one;
        [Export] private Label attribute_two;
        [Export] private Label attribute_three;
        
        public void ShowInspector()
        {
            panelContainer.Visible = !panelContainer.Visible;
        }
        public void CloseInspector()
        {
            panelContainer.Visible = false;
        }
        public void SetData(InspectorDataVariant inspectorData)
        {
            if(inspectorData == null || inspectorData.item._resourceData.isBlank) return;

            ShowInspectorItem(inspectorData.item);
            SetInspectorPosition(inspectorData.mousePosition);
        }
        private void SetInspectorPosition( Vector2 pointerPosition)
        {
            var screenSize = DisplayServer.WindowGetSize();
            var middle = (float)screenSize.X / 2;
            
            float xPosition = 0;
            if (pointerPosition.X < middle) xPosition = middle;

            panelContainer.Position = new Vector2(xPosition + 50, screenSize.Y /2  - inspectorYOffset);
          
        }
        public void ShowInspectorItem(InventoryItem  inventoryitem)
        {
            var item = inventoryitem._resourceData;
            
            itemName.Modulate = GetColorForQuality(item.quality);
            itemName.SetText(item.itemName); 
            itemImage.Texture = item.icon;
            itemType.SetText(GetItemTypeText(item));
            if (item.itemType == ItemTypes.ARMOR)
            {
                var armor = item as ArmorData;
                itemArmor.SetText( $"Armor : {armor?.armorAmount}");
            }
            else
            {
                itemArmor.SetText("");
            }
            itemValue.SetText($"Value : {item.price.ToString()}" );
            var chargeCount = inventoryitem.charges;
            var chargesText = chargeCount > 0 ? $"Charges : {inventoryitem.charges.ToString()}" : "";
            chargesValue.SetText(chargesText);
            SetAttributes(inventoryitem);
        }

        private string GetItemTypeText(InventoryData item)
        {
            return item.itemType switch
            {
                ItemTypes.EQUIPMENT => GetNameForSlot(item.thisSlotRequires),
                ItemTypes.WEAPON => GetWeaponItemTypeText(item.thisSlotRequires),
                ItemTypes.SCROLL => "Scroll",
                ItemTypes.POTION => "Potion",
                ItemTypes.CONTAINER => "Container",
                ItemTypes.QUEST_ITEM => "Quest Item",
                ItemTypes.SPELL => "Spell",
                ItemTypes.BLANK_ITEM => "",
                ItemTypes.WORLD_ITEM => "",
                _ => ""
            };
        }

        private string GetWeaponItemTypeText(Requires requirement)
        {
            return requirement switch
            {
                Requires.WEAPON_1H => "Weapon (1H)",
                Requires.WEAPON_2H => "Weapon (2H)",
                Requires.WEAPON_PRIMARY => "Primary Hand Weapon (1H)",
                Requires.WEAPON_SECONDARY => "Secondary Hand Weapon (1H)",
                _ => "Weapon"
            };
        }

        private void SetAttributes(InventoryItem item)
        {
            
            if (item.attributes == null || item.attributes is {Length: 0})
            {
                attribute_one.SetText("");
                attribute_two.SetText("");
                attribute_three.SetText("");
                return;
            }
            
            attribute_one.SetText($"+{item.attributes[0].attributeValue} {item.attributes[0].GetAttributeName(item.attributes[0].attribute)}");

            if (item.attributes.Length < 2)
            {
             attribute_two.SetText("");
             attribute_three.SetText("");
             return;
            }
            
            attribute_two.SetText($"+{item.attributes[1].attributeValue} {item.attributes[1].GetAttributeName(item.attributes[1].attribute)}");

            if (item.attributes.Length < 3)
            {
                attribute_three.SetText("");
                return;
            }
            
            attribute_three.SetText($"+{item.attributes[2].attributeValue} {item.attributes[2].GetAttributeName(item.attributes[2].attribute)}");
            
        }
        private Color GetColorForQuality(ItemQuality quality)
        {
            switch (quality)
            {
                case ItemQuality.COMMON:
                    return commonQualityColor;
                case ItemQuality.RARE:
                    return rareQualityColor;
                case ItemQuality.SUPERIOR:
                    return superiorQualityColor;
                case ItemQuality.ELITE:
                    return eliteQualityColor;
                default:
                    return new Color(211,211,211,255);;
            }
        }

        private string GetNameForSlot(Requires slot)
        {
            return slot switch
            {
                Requires.NONE => "",
                Requires.ARM => "Arm",
                Requires.CHEST => "Chest",
                Requires.EAR => "Ear",
                Requires.FEET => "Feet",
                Requires.HANDS => "Hands",
                Requires.HEAD => "Head",
                Requires.LEGS => "Legs",
                Requires.NECK => "Neck",
                Requires.RING => "Ring",
                Requires.WEAPON_1H => "Either hand.",
                Requires.WEAPON_2H => "Both hands.",
                Requires.WEAPON_PRIMARY => "Primary hand",
                Requires.WEAPON_SECONDARY => "Secondary hand",
                Requires.WRIST => "Wrist",
                _ => ""
            };
        }
    }
}
