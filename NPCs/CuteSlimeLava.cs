using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class CuteSlimeLava : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 52;
            //npc.friendly = true;
            npc.chaseable = false;
            npc.damage = 0;
            npc.defense = 2;
            npc.lifeMax = 20;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            //npc.alpha = 75;
            Main.npcCatchable[mod.NPCType("CuteSlimeLava")] = true;
            npc.catchItem = (short)mod.ItemType("CuteSlimeLavaNew");
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
            Main.dust[Dust.NewDust(npc.position + new Vector2(widthOffset, 0), npc.width - 2 * widthOffset, npc.height, 6)].noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SlimePets.CuteSlimeSpawnChance(spawnInfo, SlimePets.SpawnConditionType.Hell);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/CuteSlimeLavaAddition");
            Vector2 stupidOffset = new Vector2(0f, 4f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            drawColor.A = 255;
            spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
        }
    }
}
