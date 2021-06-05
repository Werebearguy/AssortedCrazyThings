using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeYellow : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Yellow Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimeYellowItem>();
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Overworld;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Moving with delicate grace, this slime dances about without missing a step.")
            });
        }
    }
}
