using Godot;

public partial class ExampleInventoryButton : Button
{
    private InventoryEvents _inventoryEvents;
    public override void _Ready()
    {
        base._Ready();
        _inventoryEvents = GetNode<InventoryEvents>("/root/InventoryEvents");
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        _inventoryEvents.EmitShowInventory();
    }
}
