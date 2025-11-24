using Godot;
using DMGInventorySystem;

public partial class TestLoadWeapons : Button
{
    private InventoryManager _inventoryManager;
    private InventoryExample inventoryExample; 
    
    public override void _Ready()
    {
        base._Ready();
        _inventoryManager = GetNode<InventoryManager>("/root/inventory_main/inventory_manager");
        inventoryExample = GetNode<InventoryExample>("/root/inventory_main/inventory_manager/InventoryExample");
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        var items = _inventoryManager.GetWeapons();
        inventoryExample.LoadItems(items);
    }
}
