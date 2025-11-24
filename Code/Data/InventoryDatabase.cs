using Godot;

namespace DMGInventorySystem
{
    //[CreateAssetMenu(fileName = "InventoryDatabase", menuName = "CanHazInventory/InventoryDatabase", order = 1)]
    public partial class InventoryDatabase : Resource
    {
        public InventoryData[] inventoryItems;
    }
}
