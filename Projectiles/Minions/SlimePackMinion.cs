using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions
{
	[Content(ContentType.Weapons)]
	//check this file for more info vvvvvvvv
	public class SlimePackMinion : BabySlimeBase
	{
		public const int DefDamage = 26;
		public const float DefKnockback = 4f; //same as slime staff x 2
		public const float SpikedIncrease = 0.4f;

		protected byte TotalNumberOfThese = 16; //16 for default, 13 for assorted, 16 for spiked

		protected string SlimeType = "";

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Minions/SlimePackMinions/SlimeMinion" + SlimeType + "_0"; //use fixed texture
			}
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Slime Pack Minion");
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 30;

			Projectile.DamageType = DamageClass.Summon;
			Projectile.minion = true;

			shootSpikes = false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			//AssUtils.Print("send netupdate " + PickedTexture + " " + ShootTimer);
			writer.Write((byte)PickedTexture);
			//writer.Write((byte)ShootTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			PickedTexture = reader.ReadByte();
			//ShootTimer = reader.ReadByte();
			//AssUtils.Print("recv netupdate " + PickedTexture + " " + ShootTimer);
		}

		public override bool PreAI()
		{
			AssPlayer modPlayer = Projectile.GetOwner().GetModPlayer<AssPlayer>();
			if (Projectile.GetOwner().dead)
			{
				modPlayer.slimePackMinion = false;
			}
			if (modPlayer.slimePackMinion)
			{
				Projectile.timeLeft = 2;
			}

			if (Main.netMode != NetmodeID.Server && Main.myPlayer == Projectile.owner)
			{
				if (!HasTexture)
				{
					byte tex = (byte)Main.rand.Next(TotalNumberOfThese);
					PickedTexture = tex;
					Projectile.netUpdate = true;
				}
			}

			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//Rainbow is _5, Illuminant is _15
			if (HasTexture)
			{
				Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Minions/SlimePackMinions/SlimeMinion" + SlimeType + "_" + PickedTexture).Value;
				Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
				Vector2 stupidOffset = new Vector2(0f, Projectile.gfxOffY - DrawOriginOffsetY); //gfxoffY is for when the projectile is on a slope or half brick
				SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
				Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;

				if (PickedTexture == 5 && SlimeType != "Assorted")
				{
					double cX = Projectile.Center.X + DrawOffsetX;
					double cY = Projectile.Center.Y + DrawOriginOffsetY;
					lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);
				}

				Color color = lightColor * ((255 - Projectile.alpha) / 255f);

				if (SlimeType == "Assorted" && (PickedTexture == 0 ||
					PickedTexture == 5 ||
					PickedTexture == 6 ||
					PickedTexture == 7 ||
					PickedTexture == 11)) color.A = 255;

				if (PickedTexture == 3 && SlimeType != "Assorted") //pinky
				{
					drawPos.Y += 7f;
					Projectile.scale = 0.5f;
				}

				Main.EntitySpriteDraw(image, drawPos, bounds, color, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effect, 0);
			}
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (HasTexture)
			{
				if ((PickedTexture == 15 && SlimeType != "Assorted") || (PickedTexture == 10 || PickedTexture == 12) && SlimeType == "Assorted")
				{
					Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Minions/SlimePackMinions/SlimeMinion" + SlimeType + "_" + PickedTexture + "_Glowmask").Value;
					Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
					SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
					Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
					Vector2 stupidOffset = new Vector2(0f, Projectile.gfxOffY - DrawOriginOffsetY); //gfxoffY is for when the projectile is on a slope or half brick

					if (PickedTexture == 15 && SlimeType != "Assorted") //illuminant slime
					{
						for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
						{
							//the higher the k, the older the position
							//Length is implicitely set in TrailCacheLength up there
							//start from half the length so the origninal sprite isnt super blurred
							Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + stupidOffset;
							Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - k) / (1f * Projectile.oldPos.Length)) * ((255 - 80) / 255f);
							color.A = (byte)(80 * ((Projectile.oldPos.Length - k) / (1f * Projectile.oldPos.Length)));
							Main.EntitySpriteDraw(image, drawPos, bounds, color, Projectile.oldRot[k], bounds.Size() / 2, Projectile.scale, effect, 0);
						}
					}
					else if ((PickedTexture == 10 || PickedTexture == 12) && SlimeType == "Assorted")
					{
						Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
						Color fullColor = lightColor;
						fullColor.A = 255;
						Main.EntitySpriteDraw(image, drawPos, bounds, fullColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effect, 0);

						if (PickedTexture == 10)
						{
							//Turtle
							image = Mod.Assets.Request<Texture2D>("Projectiles/Minions/SlimePackMinions/SlimeMinion" + SlimeType + "_" + PickedTexture + "_Glowmask2").Value;
							Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effect, 0);
						}
					}
				}
			}
		}
	}

	public class SlimePackAssortedMinion : SlimePackMinion
	{
		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Slime Pack Minion");
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

			//No trailing
		}

		public override void SafeSetDefaults()
		{
			//used to set dimensions (if necessary) //also use to set projectile.minion
			Projectile.width = 32;
			Projectile.height = 30;

			Projectile.minion = true;

			SlimeType = "Assorted";
			TotalNumberOfThese = 13;

			shootSpikes = false;
		}
	}

	public class SlimePackSpikedMinion : SlimePackMinion
	{
		public override void SafeSetDefaults()
		{
			//used to set dimensions (if necessary) //also use to set projectile.minion
			Projectile.width = 32;
			Projectile.height = 30;

			SlimeType = "Spiked";
			TotalNumberOfThese = 16;

			shootSpikes = true;
		}
	}
}
