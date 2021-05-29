using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.PetAccessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
                return Mod.Find<ModItem>("CuteSlimeDungeonNew").Type;
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
            NPC.scale = 1.2f;
        }

        public override void MoreNPCLoot(Rectangle pos)
        {
            int type = Main.rand.NextBool(7) ? ModContent.ItemType<PetAccessorySwallowedKey>() : ItemID.GoldenKey;
            Item.NewItem(pos, type);
        }

        public override bool MorePreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/CuteSlimes/CuteSlimeDungeonAddition").Value;
            Vector2 stupidOffset = new Vector2(0f, -12f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
            drawColor.A = 255;
            Main.spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);

            return true;
        }
    }
}
