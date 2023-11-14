using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Weapons
{
	[Content(ContentType.Weapons)]
	public abstract class GoblinUnderlingWeaponArrow : MinionShotProj
	{
		public const float Gravity = 0.1f;
		public const int TicksWithoutGravity = 15;

		public static LocalizedText CommonDisplayNameText { get; private set; }

		public int ownedGoblinWhoAmI = -1;

		public bool FromGoblin => ownedGoblinWhoAmI != -1;

		public override LocalizedText DisplayName => CommonDisplayNameText;

		public override void SafeSetStaticDefaults()
		{
			CommonDisplayNameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(GoblinUnderlingWeaponArrow)}.DisplayName"));
		}

		public override void SafeSetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.arrow = false; //Does not matter for minion shots

			//Multiple of the same arrow won't stack dps
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parentSource
				&& parentSource.Entity is Projectile parentProjectile
				&& GoblinUnderlingTierSystem.GoblinUnderlingProjs.ContainsKey(parentProjectile.type))
			{
				ownedGoblinWhoAmI = parentProjectile.whoAmI;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (FromGoblin && Main.projectile[ownedGoblinWhoAmI] is Projectile parent && parent.ModProjectile is EagerUnderlingProj goblin)
			{
				GoblinUnderlingHelperSystem.CommonModifyHitNPC(GoblinUnderlingClass.Ranged, Projectile, target, ref modifiers);
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

		public override void SafeAI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;

				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}
		}
	}

	public class GoblinUnderlingWeaponArrow_0 : GoblinUnderlingWeaponArrow
	{
		public override int ClonedType => ProjectileID.WoodenArrowFriendly;

		public override SoundStyle? SpawnSound => ContentSamples.ItemsByType[ItemID.CopperBow].UseSound;
	}

	public class GoblinUnderlingWeaponArrow_1 : GoblinUnderlingWeaponArrow
	{
		public override int ClonedType => ProjectileID.FireArrow;

		public override SoundStyle? SpawnSound => ContentSamples.ItemsByType[ItemID.TungstenBow].UseSound;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);

			if (Main.rand.NextBool(3))
			{
				target.AddBuff(BuffID.OnFire, 180);
			}
		}

		public override void SafeAI()
		{
			base.SafeAI();

			//For some godforsaken reason some arrow visuals are not in AI(), but after
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100);
		}
	}

	public class GoblinUnderlingWeaponArrow_2 : GoblinUnderlingWeaponArrow
	{
		public override int ClonedType => ProjectileID.JestersArrow;

		//No fossil bow
		public override SoundStyle? SpawnSound => ContentSamples.ItemsByType[ItemID.TungstenBow].UseSound;

		public override void SafeAI()
		{
			base.SafeAI();

			if (Main.rand.NextBool()) //Check added extra
			{
				var type = Main.rand.Next(3) switch
				{
					0 => 15,
					1 => 57,
					_ => 58,
				};
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default(Color), 1.2f);
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 0) * Projectile.Opacity;
		}
	}

	public class GoblinUnderlingWeaponArrow_3 : GoblinUnderlingWeaponArrow
	{
		public override int ClonedType => ProjectileID.UnholyArrow;

		public override SoundStyle? SpawnSound => ContentSamples.ItemsByType[ItemID.DemonBow].UseSound;

		public override void SafeAI()
		{
			base.SafeAI();

			//For some godforsaken reason some arrow visuals are not in AI(), but after
			if (Main.rand.NextBool(5))
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 14, 0f, 0f, 150, default(Color), 1.1f);
			}
		}
	}

	public class GoblinUnderlingWeaponArrow_4 : GoblinUnderlingWeaponArrow
	{
		public override int ClonedType => ProjectileID.HellfireArrow;

		public override SoundStyle? SpawnSound => ContentSamples.ItemsByType[ItemID.DaedalusStormbow].UseSound;

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			//Disable it because it breaks the explosion
			Projectile.usesIDStaticNPCImmunity = false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);

			//Enables explosion
			if (Projectile.timeLeft > 1)
			{
				Projectile.timeLeft = 1;
			}
		}
	}

	public class GoblinUnderlingWeaponArrow_5 : GoblinUnderlingWeaponArrow
	{
		public new const float Gravity = 0.07f;
		public new const int TicksWithoutGravity = 20;

		public override int ClonedType => ProjectileID.HolyArrow;

		//No palladium bow
		public override SoundStyle? SpawnSound => ContentSamples.ItemsByType[ItemID.DaedalusStormbow].UseSound;

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, 0) * Projectile.Opacity;
		}
	}
}
