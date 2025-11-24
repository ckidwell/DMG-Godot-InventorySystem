using Godot;
using System;

namespace DMGInventorySystem
{
    [GlobalClass]
    [Serializable]
    public partial class WeaponData : InventoryData
    {
        [Export] public WeaponTypes weaponType;
        [Export] public int minDamage;
        [Export] public int maxDamage;
        [Export] public bool isRanged;
        [Export] public int ActionPointCost;
    }
}
