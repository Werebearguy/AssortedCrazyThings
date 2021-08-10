using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    [Content(ContentType.Weapons)]
    public class LegendaryWoodenSwordProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Legendary Wooden Sword");
        }

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.IronShortswordStab);
            Projectile.Size = new Vector2(18); //This sets width and height to the same value (important when projectiles can rotate)
            Projectile.aiStyle = -1; //Use our own AI to customize how it behaves, if you don't want that, keep this at 161
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1; //Update 1+extraUpdates times per tick
            Projectile.timeLeft = 360; //this value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
            Projectile.hide = true;
        }

        public int Timer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float CollisionWidth => 10f * Projectile.scale;

        public const int fadeInDuration = 7;
        public const int fadeOutDuration = 4;

        public const int totalDuration = 16;

        public override void AI()
        {
            Player player = Projectile.GetOwner();

            Timer += 1;
            if (Timer >= totalDuration)
            {
                Projectile.Kill();
                return;
            }
            else
            {
                player.heldProj = Projectile.whoAmI;
            }

            //Fade in and out
            //GetLerpValue returns a value between 0f and 1f - if clamped is true - representing how far Timer got along the "distance" defined by the first two parameters
            //The first call handles the fade in, the second one the fade out.
            //Notice the second call's parameters are swapped, this means the result will be reverted
            Projectile.Opacity = Utils.GetLerpValue(0f, fadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(totalDuration, totalDuration - fadeOutDuration, Timer, clamped: true);

            //Keep locked onto the player, but extend further based on the given velocity
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
            Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

            //Set spriteDirection based on moving left or right. Left -1, right 1
            Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();

            //Point towards where it is moving, applied offset for top right of the sprite respecting spriteDirection
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

            if (Main.rand.NextBool(2))
            {
                //162 for "sparks"
                //169 for just light
                int dustType = 169;
                Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f, Projectile.width, Projectile.height, dustType, 100, Scale: 1.25f);
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.3f;
            }

            SetVisualOffsets();
        }

        private void SetVisualOffsets()
        {
            const int halfWidth = 32 / 2;
            const int halfHeight = 32 / 2;
            //7 comes from: 32/2 - 18/2 == 16 - 9

            //Vanilla configuration for "hitbox in middle of sprite"
            //X : -7
            //OX: 0
            //OY: -7
            DrawOffsetX = -(halfWidth - Projectile.width / 2);
            DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -(halfHeight - Projectile.height / 2);

            //Vanilla configuration for "hitbox towards the end"
            //if (Projectile.spriteDirection == 1)
            //{
                //sDir 1: (aka hitbox top right)
                //X : 7
                //OX: -14
                //OY: 0
                //DrawOriginOffsetX = -(Projectile.width / 2 - halfWidth);
                //DrawOffsetX = (int)-DrawOriginOffsetX * 2;
                //DrawOriginOffsetY = 0;
            //}
            //else
            //{
                //sDir -1:  (aka hitbox top left)
                //X : 0
                //OX: -7
                //OY: 0
                //DrawOriginOffsetX = (Projectile.width / 2 - halfWidth);
                //DrawOffsetX = 0;
                //DrawOriginOffsetY = 0;
            //}
        }

        public override bool ShouldUpdatePosition()
        {
            //Update Projectile.Center manually
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
            Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity * 6f;
            float collisionPoint = 0f; //Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
        }
    }
}
