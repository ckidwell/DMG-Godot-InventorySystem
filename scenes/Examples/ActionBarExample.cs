using Godot;
using DMGInventorySystem;

public partial class ActionBarExample : PanelContainer
{
    [Export] private InventoryContainer mainHandContainer;
    [Export] private InventoryContainer offHandContainer;
    
    [Export] private InventoryContainer actionBarContainer1;
    [Export] private InventoryContainer actionBarContainer2;
    [Export] private InventoryContainer actionBarContainer3;
    [Export] private InventoryContainer actionBarContainer4;
    
    private InventoryManager _inventoryManager;

    public override void _Ready()
    {
        base._Ready();
        _inventoryManager = GetNode<InventoryManager>("/root/inventory_main/inventory_manager");
    }

    public void SetInventory(int slot, InventoryItem item)
    {
        switch (slot)
        {
            case 0:
                actionBarContainer1.SetData(item);
                break;
            case 1:
                actionBarContainer2.SetData(item);
                break;
            case 2:
                actionBarContainer3.SetData(item);
                break;
            case 3:
                actionBarContainer4.SetData(item);
                break;
            default:
                break;
        }
    }
}
