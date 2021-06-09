using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeJungle : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Jungle Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimeJungleItem>();
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Jungle;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement("Life in the jungle is hard for soft-bodied visitors, but this slime proves it's not impossible.")
            });
        }
    }
}
