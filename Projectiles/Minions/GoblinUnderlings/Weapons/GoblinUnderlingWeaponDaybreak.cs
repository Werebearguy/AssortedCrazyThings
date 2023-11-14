using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Weapons
{
	//Weaker version of the vanilla daybreak without the stacking dot and debuff on hit/explosion
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingWeaponDaybreak : AssProjectile
	{
		public const float Gravity = 0.1f;
		public const int TicksWithoutGravity = 15;

		public bool Spawned
		{
			get => Projectile.localAI[0] == 1f;
			set => Projectile.localAI[0] = value ? 1 : 0;
		}

		//Reuse TargetWhoAmI field, they are used in different modes based on IsStickingToTarget
		public int AttachedTimer
		{
			get => (int)Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}

		public int Timer
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public float IsStickingToTarget
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public float TargetWhoAmI
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public int ownedGoblinWhoAmI = -1;

		public bool FromGoblin => ownedGoblinWhoAmI != -1;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.alpha = 255;
			Projectile.aiStyle = -1;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.penetrate = 2;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 240;
		}

		public override void AI()
		{
			if (!Spawned)
			{
				Spawned = true;

				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
			}

			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 50;

				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			if (Projectile.ai[0] == 0f)
			{
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
			else if (Projectile.ai[0] == 1f)
			{
				Projectile.ignoreWater = true;
				Projectile.tileCollide = false;
				int aiFactor = 10;
				bool killProj = false; // if true, kill projectile at the end
				AttachedTimer += 1;
				int projTargetIndex = (int)Projectile.ai[1];
				if (AttachedTimer >= 60 * aiFactor)
				{
					killProj = true;
				}
				else if (projTargetIndex < 0 || projTargetIndex >= 200)
				{
					killProj = true;
				}
				else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
				{
					Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
				}
				else
				{
					killProj = true;
				}

				if (killProj)
				{
					Projectile.Kill();
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GoblinUnderlingWeaponDaybreakExplosion>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			}
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
				GoblinUnderlingHelperSystem.CommonModifyHitNPC(GoblinUnderlingClass.Melee, Projectile, target, ref modifiers);
			}

			Projectile.ai[0] = 1f;
			Projectile.ai[1] = target.whoAmI;
			Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
			Projectile.netUpdate = true;
			Projectile.friendly = false;
			int maxStickingJavelins = 5;
			Point[] stickingJavelins = new Point[maxStickingJavelins];
			int javelinIndex = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != Projectile.whoAmI && currentProjectile.active && currentProjectile.owner == Main.myPlayer && currentProjectile.type == Projectile.type
					&& currentProjectile.ai[0] == 1f && currentProjectile.ai[1] == target.whoAmI)
				{
					stickingJavelins[javelinIndex++] = new Point(i, currentProjectile.timeLeft);
				}
			}

			// KillOldestJavelin will kill the oldest projectile stuck to the specified npc.
			// It only works if ai[0] is 1 when sticking and ai[1] is the target npc index, which is what IsStickingToTarget and TargetWhoAmI correspond to.
			Projectile.KillOldestJavelin(Projectile.whoAmI, Type, target.whoAmI, stickingJavelins);
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
			return new Color(255, 255, 255, 100) * Projectile.Opacity;
		}
	}
}
