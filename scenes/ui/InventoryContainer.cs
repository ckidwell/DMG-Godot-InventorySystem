using Godot;

namespace DMGInventorySystem
{
    public partial class InventoryContainer : TextureRect
    {
        [Export] private TextureRect equippedItemTextureRect;
        
        [ExportGroup("Drag and Drop Tinting")]
        [Export] private Color _default_Color = new Color(1, 0, 0, 1);
        [Export] private Color _can_NOT_drag_Color = new Color(1, 0, 0, 1);
        [Export] private Color _CAN_drag_Color = new Color(1, 0, 0, 1);
        
        [ExportGroup("Container Requirements")]
        [Export]
        public Requires slotRequirement  = Requires.NONE;
        
        [ExportGroup("Default Background Icons")]
        [Export] private Texture2D helmetIcon;
        [Export] private Texture2D chestIcon;   
        [Export] private Texture2D pantsIcon;
        [Export] private Texture2D handIcon;
        [Export] private Texture2D blankIcon;

        [ExportGroup("Reference to other Weapon Slot Nodes if Applicable")] [Export]
        public InventoryContainer _otherWeaponSlot;
        
        private InventoryEvents _inventoryEvents;
        private Texture2D _originalTextureBeforeDrag = null;
        public InventoryItem _item = null;
        public Label stackText;
        private bool _dragDropSuccessful = false;
        private bool _mouseExited = false;
        private Timer _mouseHoverTimer = null;
        private Timer _itemReuseTimer = null;
        private bool canUseItem = true;
        
        public override void _Ready()
        {
            base._Ready();
            
            _inventoryEvents = GetNode<InventoryEvents>("/root/InventoryEvents");

            _mouseHoverTimer = GetNode<Timer>("Timer");
            _mouseHoverTimer.Connect("timeout", Callable.From(OnWaitForMouseHoverTimeout));
            
            _itemReuseTimer = GetNode<Timer>("ItemReuseTimer");
            _itemReuseTimer.Connect("timeout", Callable.From(() => canUseItem = true ));
            
            stackText = GetNode<Label>("stacktext");
            stackText.SetText("");

            ClearData();
            SetSlotIcon();
   
            Connect(signal: "mouse_entered",Callable.From(OnMouseEntered));
            Connect(signal: "mouse_exited",Callable.From(OnMouseExited));
        }

     

        public bool IsBlank() =>  _item._resourceData.isBlank;
        
        private void SetSlotIcon()
        {
            Texture = slotRequirement switch
            {
                Requires.CHEST => chestIcon,
                Requires.HEAD => helmetIcon,
                Requires.FEET => pantsIcon,
                Requires.WEAPON_1H or Requires.WEAPON_2H or Requires.WEAPON_PRIMARY or Requires.WEAPON_SECONDARY => handIcon,
                _ => Texture
            };
        }
        public void SetData(InventoryItem item)
        {
            _item = item;
            equippedItemTextureRect.Texture = item._resourceData.icon;
            stackText.SetText(item._resourceData.quantity > 1 ? item._resourceData.quantity.ToString() : "");
            SetSlotIcon();
        }

        public InventoryItem ClearData()
        {
            var resourceData = new InventoryData()
            {
                icon = blankIcon,
                itemName = "",
                description = "",
                thisSlotRequires = Requires.NONE,
                price = 0,
                quantity = 0,
                isBlank = true,
            };

            _item = new InventoryItem();
            _item.SetData(resourceData);
            _item.itemName = "";
           
            return _item;
        }

        public override Variant _GetDragData(Vector2 atPosition)
        {
            var textureRect = new TextureRect();
            textureRect.Texture = equippedItemTextureRect.Texture;
            
            SetDragPreview(textureRect);
            
            var dragVariant = new DragInventoryVariant()
            {
                item = _item,
                icon = equippedItemTextureRect.Texture,
                myContainer = this,
            };
            
            _originalTextureBeforeDrag = equippedItemTextureRect.Texture;
            _dragDropSuccessful = false;
            equippedItemTextureRect.Texture = blankIcon;
            
            return dragVariant;
        }
        public override void _Notification(int notificationCode)
        {
            base._Notification(notificationCode);
        
            if (notificationCode == (int)NotificationDragEnd)
            {
                // If we have a stored original texture and the current texture is still blank,
                // it means the drag was cancelled, so restore the original texture
                if (_originalTextureBeforeDrag != null && !_dragDropSuccessful)
                {
                    equippedItemTextureRect.Texture = _originalTextureBeforeDrag;
                }
            
                // Clear the stored texture
                _originalTextureBeforeDrag = null;
                _dragDropSuccessful = false;
                
                // Reset color in case it was changed during drag
                SetColor(_default_Color);
            }
        }
        public override bool _CanDropData(Vector2 atPosition, Variant data)
        {
            var dragVariant = (DragInventoryVariant) data;

            if ( dragVariant.item == null)
                return false;

            if (_item._resourceData.isBlank) return CanDropOnBlankSlot(dragVariant);
            
            return CanDropOnNonBlankSlot(dragVariant);

        }

        private bool CanDropOnBlankSlot(DragInventoryVariant dragVariant)
        {
            var canSwap = false;
            if (SwapItemUtility.SlotHasWeaponRequirement(slotRequirement))
            {
                if (_otherWeaponSlot._item._resourceData.thisSlotRequires == Requires.WEAPON_2H)
                {
                    canSwap = false;
                }
                else if (!SwapItemUtility.SlotHasWeaponRequirement(dragVariant.item._resourceData.thisSlotRequires))
                {
                    // you are dragging a non-weapon into a weapon required slot
                    canSwap = false;
                }
                else
                {
                    //GD.Print($" checking a blank slot - but weapon requirement");
                    canSwap = SwapItemUtility.CanSwapWeapon(dragVariant, _otherWeaponSlot, slotRequirement);
                }

            }
            else
            {
                canSwap = dragVariant.item._resourceData.thisSlotRequires == slotRequirement ||
                          slotRequirement == Requires.NONE;
            }

            
            if (dragVariant.item != _item) // as long as the item is not the same as the one being dragged update the color
            {
                UpdateDragStatusColor(canSwap, dragVariant);    
            }
            
            return canSwap;
        }

        private bool CanDropOnNonBlankSlot(DragInventoryVariant dragVariant)
        {
            var canSwap = false;

            //bug - if I drag a shield onto the primary hand slot the shield icon dissapears...
            //bug - if i drag a 1h non-primary weapon from secondary slot into the primary slot when primary has a primary only weapon then it swaps when it should not
            
            if (SwapItemUtility.SlotHasWeaponRequirement(slotRequirement))
            {
                if (_otherWeaponSlot._item._resourceData.thisSlotRequires == Requires.WEAPON_2H)
                {
                    canSwap = false;
                }
                else if (!SwapItemUtility.SlotHasWeaponRequirement(dragVariant.item._resourceData.thisSlotRequires))
                {
                    // you are dragging a non-weapon into a weapon required slot
                    canSwap = false;
                }
                else
                {
                    canSwap = SwapItemUtility.CanSwapWeapon(dragVariant, _otherWeaponSlot, slotRequirement);    
                }
                
            }
            else
            {
                canSwap = SwapItemUtility.CanSwap(dragVariant, slotRequirement);    
            }
                

            if (dragVariant.item != this._item)
            {
                UpdateDragStatusColor(canSwap, dragVariant);    
            }
                
            return canSwap;
        }

        public override void _DropData(Vector2 atPosition, Variant data)
        {
            var dragVariant = (DragInventoryVariant) data;

            if (dragVariant.item == null)
                return;

            // Mark the drag as successful in the source container
            if (dragVariant.myContainer != null)
            {
                dragVariant.myContainer._dragDropSuccessful = true;
            }

            // there is already an item here, so we need to swap it with the new item
            if (!_item._resourceData.isBlank)
            {
                if (_item._resourceData.itemType == ItemTypes.WEAPON && SwapItemUtility.CanSwapWeapon(dragVariant,_otherWeaponSlot, slotRequirement))
                {
                    var swapItem = _item;
                    _item = dragVariant.item;
                    equippedItemTextureRect.Texture = dragVariant.icon;
                    dragVariant.myContainer.SetData(swapItem);
                    GD.Print($"Setting Icon for Weapon ");
                }
                else if(SwapItemUtility.CanSwap(dragVariant, slotRequirement))
                {
                    var swapItem = _item;
                    _item = dragVariant.item;
                    equippedItemTextureRect.Texture = dragVariant.icon;
                    dragVariant.myContainer.SetData(swapItem);
                    GD.Print($"Setting Icon for regular item ");
                }
                
            }
            else
            {
                // there was no item here, so we can set the new item and clear it from old slot
                GD.Print($"Clearing icon for no item here");
                _item = dragVariant.item;
                equippedItemTextureRect.Texture = dragVariant.icon;
                
                var clearedData = dragVariant.myContainer.ClearData();
                dragVariant.myContainer.SetData(clearedData);
            }
            
            SetColor(_default_Color);
            dragVariant.myContainer.SetColor(_default_Color);
        }

        public override void _GuiInput(InputEvent @event)
        {
            if (@event is not InputEventMouseButton {ButtonIndex: MouseButton.Right}) return;
            
            if (_item._resourceData.isBlank) return;
            
            if (_item._resourceData.itemType == ItemTypes.WEAPON)
            {
                if(slotRequirement != Requires.WEAPON_1H && 
                   slotRequirement != Requires.WEAPON_SECONDARY && 
                   slotRequirement != Requires.WEAPON_2H &&
                   slotRequirement != Requires.WEAPON_PRIMARY 
                   ) return;
            }
            OnRightClick();
        }
        private void OnRightClick()
        {
            if (_item == null || _item._resourceData.isBlank || !canUseItem) return;

            canUseItem = false;
            _itemReuseTimer.Start();
            
            _item.UseItem();
        }
        private void UpdateDragStatusColor(bool canSwap, DragInventoryVariant dragVariant)
        {
            var newColor = SelfModulate = canSwap ? _CAN_drag_Color : _can_NOT_drag_Color;
            SetColor(newColor);
        }

        public void SetColor(Color newColor)
        {
            SelfModulate = newColor;
        }

        private void OnMouseEntered()
        {
            if(_item._resourceData.isBlank) return;

            _mouseExited = false;
            _mouseHoverTimer.Stop();
            _mouseHoverTimer.Start();
        }

        private void OnWaitForMouseHoverTimeout()
        {
            if (_mouseExited)
                return;

            var inspectorData = new InspectorDataVariant()
            {
                item = _item,
                icon = equippedItemTextureRect.Texture,
                mousePosition = GetViewport().GetMousePosition()
            };
            _inventoryEvents.EmitShowInspector(inspectorData);
        }

        public void OnMouseExited()
        {
            _mouseExited = true;
            SetColor(_default_Color);
            _inventoryEvents.EmitCloseInspector();
        }
        public InventoryItem GetData()
        {
            return _item;
        }
    }
}