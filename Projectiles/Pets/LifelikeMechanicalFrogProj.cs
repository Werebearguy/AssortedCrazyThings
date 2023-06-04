using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class LifelikeMechanicalFrogProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.Bunny];
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 6 - 0, 4)
				.WhenNotSelected(0, 0)
				.WithOffset(2f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterGroundedPet(this, ModContent.GetInstance<LifelikeMechanicalFrogBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bunny);
			AIType = ProjectileID.Bunny;
			Projectile.width = 18;
			Projectile.height = 20;
			DrawOriginOffsetY = 1;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.bunny = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.LifelikeMechanicalFrog = false;
			}
			if (modPlayer.LifelikeMechanicalFrog)
			{
				Projectile.timeLeft = 2;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/LifelikeMechanicalFrogProj" + (mPlayer.mechFrogType == 1 ? "Crown" : "")).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + DrawOriginOffsetY + Projectile.gfxOffY);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}
}
