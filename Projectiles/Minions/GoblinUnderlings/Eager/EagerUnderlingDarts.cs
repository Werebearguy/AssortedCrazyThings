using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager
{
	[Content(ContentType.Weapons)]
	public abstract class EagerUnderlingDart : AssProjectile
	{
		public const float Gravity = 0.1f;
		public const int TicksWithoutGravity = 15;

		public static LocalizedText CommonDisplayNameText { get; private set; }

		public bool Spawned
		{
			get => Projectile.localAI[0] == 1f;
			set => Projectile.localAI[0] = value ? 1 : 0;
		}

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int ownedGoblinWhoAmI = -1;

		public bool FromGoblin => ownedGoblinWhoAmI != -1;

		public override LocalizedText DisplayName => CommonDisplayNameText;

		public override void SetStaticDefaults()
		{
			CommonDisplayNameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.{nameof(EagerUnderlingDart)}.DisplayName"));
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.aiStyle = -1;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 180;
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
			GoblinUnderlingHelperSystem.CommonModifyHitNPC(ModContent.ProjectileType<EagerUnderlingProj>(), Projectile, target, ref modifiers);
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

		public override void AI()
		{
			if (!Spawned)
			{
				Spawned = true;

				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Timer++;
			if (Timer >= TicksWithoutGravity)
			{
				Projectile.velocity.Y += Gravity;
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}

			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X <= 0).ToDirectionInt();
		}
	}

	public class EagerUnderlingDart_0 : EagerUnderlingDart
	{

	}

	public class EagerUnderlingDart_1 : EagerUnderlingDart
	{

	}

	public class EagerUnderlingDart_2 : EagerUnderlingDart
	{

	}

	public class EagerUnderlingDart_3 : EagerUnderlingDart
	{

	}

	public class EagerUnderlingDart_4 : EagerUnderlingDart
	{

	}
}