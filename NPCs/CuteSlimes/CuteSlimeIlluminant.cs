using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

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
                return mod.ItemType("CuteSlimeIlluminantNew");
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
            NPCID.Sets.TrailingMode[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 8;
        }

        public override void MoreSetDefaults()
        {
            npc.alpha = 80;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/CuteSlimes/CuteSlimeIlluminantAddition");
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2 + npc.gfxOffY + 4f);

            // The higher the k, the older the position
            // Length is implicitely set in TrailCacheLength up there
            for (int k = npc.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = npc.GetAlpha(Color.White) * ((npc.oldPos.Length - k) / (1f * npc.oldPos.Length)) * ((255 - npc.alpha) / 255f) * 0.5f;
                color.A = (byte)(npc.alpha * ((npc.oldPos.Length - k) / npc.oldPos.Length));
                spriteBatch.Draw(texture, drawPos, npc.frame, color, npc.oldRot[k], npc.frame.Size() / 2, npc.scale, effect, 0f);
            }
        }
    }
}
