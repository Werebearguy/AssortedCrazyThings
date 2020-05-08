using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Tools
{
    abstract public class ExtendoNetBaseProj : ModProjectile
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
            projectile.width = 28;
            projectile.height = 28;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1f;

            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public float MovementFactor
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = projectile.GetOwner();
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            projectile.direction = projOwner.direction;
            projectile.spriteDirection = -projectile.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            // As long as the player isn't frozen, the spear can move
            if (!projOwner.frozen)
            {
                if (MovementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    MovementFactor = initialSpeed; // Make sure the spear moves forward when initially thrown out
                    projectile.netUpdate = true; // Make sure to netUpdate this spear
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
            projectile.position += projectile.velocity * MovementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.itemAnimation == 0)
            {
                projectile.Kill();
            }
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            // Offset by 90 degrees here
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }

            if (Main.myPlayer == projectile.owner)
            {
                Vector2 between = projOwner.Center - projectile.Center;
                between.Normalize();
                Rectangle hitboxMod = new Rectangle(projectile.Hitbox.X + (int)(between.X * projectile.width * 1.3f),
                    projectile.Hitbox.Y + (int)(between.Y * projectile.height * 1.3f),
                    projectile.width,
                    projectile.height);

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.catchItem > 0)
                    {
                        if (hitboxMod.Intersects(npc.getRect())/* && (Main.npc[i].noTileCollide || projOwner.CanHit(Main.npc[i]))*/)
                        {
                            NPC.CatchNPC(i, projectile.owner);
                        }
                    }
                }
            }
        }
    }
}
