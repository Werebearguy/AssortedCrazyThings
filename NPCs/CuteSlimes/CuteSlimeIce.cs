using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeIce : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Ice Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimeIceItem>();
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Tundra;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                new FlavorTextBestiaryInfoElement("Its body may be cold to the touch, but its expression and mannerisms will warm your heart.")
            });
        }

        public override void SafeSetDefaults()
        {
            NPC.scale = 0.9f;
        }
    }
}
