using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class MiniMegalodonProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(2, 5 - 2, 8)
				.WhenNotSelected(2, 1)
				.WithOffset(-8f, 0f)
				.WithSpriteDirection(-1);

			AmuletOfManyMinionsApi.RegisterSlimePet(this, ModContent.GetInstance<MiniMegalodonBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.EyeSpring);
			Projectile.aiStyle = -1;
			Projectile.width = 32;
			Projectile.height = 24;
			DrawOffsetX = -4;
			DrawOriginOffsetY = -8;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.MiniMegalodon = false;
			}
			if (modPlayer.MiniMegalodon)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.EyeSpringAI(Projectile, flyForever: false);
		}
	}
}
