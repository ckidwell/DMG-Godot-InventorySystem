using Godot;
using System;
using DMGInventorySystem;

public partial class InventoryEvents : Node
{

    [Signal]
    public delegate void ShowInventoryEventHandler();

    [Signal]
    public delegate void ShowEquipmentEventHandler();

    [Signal]
    public delegate void ShowInspectorEventHandler(InspectorDataVariant inspectorData);

    [Signal]
    public delegate void CloseInspectorEventHandler();

    [Signal]
    public delegate void UseItemEventHandler(UseItemData data);

    public void EmitShowInventory()
    {
        EmitSignal(SignalName.ShowInventory);
    }

    public void EmitShowEquipment()
    {
        EmitSignal(SignalName.ShowEquipment);
    }

    public void EmitShowInspector(InspectorDataVariant inspectorData)
    {
        EmitSignal(SignalName.ShowInspector, inspectorData);
    }

    public void EmitCloseInspector()
    {
        EmitSignal(SignalName.CloseInspector);
    }

    public void EmitUseItem(UseItemData data)
    {
        EmitSignal(SignalName.UseItem, data);
    }
    

}
