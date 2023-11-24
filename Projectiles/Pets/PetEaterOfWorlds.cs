using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public abstract class PetEaterofWorldsBase : SimplePetProjBase
	{
		public static int[] wormTypes;

		public static LocalizedText CommonDisplayNameText { get; private set; }

		public override LocalizedText DisplayName => CommonDisplayNameText;

		public override void SetStaticDefaults()
		{
			CommonDisplayNameText ??= Mod.GetLocalization($"{LocalizationCategory}.PetEaterofWorlds.DisplayName");
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			//ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true; //doesn't work for some reason with hide = true
			//ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			AssAI.StardustDragonSetDefaults(Projectile, size: HITBOX_SIZE, minion: false);
			Projectile.alpha = 0;
		}

		//default 2
		public const int NUMBER_OF_BODY_SEGMENTS = 5;

		//default 24
		public const int HITBOX_SIZE = 24;

		//default 16
		public const int DISTANCE_BETWEEN_SEGMENTS = 17;

		public override bool PreAI()
		{
			Projectile.originalDamage = (int)(Projectile.originalDamage * 1.3f);

			return base.PreAI();
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetEaterofWorlds = false;
			}
			if (modPlayer.PetEaterofWorlds)
			{
				Projectile.timeLeft = 2;
			}

			if (Projectile.type != ModContent.ProjectileType<PetEaterofWorldsHead>())
			{
				AssAI.StardustDragonAI(Projectile, wormTypes, DISTANCE_BETWEEN_SEGMENTS);
			}
			else
			{
				AssAI.BabyEaterAI(Projectile);
				//float scaleFactor = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
				//projectile.scale = 1f + scaleFactor * 0.01f;

				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f).ToDirectionInt();
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 drawPos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle drawRect = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Color color = Projectile.GetAlpha(lightColor);
			Vector2 drawOrigin = drawRect.Size() / 2f;

			//alpha5.A /= 2;

			Main.EntitySpriteDraw(texture, drawPos, drawRect, color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

			return false;
		}
	}

	public class PetEaterofWorldsHead : PetEaterofWorldsBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			AmuletOfManyMinionsApi.RegisterWormPet(this, ModContent.GetInstance<PetEaterofWorldsBuff_AoMM>(), null, false, 140);
		}
	}

	public class PetEaterofWorldsBody1 : PetEaterofWorldsBase { }

	public class PetEaterofWorldsBody2 : PetEaterofWorldsBase { }

	public class PetEaterofWorldsTail : PetEaterofWorldsBase { }
}
