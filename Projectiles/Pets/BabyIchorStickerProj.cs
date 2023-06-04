using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class BabyIchorStickerProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 5)
				.WithOffset(-6f, -14f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<BabyIchorStickerBuff_AoMM>(), ModContent.ProjectileType<BabyIchorStickerShotProj>());
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyHornet);
			AIType = ProjectileID.BabyHornet;
			Projectile.width = 38;
			Projectile.height = 44;
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
				modPlayer.BabyIchorSticker = false;
			}
			if (modPlayer.BabyIchorSticker)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			if (Projectile.frameCounter % 2 == Main.GameUpdateCount % 2)
			{
				//Make it animate 50% slower by skipping every second increase
				Projectile.frameCounter--;
			}
		}
	}

	public class BabyIchorStickerShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.GoldenShowerFriendly;

		public override SoundStyle? SpawnSound => SoundID.Item17;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int petLevel = AmuletOfManyMinionsApi.GetPetLevel(Projectile.GetOwner());
			if (petLevel >= 5)
			{
				target.AddBuff(BuffID.Ichor, 300);
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
	}
}
