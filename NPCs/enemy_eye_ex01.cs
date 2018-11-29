using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
    public class enemy_eye_ex01 : ModNPC
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
            //aiType = NPCID.DemonEye;
            animationType = NPCID.DemonEye;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_dilated"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_purple"), 1f);
            }
        }

        public override void AI()
        {
            Main.NewText("test");
            if (npc.ai[0] == 0)
            {
                Main.NewText("test2");
                npc.rotation = 90f;
                npc.direction = 0;
                npc.velocity.X = 0;
                npc.velocity.Y = -0.022f;
            }
            npc.ai[0]++;
            npc.velocity.Y -= 0.022f * 2f;
            if (npc.timeLeft > 10)
            {
                npc.timeLeft = 10;
            }
        }
    }
}
