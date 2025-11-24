using Godot;
using DMGInventorySystem;

public partial class TestButton : Button
{
    private InventoryManager _inventoryManager;
    [Export] ActionBarExample actionBarExample; 
    
    public override void _Ready()
    {
        base._Ready();
        _inventoryManager = GetNode<InventoryManager>("/root/inventory_main/inventory_manager");
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        var items = _inventoryManager.GetRandomItems(4);
        for (var i = 0; i < items.Length; i++)
        {
            actionBarExample.SetInventory(i,items[i]);
        }
    }
}
