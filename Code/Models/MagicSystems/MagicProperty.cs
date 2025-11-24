using System;
using Godot;

namespace DMGInventorySystem
{
    [Serializable]
    public partial class MagicProperty : Node
    {
        
        private RandomNumberManager _randomNumberManager;

        public override void _Ready()
        {
            base._Ready();
            _randomNumberManager = GetNode<RandomNumberManager>("/root/RandomNumberManager");
        }

        //these are used for the randomizers below always adjust this if you add/remove suffix/prefixes
        private static int NUMSUFFIX = 11;
        private static int NUMPREFIX = 4;


        public enum MagicSuffix
        {
            NONE,
            STAMINA,
            STRENGTH,
            DEXTERITY,
            INTELLIGENCE,
            SPIRIT,
            FIRE,
            EARTH,
            WATER,
            AIR,
            POSITIVE,
            NEGATIVE

        }


        public enum MagicPrefix
        {
            NONE,
            VAMPIRIC,
            HEALING,
            DAMAGEBOOST

        }

        public MagicPrefix magicPrefix;
        public MagicSuffix magicSuffix;

        public int prefixpower;
        public int suffixpower;


        public MagicProperty(int level, int difficultyLevel, bool eliteMonster, bool boss)
        {
            //TODO make this a better random algorithm with scaling of power properly
            magicPrefix = getRandomPrefix();
            magicSuffix = getRandomSuffix();
            prefixpower = level;
            suffixpower = level;

        }

        private MagicSuffix getRandomSuffix()
        {
            var suffix = new MagicSuffix();

            var result = _randomNumberManager.GetRandomNumber(0, NUMSUFFIX + 1);

            suffix = result switch
            {
                0 => MagicSuffix.NONE,
                1 => MagicSuffix.STAMINA,
                2 => MagicSuffix.STRENGTH,
                3 => MagicSuffix.DEXTERITY,
                4 => MagicSuffix.INTELLIGENCE,
                5 => MagicSuffix.SPIRIT,
                6 => MagicSuffix.FIRE,
                7 => MagicSuffix.EARTH,
                8 => MagicSuffix.WATER,
                9 => MagicSuffix.AIR,
                10 => MagicSuffix.POSITIVE,
                11 => MagicSuffix.NEGATIVE,
                _ => suffix
            };

            return suffix;
        }

        private MagicPrefix getRandomPrefix()
        {
            var prefix = new MagicPrefix();

            var result = _randomNumberManager.GetRandomNumber(0, NUMPREFIX + 1);

            prefix = result switch
            {
                0 => MagicPrefix.NONE,
                1 => MagicPrefix.VAMPIRIC,
                2 => MagicPrefix.HEALING,
                3 => MagicPrefix.DAMAGEBOOST,
                _ => prefix
            };

            return prefix;
        }

        public string getPrefixName()
        {
            var prefixName = magicPrefix switch
            {
                MagicPrefix.NONE => "",
                MagicPrefix.VAMPIRIC => "Leeching ",
                MagicPrefix.HEALING => "Restoring ",
                MagicPrefix.DAMAGEBOOST => "Deadly ",
                _ => ""
            };

            return prefixName;
        }

        public string getSuffixName()
        {
            var suffixName = magicSuffix switch
            {
                MagicSuffix.NONE => "",
                MagicSuffix.STAMINA => " of the Bear",
                MagicSuffix.STRENGTH => " of the Giant",
                MagicSuffix.DEXTERITY => " of the Fox",
                MagicSuffix.SPIRIT => " of the Owl",
                MagicSuffix.FIRE => " of the Inferno",
                MagicSuffix.EARTH => " of Stone",
                MagicSuffix.WATER => " of Life",
                MagicSuffix.AIR => " of the Winds",
                MagicSuffix.POSITIVE => " of Light",
                MagicSuffix.NEGATIVE => " of Darkness",
                _ => ""
            };

            return suffixName;
        }
    }
}