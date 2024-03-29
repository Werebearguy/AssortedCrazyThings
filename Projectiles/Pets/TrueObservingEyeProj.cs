using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class TrueObservingEyeProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
				.WithOffset(2f, -30f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<TrueObservingEyeBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.tileCollide = false;
		}

		Vector2 eyeTowardsPosition = Vector2.Zero;

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = TextureAssets.Projectile[Projectile.type].Value;

			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 eyeCenter = new Vector2(0f, 12f);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin - eyeCenter, 1f, effects, 0);

			//Draw Eye

			image = ModContent.Request<Texture2D>(Texture + "_Eye").Value;

			Vector2 between = eyeTowardsPosition - (Projectile.position + stupidOffset);
			//between.Length(): 94 is "idle", 200 is very fast following
			//28.5f = 200f / 7f
			float magnitude = Utils.Clamp(between.Length() / 28.5f, 1.3f, 7f);

			between.Normalize();
			between *= magnitude;

			if (!Projectile.isAPreviewDummy)
			{
				drawPos += between;
			}
			drawOrigin = image.Bounds.Size() / 2;
			Main.EntitySpriteDraw(image, drawPos, image.Bounds, lightColor, Projectile.rotation, drawOrigin, 1f, effects, 0);

			return false;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.TrueObservingEye = false;
			}
			if (modPlayer.TrueObservingEye)
			{
				Projectile.timeLeft = 2;
			}

			eyeTowardsPosition = player.Center;

			bool defaultAI = true;
			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				//Split off before regular AI because only then velocity is the aomm one
				defaultAI = AoMM_AI();
			}

			if (defaultAI)
			{
				AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, veloSpeed: 0.5f, offsetX: 20f, offsetY: -60f);
				AssAI.FlickerwickPetDraw(Projectile, 6, 8);
			}
			else
			{
				AssAI.FlickerwickPetDraw(Projectile, 8, 8);
			}
		}

		private bool AoMM_AI()
		{
			if (!AmuletOfManyMinionsApi.IsAttacking(this) || !AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) 
				|| !state.IsInFiringRange || state.TargetNPC is not NPC targetNPC)
			{
				return true;
			}

			eyeTowardsPosition = targetNPC.Center;

			Vector2 between = Projectile.Center - targetNPC.Center;
			Projectile.rotation = Projectile.velocity.X * 0.05f;
			Projectile.spriteDirection = Projectile.direction = -(between.X < 0).ToDirectionInt();

			return false;
		}
	}
}
