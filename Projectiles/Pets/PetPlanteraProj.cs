using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetPlanteraProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mean Seed");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
            projectile.width = 36;
            projectile.height = 36;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eater = false; // Relic from aiType
            return true;
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

            if (Vector2.Distance(projectile.Center, player.Center) > 3000f)
            {
                projectile.Center = player.Center;
            }
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
            AssUtils.DrawTether(spriteBatch, "AssortedCrazyThings/Projectiles/Pets/PetPlanteraProj_Chain", projectile.Center, Main.player[projectile.owner].Center);
            return true;
        }

        public override void PostAI()
        {
            Vector2 between = projectile.Center - Main.player[projectile.owner].Center;
            //projectile.rotation = (float)Math.Atan2(between.Y, between.X) + 1.57f;
            projectile.rotation += 3.14159f;
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
            AssAI.ZephyrfishAI(projectile, parent: Main.projectile[(int)projectile.ai[1]], velocityFactor: 1f + projectile.whoAmI % 4, random: true, swapSides: 1, offsetX: offsetX, offsetY: offsetY);
            Vector2 between = Main.projectile[(int)projectile.ai[1]].Center - projectile.Center;
            projectile.spriteDirection = 1;
            projectile.rotation = (float)Math.Atan2(between.Y, between.X);

            AssAI.ZephyrfishDraw(projectile, 3 + Main.rand.Next(3));
        }
    }
}
