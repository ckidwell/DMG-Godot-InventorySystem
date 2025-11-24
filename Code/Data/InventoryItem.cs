using Godot;
using DMGInventorySystem;
using System;

public partial class InventoryItem : Node
{
    /*
     there is an itemName in both InventoryData and InventoryItem because of procedural 
     generation and Resources are combined.  Resources all point to the same object -  
     but when we procedurally generate based off of a resource we need the root name
     from the resource and then combine it with the prefix/suffix to get the final name
     */
    [Export] public string itemName = "Item Name";
    public InventoryData _resourceData;
    [Export]
    public Attributes[] attributes;
    [Export]
    public MagicProperty magicProperty;
    [Export]
    public string itemPrefix;
    [Export]
    public string itemSuffix;

    public int charges = 0;
    
    public void SetData(InventoryData resourceData)
    {
        _resourceData = resourceData;
    }

    public virtual void UseItem()
    {
        
    }
}
