using AssortedCrazyThings.Buffs.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Bosses)]
	public class BoneShatterLongbowProj : AssProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.extraUpdates = 1;
		}

		public override void OnSpawn(IEntitySource source)
		{
			//Compensate for increased velocity with extraUpdates
			Projectile.velocity *= 0.75f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			return true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(Color.White, lightColor, 0.5f);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 0.7f);
			if (Projectile.ai[0] >= 15)
			{
				//Compensate for increased drop speed with extraUpdates
				Projectile.velocity.Y -= 0.06f;
			}

			if (Main.rand.NextBool())
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, 135, new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(-0.4f, 0.4f)), 200, Color.White * 0.8f, 0.8f);
				dust.noGravity = false;
				dust.noLight = true;
				dust.fadeIn = Main.rand.NextFloat(0.6f, 0.9f);
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			target.AddBuff(ModContent.BuffType<SoulBurnBuff>(), 10 * 60);
		}
	}
}
