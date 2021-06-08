using AssortedCrazyThings.Base;
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
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            Projectile.aiStyle = -1;
            Projectile.width = 14;
            Projectile.height = 24;
            Projectile.alpha = 0;
        }

        //draw it with 78% "brightness" (like the NPC and item version of that soul), plus that "up/down" motion
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D image = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[Projectile.type];
            bounds.Y = Projectile.frame * bounds.Height;

            float sinY;
            sincounter = sincounter > 120 ? 0 : sincounter + 1;
            sinY = (float)((Math.Sin((sincounter / 120f) * MathHelper.TwoPi) - 1) * 10);

            Vector2 stupidOffset = new Vector2(Projectile.width / 2, (Projectile.height - 10f) + sinY);
            Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;

            lightColor = Projectile.GetAlpha(lightColor) * 0.99f; //1f is opaque
            lightColor.R = Math.Max(lightColor.R, (byte)200); //100 for dark
            lightColor.G = Math.Max(lightColor.G, (byte)200);
            lightColor.B = Math.Max(lightColor.B, (byte)200);

            Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, 0f, bounds.Size() / 2, 1f, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.SoulLightPet = false;
            }
            if (modPlayer.SoulLightPet)
            {
                Projectile.timeLeft = 2;

                AssAI.FlickerwickPetAI(Projectile);

                AssAI.FlickerwickPetDraw(Projectile, frameCounterMaxFar: 4, frameCounterMaxClose: 7);
            }
        }
    }
}
