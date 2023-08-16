using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Tools
{
	[Content(ContentType.Tools)]
	public abstract class ExtendoNetBaseProj : AssProjectile
	{
		protected float rangeMin = 3 * 16;
		protected float rangeMax = 11 * 16;

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;

			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			int duration = player.itemAnimationMax;
			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.gfxOffY = player.gfxOffY;
			Projectile.velocity = Vector2.Normalize(Projectile.velocity); //Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			Projectile.direction = player.direction;
			Projectile.spriteDirection = -Projectile.direction;
			player.heldProj = Projectile.whoAmI;

			float halfDuration = duration * 0.5f;
			float progress;

			//Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;
			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			//Move the projectile from the rangeMin to the rangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * rangeMin, Projectile.velocity * rangeMax, progress);

			// Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
			// MathHelper.ToRadians(xx degrees here)
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			// Offset by 90 degrees here
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= MathHelper.ToRadians(90f);
			}

			if (Main.myPlayer == Projectile.owner)
			{
				Vector2 between = player.Center - Projectile.Center;
				between.Normalize();
				Rectangle hitboxMod = new Rectangle(Projectile.Hitbox.X + (int)(between.X * Projectile.width * 1.3f),
					Projectile.Hitbox.Y + (int)(between.Y * Projectile.height * 1.3f),
					Projectile.width,
					Projectile.height);

				Item item = player.HeldItem;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active && npc.catchItem > 0)
					{
						NPC.CheckCatchNPC(npc, hitboxMod, item, player, Terraria.ID.ItemID.Sets.LavaproofCatchingTool[item.type]);
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 flipOffset = new Vector2(player.direction == 1 ? texture.Width : 0, 0);
			SpriteEffects effects = player.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
			Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(0, Projectile.gfxOffY) - Main.screenPosition, frame, lightColor, Projectile.rotation, flipOffset, 1f, effects, 0);
			return false;
		}
	}
}
