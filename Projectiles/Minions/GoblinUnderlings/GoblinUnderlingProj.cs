using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Base.Handlers.SpawnedNPCHandler;
using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	//Projectile names get saved to GoblinUnderlingPlayer! So use LegacyName if needed
	[Content(ContentType.Weapons)]
	public abstract class GoblinUnderlingProj : AssProjectile
	{
		public abstract string FolderName { get; }

		public abstract GoblinUnderlingChatterType ChatterType { get; }

		public const int WeaponFrameCount = 4;
		public const float Gravity = 0.4f;

		public const int StuckTimerMax = 3 * 60;
		public int stuckTimer = 0;

		public int afkTimer = 0;
		public int minionPos = 0;

		public int InCombatTimerMax = 5 * 60;
		public int inCombatTimer = 0;

		private bool skipDefaultMovement = false;
		private int oldAttackTarget = -1;

		private GoblinUnderlingProgressionTierStage magicTierCycle = GoblinUnderlingTierSystem.CurrentTier;

		private bool spawned = false;

		public GoblinUnderlingClass oldCurrentClass;

		//Needs syncing, see NetSend/Receive any time you change or increase value range
		public GoblinUnderlingClass currentClass;
		public float lastAttackAngle;
		public int AttackState { get; private set; }

		public int MovementState
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
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

		public string AssetPrefix => $"{GoblinUnderlingAssetsSystem.assetPath}/{FolderName}/";

		public override string Texture => AssetPrefix + currentClass + "_0";

		public sealed override void SetStaticDefaults()
		{
			Main.projPet[Projectile.type] = true;
			//ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; Has other right click feature instead
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

			GoblinUnderlingTierSystem.GoblinUnderlingProjs[Projectile.type] = ChatterType;
			GoblinUnderlingAssetsSystem.SetFrameCount(Projectile.type, currentClass);
			GoblinUnderlingAssetsSystem.RegisterAssetPrefix(Projectile.type, AssetPrefix);

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
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

			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{
			//Assign currentClass
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (source is not IEntitySource_WithStatsFromItem fromItem)
			{
				return;
			}

			if (fromItem.Item.ModItem is not GoblinUnderlingItem underlingItem)
			{
				return;
			}

			currentClass = underlingItem.currentClass;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			//AttackState only 0 1 2 -> 2 bits
			//currentClass only 3 values -> 2 bits
			BitsByte flags = new BitsByte();
			flags[0] = lastAttackAngle != 0f;
			byte b = (byte)((((byte)AttackState & 0b00000011) << 0) | (((byte)currentClass & 0b00000011) << 2) | (flags[0].ToInt() << 4));
			// ___0ccaa

			writer.Write((byte)b);
			//AssUtils.Print("send: " + Convert.ToString(b, 2).PadLeft(8, '0'));
			if (flags[0])
			{
				writer.Write((float)lastAttackAngle);
			}
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			byte b = reader.ReadByte();
			AttackState = (byte)((b >> 0) & 0b00000011);
			currentClass = (GoblinUnderlingClass)(byte)((b >> 2) & 0b00000011);
			BitsByte flags = new BitsByte();
			flags[0] = ((b >> 4) & 1) > 0;

			//AssUtils.Print("recv: " + Convert.ToString(b, 2).PadLeft(8, '0'));
			lastAttackAngle = flags[0] ? reader.ReadSingle() : 0f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//Custom draw to just center on the hitbox
			var tierStats = GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass);
			int texIndex = GoblinUnderlingTierSystem.CurrentTierIndex;
			var bodyAssets = GoblinUnderlingAssetsSystem.BodyAssets[Type][currentClass];
			Texture2D texture = ((Main.myPlayer == Projectile.owner && !ClientConfig.Instance.GoblinUnderlingVisibleArmor) ? bodyAssets[0] : bodyAssets[texIndex]).Value;
			Rectangle sourceRect = texture.Frame(1, GoblinUnderlingAssetsSystem.BodyAssetFrameCounts[currentClass], 0, Projectile.frame);
			Vector2 drawOrigin = sourceRect.Size() / 2f;

			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Vector2 center = Projectile.position + Projectile.Size / 2f + new Vector2(0, Projectile.gfxOffY + 4f - 2f) - Main.screenPosition;
			Color color = lightColor;

			float rotation = Projectile.rotation;
			float scale = Projectile.scale;
			Main.EntitySpriteDraw(texture, center, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);

			if (ClientConfig.Instance.GoblinUnderlingVisibleRocketBoots && GetBootsDrawParams(out Texture2D bootsTexture, out Rectangle bootsSourceRect))
			{
				Vector2 bootsDrawOrigin = drawOrigin; //Made to be the same size
				Main.EntitySpriteDraw(bootsTexture, center, bootsSourceRect, color, rotation, bootsDrawOrigin, scale, spriteEffects, 0);
			}

			if (currentClass == GoblinUnderlingClass.Melee)
			{
				if (MeleeAttacking || RangedAttacking && ((GoblinUnderlingMeleeTierStats)tierStats).showMeleeDuringRanged)
				{
					var asset = GoblinUnderlingAssetsSystem.GetWeaponAsset(GoblinUnderlingWeaponType.Sword, texIndex);
					if (asset != null)
					{
						texture = asset.Value;
						sourceRect = texture.Frame(1, WeaponFrameCount, 0, AttackFrameNumber);
						drawOrigin = sourceRect.Size() / 2f;
						Main.EntitySpriteDraw(texture, center, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);
					}
				}
			}
			else if (currentClass == GoblinUnderlingClass.Ranged)
			{
				if (RangedAttacking)
				{
					if (GetBowDrawParams(center, sourceRect, spriteEffects, lastAttackAngle, out Texture2D weaponTexture, out Vector2 weaponCenter, out Rectangle weaponSourceRect, out Vector2 weaponDrawOrigin, out float weaponRotation))
					{
						Main.EntitySpriteDraw(weaponTexture, weaponCenter, weaponSourceRect, color, weaponRotation, weaponDrawOrigin, scale, spriteEffects, 0);
					}

					GetArmDrawParams(center, spriteEffects, lastAttackAngle, out Texture2D armTexture, out Vector2 armCenter, out Rectangle armSourceRect, out Vector2 armDrawOrigin, out float armRotation);
					Main.EntitySpriteDraw(armTexture, armCenter, armSourceRect, color, armRotation, armDrawOrigin, scale, spriteEffects, 0);
				}
			}

			return false;
		}

		private static int GetBootsFrame()
		{
			return GoblinUnderlingTierSystem.CurrentTier switch
			{
				GoblinUnderlingProgressionTierStage.PreBoss or GoblinUnderlingProgressionTierStage.EoC => 0,
				GoblinUnderlingProgressionTierStage.Evil => 1,
				GoblinUnderlingProgressionTierStage.Skeletron or GoblinUnderlingProgressionTierStage.WoF => 2,
				GoblinUnderlingProgressionTierStage.Mech or GoblinUnderlingProgressionTierStage.Plantera => 3,
				GoblinUnderlingProgressionTierStage.Cultist => 4,
				_ => -1, //Should never happen
			};
		}

		private int GetBootsFrameHorizontal()
		{
			return currentClass == GoblinUnderlingClass.Melee && FolderName == "Eager" ? 1 : 0;
		}

		private bool GetBootsDrawParams(out Texture2D bootsTexture, out Rectangle sourceRect)
		{
			bootsTexture = null;
			sourceRect = Rectangle.Empty;
			if (!Flying)
			{
				return false;
			}

			if (GoblinUnderlingTierSystem.CurrentTier == GoblinUnderlingProgressionTierStage.Cultist && currentClass == GoblinUnderlingClass.Magic)
			{
				return false;
			}

			if (FolderName == "Serious" && currentClass == GoblinUnderlingClass.Magic)
			{
				return false;
			}

			int frame = GetBootsFrame();
			if (frame == -1)
			{
				return false;
			}

			bootsTexture = GoblinUnderlingAssetsSystem.RocketBootsAsset.Value;
			const int BootsFrameCount = 5;
			const int BootsFrameCountHorizontal = 2;
			int frameX = GetBootsFrameHorizontal();
			sourceRect = bootsTexture.Frame(BootsFrameCountHorizontal, BootsFrameCount, frameX, frame);

			return true;
		}

		private bool GetBowDrawParams(Vector2 drawCenter, Rectangle sourceRect, SpriteEffects spriteEffects, float attackAngle,
			out Texture2D weaponTexture, out Vector2 weaponCenter, out Rectangle weaponSourceRect, out Vector2 weaponDrawOrigin, out float weaponRotation)
		{
			weaponTexture = null;
			weaponCenter = default;
			weaponSourceRect = default;
			weaponDrawOrigin = default;
			weaponRotation = 0f;
			int texIndex = GoblinUnderlingTierSystem.CurrentTierIndex;
			var asset = GoblinUnderlingAssetsSystem.GetWeaponAsset(GoblinUnderlingWeaponType.Bow, texIndex);
			if (asset == null)
			{
				return false;
			}

			weaponTexture = asset.Value;
			//Combined center of base + weapon sprites
			weaponDrawOrigin = sourceRect.Size() / 2f;
			weaponSourceRect = weaponTexture.Frame();
			weaponDrawOrigin -= weaponSourceRect.Size() / 2f;

			float slimmerSpriteOffsetX = 2 * 2; //Sprite is less than half the width by 2 pixels, so adjust it when flipped

			//Move weapon down to align with arm
			Vector2 alignWeapon = new Vector2(8, 4);
			if (spriteEffects == SpriteEffects.FlipHorizontally)
			{
				alignWeapon.X *= -1;
				alignWeapon.X -= slimmerSpriteOffsetX;
			}
			weaponCenter = drawCenter - alignWeapon;

			//The rotation happens outside of the sprite, on the arm joint
			Vector2 rotOffset = -new Vector2(12, 10);
			if (spriteEffects == SpriteEffects.FlipHorizontally)
			{
				rotOffset.X *= -1;
				rotOffset.X += slimmerSpriteOffsetX;
			}
			weaponDrawOrigin -= rotOffset;
			weaponCenter -= rotOffset;

			weaponRotation = attackAngle;
			if (spriteEffects != SpriteEffects.FlipHorizontally)
			{
				//weaponRotation += MathHelper.Pi;
			}

			return true;
		}

		private void GetArmDrawParams(Vector2 drawCenter, SpriteEffects spriteEffects, float attackAngle,
			out Texture2D armTexture, out Vector2 armCenter, out Rectangle armSourceRect, out Vector2 armDrawOrigin, out float armRotation)
		{
			var armAssets = GoblinUnderlingAssetsSystem.RangedArmAssets[Type];
			int texIndex = GoblinUnderlingTierSystem.CurrentTierIndex;
			armTexture = ((Main.myPlayer == Projectile.owner && !ClientConfig.Instance.GoblinUnderlingVisibleArmor) ? armAssets[0] : armAssets[texIndex]).Value;

			//Ignore the config setting in this case
			if (texIndex == (int)GoblinUnderlingProgressionTierStage.Cultist)
			{
				armTexture = armAssets[texIndex].Value;
			}

			armSourceRect = armTexture.Frame();
			armDrawOrigin = armSourceRect.Size() / 2f;

			//Frame size is the same
			armCenter = drawCenter;

			//The rotation on the arm joint
			Vector2 rotOffset = -new Vector2(4, 6);
			if (spriteEffects == SpriteEffects.FlipHorizontally)
			{
				rotOffset.X *= -1;
			}
			armDrawOrigin -= rotOffset;
			armCenter -= rotOffset;

			armRotation = attackAngle;
			if (spriteEffects != SpriteEffects.FlipHorizontally)
			{
				//armRotation += MathHelper.Pi;
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (currentClass != GoblinUnderlingClass.Melee)
			{
				return;
			}

			//Save original coordinates
			int centerX = hitbox.Center.X;
			int bottomY = hitbox.Bottom;

			int increase = ((GoblinUnderlingMeleeTierStats)GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass)).meleeAttackHitboxIncrease;
			hitbox.Inflate(increase, increase / 2); //Top shouldn't grow as much, as its only going to grow upwards

			//Restore coordinates
			hitbox.Y = bottomY - hitbox.Height;
			hitbox.X = centerX - hitbox.Width / 2;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			GoblinUnderlingHelperSystem.CommonModifyHitNPC(currentClass, Projectile, target, ref modifiers);
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

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			GoblinUnderlingPlayer modPlayer = player.GetModPlayer<GoblinUnderlingPlayer>();
			if (player.dead)
			{
				modPlayer.SetHasMinion(Projectile.type, false);
			}
			if (modPlayer.GetHasMinion(Projectile.type))
			{
				Projectile.timeLeft = 2;
			}

			minionPos = modPlayer.numUnderlings;
			modPlayer.numUnderlings++;

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
			var tier = GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass);

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
			//Projectile.minionPos calculations not necessary because you can't summon more than 1. Instead, use custom minionPos only for underlings
			float minionOffsetX = minionPos * (Projectile.width + 6) * player.direction;
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
						defaultLocation.X = x * 16 + 8 - minionOffsetX;
						if (dir == 1)
						{
							defaultLocation.X += offset; //Quirk with how it always wants to align "right of" the location
						}
						return defaultLocation;
					}
				}
				//scanX++;
			}

			return player.Center - new Vector2(minionOffsetX, 0);
		}

		private void DoIdleMessage(Player player, Vector2 idleLocation)
		{
			if (Main.myPlayer != player.whoAmI)
			{
				return;
			}

			var modPlayer = player.GetModPlayer<GoblinUnderlingPlayer>();
			if (!modPlayer.firstSummon.Contains(Projectile.type))
			{
				if (ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnFirstSummon(Projectile))
				{
					modPlayer.firstSummon.Add(Projectile.type);

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
				player.afkCounter > 4 * 60 && Math.Abs(idleLocation.X - Projectile.Center.X) < 50 && Math.Abs(idleLocation.Y - Projectile.Center.Y) < 96))
			{
				return;
			}

			ModContent.GetInstance<GoblinUnderlingChatterHandler>().OnIdle(Projectile);
		}

		private int GetNextTimerValue(int attackFrameCount)
		{
			var tier = GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass);
			int time = tier.attackInterval;
			if (RangedAttacking)
			{
				if (currentClass == GoblinUnderlingClass.Melee && tier is GoblinUnderlingMeleeTierStats meleeTier)
				{
					float ranged = meleeTier.rangedAttackIntervalMultiplier * time * attackFrameCount;
					return (int)ranged;
				}
			}

			return time * attackFrameCount;
		}

		private static bool CustomEliminationCheck_Pirates(Entity otherEntity, int currentTarget) => true;

		private void UnderlingAI(Player player, out Vector2 idleLocation)
		{
			if (spawned && oldCurrentClass != currentClass)
			{
				Dust dust;
				for (int i = 0; i < 16; i++)
				{
					dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 204, Projectile.velocity.X, Projectile.velocity.Y, 0, new Color(255, 255, 255), 0.8f);
					dust.noGravity = true;
					dust.noLight = true;
				}

				ResetAttack();
				AttackFrameNumber = 0;
			}

			skipDefaultMovement = false;
			var tier = GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass);

			//if target is outside of meleeAttackRange (but inside globalAttackRange), minion stops moving horizontally
			//on the edge of the meleeAttackRange, then initiates attacking behavior but with ranged projectiles instead of melee
			int meleeAttackRange = 400; //25 * 16 = 400
			int rangedAttackRangeFromProj = (int)(256 * tier.rangedRangeMultiplier); //16 * 16
			float awayDistMax = 500f;
			float awayDistYMax = 400f; //300, increased to reduce amount of "bouncing" when player is standing on far up tiles or hooked up
			Vector2 destination = GetIdleLocation(player);
			idleLocation = destination;

			bool ranged = true;
			if (currentClass == GoblinUnderlingClass.Melee && tier is GoblinUnderlingMeleeTierStats meleeTier)
			{
				ranged = meleeTier.rangedOnly;
			}

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
				PickDestinationAndAttack(ranged, meleeAttackRange, rangedAttackRangeFromProj, attackFrameCount, attackTarget, globalAttackRange, out destination);

				PostPickDestinationAndAttack(attackTarget, oldAttackTarget);
			}

			GoFlyingPrematurely(player, awayDistMax, awayDistYMax, attackTarget);

			if (GeneralAttacking && Timer < 0)
			{
				Projectile.friendly = false;
				if (AttackCoolup(nextTimerValue))
				{
					//TODO implement when needed
					//return;
				}
			}
			else if (GeneralAttacking)
			{
				AttackAction(ranged, tier.rangedProjType, tier.rangedVelocity, attackFrameCount, nextTimerValue, tier.shootFrame, globalAttackRange, tier.gravity, tier.ticksWithoutGravity, tier.projOffset);

				if (AttackCooldown(attackCooldown))
				{
					//Stay in attack mode if possible
					if (attackTarget > -1)
					{
						PickDestinationAndAttack(ranged, meleeAttackRange, rangedAttackRangeFromProj, attackFrameCount, attackTarget, globalAttackRange, out destination);
					}

					if (attackTarget == -1)
					{
						//Combat ended
						lastAttackAngle = 0f;
					}
				}
			}

			if (!skipDefaultMovement)
			{
				if (IdleOrMoving)
				{
					GroundedMovement(player, tier.movementSpeedMult, awayDistMax, awayDistYMax, ref destination, attackTarget);
				}
				else if (Flying)
				{
					FlyingMovement(player);
				}
			}

			if (!spawned)
			{
				spawned = true;
			}

			oldCurrentClass = currentClass;
			oldAttackTarget = attackTarget;
			Projectile.spriteDirection = -Projectile.direction;
		}

		public virtual void PostPickDestinationAndAttack(int attackTarget, int oldAttackTarget)
		{

		}

		private void ModifyRangedProjType(ref int rangedProjType)
		{
			if (currentClass != GoblinUnderlingClass.Magic)
			{
				return;
			}

			//Magic cycles through all projectiles in the first or second half of the game that were unlocked
			var tierData = GoblinUnderlingTierSystem.GetTierStats(GoblinUnderlingClass.Magic);
			var tierToStartAt = GoblinUnderlingProgressionTierStage.EoC; //First and second tier have the same debuff
			if ((int)GoblinUnderlingTierSystem.CurrentTier >= (int)GoblinUnderlingProgressionTierStage.WoF)
			{
				tierToStartAt = GoblinUnderlingProgressionTierStage.WoF;
			}

			int val = (int)magicTierCycle;
			val++;
			if (val > (int)GoblinUnderlingTierSystem.CurrentTier)
			{
				val = (int)tierToStartAt;
			}
			magicTierCycle = (GoblinUnderlingProgressionTierStage)val;

			rangedProjType = tierData[magicTierCycle].rangedProjType;
		}

		private void AttackAction(bool ranged, int rangedProjType, float rangedVelocity, int attackFrameCount, int nextTimerValue, int shootFrame, int globalAttackRange, float gravity, int ticksWithoutGravity, Vector2 projOffset)
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
					if (ranged)
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
					Projectile.tileCollide = true;
				}

				int newAttackTarget = -1;
				Projectile.Minion_FindTargetInRange(globalAttackRange, ref newAttackTarget, skipIfCannotHitWithOwnBody: true, CustomEliminationCheck_Pirates);

				if (newAttackTarget != -1)
				{
					NPC npc = Main.npc[newAttackTarget];
					Vector2 position = Projectile.Center + projOffset;
					Vector2 targetPos = npc.Center + npc.velocity * 0.6f;

					Projectile.direction = (targetPos.X - position.X >= 0f).ToDirectionInt();

					if (Main.myPlayer == Projectile.owner && Timer == (int)(nextTimerValue * (1 - (float)shootFrame / nextTimerValue)))
					{
						ModifyRangedProjType(ref rangedProjType);

						//If offset would prevent collision
						if (!Collision.CanHitLine(position, 1, 1, targetPos, 1, 1))
						{
							position = Projectile.Center;
						}

						Vector2 vector = targetPos - position;
						float speed = rangedVelocity;
						float mag = vector.Length();
						if (mag > speed)
						{
							mag = speed / mag;
							vector *= mag;
						}

						if (gravity > 0)
						{
							int mult = 1 + ContentSamples.ProjectilesByType[rangedProjType].extraUpdates;
							AssUtils.ModifyVelocityForGravity(position, targetPos, gravity * mult, ref vector, ticksWithoutGravity * mult);
						}

						if (currentClass == GoblinUnderlingClass.Ranged)
						{
							lastAttackAngle = vector.ToRotation();
							if (Projectile.direction == -1)
							{
								lastAttackAngle += MathHelper.Pi;
							}
							Projectile.netUpdate = true;
						}

						Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, vector, rangedProjType, Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
			}
		}

		private void PickDestinationAndAttack(bool ranged, int meleeAttackRange, int rangedAttackRangeFromProj, int attackFrameCount, int attackTarget, int globalAttackRange, out Vector2 destination)
		{
			NPC npc = Main.npc[attackTarget];
			destination = npc.Center;
			bool oldAttacking = GeneralAttacking;
			if (Projectile.IsInRangeOfMeOrMyOwner(npc, globalAttackRange, out float projDistance, out float playerDistance, out bool _))
			{
				DecideAttack(ranged, meleeAttackRange, rangedAttackRangeFromProj, npc, projDistance, playerDistance, out bool allowJump);

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

		private void DecideAttack(bool ranged, int meleeAttackRange, int rangedAttackRangeFromProj, NPC npc, float projDistance, float playerDistance, out bool allowJump)
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

			if (!ranged && (playerDistance <= meleeAttackRange || projDistance < 120f) && canGoMelee)
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

				int rangedRange = rangedAttackRangeFromProj;
				rangedRange += minionPos * Projectile.width;
				//If enemy too far up, jump to try hitting it
				//This causes "deadzone" where no ranged attack takes place before jumping is allowed. jumping in this section doesn't make much sense tho as it violates the ranged attack range anyway
				//if (npc.Bottom.Y + rangedRange * 1.2f < Projectile.Center.Y)
				//{
				//	allowJump = true;
				//}


				if (projDistance < rangedRange && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
				{
					RangedAttacking = true;
				}
			}
		}

		private void GroundedMovement(Player player, float movementSpeedMult, float awayDistMax, float awayDistYMax, ref Vector2 destination, int attackTarget)
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
				velXChange = 1f * movementSpeedMult;
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
			AssAI.FixProjectileOverlap(Projectile, 1f, 0.05f, GoblinUnderlingTierSystem.GoblinUnderlingProjs.Keys.ToArray());

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

		private void ResetAttack()
		{
			Timer = 0;
			GeneralAttacking = false;
			Projectile.netUpdate = true;
		}

		private bool AttackCoolup(int nextTimerValue)
		{
			Timer += 1;
			if (nextTimerValue >= 0)
			{
				ResetAttack();
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
					ResetAttack();
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
			if (currentClass == GoblinUnderlingClass.Melee)
			{
				AttackFrameNumber = Utils.Clamp((int)(((float)nextTimerValue - Timer) / ((float)nextTimerValue / attackFrameCount)), 0, attackFrameCount - 1);
				Projectile.frame = startAttackFrame + AttackFrameNumber;

				if (hasJumpingAttackFrames && (Projectile.velocity.Y != 0f || Flying))
				{
					Projectile.frame += attackFrameCount;
				}
			}
			else if (currentClass == GoblinUnderlingClass.Magic || currentClass == GoblinUnderlingClass.Ranged)
			{
				AttackFrameNumber = 0;
				Projectile.frame = startAttackFrame + AttackFrameNumber;

				if (hasJumpingAttackFrames && (Projectile.velocity.Y != 0f || Flying))
				{
					Projectile.frame += 1;
				}
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
					Vector2 dustOrigin = Projectile.Bottom + new Vector2(dirOffset, -2) - Projectile.velocity.SafeNormalize(Vector2.Zero) * 2;

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
