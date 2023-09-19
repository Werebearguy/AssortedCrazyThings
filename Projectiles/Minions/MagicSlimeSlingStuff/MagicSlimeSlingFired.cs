using AssortedCrazyThings.Base;
using AssortedCrazyThings.Dusts;
using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff
{
	[Content(ContentType.Weapons)]
	public class MagicSlimeSlingFired : AssProjectile
	{
		public byte ColorType = 0;
		public Color Color => MagicSlimeSling.GetColor(ColorType);

		public override string Texture => "Terraria/Images/Item_" + ItemID.Gel;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.DamageType = DamageClass.Summon;
			//Projectile.minion = true;
			Projectile.friendly = true;
			Projectile.minionSlots = 0f;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			if (Color == default(Color)) return lightColor;
			return lightColor.MultiplyRGB(Color) * ((255 - Projectile.alpha) / 255f) * 0.7f;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)ColorType);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			ColorType = reader.ReadByte();
		}

		public override void OnSpawn(IEntitySource source)
		{
			SetColor(source);
		}

		private void SetColor(IEntitySource source)
		{
			if (source is not EntitySource_ItemUse itemSource)
			{
				return;
			}

			if (itemSource.Entity is not Player player)
			{
				return;
			}

			ColorType = player.GetModPlayer<AssPlayer>().nextMagicSlimeSlingMinion;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.8f, Pitch = 0.2f }, Projectile.Center);
			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 4, -Projectile.direction, -2f, 200, Color, 1f);
				dust.velocity *= 0.4f;
			}

			if (Projectile.active && Main.myPlayer == Projectile.owner)
			{
				Player player = Projectile.GetOwner();
				int sum = 0;
				for (int i = 0; i < MagicSlimeSling.Types.Length; i++)
				{
					sum += player.ownedProjectileCounts[MagicSlimeSling.Types[i]];
				}
				if (sum < (2 + player.maxMinions))
				{
					int type = MagicSlimeSling.Types[ColorType];
					Vector2 velo = -Vector2.UnitY * Main.rand.NextFloat(2, 4);
					int index = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Top, velo, type, Projectile.damage, Projectile.knockBack, Main.myPlayer);
					Main.projectile[index].originalDamage = Projectile.originalDamage;
				}
			}
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 15;
				if (Projectile.alpha < 0) Projectile.alpha = 0;
			}

			Projectile.rotation += Projectile.velocity.X * 0.05f;

			Projectile.velocity.Y += 0.15f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			if (Projectile.alpha > 200) return;

			//colored sparkles
			int dustType = Main.rand.Next(4);
			if (dustType == 0)
			{
				dustType = ModContent.DustType<GlitterDust15>();
			}
			else if (dustType == 1)
			{
				dustType = ModContent.DustType<GlitterDust57>();
			}
			else if (dustType == 2)
			{
				dustType = ModContent.DustType<GlitterDust58>();
			}
			else
			{
				return;
			}
			// 8f is the shootspeed of the weapon shooting this projectile
			if (Main.rand.NextFloat() < Projectile.velocity.Length() / 7f)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default(Color), 1.25f);
				dust.velocity *= 0.1f;
			}
		}
	}
}
