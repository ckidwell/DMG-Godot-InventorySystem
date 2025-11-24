using Godot;

namespace DMGInventorySystem
{
    [GlobalClass]
    [System.Serializable]
    public partial class InventoryData : Resource
    {
        [Export] public string itemName = "Item Name";
        
        [Export] public string description = "Description Text";
        
        [Export] public Texture2D icon;
        
        [Export] public bool stackable;
      
        [Export] public Requires thisSlotRequires = Requires.NONE;
        
        [Export] public ItemTypes itemType;

        [Export] public ItemQuality quality;

        [Export] public int price = 1;
        
        [Export] public int quantity = 1;

        [Export] public bool isBlank = true;
        
    

    }
    
}

