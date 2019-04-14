using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class PetGolemHeadFireball : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.Fireball;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pet Golem Head Fireball");
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Fireball);
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = 1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(0, projectile.position);
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffID.OnFire, 240);
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Item20, projectile.position);
            }
            projectile.localAI[0] += 1f;
        }
    }
}
