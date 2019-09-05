using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeDungeon : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Dungeon Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeDungeonNew");
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Dungeon;
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
            npc.scale = 1.2f;
        }

        public override void MoreNPCLoot(Rectangle pos)
        {
            int type = Main.rand.NextBool(7) ? mod.ItemType<PetAccessorySwallowedKey>() : ItemID.GoldenKey;
            Item.NewItem(pos, type);
        }

        public override bool MorePreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/CuteSlimes/CuteSlimeDungeonAddition");
            Vector2 stupidOffset = new Vector2(0f, -3f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            drawColor.A = 255;
            spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);

            return true;
        }
    }
}
