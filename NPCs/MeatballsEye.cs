using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class MeatballsEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meatball's Eye");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DemonEye];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 46;
            NPC.friendly = true;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            //AIType = NPCID.DemonEye;
            AnimationType = NPCID.DemonEye;
            NPC.catchItem = (short)ModContent.ItemType<Items.MeatballsEye>();
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                NPC.velocity.Y = -0.022f * 2f;
                NPC.netUpdate = true;
            }

            NPC.rotation = MathHelper.PiOver2;
            NPC.direction = 1;
            NPC.velocity.X = 0;
            NPC.ai[0]++;
            NPC.velocity.Y -= 0.022f * 1.5f; //0.022f * 2f;
            if (NPC.timeLeft > 80)
            {
                NPC.timeLeft = 80;
            }
        }
    }
}
