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
	[Content(ContentType.DroppedPets)]
	public class SkeletronHandProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/SkeletronHandProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 8)
				.WithOffset(2, -12f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<SkeletronHandBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;
			Projectile.aiStyle = -1;
			Projectile.width = 24;
			Projectile.height = 24;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.SkeletronHand = false;
			}
			if (modPlayer.SkeletronHand)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.BabyEaterAI(Projectile, sway: 0.8f);
			AssAI.BabyEaterDraw(Projectile);
		}

		public override void PostAI()
		{
			Projectile.rotation = Projectile.velocity.X * -0.08f;
			//projectile.rotation = projectile.velocity.ToRotation() + 1.57f; 
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Projectile.GetOwner();
			var playerPos = player.Center + new Vector2(0, player.gfxOffY);
			if (Projectile.isAPreviewDummy)
			{
				playerPos = Projectile.Center - ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type].Offset + new Vector2(-16, 0);
			}
			AssUtils.DrawSkeletronLikeArms("AssortedCrazyThings/Projectiles/Pets/SkeletronHand_Arm", Projectile.Center - new Vector2(0, Projectile.height / 2), playerPos, selfPad: Projectile.height / 2, centerPad: -20f, direction: 0);

			PetPlayer mPlayer = player.GetModPlayer<PetPlayer>();
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/SkeletronHandProj_" + mPlayer.skeletronHandType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;
			drawOrigin.Y += Projectile.height / 2;

			float betweenX = player.Center.X - Projectile.Center.X;
			SpriteEffects effects = betweenX < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0);
			return false;
		}
	}
}
