using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester
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

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			if (Main.rand.NextFloat() >= 0.5f)
			{
				target.AddBuff(BuffID.Slow, 90); //1 1/2 seconds, 50% chance
			}
		}
	}
}
