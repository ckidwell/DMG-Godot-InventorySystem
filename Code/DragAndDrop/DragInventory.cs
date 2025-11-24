using DMGInventorySystem;
using Godot;


public partial class DragInventory : Control
{
    private InventoryManager _inventoryManager;
    [Export] private Color _defaultColor = new Color(1, 1, 1, 1);
    [Signal] public delegate void DragCompletedEventHandler(InventoryData item);

    private Control source;
    private Control target;
    
    private InventoryContainer container;
    private Control preview;

    public override void _Ready()
    {
        base._Ready();
        _inventoryManager = GetNode<InventoryManager>("/root/inventory_main/inventory_manager");
    }

    public void Init(Control _source, Control _target, InventoryContainer _item, Control _preview)
    {
        source = _source;
        target = _target;
        container = _item;
        preview = _preview;
        
        preview.Connect(Node.SignalName.TreeExiting, Callable.From(OnTreeExiting));
    }
    
    private void OnTreeExiting()
    {
        container.SetColor( _defaultColor);
        EmitSignal(SignalName.DragCompleted, container);
    }
}
