using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class aaaSoul : ModNPC
    {
        public static string name = "aaaSoul";

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/CuteSlimeBlack"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 52;
            npc.scale = 0.9f;
            npc.dontTakeDamageFromHostiles = true; //both to be unhittable
            npc.friendly = true;
            npc.noGravity = true;
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.9f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            npc.alpha = 125;
            npc.color = new Color(0, 0, 0, 50);
            Main.npcCatchable[mod.NPCType(name)] = true;
            npc.catchItem = (short)ItemID.SandBlock;
            npc.timeLeft = NPC.activeTime * 5;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.025f * 0.5f;
        }

        public override void NPCLoot()
        {
            
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
        }

        private void KillInstantly(NPC npc)
        {
            // These 3 lines instantly kill the npc without showing damage numbers, dropping loot, or playing DeathSound. Use this for instant deaths
            npc.life = 0;
            //npc.HitEffect();
            npc.active = false;
            //Main.PlaySound(SoundID.NPCDeath16, npc.position); // plays a fizzle sound
        }

        public override bool CheckActive()
        {
            //manually decrease timeleft
            return false;
        }

        public override bool PreAI()
        {
            npc.timeLeft--;
            if (npc.timeLeft < 0)
            {
                npc.life = 0;
                npc.active = false;
            }
            return false;
        }
    }
}
