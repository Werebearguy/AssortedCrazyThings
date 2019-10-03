using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeLava : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Lava Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeLavaNew");
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Hell;
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

        public override bool ShouldDropGel
        {
            get
            {
                return false;
            }
        }

        public override void MoreSetDefaults()
        {
            npc.lavaImmune = true;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            drawColor = Color.White * 0.78f;
            drawColor.A = 75;
            return drawColor;
        }

        public override void DrawEffects(ref Color drawColor)
        {
            int widthOffset = 12;
            Dust.NewDustDirect(npc.position + new Vector2(widthOffset, 0), npc.width - 2 * widthOffset, npc.height, 6).noGravity = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/CuteSlimes/CuteSlimeLavaAddition");
            Vector2 stupidOffset = new Vector2(0f, -9f * npc.scale + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            drawColor.A = 255;
            spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
        }
    }
}
