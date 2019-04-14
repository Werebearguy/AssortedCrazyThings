using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //cannot be dyed since it counts as a minion and deals damage
    public class PetPlanteraProj : ModProjectile
    {
        public const int ContactDamage = 20;
        public const int ImmunityCooldown = 60;

        private const float STATE_IDLE = 0f;
        private const float STATE_ATTACK = 1f;

        public float AI_STATE
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mean Seed");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = true;
            projectile.minion = true; //only determines the damage type
            projectile.minionSlots = 0f;
            projectile.penetrate = -1;
            projectile.aiStyle = -1;

            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = ImmunityCooldown;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        private int FindTarget(Vector2 relativeCenter, float range = 300f, bool ignoreTiles = false) //finds target in range to relativeCenter
        {
            int targetIndex = -1;
            float distanceFromTarget = 100000f;
            Vector2 targetCenter = relativeCenter;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.active && npc.CanBeChasedBy(this))
                {
                    float between = Vector2.Distance(npc.Center, relativeCenter);
                    if (((between < range && Vector2.Distance(relativeCenter, targetCenter) > between && between < distanceFromTarget) || targetIndex == -1) &&
                        (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) || ignoreTiles))
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        targetIndex = k;
                    }
                }
            }
            return distanceFromTarget < range? targetIndex: -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetPlantera = false;
            }
            if (modPlayer.PetPlantera)
            {
                projectile.timeLeft = 2;
            }

            #region HandleState
            int targetIndex = FindTarget(player.Center); //check for player surrounding
            if (targetIndex == -1)
            {
                if (AI_STATE == STATE_ATTACK)
                {
                    targetIndex = FindTarget(player.Center, range: 400f, ignoreTiles: true); //check for player surrounding
                    if (targetIndex == -1) //check for proj surrounding
                    {
                        AI_STATE = STATE_IDLE;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    //keep idling
                }
            }
            else //target found
            {
                if (AI_STATE == STATE_IDLE)
                {
                    AI_STATE = STATE_ATTACK;
                    projectile.netUpdate = true;
                }
                else
                {
                    //keep attacking
                }
            }
            #endregion

            #region ActUponState
            if (AI_STATE == STATE_IDLE)
            {
                projectile.friendly = false;
                AssAI.BabyEaterAI(projectile);

                AssAI.BabyEaterDraw(projectile);
                projectile.rotation += 3.14159f;
            }
            else //STATE_ATTACK
            {
                projectile.friendly = true;

                if (targetIndex != -1)
                {
                    NPC npc = Main.npc[targetIndex];
                    Vector2 distanceToTargetVector = npc.Center - projectile.Center;
                    float distanceToTarget = distanceToTargetVector.Length();

                    if (distanceToTarget > 30f)
                    {
                        distanceToTargetVector.Normalize();
                        distanceToTargetVector *= 8f;
                        projectile.velocity = (projectile.velocity * (16f - 1) + distanceToTargetVector) / 16f;

                        projectile.rotation = (float)Math.Atan2(distanceToTargetVector.Y, distanceToTargetVector.X) + 1.57f;
                    }
                }

                AssAI.BabyEaterDraw(projectile, 4);
            }
            #endregion
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int tentacleCount = 0;

            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == mod.ProjectileType<PetPlanteraProjTentacle>() && projectile.owner == Main.projectile[i].owner)
                {
                    AssUtils.DrawTether(spriteBatch, "AssortedCrazyThings/Projectiles/Pets/PetPlanteraProj_Chain", Main.projectile[i].Center, projectile.Center);
                    tentacleCount++;
                }
                if (tentacleCount >= 4) break;
            }
            AssUtils.DrawTether(spriteBatch, "AssortedCrazyThings/Projectiles/Pets/PetPlanteraProj_Chain", Main.player[projectile.owner].Center, projectile.Center);
            return true;
        }
    }

    public class PetPlanteraProjTentacle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mean Seed Tentacle");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.aiStyle = -1;
            projectile.width = 14; //14
            projectile.height = 19; //19
            //gets set in the buff
            //projectile.damage = 1; //to prevent dyes from working on it
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetPlantera = false;
            }
            if (modPlayer.PetPlantera)
            {
                projectile.timeLeft = 2;
            }
            float offsetX = 0;
            float offsetY = 0;
            switch (projectile.whoAmI % 4)
            {
                case 0:
                    offsetX = -120 + Main.rand.Next(20);
                    offsetY = 0;
                    break;
                case 1:
                    offsetX = -120 + Main.rand.Next(20);
                    offsetY = 120;
                    break;
                case 2:
                    offsetX = 0 - Main.rand.Next(20);
                    offsetY = 120;
                    break;
                default: //case 3
                    break;
            }
            AssAI.ZephyrfishAI(projectile, parent: Main.projectile[(int)projectile.ai[1]], velocityFactor: 1.5f + projectile.whoAmI % 4, random: true, swapSides: 1, offsetX: offsetX, offsetY: offsetY);
            Vector2 between = Main.projectile[(int)projectile.ai[1]].Center - projectile.Center;
            projectile.spriteDirection = 1;
            projectile.rotation = (float)Math.Atan2(between.Y, between.X);

            AssAI.ZephyrfishDraw(projectile, 3 + Main.rand.Next(3));
        }
    }
}
