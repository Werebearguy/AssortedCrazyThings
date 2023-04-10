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
	public class MetroidPetProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/MetroidPetProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Metroid");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<MetroidPetBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;
			Projectile.aiStyle = -1;
			Projectile.width = 40;
			Projectile.height = 38;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.MetroidPet = false;
			}
			if (modPlayer.MetroidPet)
			{
				Projectile.timeLeft = 2;
			}

			AssAI.BabyEaterAI(Projectile, velocityFactor: 1.5f, sway: 0.6f);
			AssAI.BabyEaterDraw(Projectile);

			Projectile.rotation = 0f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Projectile.GetOwner();

			PetPlayer mPlayer = player.GetModPlayer<PetPlayer>();
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/MetroidPetProj_" + mPlayer.metroidPetType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;
			drawOrigin.Y += Projectile.height / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0);
			return false;
		}
	}
}
