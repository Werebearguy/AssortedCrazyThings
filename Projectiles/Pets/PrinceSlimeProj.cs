using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	//check this file for more info vvvvvvvv
	public class PrinceSlimeProj : BabySlimeBase
	{
		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Prince Slime");

			AmuletOfManyMinionsApi.RegisterSlimePet(this, ModContent.GetInstance<PrinceSlimeBuff_AoMM>(), null);
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
				modPlayer.PrinceSlime = false;
			}
			if (modPlayer.PrinceSlime)
			{
				Projectile.timeLeft = 2;
			}
			return true;
		}

		public override void PostDraw(Color drawColor)
		{
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PrinceSlimeProj_Glowmask").Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(0f, Projectile.gfxOffY - DrawOriginOffsetY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f + DrawOriginOffsetY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;

			drawColor.A = 255;

			Main.EntitySpriteDraw(image, drawPos, bounds, drawColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effect, 0);
		}
	}
}
