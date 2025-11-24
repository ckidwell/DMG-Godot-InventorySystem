using Godot;
using System;

public partial class UseItemData : Node
{
    public ActionTaken action { get; set; }
    
    public InventoryItem item;

    public Node[] targets;
}
