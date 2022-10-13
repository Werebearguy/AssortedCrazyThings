using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class DocileDemonEyeProj : SimplePetProjBase
	{
		public const byte TotalNumberOfThese = 12;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/DocileDemonEyeProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Docile Demon Eye");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			//Some forms spawn projectile
			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<DocileDemonEyeBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.eater = false; // Relic from AIType

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.DocileDemonEye = false;
			}
			if (modPlayer.DocileDemonEye)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private void AoMM_AI()
		{
			if (!AmuletOfManyMinionsApi.IsAttacking(this) || !AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var destination))
			{
				return;
			}

			destination.FiredProjectileId = null; //Dynamically set projectile

			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			int eyeType = mPlayer.petEyeType; //Make the laser variants shoot a laser
			if (eyeType >= 9 && eyeType <= 11)
			{
				destination.FiredProjectileId = ModContent.ProjectileType<DocileDemonEyeShotProj>();
			}
			AmuletOfManyMinionsApi.UpdateParamsDirect(this, destination);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/DocileDemonEyeProj_" + mPlayer.petEyeType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}

		public override void PostAI()
		{
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X < 0).ToDirectionInt();
		}
	}

	public class DocileDemonEyeShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.MiniRetinaLaser; //Optic staff laser

		public override void OnSpawn(IEntitySource source)
		{
			//Due to increased extraUpdates (2), but too slow (0.25f) looks unnatural for a laser
			Projectile.velocity *= 0.5f;
		}
	}
}
