using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Handlers.CharacterPreviewAnimationsHandler;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public abstract class WallFragmentProjBase : SimplePetProjBase
	{
		public sealed override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 8)
				.WithOffset(-4, -2f)
				.WithSpriteDirection(-1)
				.WithCode(DocileDemonEyeProj.FloatAndRotate);

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;
			Projectile.width = 26;
			Projectile.height = 40;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.eater = false; // Relic from AIType

			Projectile.originalDamage = (int)(Projectile.originalDamage * 0.65f);

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.WallFragment = false;
			}
			if (modPlayer.WallFragment)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/" + Name + "_" + mPlayer.wallFragmentType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}

	public class WallFragmentMouth : WallFragmentProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/WallFragmentMouth_0"; //temp
			}
		}

		public override void SafeSetStaticDefaults()
		{
			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<WallFragmentBuff_AoMM>(), null);
		}
	}

	public class WallFragmentEye1 : WallFragmentProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/WallFragmentEye1_0"; //temp
			}
		}

		public override void SafeSetStaticDefaults()
		{
			SecondaryPetHandler.AddToMainProj(ModContent.ProjectileType<WallFragmentMouth>(), Projectile.type, false);

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type]
				.WithOffset(-8, 12f);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<WallFragmentBuff_AoMM>(), ModContent.ProjectileType<WallFragmentEyeShotProj>());
		}
	}

	public class WallFragmentEye2 : WallFragmentEye1
	{
		public override void SafeSetStaticDefaults()
		{
			base.SafeSetStaticDefaults();

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type]
				.WithOffset(-8, -18f);
		}

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/WallFragmentEye2_0"; //temp
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class WallFragmentEyeShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.MiniRetinaLaser; //Optic staff laser

		public override void SafeSetDefaults()
		{
			//To make the two lasers hit simultaneously, need to reset the vanilla behavior, then set custom one
			Projectile.usesIDStaticNPCImmunity = false;
			Projectile.idStaticNPCHitCooldown = 0;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 12;
		}

		public override void OnSpawn(IEntitySource source)
		{
			//Due to increased extraUpdates (2), but too slow (0.25f) looks unnatural for a laser
			Projectile.velocity *= 0.5f;
		}
	}
}
