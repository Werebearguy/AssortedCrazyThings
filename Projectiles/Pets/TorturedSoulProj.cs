using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class TorturedSoulProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tortured Soul");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<TorturedSoulBuff_AoMM>(), ModContent.ProjectileType<TorturedSoulShotProj>());
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.zephyrfish = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.TorturedSoul = false;
			}
			if (modPlayer.TorturedSoul)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override void PostDraw(Color lightColor)
		{
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D image = ModContent.Request<Texture2D>(Texture + "_Glowmask").Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, bounds.Height / 2 + Projectile.gfxOffY);

			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 origin = bounds.Size() / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0);
		}
	}

	public class TorturedSoulShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.CopperCoin;

		public override SoundStyle? SpawnSound => SoundID.Coins;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Copied from golden slime HitEffect
			int numParticles = 7;
			float scale = 1.1f;
			var source = Projectile.GetSource_OnHit(target);
			if (target.life <= 0)
			{
				scale = 1.5f;
				numParticles = 40;
				for (int i = 0; i < 8; i++)
				{
					Gore gore = Gore.NewGoreDirect(source, new Vector2(target.position.X, target.Center.Y - 10f), Vector2.Zero, 1218);
					gore.velocity = new Vector2(Main.rand.Next(1, 10) * 0.3f * 2.5f * Math.Sign(Projectile.velocity.X), 0f - (3f + Main.rand.Next(4) * 0.3f));
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					Gore gore = Gore.NewGoreDirect(source, new Vector2(target.position.X, target.Center.Y - 10f), Vector2.Zero, 1218);
					gore.velocity = new Vector2(Main.rand.Next(1, 10) * 0.3f * 2f * Math.Sign(Projectile.velocity.X), 0f - (2.5f + Main.rand.Next(4) * 0.3f));
				}
			}

			for (int j = 0; j < numParticles; j++)
			{
				Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, 10, 2 * Math.Sign(Projectile.velocity.X), -1f, 80, default(Color), scale);
				if (!Main.rand.NextBool(3))
				{
					dust.noGravity = true;
				}
			}
		}
	}
}
