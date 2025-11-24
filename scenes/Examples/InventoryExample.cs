using System.Collections.Generic;
using System.Linq;
using DMGInventorySystem;
using Godot;


public partial class InventoryExample : Control
{
    [Export] private Panel inventoryPanel;
    [Export] private GridContainer inventoryGridContainer;
    private List<InventoryContainer> inventory = [];

    public override void _Ready()
    {
        base._Ready();
        inventory = inventoryGridContainer.GetChildren().OfType<InventoryContainer>().ToList();
    }

    public void ShowInventory()
    {
        inventoryPanel.Visible = !inventoryPanel.Visible;
    }

    public void LoadItems(InventoryItem[] items)
    {
        var emptySlots = inventory.Where(slot => slot.IsBlank()).ToList();
        foreach (var item in items)
        {
            if (emptySlots.Count > 0)
            {
                emptySlots[0].SetData(item);
                emptySlots.RemoveAt(0);
            }
        }
    }
    
}
