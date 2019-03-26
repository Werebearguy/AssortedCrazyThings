using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    //this version of "retexture" is old and not recommended, refer to DemonEyeFractured or similar

    public class AnimatedTome : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Tome");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GiantBat];
        }

        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 32;
            npc.damage = 13;
            npc.defense = 2;
            npc.lifeMax = 16;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = 0.8f;
            npc.aiStyle = 14;
            npc.noGravity = true;
            aiType = NPCID.GiantBat;
            animationType = NPCID.GiantBat;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Dungeon.Chance * 0.01f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Book);
            if (Main.rand.NextBool(50)) Item.NewItem(npc.getRect(), mod.ItemType("OrigamiManual")); 
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            int i = npc.whoAmI % 5; //needs to be fixed per NPC instance
            if (i != 4)
            {
                Texture2D texture = mod.GetTexture("NPCs/AnimatedTome_" + i);
                float stupidOffset = 4f;
                SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
                Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + new Vector2(0f, stupidOffset);
                spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            }

            if (Main.rand.NextFloat() < 0.1f)
            {
                Dust dust = Main.dust[Dust.NewDust(npc.Center, 30, 30, 15, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
            }
        }
    }
}
