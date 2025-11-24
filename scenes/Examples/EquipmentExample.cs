using DMGInventorySystem;
using Godot;

public partial class EquipmentExample : Control
{
    [Export] private Panel equipmentPanel;
    
    public void ShowEquipment()
    {
        equipmentPanel.Visible = !equipmentPanel.Visible;
    }
}
