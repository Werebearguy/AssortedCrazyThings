using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
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
            npc.color = new Color(0, 0, 0, 50);
            Main.npcCatchable[mod.NPCType(name)] = true;
            npc.catchItem = (short)ItemID.SandBlock;
            npc.timeLeft = NPC.activeTime * 5;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.DiscoColor;
        }

        private void KillInstantly()
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

        protected int HarvesterTarget()
        {
            int tar = 200;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) != -1)
                {
                    tar = j;
                    break;
                }
            }
            return tar;
        }

        protected Entity GetTarget()
        {
            int res = HarvesterTarget();
            if (res != 200) return Main.npc[res];
            else return npc;
        }

        protected bool IsTargetActive()
        {
            return GetTarget().active;
        }

        public override void AI()
        {
            npc.noTileCollide = false;
            if (npc.ai[0] == 0)
            {
                npc.noGravity = true;
                //npc.position - new Vector2(10f, 4f), npc.width + 20, npc.height + 4

                //concider only the bottom half of the hitbox, a bit wider (minus a small bit below)
                if (Collision.SolidCollision(npc.position + new Vector2(-10f, npc.height / 2), npc.width + 20, npc.height / 2 -2))
                {
                    if (!GetTarget().Equals(npc))
                    {
                        npc.noTileCollide = true;
                        Vector2 between = GetTarget().Center - npc.Center;
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
                if ((npc.getRect().Intersects(tarnpc.getRect()) && npc.ai[0] == 0)/* && tarnpc.velocity.Y <= 0*/) // tarnpc.velocity.Y <= 0 for only when it jumps
                {
                    npc.ai[0] = 1;
                    npc.velocity.Y = 1f;
                }
                else if(!npc.getRect().Intersects(tarnpc.getRect()) && npc.ai[0] == 1)
                {
                    npc.velocity.X *= 0.1f;
                }
            }

            if(npc.ai[0] == 1 && npc.velocity.Y != 0)
            {
                if (Math.Abs(tarnpc.Center.X - npc.Center.X) > 3f)
                {
                    Vector2 between = new Vector2(tarnpc.Center.X - npc.Center.X, 0);
                    float factor = 0.6f; //2f
                    int acc = 6; //4
                    between.Normalize();
                    between *= factor;
                    npc.velocity.X = (npc.velocity.X * (acc - 1) + between.X) / acc;
                    //npc.noGravity = false;
                    npc.noTileCollide = false;
                }
                else
                {
                    npc.velocity.X = 0;
                }
                npc.velocity.Y += 0.06f;
            }

            if (npc.ai[0] == 1 && !npc.getRect().Intersects(tarnpc.getRect()) && (npc.velocity.Y == 0 || (npc.velocity.Y < 2f && npc.velocity.Y > 0f)))
            {
                //slowdown when outside of collision, but after being knocked
                npc.velocity.X *= 0.1f;
            }

            if (npc.timeLeft <= aaaHarvester2.EatTimeConst)
            {
                npc.velocity.X = 0;
            }

            if(npc.ai[0] == 1)--npc.timeLeft;
            if (npc.timeLeft < 0)
            {
                KillInstantly();
            }
        }
    }
}
