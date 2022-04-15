using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimePink : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Pink Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimePinkItem>();
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Overworld;
            }
        }

        public override Color DustColor => new Color(255, 155, 154, 100);

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Dimminutive does not mean lacking. Despite its size, this slime has more spunk than an entire party.")
            });
        }

        public override void SafeSetDefaults()
        {
            NPC.scale = 0.5f;
            DrawOffsetY = -2f;
        }
    }
}
