using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Base.Handlers.SpawnedNPCHandler;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager
{
	//TODO goblin recheck all references and point to base class if needed
	[Content(ContentType.Weapons)]
	public class EagerUnderlingProj : AssProjectile
	{
		public const float Gravity = 0.4f;

		public const int StuckTimerMax = 3 * 60;
		public int stuckTimer = 0;

		public int afkTimer = 0;

		public int InCombatTimerMax = 5 * 60;
		public int inCombatTimer = 0;

		private bool skipDefaultMovement = false;
		private int oldAttackTarget = -1;

		public const int WeaponFrameCount = 4;

		public const string AssetPrefix = "AssortedCrazyThings/Projectiles/Minions/GoblinUnderlings/Eager/Melee";

		public override string Texture => AssetPrefix + "_0";

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 20;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

			GoblinUnderlingAssetsSystem.RegisterAssetPrefix(Projectile.type, AssetPrefix);
			GoblinUnderlingTierSystem.GoblinUnderlingProjs[Projectile.type] = GoblinUnderlingChatterType.Eager;

			var tierStats = new Dictionary<GoblinUnderlingProgressionTierStage, GoblinUnderlingTierStats>
			{
				//PreBoss = Baseline values in Item/AI code																								  //dmg    kb    ap  sp     m  hb  ran   ransp
				{ GoblinUnderlingProgressionTierStage.PreBoss   , new GoblinUnderlingTierStats(ModContent.ProjectileType<EagerUnderlingDart_0>()          , 1f   , 1f  , 0 , 0.3f , 6, 0 , 1.5f, 8f , 1f) },
				{ GoblinUnderlingProgressionTierStage.EoC       , new GoblinUnderlingTierStats(ModContent.ProjectileType<EagerUnderlingDart_1>()          , 1.50f, 1.2f, 0 , 0.35f, 6, 2 , 1.5f, 9f , 1.2f) },
				{ GoblinUnderlingProgressionTierStage.Evil      , new GoblinUnderlingTierStats(ModContent.ProjectileType<EagerUnderlingDart_2>()          , 1.70f, 1.4f, 5 , 0.4f , 6, 4 , 1.5f, 10f, 1.3f) },
				{ GoblinUnderlingProgressionTierStage.Skeletron , new GoblinUnderlingTierStats(ModContent.ProjectileType<EagerUnderlingDart_3>()          , 1.75f, 1.6f, 5 , 0.45f, 5, 6 , 1.5f, 11f, 1.4f) },
				{ GoblinUnderlingProgressionTierStage.WoF       , new GoblinUnderlingTierStats(ModContent.ProjectileType<EagerUnderlingDart_3>()          , 2.2f , 1.6f, 10, 0.45f, 5, 6 , 1.5f, 11f, 1.6f) }, //Mostly a copy of previous tier with more damage, same visuals too
				{ GoblinUnderlingProgressionTierStage.Mech      , new GoblinUnderlingTierStats(ModContent.ProjectileType<EagerUnderlingDart_4>()          , 2.8f , 1.8f, 10, 0.6f , 5, 6 , 1.5f, 12f, 1.7f) },
				{ GoblinUnderlingProgressionTierStage.Plantera  , new GoblinUnderlingTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponTerraBeam>(), 3.0f , 2f  , 10, 0.7f , 4, 10, 1f  , 14f, 2f , true) },
				{ GoblinUnderlingProgressionTierStage.Cultist   , new GoblinUnderlingTierStats(ModContent.ProjectileType<WeaponSwordDaybreak>()           , 3.25f, 2.2f, 10, 0.8f , 4, 10, 1f  , 16f, 2f , false, true) },
			};
			GoblinUnderlingTierSystem.RegisterStats(Projectile.type, tierStats);
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 40;

			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minion = true;
			Projectile.minionSlots = 1;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			Projectile.decidesManualFallThrough = true;
			Projectile.manualDirectionChange = true;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 18; //Facilitates attack 5 * 4 duration (default pirates), dynamic in AI
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//Custom draw to just center on the hitbox
			var tierStats = GoblinUnderlingTierSystem.GetCurrentTierStats(Type);
			int texIndex = GoblinUnderlingTierSystem.CurrentTierIndex;
			Texture2D texture;
			var bodyAssets = GoblinUnderlingAssetsSystem.BodyAssets[Type];
			if (Main.myPlayer == Projectile.owner && !ClientConfig.Instance.SatchelofGoodiesVisibleArmor)
			{
				texture = bodyAssets[0].Value;
			}
			else
			{
				texture = bodyAssets[texIndex].Value;
			}
			Rectangle sourceRect = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Vector2 drawOrigin = sourceRect.Size() / 2f;

			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Vector2 drawPos = Projectile.position + Projectile.Size / 2f + new Vector2(0, Projectile.gfxOffY + 4f - 2f) - Main.screenPosition;
			Color color = lightColor;

			float rotation = Projectile.rotation;
			float scale = Projectile.scale;
			Main.EntitySpriteDraw(texture, drawPos, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);

			if (MeleeAttacking || RangedAttacking && tierStats.showMeleeDuringRanged)
			{
				var asset = GoblinUnderlingAssetsSystem.GetWeaponAsset(GoblinUnderlingWeaponType.Sword, texIndex);
				if (asset != null)
				{
					texture = asset.Value;
					sourceRect = texture.Frame(1, WeaponFrameCount, 0, AttackFrameNumber);
					drawOrigin = sourceRect.Size() / 2f;
					Main.EntitySpriteDraw(texture, drawPos, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);
				}
			}

			return false;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			//Save original coordinates
			int centerX = hitbox.Center.X;
			int bottomY = hitbox.Bottom;

			int increase = GoblinUnderlingTierSystem.GetCurrentTierStats(Type).meleeAttackHitboxIncrease;
			hitbox.Inflate(increase, increase / 2); //Top shouldn't grow as much, as its only going to grow upwards

			//Restore coordinates
			hitbox.Y = bottomY - hitbox.Height;
			hitbox.X = centerX - hitbox.Width / 2;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			GoblinUnderlingHelperSystem.CommonModifyHitNPC(Type, Projectile, target, ref modifiers);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (!target.boss && OutOfCombat())
			{
				ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnAttacking(Projectile, target, hit, damageDone);
			}

			SetInCombat();
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public int MovementState
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int AttackState
		{
			get => (int)Projectile.ai[2];
			set => Projectile.ai[2] = value;
		}

		public int Timer
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public bool IdleOrMoving => MovementState == 0;

		public bool Flying
		{
			get => MovementState == 1;
			set => MovementState = value ? 1 : 0;
		}

		public bool MeleeAttacking
		{
			get => AttackState == 1;
			set => AttackState = value ? 1 : 0;
		}

		public bool RangedAttacking
		{
			get => AttackState == 2;
			set => AttackState = value ? 2 : 0;
		}

		public bool GeneralAttacking
		{
			get => AttackState > 0;
			set
			{
				if (!value)
				{
					//Only allow false to disable attack
					AttackState = 0;
				}
			}
		}

		public int AttackFrameNumber
		{
			get => (int)Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			EagerUnderlingPlayer modPlayer = player.GetModPlayer<EagerUnderlingPlayer>();
			if (player.dead)
			{
				modPlayer.hasMinion = false;
			}
			if (modPlayer.hasMinion)
			{
				Projectile.timeLeft = 2;
			}

			//Has to be in PreAI so that damage works in AI properly
			SetScaledDamage(player);

			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();

			SetFrame(); //Has to be before due to velocity checks and attacking overriding frame

			UnderlingAI(player, out Vector2 idleLocation);

			DoIdleMessage(player, idleLocation);

			HandleCombatState();

			Projectile.localNPCHitCooldown = GetNextTimerValue(WeaponFrameCount) - 2;
		}

		public void SetInCombat()
		{
			inCombatTimer = InCombatTimerMax;

			ModContent.GetInstance<GoblinUnderlingChatterHandler>().PutIdleOnCooldown(Projectile);
		}

		public bool OutOfCombat()
		{
			return inCombatTimer == 0;
		}

		private void HandleCombatState()
		{
			if (inCombatTimer > 0)
			{
				inCombatTimer--;
			}
		}

		private void SetScaledDamage(Player player)
		{
			var tier = GoblinUnderlingTierSystem.GetCurrentTierStats(Type);

			//Copied scaling from vanilla, but summoner is adjusted by our scaling
			int originalDamage = Projectile.originalDamage;
			StatModifier summoner = player.GetDamage(DamageClass.Summon);

			StatModifier tieredSummoner = summoner.CombineWith(new StatModifier(1f, tier.damageMult)); //Modification

			StatModifier allDamage = player.GetDamage(DamageClass.Generic);

			Projectile.damage = (int)allDamage.CombineWith(tieredSummoner).ApplyTo(originalDamage);
		}

		private Vector2 GetIdleLocation(Player player)
		{
			//Projectile will align on the right side of the default location
			Vector2 defaultLocation = player.Center;
			int distIdle = 48;
			int offset = Projectile.width / 2;
			defaultLocation.X -= (player.width / 2) * player.direction;
			//defaultLocation.X -= (Projectile.minionPos * (Projectile.width + 6)) * player.direction;
			//TODO like PetPlayer.numSlimePets
			//minionPos calculations not necessary because you can't summon more than 1. Instead, use custom minionPos only for underlings
			if (player.direction == 1)
			{
				defaultLocation.X -= distIdle;
			}
			else if (player.direction == -1)
			{
				defaultLocation.X += distIdle + offset;
			}

			//Check the tiles between default and player center, starting from default, for walkable tiles 5 tiles down
			//If any are found, change defaultLocation.X to that coordinate, and return that. Otherwise, set location to the player center aswell

			//dir ==  1 -> g___p>
			//dir == -1 -> <p___g

			int startX = (int)defaultLocation.X / 16;
			int startY = (int)player.Bottom.Y / 16; //Tile player is standing on
			int dir = player.direction;
			int diff = Math.Abs(startX - (int)player.Center.X / 16);
			int endX = startX + dir * (diff + 1); //+1 for the tile player might be standing on near the edge
			int endY = startY + 5;

			//int scanX = 0;
			for (int x = startX; (dir == 1 && x >= startX && x <= endX) || (dir == -1 && x >= endX && x <= startX); x += dir)
			{
				for (int y = startY; y <= endY; y++)
				{
					//If atleast one of the tiles below the position are active and walkable
					if (WorldGen.InWorld(x, y) && (WorldGen.ActiveAndWalkableTile(x, y) || Main.tileSolidTop[Main.tile[x, y].TileType]))
					{
						defaultLocation.X = x * 16 + 8;
						if (dir == 1)
						{
							defaultLocation.X += offset; //Quirk with how it always wants to align "right of" the location
						}
						return defaultLocation;
					}
				}
				//scanX++;
			}

			return player.Center;
		}

		private void DoIdleMessage(Player player, Vector2 idleLocation)
		{
			if (Main.myPlayer != player.whoAmI)
			{
				return;
			}

			var modPlayer = player.GetModPlayer<EagerUnderlingPlayer>();
			if (modPlayer.firstSummon)
			{
				if (ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnFirstSummon(Projectile))
				{
					modPlayer.firstSummon = false;

					return;
				}
			}

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];

				if (npc.active && (npc.type == NPCID.EaterofWorldsHead || SpawnedNPCSystem.IsABoss(npc)) && npc.DistanceSQ(player.Center) < 1280 * 1280)
				{
					return;
				}
			}

			if (Projectile.velocity.X == 0f && Projectile.oldVelocity.X == 0f &&
				Projectile.oldVelocity.Y == 0f)
			{
				afkTimer++;
			}
			else if (afkTimer > 0)
			{
				afkTimer -= 2;
			}

			if (!(!GeneralAttacking && !Flying &&
				afkTimer > 4 * 60 &&
				player.afkCounter > 4 * 60 && Math.Abs(idleLocation.X - Projectile.Center.X) < 50 && Math.Abs(idleLocation.Y - Projectile.Center.Y) < 32))
			{
				return;
			}

			ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnIdle(Projectile);
		}

		private int GetNextTimerValue(int attackFrameCount)
		{
			var tier = GoblinUnderlingTierSystem.GetCurrentTierStats(Type);
			int time = tier.meleeAttackInterval;
			if (RangedAttacking)
			{
				float ranged = tier.rangedAttackIntervalMultiplier * time * attackFrameCount;
				return (int)ranged;
			}
			else
			{
				//Use the faster variant as fallback
				return time * attackFrameCount;
			}
		}

		private static bool CustomEliminationCheck_Pirates(Entity otherEntity, int currentTarget) => true;

		private void UnderlingAI(Player player, out Vector2 idleLocation)
		{
			skipDefaultMovement = false;
			var tier = GoblinUnderlingTierSystem.GetCurrentTierStats(Type);

			//if target is outside of meleeAttackRange (but inside globalAttackRange), minion stops moving horizontally
			//on the edge of the meleeAttackRange, then initiates attacking behavior but with ranged projectiles instead of melee
			int meleeAttackRange = 400; //25 * 16 = 400
			int rangedAttackRangeFromProj = (int)(256 * tier.rangedRangeMultiplier); //16 * 16
			float awayDistMax = 500f;
			float awayDistYMax = 400f; //300, increased to reduce amount of "bouncing" when player is standing on far up tiles or hooked up
			Vector2 destination = GetIdleLocation(player);
			idleLocation = destination;

			if (Timer == 0 && Projectile.HandleStuck(destination.X, ref stuckTimer, StuckTimerMax))
			{
				Flying = true;
				Projectile.tileCollide = false;
			}

			Projectile.shouldFallThrough = player.Bottom.Y - 12f > Projectile.Bottom.Y;
			Projectile.friendly = false;
			int attackCooldown = 0;
			int attackFrameCount = WeaponFrameCount;
			int nextTimerValue = GetNextTimerValue(attackFrameCount);

			int attackTarget = -1;

			bool checkBosses = false;
			rangedAttackRangeFromProj *= 2;
			int globalAttackRange = meleeAttackRange + rangedAttackRangeFromProj + Projectile.width; //800, calc same as below
			Projectile.Minion_FindTargetInRange(globalAttackRange, ref attackTarget, skipIfCannotHitWithOwnBody: true, CustomEliminationCheck_Pirates);
			if (attackTarget > -1)
			{
				if (Main.npc[attackTarget].boss)
				{
					checkBosses = true;
				}
			}

			if (!checkBosses)
			{
				rangedAttackRangeFromProj /= 2;
				globalAttackRange = meleeAttackRange + rangedAttackRangeFromProj + Projectile.width;
			}

			if (attackTarget > -1)
			{
				PickDestinationAndAttack(tier, meleeAttackRange, rangedAttackRangeFromProj, attackFrameCount, attackTarget, globalAttackRange, out destination);

				//Do a little jump when it goes from no target to target
				if (oldAttackTarget == -1 && IdleOrMoving && Projectile.velocity.Y == 0)
				{
					Projectile.velocity.Y -= 4;
				}
			}

			GoFlyingPrematurely(player, awayDistMax, awayDistYMax, attackTarget);

			if (GeneralAttacking && Timer < 0)
			{
				Projectile.friendly = false;
				if (AttackCoolup(nextTimerValue))
				{
					return;
				}
			}
			else if (GeneralAttacking)
			{
				AttackAction(tier, attackFrameCount, nextTimerValue, globalAttackRange);

				if (AttackCooldown(attackCooldown))
				{
					return;
				}
			}

			if (!skipDefaultMovement)
			{
				if (IdleOrMoving)
				{
					GroundedMovement(player, tier, awayDistMax, awayDistYMax, ref destination, attackTarget);
				}
				else if (Flying)
				{
					FlyingMovement(player);
				}
			}

			oldAttackTarget = attackTarget;
			Projectile.spriteDirection = -Projectile.direction;
		}

		private void AttackAction(GoblinUnderlingTierStats tier, int attackFrameCount, int nextTimerValue, int globalAttackRange)
		{
			AttackingAnimation(attackFrameCount, nextTimerValue);

			if (MeleeAttacking)
			{
				Flying = false;
				Projectile.friendly = true;
				int newAttackTarget = -1;
				Projectile.Minion_FindTargetInRange(globalAttackRange, ref newAttackTarget, skipIfCannotHitWithOwnBody: true, CustomEliminationCheck_Pirates);
				if (newAttackTarget != -1 && Timer > nextTimerValue * 0.8f)
				{
					if (Main.npc[newAttackTarget].Hitbox.Intersects(Projectile.Hitbox))
					{
						Projectile.velocity.X *= Projectile.velocity.Y == 0f ? 0.75f : 0.85f;
					}
				}
			}
			else if (RangedAttacking)
			{
				if (!Flying)
				{
					skipDefaultMovement = true;
					if (tier.rangedOnly)
					{
						//Don't slow down as fast when in ranged only mode
						Projectile.velocity.X *= Projectile.velocity.Y == 0f ? 0.85f : 0.9f;
					}
					else
					{
						Projectile.velocity.X *= Projectile.velocity.Y == 0f ? 0.75f : 0.85f;
					}

					//If it was jumping and then goes into ranged attacking it would start floating here
					Projectile.velocity.Y += Gravity;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}

				int newAttackTarget = -1;
				Projectile.Minion_FindTargetInRange(globalAttackRange, ref newAttackTarget, skipIfCannotHitWithOwnBody: true, CustomEliminationCheck_Pirates);

				if (newAttackTarget != -1)
				{
					NPC npc = Main.npc[newAttackTarget];
					Projectile.direction = (npc.Center.X - Projectile.Center.X >= 0f).ToDirectionInt();

					if (Main.myPlayer == Projectile.owner && Timer == (int)(nextTimerValue * 0.75f))
					{
						Vector2 position = Projectile.Center;
						Vector2 targetPos = npc.Center + npc.velocity * 0.6f;
						Vector2 vector = targetPos - position;
						float speed = tier.rangedVelocity;
						float mag = vector.Length();
						if (mag > speed)
						{
							mag = speed / mag;
							vector *= mag;
						}

						if (EagerUnderlingDart.IsDart.Contains(tier.rangedProjType))
						{
							AssUtils.ModifyVelocityForGravity(position, targetPos, EagerUnderlingDart.Gravity, ref vector, EagerUnderlingDart.TicksWithoutGravity);
						}

						Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, vector, tier.rangedProjType, Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
			}
		}

		private void PickDestinationAndAttack(GoblinUnderlingTierStats tier, int meleeAttackRange, int rangedAttackRangeFromProj, int attackFrameCount, int attackTarget, int globalAttackRange, out Vector2 destination)
		{
			NPC npc = Main.npc[attackTarget];
			destination = npc.Center;
			bool oldAttacking = GeneralAttacking;
			if (Projectile.IsInRangeOfMeOrMyOwner(npc, globalAttackRange, out float projDistance, out float playerDistance, out bool _))
			{
				DecideAttack(tier, meleeAttackRange, rangedAttackRangeFromProj, npc, projDistance, playerDistance, out bool allowJump);

				if (allowJump)
				{
					bool canJump = Projectile.velocity.Y == 0f;
					if (Projectile.wet && Projectile.velocity.Y > 0f && !Projectile.shouldFallThrough)
					{
						canJump = true;
					}

					if (npc.Center.Y < Projectile.Center.Y - 20f && canJump)
					{
						float num25 = (npc.Center.Y - Projectile.Center.Y) * -1f;
						float num26 = Gravity;
						float velY = (float)Math.Sqrt(num25 * 2f * num26);
						if (velY > 26f)
						{
							velY = 26f;
						}

						Projectile.velocity.Y = -velY;
					}
				}

				//If an attack was decided
				if (!oldAttacking && GeneralAttacking)
				{
					Timer = GetNextTimerValue(attackFrameCount);

					Projectile.netUpdate = true;
					Projectile.direction = (npc.Center.X - Projectile.Center.X >= 0f).ToDirectionInt();
				}
			}
		}

		private void DecideAttack(GoblinUnderlingTierStats tier, int meleeAttackRange, int rangedAttackRangeFromProj, NPC npc, float projDistance, float playerDistance, out bool allowJump)
		{
			allowJump = false;
			float toTargetMaxDist = 20f;

			//If target in range, handle fallthrough, jumping, and deciding when to initiate attack
			Projectile.shouldFallThrough = npc.Center.Y > Projectile.Bottom.Y;

			//only go into melee if NPC is either grounded, or 4 blocks above standable ground
			bool canGoMelee = npc.velocity.Y == 0f;
			if (!canGoMelee)
			{
				int tilesToCheck = 4;
				int npcY = (int)npc.Bottom.Y / 16;
				int x = (int)npc.Bottom.X / 16;

				bool atleastOneSolid = false;
				for (int y = npcY; y < npcY + tilesToCheck + 1; y++)
				{
					//If atleast one of the tiles below the NPC are active and walkable
					if (WorldGen.InWorld(x, y) && WorldGen.ActiveAndWalkableTile(x, y))
					{
						atleastOneSolid = true;
						break;
					}
				}

				if (atleastOneSolid)
				{
					canGoMelee = true;
				}
			}

			canGoMelee &= !Flying;

			if (!tier.rangedOnly && (playerDistance <= meleeAttackRange || projDistance < 120f) && canGoMelee)
			{
				//Melee range
				allowJump = true;

				if (projDistance < toTargetMaxDist || npc.Hitbox.Intersects(Projectile.Hitbox))
				{
					float len = Projectile.velocity.Length();
					if (len > 10f)
					{
						Projectile.velocity /= len / 10f;
					}

					MeleeAttacking = true;
				}
			}
			else
			{
				//Ranged range
				if (npc.noTileCollide || npc.noGravity)
				{
					//Should not try going through platforms if NPC is flying
					Projectile.shouldFallThrough = false;
				}

				//If enemy too far up, jump to try hitting it
				if (npc.Bottom.Y + rangedAttackRangeFromProj * 1.2f < Projectile.Center.Y)
				{
					allowJump = true;
				}

				if (projDistance < rangedAttackRangeFromProj && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
				{
					RangedAttacking = true;
				}
			}
		}

		private void GroundedMovement(Player player, GoblinUnderlingTierStats tier, float awayDistMax, float awayDistYMax, ref Vector2 destination, int attackTarget)
		{
			//Here destination can either be player or NPC
			if (attackTarget < 0)
			{
				if (Projectile.Distance(player.Center) > 60f && Projectile.Distance(destination) > 60f && Math.Sign(destination.X - player.Center.X) != Math.Sign(Projectile.Center.X - player.Center.X))
				{
					destination = player.Center;
				}

				Rectangle rect = Utils.CenteredRectangle(destination, Projectile.Size);
				for (int i = 0; i < 20; i++)
				{
					if (Collision.SolidCollision(rect.TopLeft(), rect.Width, rect.Height))
					{
						break;
					}

					rect.Y += 16;
					destination.Y += 16f;
				}

				Vector2 position = player.Center - Projectile.Size / 2f;
				Vector2 postCollision = Collision.TileCollision(position, destination - player.Center, Projectile.width, Projectile.height);
				destination = position + postCollision;
				if (Projectile.Distance(destination) < 32f)
				{
					float distPlayerToDestination = player.Distance(destination);
					if (player.Distance(Projectile.Center) < distPlayerToDestination)
					{
						destination = Projectile.Center;
					}
				}

				Vector2 fromDestToPlayer = player.Center - destination;
				if (fromDestToPlayer.Length() > awayDistMax || Math.Abs(fromDestToPlayer.Y) > awayDistYMax)
				{
					Rectangle rect2 = Utils.CenteredRectangle(player.Center, Projectile.Size);
					Vector2 fromPlayerToDest = destination - player.Center;
					Vector2 topLeft = rect2.TopLeft();
					for (float i = 0f; i < 1f; i += 0.05f)
					{
						Vector2 newTopLeft = rect2.TopLeft() + fromPlayerToDest * i;
						if (Collision.SolidCollision(rect2.TopLeft() + fromPlayerToDest * i, rect2.Width, rect2.Height))
						{
							break;
						}

						topLeft = newTopLeft;
					}

					destination = topLeft + Projectile.Size / 2f;
				}
			}

			Projectile.tileCollide = true;
			float velXChange = 0.5f; //0.5f
			float velXChangeMargin = 4f; //4f
			float velXChangeMax = 4f; //4f
			float velXChangeSmall = 0.1f;

			if (attackTarget != -1)
			{
				//Formula: margin/max is 12.5 x change
				velXChange = 1f * tier.movementSpeedMult;
				velXChangeMargin = velXChange * 12.5f;
				velXChangeMax = velXChangeMargin;

				//velXChange = 0.4f; //1f
				//velXChangeMargin = 5f; //8f
				//velXChangeMax = 5f; //8f
			}

			if (velXChangeMax < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				velXChangeMax = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
				velXChange = 0.7f;
			}

			int xOff = 0;
			bool canJumpOverTiles = false;
			float toDestinationX = destination.X - Projectile.Center.X;
			Vector2 toDestination = destination - Projectile.Center;
			if (Math.Abs(toDestinationX) > 5f)
			{
				if (toDestinationX < 0f)
				{
					xOff = -1;
					if (Projectile.velocity.X > -velXChangeMargin)
					{
						Projectile.velocity.X -= velXChange;
					}
					else
					{
						Projectile.velocity.X -= velXChangeSmall;
					}
				}
				else
				{
					xOff = 1;
					if (Projectile.velocity.X < velXChangeMargin)
					{
						Projectile.velocity.X += velXChange;
					}
					else
					{
						Projectile.velocity.X += velXChangeSmall;
					}
				}
			}
			else
			{
				Projectile.velocity.X *= 0.9f;
				if (Math.Abs(Projectile.velocity.X) < velXChange * 2f)
				{
					Projectile.velocity.X = 0f;
				}
			}

			bool tryJumping = Math.Abs(toDestination.X) >= 64f || (toDestination.Y <= -48f && Math.Abs(toDestination.X) >= 8f);
			if (xOff != 0 && tryJumping)
			{
				int x = (int)Projectile.Center.X / 16;
				int startY = (int)Projectile.position.Y / 16;
				x += xOff;
				x += (int)Projectile.velocity.X;
				for (int y = startY; y < startY + Projectile.height / 16 + 1; y++)
				{
					if (WorldGen.SolidTile(x, y))
					{
						canJumpOverTiles = true;
					}
				}
			}

			Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
			float nextVelocityY = Utils.GetLerpValue(0f, 100f, toDestination.Y, clamped: true) * Utils.GetLerpValue(-2f, -6f, Projectile.velocity.Y, clamped: true);
			if (Projectile.velocity.Y == 0f && canJumpOverTiles)
			{
				for (int k = 0; k < 3; k++)
				{
					int x = (int)(Projectile.Center.X) / 16;
					if (k == 0)
					{
						x = (int)Projectile.Left.X / 16;
					}

					if (k == 2)
					{
						x = (int)(Projectile.Right.X) / 16;
					}

					int y = (int)(Projectile.Bottom.Y) / 16;
					if (!WorldGen.SolidTile(x, y) && !Main.tile[x, y].IsHalfBlock && Main.tile[x, y].Slope <= 0 && (!TileID.Sets.Platforms[Main.tile[x, y].TileType] || !Main.tile[x, y].HasTile || Main.tile[x, y].IsActuated))
					{
						continue;
					}

					try
					{
						x = (int)(Projectile.Center.X) / 16;
						y = (int)(Projectile.Center.Y) / 16;
						x += xOff;
						x += (int)Projectile.velocity.X;
						if (!WorldGen.SolidTile(x, y - 1) && !WorldGen.SolidTile(x, y - 2))
						{
							Projectile.velocity.Y = -5.1f;
						}
						else if (!WorldGen.SolidTile(x, y - 2))
						{
							Projectile.velocity.Y = -7.1f;
						}
						else if (WorldGen.SolidTile(x, y - 5))
						{
							Projectile.velocity.Y = -11.1f;
						}
						else if (WorldGen.SolidTile(x, y - 4))
						{
							Projectile.velocity.Y = -10.1f;
						}
						else
							Projectile.velocity.Y = -9.1f;
					}
					catch
					{
						Projectile.velocity.Y = -9.1f;
					}
				}

				if (destination.Y - Projectile.Center.Y < -48f)
				{
					float height = destination.Y - Projectile.Center.Y;
					height *= -1f;
					if (height < 60f)
					{
						Projectile.velocity.Y = -6f;
					}
					else if (height < 80f)
					{
						Projectile.velocity.Y = -7f;
					}
					else if (height < 100f)
					{
						Projectile.velocity.Y = -8f;
					}
					else if (height < 120f)
					{
						Projectile.velocity.Y = -9f;
					}
					else if (height < 140f)
					{
						Projectile.velocity.Y = -10f;
					}
					else if (height < 160f)
					{
						Projectile.velocity.Y = -11f;
					}
					else if (height < 190f)
					{
						Projectile.velocity.Y = -12f;
					}
					else if (height < 210f)
					{
						Projectile.velocity.Y = -13f;
					}
					else if (height < 270f)
					{
						Projectile.velocity.Y = -14f;
					}
					else if (height < 310f)
					{
						Projectile.velocity.Y = -15f;
					}
					else
					{
						Projectile.velocity.Y = -16f;
					}
				}

				if (Projectile.wet && nextVelocityY == 0f)
				{
					Projectile.velocity.Y *= 2f;
				}
			}

			if (Projectile.velocity.X > velXChangeMax)
			{
				Projectile.velocity.X = velXChangeMax;
			}

			if (Projectile.velocity.X < -velXChangeMax)
			{
				Projectile.velocity.X = -velXChangeMax;
			}

			if (Projectile.velocity.X < 0f)
			{
				Projectile.direction = -1;
			}

			if (Projectile.velocity.X > 0f)
			{
				Projectile.direction = 1;
			}

			if (Projectile.velocity.X == 0f)
			{
				Projectile.direction = (player.Center.X > Projectile.Center.X).ToDirectionInt();
			}

			if (Projectile.velocity.X > velXChange && xOff == 1)
			{
				Projectile.direction = 1;
			}

			if (Projectile.velocity.X < -velXChange && xOff == -1)
			{
				Projectile.direction = -1;
			}

			Projectile.velocity.Y += Gravity + nextVelocityY * 1f;
			if (Projectile.velocity.Y > 10f)
			{
				Projectile.velocity.Y = 10f;
			}
		}

		private void FlyingMovement(Player player)
		{
			Projectile.tileCollide = false;
			float velChange = 0.2f;
			float toPlayerSpeed = 10f;
			int maxLen = 200;
			if (toPlayerSpeed < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
			{
				toPlayerSpeed = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
			}

			Vector2 toPlayer = player.Center - Projectile.Center;
			float len = toPlayer.Length();

			AssAI.TeleportIfTooFar(Projectile, player.Center);

			if (len < maxLen && player.velocity.Y == 0f && Projectile.Bottom.Y <= player.Bottom.Y && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				//Reset back from flying
				Flying = false;
				Projectile.netUpdate = true;
				if (Projectile.velocity.Y < -6f)
				{
					Projectile.velocity.Y = -6f;
				}
			}

			if (!(len < 60f))
			{
				toPlayer.Normalize();
				toPlayer *= toPlayerSpeed;
				if (Projectile.velocity.X < toPlayer.X)
				{
					Projectile.velocity.X += velChange;
					if (Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += velChange * 1.5f;
					}
				}

				if (Projectile.velocity.X > toPlayer.X)
				{
					Projectile.velocity.X -= velChange;
					if (Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= velChange * 1.5f;
					}
				}

				if (Projectile.velocity.Y < toPlayer.Y)
				{
					Projectile.velocity.Y += velChange;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += velChange * 1.5f;
					}
				}

				if (Projectile.velocity.Y > toPlayer.Y)
				{
					Projectile.velocity.Y -= velChange;
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= velChange * 1.5f;
					}
				}
			}

			if (Projectile.velocity.X != 0f && !GeneralAttacking)
			{
				Projectile.direction = Math.Sign(Projectile.velocity.X);
			}
		}

		private bool AttackCoolup(int nextTimerValue)
		{
			Timer += 1;
			if (nextTimerValue >= 0)
			{
				Timer = 0;
				GeneralAttacking = false;
				Projectile.netUpdate = true;
				return true;
			}

			return false;
		}

		private bool AttackCooldown(int attackCooldown)
		{
			Timer -= 1;
			if (Timer <= 0)
			{
				if (attackCooldown <= 0)
				{
					Timer = 0;
					GeneralAttacking = false;
					Projectile.netUpdate = true;
					return true;
				}

				Timer = -attackCooldown;
			}

			return false;
		}

		private void GoFlyingPrematurely(Player player, float awayDistMax, float awayDistYMax, int attackTarget)
		{
			if (IdleOrMoving && attackTarget < 0)
			{
				if (player.rocketDelay2 > 0 && player.wings != 45)
				{
					Flying = true;
					Projectile.netUpdate = true;
				}

				Vector2 toPlayer = player.Center - Projectile.Center;
				if (!AssAI.TeleportIfTooFar(Projectile, player.Center) && toPlayer.Length() > awayDistMax || Math.Abs(toPlayer.Y) > awayDistYMax)
				{
					Flying = true;
					Projectile.netUpdate = true;
					if (Projectile.velocity.Y > 0f && toPlayer.Y < 0f)
					{
						Projectile.velocity.Y = 0f;
					}

					if (Projectile.velocity.Y < 0f && toPlayer.Y > 0f)
					{
						Projectile.velocity.Y = 0f;
					}
				}
			}
		}

		private void AttackingAnimation(int attackFrameCount, int nextTimerValue)
		{
			Projectile.spriteDirection = -Projectile.direction;
			Projectile.rotation = 0f;

			int startAttackFrame = 12;
			bool hasJumpingAttackFrames = true;
			AttackFrameNumber = Utils.Clamp((int)(((float)nextTimerValue - Timer) / ((float)nextTimerValue / attackFrameCount)), 0, attackFrameCount - 1);
			Projectile.frame = startAttackFrame + AttackFrameNumber;

			if (hasJumpingAttackFrames && (Projectile.velocity.Y != 0f || Flying))
			{
				Projectile.frame += attackFrameCount;
			}
		}

		private void SetFrame()
		{
			//Idle, Jump, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Walk, Attack, Attack, Attack, Attack. Jump Attack, Jump Attack, Jump Attack, Jump Attack
			if (Flying)
			{
				//Flying animation
				Projectile.frameCounter = 0;
				Projectile.frame = 1;

				Projectile.rotation = 0f;

				//Propulsion dust
				float dustChance = Math.Clamp(Math.Abs(Projectile.velocity.Length()) / 3f, 0.5f, 1f);
				if (Main.rand.NextFloat() < dustChance)
				{
					int dirOffset = 0;
					if (Projectile.direction == -1)
					{
						dirOffset = -4;
					}
					Vector2 dustOrigin = Projectile.Bottom + new Vector2(dirOffset, -4) - Projectile.velocity.SafeNormalize(Vector2.Zero) * 2;

					for (int i = 0; i < 2; i++)
					{
						Vector2 bootOffset = new Vector2((i == 0).ToDirectionInt() * 5, 0);
						Dust dust = Dust.NewDustDirect(dustOrigin - Vector2.One * 2f + bootOffset, 4, 4, DustID.Cloud, -Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 50, default(Color), 1.3f);
						dust.velocity.X *= 0.2f;
						dust.velocity.Y *= 0.2f;
						dust.noGravity = true;
					}
				}
			}
			else
			{
				Projectile.rotation = 0f;
				if (Projectile.velocity.Y == 0f)
				{
					if (Projectile.velocity.X == 0f)
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 0;
					}
					else if (Math.Abs(Projectile.velocity.X) >= 0.5f)
					{
						Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 10)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}

						if (Projectile.frame > 11 || Projectile.frame < 2)
						{
							Projectile.frame = 2;
						}
					}
					else
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 5;
					}
				}
				else if (Projectile.velocity.Y != 0f)
				{
					Projectile.frameCounter = 0;
					Projectile.frame = 1;
				}
			}
		}
	}
}
