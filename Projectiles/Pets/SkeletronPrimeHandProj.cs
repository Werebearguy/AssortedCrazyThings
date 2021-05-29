using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class SkeletronPrimeHandProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/SkeletronPrimeHandProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletron Prime Pet Hand");
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
                modPlayer.SkeletronPrimeHand = false;
            }
            if (modPlayer.SkeletronPrimeHand)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.BabyEaterAI(Projectile, sway: 0.8f);
            AssAI.BabyEaterDraw(Projectile);
        }

        public override void PostAI()
        {
            Projectile.rotation = Projectile.velocity.X * -0.08f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            AssUtils.DrawSkeletronLikeArms("AssortedCrazyThings/Projectiles/Pets/SkeletronPrimeHand_Arm", Projectile.Center, Projectile.GetOwner().Center, selfPad: Projectile.height / 2, centerPad: -20f, direction: 0);

            PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            Texture2D image = Mod.GetTexture("Projectiles/Pets/SkeletronPrimeHandProj_" + mPlayer.skeletronPrimeHandType).Value;
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[Projectile.type];
            bounds.Y = Projectile.frame * bounds.Height;

            Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height);
            Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
            Vector2 drawOrigin = bounds.Size() / 2;
            drawOrigin.Y += Projectile.height / 2;

            Main.spriteBatch.Draw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
