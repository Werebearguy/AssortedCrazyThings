using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class PetMoonProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/PetMoonProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.alpha = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();

			Projectile.frame = Main.moonPhase;

			if (Projectile.frame == 4 || Main.eclipse)
			{
				//Invisible
				return false;
			}

			//int texture = Main.moonType; //0, 1 and 2
			int texture = mPlayer.petMoonType;
			if (Main.pumpkinMoon)
			{
				texture = 9;
			}
			else if (Main.snowMoon)
			{
				texture = 10;
			}
			else if (WorldGen.drunkWorldGen)
			{
				texture = 11;
			}

			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PetMoonProj_" + texture).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height - 18f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;

			Main.EntitySpriteDraw(image, drawPos, bounds, Color.LightGray, 0f, bounds.Size() / 2, 1f, SpriteEffects.None, 0);
			return false;
		}

		public override void AI()
		{
			Vector3 lightVector = Vector3.One * 0.6f;
			float lightFactor = 1f;
			//0 is 1f, 4 is not drawn at all
			if (Projectile.frame == 1 || Projectile.frame == 7)
			{
				lightFactor = 0.85f;
			}
			else if (Projectile.frame == 2 || Projectile.frame == 6)
			{
				lightFactor = 0.7f;
			}
			else if (Projectile.frame == 3 || Projectile.frame == 5)
			{
				lightFactor = 0.55f;
			}

			Lighting.AddLight(Projectile.Center, lightVector * lightFactor);

			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetMoon = false;
			}
			if (modPlayer.PetMoon)
			{
				Projectile.timeLeft = 2;

				AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, offsetX: 20f, offsetY: -32f);
			}
		}
	}
}
