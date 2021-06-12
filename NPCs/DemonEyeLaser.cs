using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DemonEyeLaser : DemonEyeRecolorBase
    {
        public override int TotalNumberOfThese => 3;

        /*LG = 0
        * LP = 1
        * LR = 2
        */
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeLaser_0"; //use fixed texture
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !NPC.downedMechBoss2 ? 0f : SpawnCondition.OverworldNightMonster.Chance * 0.025f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                switch ((int)AiTexture)
                {
                    case 0:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 1:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    case 2:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeRedGore_0").Type, 1f);
                        break;
                    default:
                        break;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/DemonEyeLaser_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(-4f, 0f); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/DemonEyeLaser_Glowmask").Value;

            Vector2 stupidOffset = new Vector2(-4f, 0f);
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
        }

        public Vector2 RotToNormal(float rotation)
        {
            return new Vector2((float)Math.Sin(rotation), (float)-Math.Cos(rotation));
        }

        public float AngleBetween(Vector2 v1, Vector2 v2)
        {
            double sin = v1.X * v2.Y - v2.X * v1.Y;
            double cos = v1.X * v2.X + v1.Y * v2.Y;

            return (float)Math.Atan2(sin, cos);
        }

        public ref float AiShootTimer => ref NPC.ai[0];

        public ref float AiShootCount => ref NPC.ai[1];

        public override void PostAI()
        {
            if (!NPC.HasValidTarget) return;

            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            Vector2 npcposition = NPC.Center;
            Player player = Main.player[NPC.target];
            Vector2 distance = new Vector2(player.position.X + player.width / 2 - npcposition.X,
                                           player.position.Y + player.height / 2 - npcposition.Y);
            float rot = NPC.rotation;
            if (NPC.spriteDirection == 1)
            {
                rot += MathHelper.TwoPi / 2;
            }

            //Vector fuckery
            bool canShoot = Math.Abs(AngleBetween(RotToNormal(rot - MathHelper.TwoPi / 4), distance / distance.Length())) < 0.3f;
            float shootDelay = 180f;

            //Main.NewText("rotation: " + (rot - MathHelper.TwoPi/4)); //(npc.rotation + MathHelper.TwoPi/4)
            //Main.NewText("distance: " + AngleBetween(RotToNormal(rot - MathHelper.TwoPi/4), distance / distance.Length()));
            //Main.NewText("rotation vector: " + RotToNormal(rot - MathHelper.TwoPi / 4));
            //Main.NewText("distance vector: " + distance/distance.Length());
            if (canShoot)
            {
                AiShootTimer++;
                if (AiShootCount < 2f)
                {
                    shootDelay = 30f;
                }
                else if (AiShootCount == 2f)
                {
                    shootDelay = 180f;
                }
                else if (AiShootCount > 2f)
                {
                    AiShootTimer = 0f;
                    AiShootCount = 0f;
                }
            }
            else
            {
                AiShootTimer = 0f;
                AiShootCount = 0f;
            }
            if (canShoot && (AiShootTimer > shootDelay) && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
            {
                AiShootCount++;
                AiShootTimer = 0f;
                float distancex = distance.X;
                float distancey = distance.Y;
                float num427 = 8.5f;
                int damage = 8;
                int type = ProjectileID.PinkLaser; //the gastropod one
                if (Main.expertMode)
                {
                    num427 = 10f;
                    damage = 6;
                }
                float distancen = (float)Math.Sqrt((double)(distancex * distancex + distancey * distancey));
                distancen = num427 / distancen;
                distancex *= distancen;
                distancey *= distancen;
                npcposition.X += distancex * 5f;
                npcposition.Y += distancey * 5f;
                Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), npcposition.X, npcposition.Y, distancex, distancey, type, damage, 0f, Owner: Main.myPlayer);
            }
        }
    }
}
