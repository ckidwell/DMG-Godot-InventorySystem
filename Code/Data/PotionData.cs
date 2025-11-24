using Godot;

namespace DMGInventorySystem
{
    [GlobalClass]
    [System.Serializable]
    public partial class PotionData : InventoryData
    {
        [Export] PotionTypes potionType;
        [Export] private int amount;

    }
}