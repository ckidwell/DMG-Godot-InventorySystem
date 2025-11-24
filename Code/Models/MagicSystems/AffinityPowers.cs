using System.Collections;

namespace DMGInventorySystem
{
    public class AffinityPowers
    {

        // these are stat adjusted and recalculated as needed
        public int fireAffinity;
        public int earthAffinity;
        public int waterAffinity;
        public int airAffinity;
        public int lightAffinity;
        public int darkAffinity;

        // these are the base powers - only adjusted at creation/levelup
        public int base_fireAffinity;
        public int base_earthAffinity;
        public int base_waterAffinity;
        public int base_airAffinity;
        public int base_lightAffinity;
        public int base_darkAffinity;

        public void initAllPowersAtLevel(int powerLevel)
        {
            this.fireAffinity = powerLevel * 10;
            this.earthAffinity = powerLevel * 10;
            this.waterAffinity = powerLevel * 10;
            this.airAffinity = powerLevel * 10;
            this.lightAffinity = powerLevel * 10;
            this.darkAffinity = powerLevel * 10;

            this.base_fireAffinity = powerLevel * 10;
            this.base_earthAffinity = powerLevel * 10;
            this.base_waterAffinity = powerLevel * 10;
            this.base_airAffinity = powerLevel * 10;
            this.base_lightAffinity = powerLevel * 10;
            this.base_darkAffinity = powerLevel * 10;
        }

    }
}