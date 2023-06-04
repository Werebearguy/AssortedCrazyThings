using AssortedCrazyThings.Dusts;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff
{
	[Content(ContentType.Weapons)]
	public abstract class MagicSlimeSlingMinionBase : BabySlimeBase
	{
		private const int TimeLeft = 360;

		private const int PulsatingLimit = 30;

		private bool Increment = true;

		private bool Spawned = false;

		public byte ColorType = 0;
		public Color Color => MagicSlimeSling.GetColor(ColorType);

		public override string Texture => "AssortedCrazyThings/Projectiles/Minions/MagicSlimeSlingStuff/MagicSlimeSlingMinion";

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCs.Add(index);
		}

		private int pulsatingCounter;

		private float PulsatingAlpha
		{
			get
			{
				//0.7f to 1f when full TimeLeft, drops down to 0.7f
				return 0.7f + ((float)pulsatingCounter / PulsatingLimit) * ((float)Projectile.timeLeft / TimeLeft);
			}
		}

		public override bool UseJumpingFrame => false;

		public static LocalizedText CommonDisplayNameText { get; private set; }

		public override LocalizedText DisplayName => CommonDisplayNameText;

		public override void SafeSetStaticDefaults()
		{
			CommonDisplayNameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.MagicSlimeSlingMinion.DisplayName"));
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;

			Projectile.DamageType = DamageClass.Summon;
			Projectile.minion = true;
			customMinionSlots = 0f;
			Projectile.timeLeft = TimeLeft;
			Projectile.hide = true;

			DrawOriginOffsetY = 3;
			DrawOffsetX = -4;
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
			if (source is not EntitySource_Parent parent)
			{
				return;
			}

			if (parent.Entity is not Projectile parentProj)
			{
				return;
			}

			if (parentProj.ModProjectile is not MagicSlimeSlingFired fired)
			{
				return;
			}

			ColorType = fired.ColorType;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 4, -Projectile.direction, -2f, 200, Color, 1f);
				dust.velocity *= 0.5f;
			}
			SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.7f, Pitch = 0.2f }, Projectile.Center);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			if (Color == default(Color)) return lightColor;
			Color color = lightColor.MultiplyRGB(Color) * PulsatingAlpha;
			if (color.A > 220) color.A = 220;
			return color;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.Inflate(6, 6);
			hitbox.Y -= 6;
		}

		public override void PostAI()
		{
			if (!Spawned)
			{
				Spawned = true;
				SoundEngine.PlaySound(SoundID.Item9 with { Volume = 0.7f }, Projectile.Center);
			}

			if (Increment)
			{
				pulsatingCounter++;
				if (pulsatingCounter >= PulsatingLimit) Increment = false;
			}
			else
			{
				pulsatingCounter--;
				if (pulsatingCounter <= 0) Increment = true;
			}

			if (Projectile.frame >= Main.projFrames[Projectile.type])
			{
				Projectile.frame = 1;
			}

			if (InAir)
			{
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

				if (Main.rand.NextFloat() < Projectile.velocity.LengthSquared() / (7 * 7))
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default(Color), 1.25f);
					dust.velocity *= 0.1f;
				}
			}
		}
	}

	//Separate classes so projectile.usesIDStaticNPCImmunity works
	public class MagicSlimeSlingMinion1 : MagicSlimeSlingMinionBase { }

	public class MagicSlimeSlingMinion2 : MagicSlimeSlingMinionBase { }

	public class MagicSlimeSlingMinion3 : MagicSlimeSlingMinionBase { }
}
