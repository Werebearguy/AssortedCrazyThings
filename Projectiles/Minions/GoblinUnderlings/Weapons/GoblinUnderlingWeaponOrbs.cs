using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
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

		protected int MaxChargeDuration = 1;
		protected ref float ChargeDuration => ref Projectile.ai[0];
		public bool Homing => ChargeDuration == -1;
		public bool ChargeUp => ChargeDuration > 0;
		protected ref float OrigSpeed => ref Projectile.ai[1];

		public int ParentIdentity
		{
			get => (int)Projectile.ai[2] - 1;
			set => Projectile.ai[2] = value + 1;
		}

		//Since the index might be different between clients, using ai[] for it will break stuff
		public int ParentIndex
		{
			get => (int)Projectile.localAI[0] - 1;
			set => Projectile.localAI[0] = value + 1;
		}

		public bool FromGoblin => ParentIndex != -1;

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
			Projectile.width = 16;
			Projectile.height = 16;
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
				ParentIdentity = parentProjectile.identity;

				ChargeDuration = GoblinUnderlingTierSystem.GetCurrentTierStats(GoblinUnderlingClass.Magic).attackInterval * GoblinUnderlingProj.WeaponFrameCount;
				OrigSpeed = Projectile.velocity.Length();
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (FromGoblin && Main.projectile[ParentIndex] is Projectile parent && parent.ModProjectile is GoblinUnderlingProj goblin)
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
			if (target.boss || !FromGoblin || Main.projectile[ParentIndex] is not Projectile parent || parent.ModProjectile is not GoblinUnderlingProj goblin)
			{
				return;
			}

			if (goblin.OutOfCombat())
			{
				ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnAttacking(parent, target, hit, damageDone);
			}

			goblin.SetInCombat();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			int count = 4;
			float ratio = 1f - (1 + ChargeDuration) / MaxChargeDuration;
			if (Homing)
			{
				ratio = 1f;
			}
			for (int i = 0; i < count; i++)
			{
				AssUtils.DrawLikeVanilla(Projectile, Projectile.GetAlpha(lightColor) * (1f / count) * 1.2f, offset: Vector2.UnitX.RotatedBy((MathHelper.TwoPi * i / count) + Projectile.rotation) * 3 * ratio);
			}

			return base.PreDraw(ref lightColor);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color * Projectile.Opacity * 0.5f;
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

				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				SoundEngine.PlaySound(SpawnSound, Projectile.Center);
			}

			if (ChargeUp)
			{
				if (ParentIdentity <= -1 || ParentIdentity > Main.maxProjectiles)
				{
					Projectile.Kill();
					return;
				}

				Projectile parent = null;
				if (ParentIndex <= -1)
				{
					//Find parent based on identity
					Projectile test = AssUtils.NetGetProjectile(Projectile.owner, ParentIdentity, GoblinUnderlingTierSystem.GoblinUnderlingProjs.Keys.ToArray(), out int index);
					if (test != null)
					{
						//Important not to use test.whoAmI here
						ParentIndex = index;
					}
				}

				if (ParentIndex > -1 && ParentIndex <= Main.maxProjectiles)
				{
					parent = Main.projectile[ParentIndex];
				}

				if (parent == null)
				{
					//If the parent was not found, despawn
					Projectile.Kill();
					return;
				}

				parent = Main.projectile[ParentIndex];
				if (!parent.active || !GoblinUnderlingTierSystem.GoblinUnderlingProjs.ContainsKey(parent.type))
				{
					Projectile.Kill();
					return;
				}

				if (MaxChargeDuration == 1)
				{
					MaxChargeDuration = (int)ChargeDuration;
				}
				ChargeDuration--;

				if (FromGoblin)
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

			Projectile.rotation -= 0.1f + ratio * 0.3f;
			Projectile.Opacity = Math.Clamp(ratio * ratio, 0f, 1f); //Slow ramp up

			SafeAI();
		}

		public virtual void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{

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
	}

	public class GoblinUnderlingWeaponOrb_1 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(161, 255, 253);

		public override int BuffType => BuffID.Frostburn;

		public override int DustType => DustID.IceTorch;
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
	}

	public class GoblinUnderlingWeaponOrb_4 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(161, 255, 253);

		public override int BuffType => BuffID.Frostburn2;

		public override int DustType => DustID.IceTorch;
	}

	public class GoblinUnderlingWeaponOrb_5 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(218, 253, 9);

		public override int BuffType => BuffID.CursedInferno;

		public override int DustType => DustID.CursedTorch;
	}

	public class GoblinUnderlingWeaponOrb_6 : GoblinUnderlingWeaponOrb
	{
		public override Color Color => new Color(66, 69, 84);

		public override int BuffType => BuffID.Oiled;

		public override int DustType => DustID.TintableDust;

		public override void ModifyDust(Dust dust, bool fromChargeUp = false, bool fromLaunch = false, bool fromHoming = false)
		{
			if (Main.rand.NextBool())
			{
				dust.alpha += 25;
			}

			if (Main.rand.NextBool())
			{
				dust.alpha += 25;
			}

			dust.color = new Color(0, 0, 0, 250);
			dust.noLight = true;
			dust.velocity *= 0.25f;
		}
	}
}
