using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class GhostMartianProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<GhostMartianBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 22;
			Projectile.height = 42;
			Projectile.alpha = 70;
		}

		private const int sincounterMax = 130;
		private int sincounter;

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = TextureAssets.Projectile[Type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			float sinY = (float)((Math.Sin(((float)sincounter / sincounterMax) * MathHelper.TwoPi) - 1) * 2);

			Vector2 stupidOffset = new Vector2(image.Width / 2, Projectile.height / 2 + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;

			Main.EntitySpriteDraw(image, drawPos, bounds, Projectile.GetAlpha(lightColor), 0f, bounds.Size() / 2, 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}

		public override bool PreAI()
		{
			AmuletOfManyMinionsApi.ReleaseControl(this);

			return base.PreAI();
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();

			sincounter = sincounter > sincounterMax ? 0 : sincounter + 1;

			if (player.dead)
			{
				modPlayer.GhostMartian = false;
			}
			if (modPlayer.GhostMartian)
			{
				Projectile.timeLeft = 2;

				if (AmuletOfManyMinionsApi.IsActive(this))
				{
					AoMM_AI();
				}

				AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, offsetX: -6f, offsetY: 6f);

				Projectile.spriteDirection = (player.Center.X <= Projectile.Center.X).ToDirectionInt();

				AssAI.FlickerwickPetDraw(Projectile, frameCounterMaxFar: 4, frameCounterMaxClose: 10);
			}
		}

		private bool atleastOneEnemyInAttackRange = false; //Acts as a spawn/despawn handler

		private int attackRangeScanTimer = 0;
		private int attackRangeScanTimerMax = 60;

		private void AoMM_AI()
		{
			attackRangeScanTimer++;
			if (attackRangeScanTimer >= attackRangeScanTimerMax)
			{
				attackRangeScanTimer = 0;
				if (!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state))
				{
					return;
				}

				bool oldAtleastOneEnemyInAttackRange = atleastOneEnemyInAttackRange;
				atleastOneEnemyInAttackRange = false;
				int radius = 50 + state.SearchRange;

				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < radius * radius)
					{
						atleastOneEnemyInAttackRange = true;
						break;
					}
				}

				if (Main.myPlayer == Projectile.owner)
				{
					int shotType = ModContent.ProjectileType<GhostMartianShotProj>();
					Player player = Projectile.GetOwner();

					if (oldAtleastOneEnemyInAttackRange && !atleastOneEnemyInAttackRange)
					{
						//Turn inactive so it despawns automatically later
						for (int i = 0; i < Main.maxProjectiles; i++)
						{
							Projectile proj = Main.projectile[i];

							if (proj.active && proj.owner == Projectile.owner && proj.type == shotType && proj.ModProjectile is GhostMartianShotProj shotProj && !shotProj.Retreat)
							{
								shotProj.Retreat = true;
								proj.NetSync();
							}
						}
					}

					if (atleastOneEnemyInAttackRange)
					{
						if (player.ownedProjectileCounts[shotType] == 0)
						{
							//Spawn new one if not spawned yet
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center + new Vector2(Main.rand.NextBool().ToDirectionInt() * (200 + Main.rand.Next(300)), -150), Vector2.Zero, shotType, Projectile.damage, 0, Main.myPlayer);
						}
						else
						{
							//Turn active so it won't despawn
							for (int i = 0; i < Main.maxProjectiles; i++)
							{
								Projectile proj = Main.projectile[i];

								if (proj.active && proj.owner == Projectile.owner && proj.type == shotType && proj.ModProjectile is GhostMartianShotProj shotProj && shotProj.Retreat)
								{
									shotProj.Retreat = false;
									proj.NetSync();
								}
							}
						}
					}
				}
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.OtherPets)]
	public class GhostMartianShotProj : AssProjectile
	{
		public int ParentIdentity
		{
			get => (int)Projectile.ai[0] - 1;
			set => Projectile.ai[0] = value + 1;
		}

		//Should be set only from outside of this class
		public bool Retreat
		{
			get => Projectile.ai[1] == 1f;
			set => Projectile.ai[1] = value ? 1f : 0f;
		}

		public int ParentIndex
		{
			get => (int)Projectile.localAI[0] - 1;
			set => Projectile.localAI[0] = value + 1;
		}

		public bool Spawned
		{
			get => Projectile.localAI[1] == 1f;
			set => Projectile.localAI[1] = value ? 1f : 0f;
		}

		public Vector2 AttackCircleCenter => Projectile.Bottom + new Vector2(0, attackRadius);

		public Rectangle AttackCircleHitbox => Utils.CenteredRectangle(AttackCircleCenter, new Vector2(2 * attackRadius));

		public bool hasTarget = false;

		public bool attacking = false;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 240; //Doesn't matter

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ScalingArmorPenetration += 1f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//Gets reset every tick, but in same tick, reduce subsequent damage
			Projectile.damage = (int)(Projectile.damage * 0.8f);
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = AttackCircleHitbox; //Just for visualization purposes
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!attacking)
			{
				return false;
			}

			//Vanilla adjusts its hitbox in AI, we don't do that, so some checks are omitted
			if (targetHitbox.Intersects(AttackCircleHitbox) && targetHitbox.Distance(AttackCircleCenter) < attackRadius - 8)
			{
				if (Projectile.AI_137_CanHit(targetHitbox.Center.ToVector2()))
				{
					return true;
				}

				if (Projectile.AI_137_CanHit(targetHitbox.TopLeft() + new Vector2(targetHitbox.Width / 2, 0f)))
				{
					return true;
				}
			}

			return false;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (source is not EntitySource_Parent parentSource)
			{
				return;
			}

			if (parentSource.Entity is not Projectile parent)
			{
				return;
			}

			ParentIdentity = parent.identity;
		}

		public override void AI()
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
				Projectile test = AssUtils.NetGetProjectile(Projectile.owner, ParentIdentity, ModContent.ProjectileType<GhostMartianProj>(), out int index);
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
			if (!parent.active || parent.type != ModContent.ProjectileType<GhostMartianProj>())
			{
				Projectile.Kill();
				return;
			}

			if (parent.ModProjectile is not GhostMartianProj ghostMartian)
			{
				return;
			}

			Visuals();

			if (!Spawned)
			{
				Spawned = true;
				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);

				for (int i = 0; i < 20; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric, Scale: 1);
					dust.noLight = true;
					dust.noGravity = true;
				}
			}

			Player player = Projectile.GetOwner();
			Projectile.damage = (int)(parent.originalDamage * 0.9f);
			Projectile.timeLeft = 2;
			//Not knockback

			if (!AmuletOfManyMinionsApi.TryGetStateDirect(ghostMartian, out var state))
			{
				return;
			}

			//Attack on its own, don't rely on targeted enemy, it may reset constantly due to LOS change
			int range = state.SearchRange;

			hasTarget = false;
			if (Retreat)
			{
				Retreat_AI(player);
			}
			else
			{
				retreatDir = 0;
				int targetIndex = AssAI.FindTarget(Projectile, player.Center, range);
				if (targetIndex == -1)
				{
					Idle_AI();
				}
				else
				{
					hasTarget = true;
					Attack_AI(targetIndex);
				}
			}

			Projectile.rotation = Projectile.velocity.X * 0.02f;
		}

		private int retreatDir = 0;

		public void Retreat_AI(Player player)
		{
			if (retreatDir == 0)
			{
				retreatDir = -Math.Sign(player.Center.X - Projectile.Center.X);
			}

			//Fly away from the player, then despawn after sufficient distance travelled
			Vector2 destination = player.Center + new Vector2(retreatDir * 1000, -450);
			Vector2 directionTo = destination - Projectile.Center;
			float distSQ = directionTo.LengthSquared();
			if (Main.myPlayer == Projectile.owner && distSQ < 30 * 30)
			{
				Projectile.Kill();
				return;
			}
			directionTo = directionTo.SafeNormalize(Vector2.UnitX * retreatDir);

			float inertia = 30;
			float speed = 12f;
			Projectile.velocity = (Projectile.velocity * (inertia - 1) + directionTo * speed) / inertia;

		}

		public void Idle_AI()
		{
			AssAI.ZephyrfishAI(Projectile);
		}

		public int attackRadius = 16 * 2;

		public void Attack_AI(int targetIndex)
		{
			NPC target = Main.npc[targetIndex];

			//Fly towards the top of the target
			Vector2 destination = target.Top + target.velocity * 3 - new Vector2(0, attackRadius);
			Vector2 directionTo = destination - Projectile.Center;
			float distSQ = directionTo.LengthSquared();
			directionTo = directionTo.SafeNormalize(Vector2.UnitX);

			float inertia = 10;
			float speed = 10f;
			Projectile.velocity = (Projectile.velocity * (inertia - 1) + directionTo * speed) / inertia;

			if (distSQ < 2 * 2)
			{
				Projectile.Center = destination;
				Projectile.velocity *= 0;
			}

			attacking = distSQ < attackRadius * attackRadius;

			if (!attacking)
			{
				return;
			}
			
			if (Projectile.soundDelay == 0)
			{
				SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Projectile.Center);

				Projectile.soundDelay = 10;
			}

			int visualRadius = attackRadius - 2;
			DelegateMethods.v3_1 = new Vector3(0.2f, 0.7f, 1f);
			Utils.PlotTileLine(AttackCircleCenter + Vector2.UnitX * -visualRadius, AttackCircleCenter + Vector2.UnitX * visualRadius, 2 * visualRadius, DelegateMethods.CastLightOpen);

			//Circular around center
			Vector2 top = new Vector2(AttackCircleCenter.X, AttackCircleCenter.Y - visualRadius);
			for (int j = 0; j < 8; j++)
			{
				if (!Main.rand.NextBool(6))
				{
					continue;
				}

				Vector2 random = Main.rand.NextVector2Unit();
				if (Math.Abs(random.X) < 0.05f)
				{
					continue;
				}

				Vector2 dustCenter = AttackCircleCenter + random * visualRadius;
				if (!WorldGen.SolidTile((int)dustCenter.X / 16, (int)dustCenter.Y / 16) && Projectile.AI_137_CanHit(dustCenter))
				{
					Dust dust = Dust.NewDustDirect(dustCenter, 0, 0, DustID.Electric, 0f, 0f, 100);
					dust.position = dustCenter;
					dust.velocity = (top - dust.position).SafeNormalize(Vector2.Zero);
					dust.scale = 0.5f;
					dust.fadeIn = 0.3f;
					dust.noGravity = true;
					dust.noLight = true;
				}
			}

			//Vertical line between center and top
			/*
			for (int l = 0; l < 4; l++)
			{
				if (!Main.rand.NextBool(10))
				{
					continue;
				}

				Dust dust = Dust.NewDustDirect(top - new Vector2(8f, 0f), 16, visualRadius, DustID.Electric, 0f, 0f, 100);
				dust.velocity *= 0.3f;
				dust.velocity += -Vector2.UnitY;
				dust.scale = 0.7f;
				dust.noGravity = true;
				dust.noLight = true;
			}
			*/
		}

		private void Visuals()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 20;

				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			int startFrame = hasTarget ? 4 : 0;
			int endFrame = hasTarget ? -1 : 3;
			Projectile.LoopAnimation(6, startFrame, endFrame);
		}
	}
}
