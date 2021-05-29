using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Juggerllusc : ModNPC
    {
        private int index = 0;
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/Juggerllusc_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Juggerllusc");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SeaSnail];
        }

        public override void SetDefaults()
        {
            NPC.width = 112;
            NPC.height = 67;
            NPC.damage = 40;
            NPC.defense = 20;
            NPC.lifeMax = 900;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 640f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 3;
            AIType = NPCID.SeaSnail;
            AnimationType = NPCID.SeaSnail;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Slow, 240, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.downedBoss1 ? SpawnCondition.Ocean.Chance * 0.005f : 0f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.PurpleMucos);
            if (Main.rand.NextBool(20))
            {
                Item.NewItem(NPC.getRect(), ItemID.Flipper, prefixGiven: -1);
            }
            if (Main.rand.NextBool(50))
            {
                Item.NewItem(NPC.getRect(), ItemID.DivingHelmet, prefixGiven: -1);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //slightly different approach than in AnimatedTome
            index = NPC.whoAmI % 3;
            if (index != 0)
            {
                Texture2D texture = Mod.GetTexture("NPCs/Juggerllusc_" + index).Value;
                Vector2 stupidOffset = new Vector2(0f, 6f);
                SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
                Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
                spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
                return false;
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/JuggerlluscGore_" + index).Type, 1f);
            }
        }
    }
}
