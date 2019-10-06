using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetCultistProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/PetCultistProj_0"; //temp
            }
        }
        private float sinY; //depends on projectile.ai[1], no need to sync

        private float Sincounter
        {
            get
            {
                return projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dwarf Cultist");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.aiStyle = -1;
            projectile.width = 20;
            projectile.height = 26;
            projectile.tileCollide = false;
        }

        private void CustomDraw()
        {
            //frame 0: idle
            //frame 0 to 3: loop back and forth while healing
            Player player = projectile.GetOwner();

            if (player.statLife < player.statLifeMax2 / 2)
            {
                Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.5f);
                Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.5f);
            }

            if (projectile.velocity.Length() < 6f && player.statLife < player.statLifeMax2 / 2)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter <= 30)
                {
                    projectile.frame = 0;
                }
                else if (projectile.frameCounter <= 35)
                {
                    projectile.frame = 1;
                }
                else if (projectile.frameCounter <= 40)
                {
                    projectile.frame = 2;
                }
                else if (projectile.frameCounter <= 70)
                {
                    projectile.frame = 3;
                }
                else if (projectile.frameCounter <= 75)
                {
                    projectile.frame = 2;
                }
                else if (projectile.frameCounter <= 80)
                {
                    projectile.frame = 1;
                }
                else
                {
                    projectile.frameCounter = 0;
                }
            }
            else
            {
                projectile.frameCounter = 0;
                projectile.frame = 0;
            }
        }

        private void CustomAI()
        {
            Player player = projectile.GetOwner();

            Sincounter = Sincounter >= 240 ? 0 : Sincounter + 1;
            sinY = (float)((Math.Sin((Sincounter / 120f) * MathHelper.TwoPi) - 1) * 4);

            if (projectile.velocity.Length() < 6f && player.statLife < player.statLifeMax2 / 2)
            {
                if (Sincounter % 80 == 30)
                {
                    int heal = 1;
                    player.statLife += heal;
                    player.HealEffect(heal, false);
                }
                Vector2 spawnOffset = new Vector2(projectile.width * 0.5f, -20f + projectile.height * 0.5f);
                Vector2 spawnPos = projectile.position + spawnOffset;

                Dust dust = Dust.NewDustPerfect(spawnPos, 175, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)));
                dust.noGravity = true;
                dust.fadeIn = 1.2f;

                //basically remaps player health from (max / 2) to 0 => 0.1f to 0.9f
                float complicatedFormula = (((float)(player.statLifeMax2 / 2) - player.statLife) * 0.8f) / ((float)player.statLifeMax2 / 2) + 0.1f;

                if (Main.rand.NextFloat() < complicatedFormula)
                {
                    spawnOffset = new Vector2(0f, -20f);
                    spawnPos = projectile.position + spawnOffset;
                    dust = Dust.NewDustDirect(new Vector2(spawnPos.X, spawnPos.Y), projectile.width, projectile.height, 175, 0f, 0f, 0, default(Color), 1.5f);
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    dust.velocity = Vector2.Normalize(player.MountedCenter - new Vector2(0f, player.height / 2) - (projectile.Center + spawnOffset)) * (Main.rand.NextFloat() + 5f) + projectile.velocity * 1.5f;
                }
            }
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.PetCultist = false;
            }
            if (modPlayer.PetCultist)
            {
                projectile.timeLeft = 2;
            }
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, vanityPet: true, veloXToRotationFactor: 0.5f, offsetX: 16f, offsetY: (player.statLife < player.statLifeMax2 / 2) ? -26f : 2f);

            CustomAI();
            CustomDraw();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection != 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            PetPlayer mPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            Texture2D image = mod.GetTexture("Projectiles/Pets/PetCultistProj_" + mPlayer.petCultistType);
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;
            Vector2 stupidOffset = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f - projectile.gfxOffY + sinY);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
