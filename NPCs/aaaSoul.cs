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
            npc.aiStyle = -1;
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

        protected int HarvesterTargetClosest()
        {
            int closest = 200;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].type == mod.NPCType(AssWorld.harvesterName))
                {
                    closest = j;
                    break;
                }
            }
            return closest;
        }

        protected Entity GetTarget()
        {
            int res = HarvesterTargetClosest();
            if (res != 200) return Main.npc[res];
            else return npc;
        }

        protected bool IsTargetActive()
        {
            return GetTarget().active;
        }

        public override void AI()
        {
            if (npc.ai[0] == 0)
            {
                npc.noGravity = true;
                if (Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    if (!GetTarget().Equals(npc))
                    {
                        npc.noTileCollide = true;
                        Vector2 between = GetTarget().Center - npc.Center;
                        float distance = between.Length();
                        float factor = 2f; //2f
                        int acc = 100; //4
                        between.Normalize();
                        between *= factor;
                        npc.velocity = (npc.velocity * (acc - 1) + between) / acc;
                    }
                }
                else
                {
                    npc.noTileCollide = false;
                    npc.velocity *= 0.1f;
                }
            }

            Entity tar = GetTarget();
            NPC tarnpc = new NPC();
            if (tar is NPC)
            {
                tarnpc = (NPC)tar;
            }

            if (!tarnpc.Equals(npc))
            {
                if (npc.getRect().Intersects(tarnpc.getRect()) && npc.ai[0] == 0)
                {
                    npc.ai[0] = 1;
                    npc.noGravity = false;
                    npc.velocity.Y = 0.3f;
                }
            }

            if(npc.ai[0] == 1 && npc.velocity.Y != 0)
            {
                npc.velocity.Y += 0.04f;
            }

            Main.NewText(npc.velocity.Y);

            npc.timeLeft--;
            if (npc.timeLeft < 0)
            {
                npc.life = 0;
                npc.active = false;
            }
        }
    }
}
