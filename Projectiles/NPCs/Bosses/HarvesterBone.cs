using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses
{
    //used in BaseHarvester, same as bone, just applies slow
    [Content(ContentType.Bosses)]
    public class HarvesterBone : AssProjectile
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Images/Projectile_" + ProjectileID.SkeletonBone;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harvester Bone");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SkeletonBone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (Main.rand.NextFloat() >= 0.5f)
            {
                target.AddBuff(BuffID.Slow, 90); //1 1/2 seconds, 50% chance
            }
        }
    }
}
