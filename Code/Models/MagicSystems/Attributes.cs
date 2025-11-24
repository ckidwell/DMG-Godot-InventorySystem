using System;
using System.Linq;
using Godot;


namespace DMGInventorySystem
{
    [Serializable]
    public partial class Attributes : Node
    {
        private RandomNumberManager _randomNumberManager;
        [ExportGroup("Attribute Names and Max Ranges")]
        [Export] private string attribute_1_name = "Intelligence";
        [Export(PropertyHint.Range, "1,20,1")] private int attribute_1_max = 1;
        
        [Export] private string attribute_2_name = "Strength";
        [Export(PropertyHint.Range, "1,20,1")] private int attribute_2_max = 1;
        
        [Export] private string attribute_3_name = "Stamina";
        [Export(PropertyHint.Range, "1,20,1")] private int attribute_3_max = 1;
        
        [ExportGroup("Max Attribute Per Item")]
        [Export(PropertyHint.Range, "1,3,1")] private int MAX_NUM_ATTRIBUTES = 1;

        [ExportGroup("Attribute Values Max Ranges")]
        [Export] private int attribute_1_max_value = 3;
        [Export] private int attribute_2_max_value = 5;
        [Export] private int attribute_3_max_value = 8;
        
        
        public int attributeValue;
        public AttributeTypes attribute;
        
        public override void _Ready()
        {
            base._Ready();
            _randomNumberManager = GetNode<RandomNumberManager>("/root/RandomNumberManager");
        }

        public enum AttributeTypes
        {
            ATTRIBUTE_1,
            ATTRIBUTE_2,
            ATTRIBUTE_3,
        }

       

        public string GetAttributeName(AttributeTypes attrib)
        {
            return attrib switch
            {
                AttributeTypes.ATTRIBUTE_1 => attribute_1_name,
                AttributeTypes.ATTRIBUTE_2 => attribute_2_name,
                AttributeTypes.ATTRIBUTE_3 => attribute_3_name,
                _ => ""
            };
        }
        public int GetMaxAttributeValueForQualityLevel(ItemQuality quality)
        {
            return quality switch
            {
                ItemQuality.COMMON => 0,
                ItemQuality.RARE => attribute_1_max_value,
                ItemQuality.SUPERIOR => 4,
                ItemQuality.ELITE => 8,
                _ => 0
            };
        }
        public static int GetNumAttributesForQualityLevel(ItemQuality quality)
        {
            return quality switch
            {
                ItemQuality.COMMON => 0,
                ItemQuality.RARE => 1,
                ItemQuality.SUPERIOR => 2,
                ItemQuality.ELITE => 3,
                _ => 0
            };
        }
        
        public Attributes[] GetNewRandomAttributes(int numberOfAttributes, ItemQuality quality = ItemQuality.COMMON)
        {
            if(numberOfAttributes > MAX_NUM_ATTRIBUTES)
                numberOfAttributes = MAX_NUM_ATTRIBUTES;

            if (numberOfAttributes < 1)
                return null;
            
            var newAttributesTypes = new AttributeTypes[numberOfAttributes];
            var count = 0;
            
            while (count < numberOfAttributes -1)
            {
                var randomAttribute = GetRandomAttribute();

                if (newAttributesTypes.Contains(randomAttribute)) continue;
                
                newAttributesTypes[count] = randomAttribute;
                count++;
            }
            var newAttributes = new Attributes[numberOfAttributes];

            for (var i = 0; i < numberOfAttributes; i++)
            {
                newAttributes[i] = new Attributes()
                {
                    attributeValue = _randomNumberManager.GetRandomNumber(1, GetMaxAttributeValueForQualityLevel(quality)),
                    attribute = newAttributesTypes[i]
                };
            }

            return newAttributes;
        }

       

        public AttributeTypes GetRandomAttribute()
        {
            var values = Enum.GetValues(typeof(AttributeTypes));

            return (AttributeTypes) values.GetValue(_randomNumberManager.GetRandomNumber(0, values.Length -1));
        }
    }
}