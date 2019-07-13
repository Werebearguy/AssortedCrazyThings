using System;
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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DemonEye];
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 46;
            npc.friendly = true;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            //aiType = NPCID.DemonEye;
            animationType = NPCID.DemonEye;
            Main.npcCatchable[mod.NPCType("MeatballsEye")] = true;
            npc.catchItem = (short)mod.ItemType("MeatballsEye");
        }

        public override void AI()
        {
            if (npc.ai[0] == 0)
            {
                npc.velocity.Y = -0.022f * 2f;
                npc.netUpdate = true;
            }

            npc.rotation = (float)Math.PI / 2;
            npc.direction = 1;
            npc.velocity.X = 0;
            npc.ai[0]++;
            npc.velocity.Y -= 0.022f * 1.5f; //0.022f * 2f;
            if (npc.timeLeft > 80)
            {
                npc.timeLeft = 80;
            }
        }
    }
}
