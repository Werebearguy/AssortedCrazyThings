using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class GhostMartianProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Martian");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 22;
			Projectile.height = 42;
			Projectile.alpha = 70;
		}

		private const int sincounterMax = 130;
		private int sincounter;

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = TextureAssets.Projectile[Type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			float sinY = (float)((Math.Sin(((float)sincounter / sincounterMax) * MathHelper.TwoPi) - 1) * 2);

			Vector2 stupidOffset = new Vector2(image.Width / 2, Projectile.height / 2 + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, 0f, bounds.Size() / 2, 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();

			sincounter = sincounter > sincounterMax ? 0 : sincounter + 1;

			if (player.dead)
			{
				modPlayer.GhostMartian = false;
			}
			if (modPlayer.GhostMartian)
			{
				Projectile.timeLeft = 2;

				AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, offsetX: -6f, offsetY: 6f);

				Projectile.spriteDirection = (player.Center.X <= Projectile.Center.X).ToDirectionInt();

				AssAI.FlickerwickPetDraw(Projectile, frameCounterMaxFar: 4, frameCounterMaxClose: 10);
			}
		}
	}
}
