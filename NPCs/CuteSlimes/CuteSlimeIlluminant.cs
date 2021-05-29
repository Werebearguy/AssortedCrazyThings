using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeIlluminant : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Illuminant Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeIlluminantNew").Type;
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Hallow;
            }
        }

        public override bool IsFriendly
        {
            get
            {
                return false;
            }
        }

        public override bool ShouldDropRandomItem
        {
            get
            {
                return false;
            }
        }

        public override void MoreSetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
        }

        public override void MoreSetDefaults()
        {
            DrawOffsetY = 1f;
            NPC.alpha = 80;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/CuteSlimes/CuteSlimeIlluminantAddition").Value;
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2 + NPC.gfxOffY - 5f);

            // The higher the k, the older the position
            // Length is implicitely set in TrailCacheLength up there
            for (int k = NPC.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = NPC.GetAlpha(Color.White) * ((NPC.oldPos.Length - k) / (1f * NPC.oldPos.Length)) * ((255 - NPC.alpha) / 255f) * 0.5f;
                color.A = (byte)(NPC.alpha * ((NPC.oldPos.Length - k) / NPC.oldPos.Length));
                spriteBatch.Draw(texture, drawPos, NPC.frame, color, NPC.oldRot[k], NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            }
        }
    }
}
