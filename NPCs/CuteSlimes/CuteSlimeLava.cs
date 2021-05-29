using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

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
                return Mod.Find<ModItem>("CuteSlimeLavaNew").Type;
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
            NPC.lavaImmune = true;
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
            Dust.NewDustDirect(NPC.position + new Vector2(widthOffset, -20), NPC.width - 2 * widthOffset, NPC.height + 20, 6).noGravity = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/CuteSlimes/CuteSlimeLavaAddition").Value;
            Vector2 stupidOffset = new Vector2(0f, -9f * NPC.scale + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
            drawColor.A = 255;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
        }
    }
}
