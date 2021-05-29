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
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.GiantBat];
        }

        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 32;
            NPC.damage = 13;
            NPC.defense = 2;
            NPC.lifeMax = 16;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.8f;
            NPC.aiStyle = 14;
            NPC.noGravity = true;
            AIType = NPCID.GiantBat;
            AnimationType = NPCID.GiantBat;
            Main.npcCatchable[Mod.Find<ModNPC>("AnimatedTome").Type] = true;
            NPC.catchItem = (short)Mod.Find<ModItem>("AnimatedTomeItem").Type;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.DungeonNormal.Chance * 0.01f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Book);
            if (Main.rand.NextBool(50)) Item.NewItem(NPC.getRect(), Mod.Find<ModItem>("OrigamiManual").Type);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            int i = NPC.whoAmI % 5; //needs to be fixed per NPC instance
            if (i != 4)
            {
                Texture2D texture = Mod.GetTexture("NPCs/AnimatedTome_" + i).Value;
                Vector2 stupidOffset = new Vector2(0f, 4f);
                SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Width() * 0.5f, NPC.height * 0.5f);
                Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
                spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            }

            if (Main.rand.NextFloat() < 0.1f)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 15);
            }
        }
    }
}
