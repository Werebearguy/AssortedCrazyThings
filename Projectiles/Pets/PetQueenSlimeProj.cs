using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Handlers.CharacterPreviewAnimationsHandler;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Items.Pets;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetQueenSlimeAirProj : SimplePetProjBase
	{
		public static LocalizedText CommonDisplayNameText { get; private set; }

		public override LocalizedText DisplayName => CommonDisplayNameText;

		public override void SetStaticDefaults()
		{
			CommonDisplayNameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.PetQueenSlimeProj.DisplayName"));
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 5)
				.WithOffset(-10f, -24f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<PetQueenSlimeBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
			Projectile.width = 20;
			Projectile.height = 20;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.zephyrfish = false; // Relic from AIType

			Projectile.originalDamage = (int)(Projectile.originalDamage * 0.65f);
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetQueenSlime = false;
			}
			if (modPlayer.PetQueenSlime)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}
	}

	[Content(ContentType.DroppedPets)]
	public abstract class PetQueenSlimeGroundProj : BabySlimeBase
	{
		const int period = 70;
		protected bool front;

		protected int timer = 0;

		public override bool UseJumpingFrame => false;

		public PetQueenSlimeGroundProj(bool front)
		{
			this.front = front;
		}

		public override LocalizedText DisplayName => PetQueenSlimeAirProj.CommonDisplayNameText;

		public override void SafeSetStaticDefaults()
		{
			SecondaryPetHandler.AddToMainProj(ModContent.ProjectileType<PetQueenSlimeAirProj>(), Projectile.type, false);

			if (front)
			{
				ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type]
					.WithOffset(8, 0);
			}
			else
			{
				ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type]
					.WithOffset(-2, 0)
					.WithCode(SlimePet2Offset);
			}

			AmuletOfManyMinionsApi.RegisterSlimePet(this, ModContent.GetInstance<PetQueenSlimeBuff_AoMM>(), null);
		}

		public static void SlimePet2Offset(Projectile proj, bool walking)
		{
			if (walking)
			{
				float percent = (float)Main.timeForVisualEffects % 30f / 30f;
				float change = Utils.MultiLerp(percent, 0f, 16f, 20f, 20f, 16f, 0f, 0f/*, 0f*/);
				proj.position.Y -= change;
			}
		}

		public override void SafeSetDefaults()
		{
			Projectile.minion = false;

			Projectile.width = 20;
			Projectile.height = 18;
			Projectile.hide = true;

			DrawOriginOffsetY = 0;
			DrawOffsetX = -20;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetQueenSlime = false;
			}
			if (modPlayer.PetQueenSlime)
			{
				Projectile.timeLeft = 2;
			}

			Projectile.originalDamage = (int)(Projectile.originalDamage * 0.65f);

			return true;
		}

		public override void PostAI()
		{
			timer = (timer + 1) % period;

			alignFront = (timer - period / 2) > 0;
			if (!front)
			{
				alignFront = !alignFront;
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			(alignFront ? overPlayers : behindProjectiles).Add(index);
		}
	}

	public class PetQueenSlimeGround1Proj : PetQueenSlimeGroundProj
	{
		public PetQueenSlimeGround1Proj() : base(true)
		{

		}
	}

	public class PetQueenSlimeGround2Proj : PetQueenSlimeGroundProj
	{
		public PetQueenSlimeGround2Proj() : base(false)
		{

		}
	}
}
