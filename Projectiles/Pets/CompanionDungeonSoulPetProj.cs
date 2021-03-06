﻿using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CompanionDungeonSoulPetProj : ModProjectile
    {
        private int sincounter;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 14;
            projectile.height = 24;
            projectile.alpha = 0;
        }

        //draw it with 78% "brightness" (like the NPC and item version of that soul), plus that "up/down" motion
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[projectile.type];
            bounds.Y = projectile.frame * bounds.Height;

            float sinY;
            sincounter = sincounter > 120 ? 0 : sincounter + 1;
            sinY = (float)((Math.Sin((sincounter / 120f) * MathHelper.TwoPi) - 1) * 10);

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 10f) + sinY);
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;

            lightColor = projectile.GetAlpha(lightColor) * 0.99f; //1f is opaque
            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);

            spriteBatch.Draw(image, drawPos, bounds, Color.White, 0f, bounds.Size() / 2, 1f, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }

        //public override bool PreAI()
        //{
        //    Player player = projectile.GetOwner();
        //    player.petFlagDD2Ghost = false; // Relic from aiType
        //    return true;
        //}

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.SoulLightPet = false;
            }
            if (modPlayer.SoulLightPet)
            {
                projectile.timeLeft = 2;

                AssAI.FlickerwickPetAI(projectile);

                AssAI.FlickerwickPetDraw(projectile, frameCounterMaxFar: 4, frameCounterMaxClose: 10);
            }
        }
    }
}
