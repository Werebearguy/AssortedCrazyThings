using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeCrimson : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Crimson Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimeCrimsonItem>();
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Crimson;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement("Though it looks like it has an unstable mind, this slime is incredibly calm and collected.")
            });
        }

        public override void SafeSetDefaults()
        {
            NPC.scale = 1.2f;
        }
    }
}
