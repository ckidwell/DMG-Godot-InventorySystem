using Godot;
using System;
using DMGInventorySystem;

namespace DMGInventorySystem
{
    public static class SwapItemUtility
    {
        public static bool CanSwap(DragInventoryVariant dragVariant, Requires slotRequirement)
        {
            var matchesSlotRequirement = dragVariant.item._resourceData.thisSlotRequires == slotRequirement;
            var noRequirement = slotRequirement == Requires.NONE;
            var otherSlotAcceptsThisItem = dragVariant.myContainer.slotRequirement == slotRequirement ||
                                           dragVariant.myContainer.slotRequirement == Requires.NONE;
            var canSwap = (matchesSlotRequirement || noRequirement) && otherSlotAcceptsThisItem;

            return canSwap;
        }
      
        public static bool CanSwapWeapon(DragInventoryVariant draggedVariant, InventoryContainer otherWeapon, Requires slotRequirement)
        {
            var dragged_SlotRequirement = draggedVariant.item._resourceData.thisSlotRequires;

            if (dragged_SlotRequirement == Requires.WEAPON_2H)
            {
                if (otherWeapon == null)
                {
                    return true;
                }
                if (!otherWeapon._item._resourceData.isBlank || otherWeapon._item._resourceData.thisSlotRequires == Requires.WEAPON_2H ) return false;
                return true;
            }

            // if a primary or secondary item is going in an appropriate slot, allow it
            if (dragged_SlotRequirement == Requires.WEAPON_PRIMARY && slotRequirement == Requires.WEAPON_PRIMARY ||
                dragged_SlotRequirement == Requires.WEAPON_SECONDARY && slotRequirement == Requires.WEAPON_SECONDARY)
            {
                return true;
            }
            
            
            // if a primary or secondary required is going in the wrong slot, return false
            if (dragged_SlotRequirement == Requires.WEAPON_PRIMARY && slotRequirement != Requires.WEAPON_PRIMARY)
            {
                return false;
            }

            if (dragged_SlotRequirement == Requires.WEAPON_SECONDARY && slotRequirement != Requires.WEAPON_SECONDARY)
            {
                return false;
            }
            
            // if we are dragging from a non-weapon slot we do not need to perform these checks
            if (draggedVariant.myContainer.slotRequirement != Requires.NONE)
            {
                if (slotRequirement == Requires.WEAPON_PRIMARY ||
                    slotRequirement == Requires.WEAPON_SECONDARY)
                {
                    return !EitherSlotRequiresAndContainsAPrimaryOrSecondary(otherWeapon);
                }
                // if we are trying to drag a normal 1H into a slot that already has a PRIMARY or SECONDARY then do not allow it
                if (otherWeapon._item._resourceData.thisSlotRequires == Requires.WEAPON_PRIMARY || dragged_SlotRequirement == Requires.WEAPON_PRIMARY)
                {
                    // do not allow when a WEAPON_PRIMARY is already in the slot
                    return  false;
                }
                if (otherWeapon._item._resourceData.thisSlotRequires == Requires.WEAPON_SECONDARY || dragged_SlotRequirement == Requires.WEAPON_SECONDARY)
                {
                    // do not allow when a WEAPON_SECONDARY is already in the slot
                    return  false;
                }

                if (OtherSlotRequiresAndContainsAPrimaryOrSecondary(otherWeapon)) return false;
            }
            
            
            // dragging a general 1H into a weapon slot
            if (dragged_SlotRequirement == Requires.WEAPON_1H && slotRequirement == Requires.WEAPON_PRIMARY ||
                dragged_SlotRequirement == Requires.WEAPON_1H && slotRequirement == Requires.WEAPON_SECONDARY)
            {

                if (OtherSlotRequiresAndContainsAPrimaryOrSecondary(otherWeapon))
                {
                    return false;
                }
                return true;
            }
            
            return dragged_SlotRequirement == slotRequirement;
        }
      
        private static bool EitherSlotRequiresAndContainsAPrimaryOrSecondary(InventoryContainer otherWeapon)
        {
            // this is very hacky - but not sure a better way to do this
            // essentially each PRI/SEC weapon slot has a reference to the other, so we are relying on that to go back n forth
            if (otherWeapon._otherWeaponSlot.slotRequirement == Requires.WEAPON_PRIMARY &&
                otherWeapon._otherWeaponSlot._item._resourceData.thisSlotRequires == Requires.WEAPON_PRIMARY ||
                otherWeapon._otherWeaponSlot.slotRequirement == Requires.WEAPON_SECONDARY &&
                otherWeapon._otherWeaponSlot._item._resourceData.thisSlotRequires == Requires.WEAPON_SECONDARY ||
                otherWeapon._item._resourceData.thisSlotRequires == Requires.WEAPON_PRIMARY &&
                otherWeapon.slotRequirement == Requires.WEAPON_PRIMARY ||
                otherWeapon._item._resourceData.thisSlotRequires == Requires.WEAPON_SECONDARY &&
                otherWeapon.slotRequirement == Requires.WEAPON_SECONDARY)
                
            {
                return true;
            }

            return false;
        }
        private static bool OtherSlotRequiresAndContainsAPrimaryOrSecondary(InventoryContainer otherWeapon)
        {
            // this is very hacky - but not sure a better way to do this
            // essentially each PRI/SEC weapon slot has a reference to the other, so we are relying on that to go back n forth
            if (otherWeapon._otherWeaponSlot.slotRequirement == Requires.WEAPON_PRIMARY &&
                otherWeapon._item._resourceData.thisSlotRequires == Requires.WEAPON_PRIMARY ||
                otherWeapon._otherWeaponSlot.slotRequirement == Requires.WEAPON_SECONDARY &&
                otherWeapon._item._resourceData.thisSlotRequires == Requires.WEAPON_SECONDARY)
            {
                return true;
            }

            return false;
        }

        public static bool SlotHasWeaponRequirement(Requires slotRequirement)
        {
            var isWeaponRequirement = slotRequirement is 
                Requires.WEAPON_1H or 
                Requires.WEAPON_2H or 
                Requires.WEAPON_PRIMARY or 
                Requires.WEAPON_SECONDARY;
            
            return isWeaponRequirement;
        }
         public static bool OldCanSwapWeapon(InventoryData drag_resource, InventoryContainer otherWeapon, Requires slotRequirement)
        {

            // Check if it's a 1H weapon
            if (drag_resource.thisSlotRequires == Requires.WEAPON_1H)
            {
                return slotRequirement == Requires.WEAPON_1H ||
                       slotRequirement == Requires.WEAPON_PRIMARY ||
                       slotRequirement == Requires.WEAPON_SECONDARY ||
                       slotRequirement == Requires.NONE;
            }

            // Check if it's a two-handed weapon and the other slot is empty
            if (drag_resource.thisSlotRequires == Requires.WEAPON_2H)
            {
                //_otherWeaponSlot
                // For 2H weapons, we need to check if this is a weapon slot and if the other hand slot is empty
                if (slotRequirement == Requires.WEAPON_PRIMARY || slotRequirement == Requires.WEAPON_SECONDARY)
                {
                    // TODO: Need to check if the other weapon slot is empty
                    // This would require access to the other weapon slot container
                    // For now, allowing 2H weapons on primary/secondary slots
                    return true;
                }

                return slotRequirement == Requires.WEAPON_2H;
            }

            // Check if it's a primary or secondary slot requirement and you are on the proper slot
            if (drag_resource.thisSlotRequires == Requires.WEAPON_PRIMARY)
            {
                return slotRequirement == Requires.WEAPON_PRIMARY;
            }

            if (drag_resource.thisSlotRequires == Requires.WEAPON_SECONDARY)
            {
                return slotRequirement == Requires.WEAPON_SECONDARY;
            }

            // For non-weapon items, don't allow them on weapon slots
            if (slotRequirement == Requires.WEAPON_1H ||
                slotRequirement == Requires.WEAPON_2H ||
                slotRequirement == Requires.WEAPON_PRIMARY ||
                slotRequirement == Requires.WEAPON_SECONDARY)
            {
                return false;
            }

            // Default case - allow if slot requirements match or no requirement
            return drag_resource.thisSlotRequires == slotRequirement;
        }
    }
}