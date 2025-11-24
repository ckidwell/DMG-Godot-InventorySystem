using Godot;
using System;

namespace DMGInventorySystem
{
    [GlobalClass]
    [Serializable]
    public partial class ArmorData : InventoryData
    {
        [Export] public int armorAmount;
        [Export] public ArmorTypes armorType;
    }
}