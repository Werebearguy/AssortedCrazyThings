using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class PetSunProj : SimplePetProjBase
	{
		private float coronaRotation = 0f;
		private const float coronaRotationSpeed = 0.008f;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/PetSunProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.alpha = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, Vector3.One);

			int texture = 0;
			if (Main.eclipse) //takes priority
			{
				texture = 2;
			}
			else if (Projectile.GetOwner().head == 12)
			{
				texture = 1;
			}

			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PetSunProj_" + texture).Value;

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height - 28f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;

			Texture2D corona = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PetSunProj_Corona").Value;
			Main.EntitySpriteDraw(corona, drawPos, image.Bounds, Color.White, coronaRotation, image.Bounds.Size() / 2, 1f, SpriteEffects.None, 0);

			Main.EntitySpriteDraw(image, drawPos, image.Bounds, Color.White, 0f, image.Bounds.Size() / 2, 1f, SpriteEffects.None, 0);
			return false;
		}

		public override void AI()
		{
			coronaRotation += coronaRotationSpeed;
			if (coronaRotation > MathHelper.TwoPi)
			{
				coronaRotation %= MathHelper.TwoPi;
			}

			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetSun = false;
			}
			if (modPlayer.PetSun)
			{
				Projectile.timeLeft = 2;

				AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, offsetX: 20f, offsetY: -32f);
			}
		}
	}
}
