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
	[Content(ContentType.FriendlyNPCs)]
	public class YoungHarpyProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/YoungHarpyProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type])
				.WithOffset(-6f, -18f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<YoungHarpyBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyHornet);
			AIType = ProjectileID.BabyHornet;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.hornet = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.YoungHarpy = false;
			}
			if (modPlayer.YoungHarpy)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/YoungHarpyProj_" + mPlayer.youngHarpyType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2 + DrawOffsetX, Projectile.height / 2 + Projectile.gfxOffY);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}
}
