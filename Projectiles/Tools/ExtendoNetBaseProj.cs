using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Tools
{
    [Content(ContentType.Tools)]
    abstract public class ExtendoNetBaseProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("EXTENDO!");
        }

        protected float initialSpeed = 10f;
        protected float extendSpeed = 2.5f;
        protected float retractSpeed = 2.3f;

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public float MovementFactor
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = Projectile.GetOwner();
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            Projectile.spriteDirection = -Projectile.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!projOwner.frozen)
            {
                if (MovementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    MovementFactor = initialSpeed; // Make sure the spear moves forward when initially thrown out
                    Projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
                {
                    MovementFactor -= retractSpeed;
                }
                else // Otherwise, increase the movement factor
                {
                    MovementFactor += extendSpeed;
                }
            }
            // Change the spear position based off of the velocity and the movementFactor
            Projectile.position += Projectile.velocity * MovementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            // Offset by 90 degrees here
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            if (Main.myPlayer == Projectile.owner)
            {
                Vector2 between = projOwner.Center - Projectile.Center;
                between.Normalize();
                Rectangle hitboxMod = new Rectangle(Projectile.Hitbox.X + (int)(between.X * Projectile.width * 1.3f),
                    Projectile.Hitbox.Y + (int)(between.Y * Projectile.height * 1.3f),
                    Projectile.width,
                    Projectile.height);

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.catchItem > 0)
                    {
                        if (hitboxMod.Intersects(npc.getRect())/* && (Main.npc[i].noTileCollide || projOwner.CanHit(Main.npc[i]))*/)
                        {
                            NPC.CatchNPC(i, Projectile.owner);
                        }
                    }
                }
            }
        }
    }
}
