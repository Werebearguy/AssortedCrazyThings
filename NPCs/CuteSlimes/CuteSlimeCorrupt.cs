using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeCorrupt : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Corrupt Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimeCorruptItem>();
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Corruption;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement("Hollow eyes and a dark color makes you think this slime is dangerous, but it's just not very social.")
            });
        }

        public override void SafeSetDefaults()
        {
            NPC.scale = 1.2f;
        }
    }
}
