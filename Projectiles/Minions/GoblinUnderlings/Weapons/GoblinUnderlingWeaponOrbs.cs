using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Weapons
{
	[Content(ContentType.Weapons)]
	public abstract class GoblinUnderlingWeaponOrb : AssProjectile
	{
		public static LocalizedText CommonDisplayNameText { get; private set; }

		public override string Texture => $"AssortedCrazyThings/Projectiles/Minions/GoblinUnderlings/Weapons/{nameof(GoblinUnderlingWeaponOrb)}";

		public const int DefaultBuffDuration = 180;
		public const int DefaultBuffRadius = 3 * 16;

		public int ownedGoblinWhoAmI = -1;

		protected int MaxChargeDuration = 1;
		protected ref float ChargeDuration => ref Projectile.ai[0];
		public bool Homing => ChargeDuration == -1;
		public bool ChargeUp => ChargeDuration > 0;
		protected ref float OrigSpeed => ref Projectile.ai[1];

		public bool FromGoblin => ownedGoblinWhoAmI != -1;

		public virtual SoundStyle? SpawnSound => SoundID.Item8;

		public abstract int DustType { get; }

		public abstract Color Color { get; }

		public abstract int BuffType { get; }

		public override LocalizedText DisplayName => CommonDisplayNameText;

		private bool spawned = false;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;

			CommonDisplayNameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(GoblinUnderlingWeaponOrb)}.DisplayName"));
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.AmethystBolt);
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 240 + 60;
			Projectile.alpha = 255;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parentSource
				&& parentSource.Entity is Projectile parentProjectile
				&& GoblinUnderlingTierSystem.GoblinUnderlingProjs.ContainsKey(parentProjectile.type))
			{
				ownedGoblinWhoAmI = parentProjectile.whoAmI;

				ChargeDuration = GoblinUnderlingTierSystem.GetCurrentTierStats(GoblinUnderlingClass.Magic).attackInterval * EagerUnderlingProj.WeaponFrameCount;
				OrigSpeed = Projectile.velocity.Length();
			}
		}

		public virtual void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{

		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (FromGoblin && Main.projectile[ownedGoblinWhoAmI] is Projectile parent && parent.ModProjectile is EagerUnderlingProj goblin)
			{
				GoblinUnderlingHelperSystem.CommonModifyHitNPC(GoblinUnderlingClass.Magic, Projectile, target, ref modifiers);
			}

			if (NPCID.Sets.ImmuneToRegularBuffs[target.type])
			{
				//Since buffs are a significant part of the dps, buff the damage directly if buffs can't be applied
				modifiers.SourceDamage += 0.15f;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (target.boss || !FromGoblin || Main.projectile[ownedGoblinWhoAmI] is not Projectile parent || parent.ModProjectile is not EagerUnderlingProj goblin)
			{
				return;
			}

			if (goblin.OutOfCombat())
			{
				ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnAttacking(parent, target, hit, damageDone);
			}

			goblin.SetInCombat();
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color * Projectile.Opacity, 0.4f) * 0.6f;
		}

		public override bool ShouldUpdatePosition()
		{
			//Don't move from velocity
			return ChargeDuration <= 0;
		}

		public sealed override void AI()
		{
			if (!spawned)
			{
				spawned = true;

				SoundEngine.PlaySound(SpawnSound, Projectile.Center);
			}

			if (ChargeUp)
			{
				if (MaxChargeDuration == 1)
				{
					MaxChargeDuration = (int)ChargeDuration;
				}
				ChargeDuration--;

				if (FromGoblin && Main.projectile[ownedGoblinWhoAmI] is Projectile parent && parent.ModProjectile is EagerUnderlingProj goblin)
				{
					Vector2 pos = parent.Center + GoblinUnderlingTierSystem.GetCurrentTierStats(GoblinUnderlingClass.Magic).projOffset;
					Vector2 extraSizeBecauseThisMethodIsStupid = Vector2.One * 8;
					if (!Collision.SolidCollision(pos - Projectile.Size / 2 - extraSizeBecauseThisMethodIsStupid / 2,
						Projectile.width + (int)extraSizeBecauseThisMethodIsStupid.X,
						Projectile.height + (int)extraSizeBecauseThisMethodIsStupid.Y))
					{
						Projectile.Center = pos;
					}
					else
					{
						Projectile.Center = parent.Center;
					}
				}
			}
			if (ChargeDuration == 0)
			{
				//Launch
				ChargeDuration = -1;
				int targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 1200, ignoreTiles: true);
				if (targetIndex != -1)
				{
					NPC npc = Main.npc[targetIndex];
					Vector2 velocity = npc.Center + npc.velocity * 5f - Projectile.Center;
					velocity.Normalize();
					Projectile.velocity = velocity;

					Launch();
				}
				else
				{
					Projectile.active = false;
					Projectile.netUpdate = true;
				}
			}

			float ratio = 1f - (1 + ChargeDuration) / MaxChargeDuration;
			if (Homing)
			{
				ratio = 1f;
				int targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 1200, ignoreTiles: true);
				if (targetIndex != -1)
				{
					NPC npc = Main.npc[targetIndex];
					Vector2 velocity = npc.Center + npc.velocity * 5f - Projectile.Center;
					velocity.Normalize();
					velocity *= OrigSpeed;
					float accel = Utils.Clamp(20 - OrigSpeed, 2, 20); //Better curving with more speed
					Projectile.velocity = (Projectile.velocity * (accel - 1) + velocity) / accel;
				}
			}

			Projectile.rotation -= 0.02f + ratio * 0.1f;
			Projectile.Opacity = Math.Clamp(ratio * ratio * ratio, 0f, 1f); //Slow ramp up

			SafeAI();
		}

		public virtual void SafeAI()
		{
			if (Homing)
			{
				for (int j = 0; j < 6; j++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
					dust.velocity *= 1.5f;
					dust.noGravity = true;
					dust.scale = 0.8f;
					ModifyDust(dust, fromHoming: true);
				}
			}
			else if (ChargeUp)
			{
				//Four points spawning dust
				float invRatio = (1 + ChargeDuration) / MaxChargeDuration;
				if (invRatio < 0.2f)
				{
					return;
				}

				for (int i = 0; i < 4; i++)
				{
					float rot = MathHelper.PiOver2 * i + 4 * invRatio; //3 full spins
					for (int j = 1; j < 2; j++)
					{
						Vector2 pos = Projectile.Center + new Vector2(invRatio * 10 + j, (1f - invRatio) * 2 * j).RotatedBy(rot);
						Dust dust = Dust.NewDustDirect(pos, 1, 1, DustType);
						dust.position -= new Vector2(4);
						dust.noGravity = true;
						dust.scale = 0.9f;
						ModifyDust(dust, fromChargeUp: true);
					}
				}
			}
		}

		public virtual void Launch()
		{
			for (int j = 0; j < 20; j++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType);
				dust.velocity *= 2;
				dust.velocity += 4 * Projectile.velocity;
				dust.noGravity = true;
				dust.scale = 0.8f;
				ModifyDust(dust, fromLaunch: true);
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (Main.myPlayer != Projectile.owner || !Homing)
			{
				return;
			}

			int duration = DefaultBuffDuration;
			int radius = DefaultBuffRadius;
			if (GoblinUnderlingTierSystem.GetCurrentTierStats(GoblinUnderlingClass.Magic) is GoblinUnderlingMagicTierStats magicTier)
			{
				duration = magicTier.buffDuration;
				radius = magicTier.radius;
			}

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];

				if (!(npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy))
				{
					continue;
				}

				if (npc.DistanceSQ(Projectile.Center) < radius * radius)
				{
					npc.AddBuff(BuffType, duration);
				}
			}
		}
	}

	public class GoblinUnderlingWeaponOrb_0 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(253, 221, 2);

		public override int BuffType => BuffID.OnFire;

		public override int DustType => DustID.Torch;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{

		}
	}

	public class GoblinUnderlingWeaponOrb_1 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(161, 255, 253);

		public override int BuffType => BuffID.Frostburn;

		public override int DustType => DustID.IceTorch;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{

		}
	}

	public class GoblinUnderlingWeaponOrb_2 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(202, 151, 255);

		public override int BuffType => BuffID.ShadowFlame;

		public override int DustType => DustID.GoblinSorcerer;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{
			dust.velocity *= 0.25f;
		}
	}

	public class GoblinUnderlingWeaponOrb_3 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(253, 221, 2);

		public override int BuffType => BuffID.OnFire3;

		public override int DustType => DustID.Torch;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{

		}
	}

	public class GoblinUnderlingWeaponOrb_4 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(161, 255, 253);

		public override int BuffType => BuffID.Frostburn2;

		public override int DustType => DustID.IceTorch;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{

		}
	}

	public class GoblinUnderlingWeaponOrb_5 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(218, 253, 9);

		public override int BuffType => BuffID.CursedInferno;

		public override int DustType => DustID.CursedTorch;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{

		}
	}

	public class GoblinUnderlingWeaponOrb_6 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(66, 69, 84);

		public override int BuffType => BuffID.Oiled;

		public override int DustType => DustID.TintableDust;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{
			if (Main.rand.Next(2) == 0)
			{
				dust.alpha += 25;
			}

			if (Main.rand.Next(2) == 0)
			{
				dust.alpha += 25;
			}

			dust.color = new Color(0, 0, 0, 250);
			dust.noLight = true;
			//dust.scale *= 1.4f;
			dust.velocity *= 0.25f;
		}
	}
}
