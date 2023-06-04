using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	//check this file for more info vvvvvvvv
	public class JoyousSlimeProj : BabySlimeBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/JoyousSlimeProj_0"; //temp
			}
		}

		public override void SafeSetStaticDefaults()
		{
			AmuletOfManyMinionsApi.RegisterSlimePet(this, ModContent.GetInstance<JoyousSlimeBuff_AoMM>(), null);
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 30;

			Projectile.minion = false;
		}

		public override bool PreAI()
		{
			PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			if (Projectile.GetOwner().dead)
			{
				modPlayer.JoyousSlime = false;
			}
			if (modPlayer.JoyousSlime)
			{
				Projectile.timeLeft = 2;
			}
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/JoyousSlimeProj_" + mPlayer.joyousSlimeType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2 + Projectile.gfxOffY);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}
}
