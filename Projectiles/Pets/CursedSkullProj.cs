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
	public class CursedSkullProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/CursedSkullProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cursed Skull");
			Main.projFrames[Projectile.type] = 3;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<CursedSkullBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
			DrawOriginOffsetY = 2;
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
				modPlayer.CursedSkull = false;
			}
			if (modPlayer.CursedSkull)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override void PostAI()
		{
			if (Projectile.frame >= 3) Projectile.frame = 0;

			if (!AmuletOfManyMinionsApi.IsActive(this) || !AmuletOfManyMinionsApi.IsAttacking(this))
			{
				Projectile.spriteDirection = (Projectile.Center.X - Projectile.GetOwner().Center.X > 0f).ToDirectionInt();
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/CursedSkullProj_" + mPlayer.cursedSkullType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + 2f + Projectile.gfxOffY);

			//BEWARE, HERE THE COLOR IS Color.White INSTEAD OF lightColor
			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, Color.White, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}
}
