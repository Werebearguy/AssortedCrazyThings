using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeGreen : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Green Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimeGreenItem>();
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Overworld;
            }
        }

        public override Color DustColor => new Color(27, 246, 66, 100);

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Don't let the closed eyes fool you; this slime is attentive and ready to act at a moment's notice.")
            });
        }

        public override void SafeSetDefaults()
        {
            NPC.scale = 0.9f;
        }
    }
}
