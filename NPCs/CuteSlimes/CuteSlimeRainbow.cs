using Terraria.GameContent.Bestiary;
using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeRainbow : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Rainbow Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return ModContent.ItemType<CuteSlimeRainbowItem>();
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
                new FlavorTextBestiaryInfoElement("Text here.")
            });
        }

        public override void MoreSetDefaults()
        {
            NPC.scale = 1.2f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Main.DiscoColor;
            lightColor *= (255f - NPC.alpha) / 255f;
            return lightColor;
        }
    }
}
