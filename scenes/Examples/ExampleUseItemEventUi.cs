using Godot;
using System;
using System.Threading.Tasks;

public partial class ExampleUseItemEventUi : CanvasLayer
{
    [Export] private VBoxContainer useItemEventContainer;

    private InventoryEvents _inventoryEvents;


    public override void _Ready()
    {
        base._Ready();
        _inventoryEvents = GetNode<InventoryEvents>("/root/InventoryEvents");
        _inventoryEvents.UseItem += OnUseItem;
    }

    private void OnUseItem(UseItemData data)
    {
        var label = new Label
        {
            Text = $"Used item: {data.item.itemName}"
                   + $"\nAction taken: {data.action}"
        };

        label.Modulate = new Color(1, 1, 1, 1);

        useItemEventContainer.AddChild(label);
        
        _ = FadeOutAndRemoveLabel(label);
    }

    private async Task FadeOutAndRemoveLabel(Label label)
    {
        // Use an ease-in curve so most of the change happens near the end.
        var tween = GetTree().CreateTween();
        tween.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(label, "modulate:a", 0f, 2.8);

        // Wait for the tween to finish, then remove the label
        await ToSignal(tween, Tween.SignalName.Finished);

        if (IsInstanceValid(label))
        {
            label.QueueFree();
        }
    }
}
