using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	//Implementing classes at the bottom
	[Content(ContentType.Bosses)]
	public abstract class CompanionDungeonSoulPetProjBase : SimplePetProjBase
	{
		private int sincounter;

		public static Asset<Texture2D> faceAsset;

		protected abstract ref bool GetBool(PetPlayer petPlayer);

		protected abstract int GetOtherProj(Player player);

		public virtual bool ReverseSide => false;

		public override void Load()
		{
			if (!Main.dedServ && faceAsset == null)
			{
				faceAsset = Mod.Assets.Request<Texture2D>("Projectiles/Pets/CompanionDungeonSoulPetProj_Face");
			}
		}

		public override void Unload()
		{
			faceAsset = null;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 18;
			Projectile.height = 28;
			Projectile.alpha = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			float sinY = (float)((Math.Sin((sincounter / 120f) * MathHelper.TwoPi) - 1) * 10);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, (Projectile.height - 10f) + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;

			float rotation = Projectile.rotation;
			Vector2 origin = bounds.Size() / 2;
			float scale = Projectile.scale;
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, rotation, origin, scale, effects, 0);

			image = faceAsset.Value;
			Rectangle faceBounds = bounds;
			faceBounds.Y = faceBounds.Height * GetFaceFrame(Projectile.GetOwner());
			Main.EntitySpriteDraw(image, drawPos, faceBounds, Color.White, rotation, origin, scale, effects, 0);

			return false;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();

			sincounter = sincounter > 120 ? 0 : sincounter + 1;

			ref bool petBool = ref GetBool(modPlayer);
			if (player.dead)
			{
				petBool = false;
			}
			if (petBool)
			{
				Projectile.timeLeft = 2;

				bool reverseSide = ReverseSide;
				int otherProj = GetOtherProj(player);
				bool staticDirection = player.ownedProjectileCounts[otherProj] > 0; //Do not swap sides if both are summoned

				int aommDir = 0;
				if (AmuletOfManyMinionsApi.IsActive(this) && !AmuletOfManyMinionsApi.IsIdle(this))
				{
					aommDir = (Projectile.velocity.X > 0).ToDirectionInt();
				}

				AssAI.FlickerwickPetAI(Projectile, staticDirection: staticDirection, reverseSide: reverseSide);

				AssAI.FlickerwickPetDraw(Projectile, frameCounterMaxFar: 4, frameCounterMaxClose: 7);

				Projectile.direction = Projectile.spriteDirection = -player.direction;

				if (aommDir != 0)
				{
					Projectile.direction = aommDir;
				}

				Projectile.rotation = 0f;
			}
		}

		private int GetFaceFrame(Player player)
		{
			//Sorted by priority
			//0: player select
			//6: aomm only, when not idle
			//3: 25% health
			//2: enemies nearby (60 tile radius)
			//0: idle for long (2 minutes)
			//5: idle for short (15 seconds)
			//4: idle boss slain (1 minute ago)
			//1: idle regular

			if (Projectile.isAPreviewDummy)
			{
				return 0;
			}

			if (AmuletOfManyMinionsApi.IsActive(this) && !AmuletOfManyMinionsApi.IsIdle(this))
			{
				return 6;
			}

			if (player.statLife <= player.statLifeMax2 * 0.25f)
			{
				return 3;
			}

			AssPlayer aPlayer = player.GetModPlayer<AssPlayer>();
			aPlayer.needsNearbyEnemyNumber = true;
			if (aPlayer.nearbyEnemyNumber >= 5)
			{
				return 2;
			}

			if (player.afkCounter > 120 * 60)
			{
				return 0;
			}

			if (player.afkCounter > 15 * 60)
			{
				return 5;
			}

			if (aPlayer.HasSlainBossSecondsAgo(60))
			{
				return 4;
			}

			return 1;
		}
	}

	public class CompanionDungeonSoulPetProj : CompanionDungeonSoulPetProjBase
	{
		protected override ref bool GetBool(PetPlayer petPlayer)
		{
			return ref petPlayer.SoulLightPet;
		}

		protected override int GetOtherProj(Player player)
		{
			return ModContent.ProjectileType<CompanionDungeonSoulPetProj2>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}
	}

	public class CompanionDungeonSoulPetProj2 : CompanionDungeonSoulPetProjBase
	{
		public override string Texture => "AssortedCrazyThings/Projectiles/Pets/CompanionDungeonSoulPetProj";

		protected override ref bool GetBool(PetPlayer petPlayer)
		{
			return ref petPlayer.SoulLightPet2;
		}

		protected override int GetOtherProj(Player player)
		{
			return ModContent.ProjectileType<CompanionDungeonSoulPetProj>();
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 10)
				.WithOffset(2, -12f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<CompanionDungeonSoulPetBuff2_AoMM>(), null);
		}

		public override bool ReverseSide => true;
	}
}
