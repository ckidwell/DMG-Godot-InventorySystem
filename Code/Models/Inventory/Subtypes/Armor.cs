
namespace DMGInventorySystem
{
    [System.Serializable]
    public partial class Armor : InventoryItem
    {
        
        public int armor;
        private ArmorTypes armorType;
        public ArmorTypes ArmorType { get; set; }
        
        public string GetSlotName(Requires _slot, ArmorTypes _type)
        {
            var slotName = "";

            switch (_slot)
            {
                case Requires.ARM:
                {
                    if (_type == ArmorTypes.LIGHT)
                    {
                        slotName = "Sleeves";
                    }
                    else if (_type == ArmorTypes.MEDIUM)
                    {
                        slotName = "Bracers";
                    }
                    else
                    {
                        slotName = "Vambrace";
                    }

                    break;
                }
                case Requires.CHEST:
                {
                    if (_type == ArmorTypes.LIGHT)
                    {
                        slotName = "Garment";
                    }
                    else if (_type == ArmorTypes.MEDIUM)
                    {
                        slotName = "Hauberk";
                    }
                    else
                    {
                        slotName = "Cuirass";
                    }

                    break;
                }
                case Requires.EAR:
                {
                    slotName = "Earring";
                    break;
                }
                case Requires.FEET:
                {
                    if (_type == ArmorTypes.LIGHT || _type == ArmorTypes.MEDIUM)
                    {
                        slotName = "Boots";
                    }
                    else
                    {
                        slotName = "Sabaton";
                    }

                    break;
                }
                case Requires.HANDS:
                {
                    if (_type == ArmorTypes.LIGHT || _type == ArmorTypes.MEDIUM)
                    {
                        slotName = "Gloves";
                    }
                    else
                    {
                        slotName = "Gauntlets";
                    }

                    break;
                }
                case Requires.HEAD:
                {
                    if (_type == ArmorTypes.LIGHT)
                    {
                        slotName = "Hood";
                    }
                    else if (_type == ArmorTypes.MEDIUM)
                    {
                        slotName = "Helmet";
                    }
                    else
                    {
                        slotName = "Barbute";
                    }

                    break;
                }
                case Requires.LEGS:
                {
                    if (_type == ArmorTypes.LIGHT || _type == ArmorTypes.MEDIUM)
                    {
                        slotName = "Pants";
                    }
                    else
                    {
                        slotName = "Greaves";
                    }

                    break;
                }
                case Requires.NECK:
                {
                    slotName = "Necklace";
                    break;
                }
                case Requires.RING:
                {
                    slotName = "Ring";
                    break;
                }
                default:
                {
                    slotName = "Undefined";
                    break;
                }
            }
            return slotName;
        }
    }
}