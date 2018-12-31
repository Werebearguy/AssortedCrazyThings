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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SeaSnail];
        }

        public override void SetDefaults()
        {
            npc.width = 112;
            npc.height = 67;
            npc.damage = 40;
            npc.defense = 20;
            npc.lifeMax = 900;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 640f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 3;
            aiType = NPCID.SeaSnail;
            animationType = NPCID.SeaSnail;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Slow, 240, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.downedBoss1 ? SpawnCondition.Ocean.Chance * 0.005f : 0f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.PurpleMucos);
            if (Main.rand.NextBool(20))
            {
                Item.NewItem(npc.getRect(), ItemID.Flipper);
            }
            if (Main.rand.NextBool(50))
            {
                Item.NewItem(npc.getRect(), ItemID.DivingHelmet);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //slightly different approach than in AnimatedTome
            index = npc.whoAmI % 3;
            if (index != 0)
            {
                Texture2D texture = mod.GetTexture("NPCs/Juggerllusc_" + index);
                Vector2 stupidOffset = new Vector2(0f, 6f);
                SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
                Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
                spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
                return false;
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_megasnail_" + index), 1f);
            }
        }
    }
}
