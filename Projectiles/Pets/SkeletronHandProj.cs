using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class SkeletronHandProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/SkeletronHandProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletron Pet Hand");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
            DrawOriginOffsetY = -8;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyEater);
            AIType = ProjectileID.BabyEater;
            Projectile.aiStyle = -1;
            Projectile.width = 24;
            Projectile.height = 32;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.SkeletronHand = false;
            }
            if (modPlayer.SkeletronHand)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.BabyEaterAI(Projectile, sway: 0.8f);
            AssAI.BabyEaterDraw(Projectile);
        }

        public override void PostAI()
        {
            Projectile.rotation = Projectile.velocity.X * -0.08f;
            //projectile.rotation = projectile.velocity.ToRotation() + 1.57f; 
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Projectile.GetOwner();
            AssUtils.DrawSkeletronLikeArms("AssortedCrazyThings/Projectiles/Pets/SkeletronHand_Arm", Projectile.Center, player.Center + new Vector2(0, player.gfxOffY), selfPad: Projectile.height / 2, centerPad: -20f, direction: 0);

            PetPlayer mPlayer = player.GetModPlayer<PetPlayer>();
            Texture2D image = Mod.GetTexture("Projectiles/Pets/SkeletronHandProj_" + mPlayer.skeletronHandType).Value;
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[Projectile.type];
            bounds.Y = Projectile.frame * bounds.Height;

            Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height);
            Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;
            drawOrigin.Y += Projectile.height / 2;

            float betweenX = player.Center.X - Projectile.Center.X;
            SpriteEffects effects = betweenX < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0);
            return false;
        }
    }
}
