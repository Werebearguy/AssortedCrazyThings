using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class EnchantedSwordProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Enchanted Sword");
			Main.projFrames[Projectile.type] = 8;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<EnchantedSwordBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.TikiSpirit);
			AIType = ProjectileID.TikiSpirit;
			Projectile.width = 26;
			Projectile.height = 26;
			DrawOriginOffsetY = -14;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.tiki = false;
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.EnchantedSword = false;
			}
			if (modPlayer.EnchantedSword)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				if (AmuletOfManyMinionsApi.IsAttacking(this))
				{
					Projectile.rotation = Projectile.velocity.X * 0.08f + MathHelper.Pi;
				}
				else
				{
					Projectile.rotation += MathHelper.Pi;
				}
			}
		}
	}
}
