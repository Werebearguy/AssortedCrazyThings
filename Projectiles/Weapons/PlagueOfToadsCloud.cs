using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Weapons)]
	public class PlagueOfToadsCloud : AssProjectile
	{
		public const int Lifetime = 18000;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.width = 54;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = Lifetime;
		}

		public override void AI()
		{
			Projectile.LoopAnimation(6);
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= Lifetime)
			{
				Projectile.alpha += 5;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}
			else
			{
				Projectile.ai[0] += 1f;
				//8f
				if (Projectile.ai[0] > 21f)
				{
					Projectile.ai[0] = 0f;
					if (Projectile.owner == Main.myPlayer)
					{
						int rainSpawnX = (int)(Projectile.position.X + 14f + Main.rand.Next(Projectile.width - 28));
						int rainSpawnY = (int)(Projectile.position.Y + Projectile.height + 4f);
						//speedY = 5f;
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), rainSpawnX, rainSpawnY, 0f, 2f, ModContent.ProjectileType<PlagueOfToadsProj>(), Projectile.damage, 0f, Projectile.owner, Main.rand.Next(5), Main.rand.NextFloat(0.005f, 0.015f));
					}
				}
			}
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f)
			{
				Projectile.localAI[0] = 0f;
				int cloudCount = 0;
				int cloudIndex = 0;
				float cloudAi1 = 0f;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					//check for other plague of toads clouds
					Projectile other = Main.projectile[i];
					if (other.active && other.owner == Projectile.owner && other.type == Projectile.type && other.ai[1] < Lifetime)
					{
						cloudCount++;
						if (other.ai[1] > cloudAi1)
						{
							cloudIndex = i;
							cloudAi1 = other.ai[1];
						}
					}
				}
				if (cloudCount > 1)
				{
					Projectile projectile = Main.projectile[cloudIndex];
					projectile.netUpdate = true;
					projectile.ai[1] = Lifetime;
				}
			}
		}
	}
}
