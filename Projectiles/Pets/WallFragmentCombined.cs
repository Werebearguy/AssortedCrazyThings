using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class WallFragmentProjBase : ModProjectile
    {
        public sealed override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }

        public virtual void SafeSetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyEater);
            AIType = ProjectileID.BabyEater;
            Projectile.width = 26;
            Projectile.height = 40;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.eater = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.WallFragment = false;
            }
            if (modPlayer.WallFragment)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = Mod.GetTexture("Projectiles/Pets/" + Name + "_" + mPlayer.wallFragmentType).Value;
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = Projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[Projectile.type]
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);

            Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

            return false;
        }
    }

    public class WallFragmentMouth : WallFragmentProjBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/WallFragmentMouth_0"; //temp
            }
        }

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Mouth");
        }
    }

    public class WallFragmentEye1 : WallFragmentProjBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/WallFragmentEye1_0"; //temp
            }
        }

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Eye");
        }
    }

    public class WallFragmentEye2 : WallFragmentEye1
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/WallFragmentEye2_0"; //temp
            }
        }
    }
}
