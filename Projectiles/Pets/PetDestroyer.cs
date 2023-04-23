using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public abstract class PetDestroyerBase : SimplePetProjBase
	{
		public static int[] wormTypes;

		public static LocalizedText CommonDisplayNameText { get; private set; }

		public override LocalizedText DisplayName => CommonDisplayNameText;

		public override void SetStaticDefaults()
		{
			CommonDisplayNameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.PetDestroyer.DisplayName"));
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			//ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true; //doesn't work for some reason with hide = true
			//ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			AssAI.StardustDragonSetDefaults(Projectile, size: HITBOX_SIZE, minion: false);
			Projectile.alpha = 0;
		}

		public const int NUMBER_OF_BODY_SEGMENTS = 6;

		//default 24
		public const int HITBOX_SIZE = 24;

		//default 16
		public const int DISTANCE_BETWEEN_SEGMENTS = 25;

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetDestroyer = false;
			}
			if (modPlayer.PetDestroyer)
			{
				Projectile.timeLeft = 2;
			}

			if (Projectile.type != ModContent.ProjectileType<PetDestroyerHead>())
			{
				AssAI.StardustDragonAI(Projectile, wormTypes, DISTANCE_BETWEEN_SEGMENTS);
			}
			else
			{
				AssAI.BabyEaterAI(Projectile, originOffset: new Vector2(0f, -100f));
				if (Projectile.owner == Main.myPlayer && Math.Sign(Projectile.oldVelocity.X) != Math.Sign(Projectile.velocity.X))
				{
					Projectile.netUpdate = true;
				}
				//float scaleFactor = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
				//projectile.scale = 1f + scaleFactor * 0.01f;

				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f).ToDirectionInt();
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 drawPos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle drawRect = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			Color color = Projectile.GetAlpha(lightColor);
			Vector2 drawOrigin = drawRect.Size() / 2f;

			//alpha5.A /= 2;

			Main.EntitySpriteDraw(texture, drawPos, drawRect, color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

			texture = ModContent.Request<Texture2D>(Texture + "_Glowmask").Value;
			Main.EntitySpriteDraw(texture, drawPos, drawRect, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

			return false;
		}
	}

	public class PetDestroyerHead : PetDestroyerBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			AmuletOfManyMinionsApi.RegisterWormPet(this, ModContent.GetInstance<PetDestroyerBuff_AoMM>(), null, false, 200);
		}
	}

	public class PetDestroyerBody1 : PetDestroyerBase { }

	public class PetDestroyerBody2 : PetDestroyerBase { }

	public class PetDestroyerTail : PetDestroyerBase { }

	public class PetDestroyerProbe : SimplePetProjBase
	{
		//since the index might be different between clients, using ai[] for it will break stuff
		public int ParentIndex
		{
			get
			{
				return (int)Projectile.localAI[0] - 1;
			}
			set
			{
				Projectile.localAI[0] = value + 1;
			}
		}

		public int AttackCounter
		{
			get
			{
				return (int)Projectile.ai[1];
			}
			set
			{
				Projectile.ai[1] = value;
			}
		}

		public float rot;

		public const int AttackDelay = 90;

		private const int LaserDamage = 8;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;

			//TODO preview

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<PetDestroyerBuff_AoMM>(), ModContent.ProjectileType<PetDestroyerDroneLaser>());
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			Projectile.width = 20;
			Projectile.height = 18;
		}

		public override bool PreAI()
		{
			Projectile.originalDamage = (int)(Projectile.originalDamage * 0.6f);

			return base.PreAI();
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetDestroyer = false;
			}
			if (modPlayer.PetDestroyer)
			{
				Projectile.timeLeft = 2;
			}

			#region Find Parent
			//set parent when spawned

			int parentType = Projectile.identity % 2 == 0 ? ModContent.ProjectileType<PetDestroyerHead>() : ModContent.ProjectileType<PetDestroyerTail>();
			if (ParentIndex < 0)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile p = Main.projectile[i];
					if (p.active && p.type == parentType && Projectile.owner == p.owner)
					{
						ParentIndex = i;
						//projectile.netUpdate = true;
						break;
					}
				}
			}

			//if something goes wrong, abort mission
			if (ParentIndex < 0)
			{
				Projectile.Kill();
				return;
			}
			Projectile parent = Main.projectile[ParentIndex];
			#endregion

			//offsets to the drones
			Vector2 between = parent.Center - Projectile.Center;
			float offsetX = (between.X < 0f).ToDirectionInt() * 60f;
			float offsetY = 60;

			AssAI.TeleportIfTooFar(Projectile, parent.Center);

			if (!parent.active)
			{
				Projectile.active = false;
				return;
			}

			AssAI.ZephyrfishAI(Projectile, parent: parent, velocityFactor: 1f, random: false, swapSides: 1, offsetX: offsetX, offsetY: offsetY);

			int targetIndex = -1;
			
			if (AmuletOfManyMinionsApi.IsActive(this) &&
				 AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) &&
				 state.IsInFiringRange && state.TargetNPC is NPC targetNPC)
			{
				//Just need to get the index to set rotation later
				targetIndex = targetNPC.whoAmI;
			}
			else
			{
				Regular_AI(ref targetIndex);
			}

			if (targetIndex != -1)
			{
				rot = (Main.npc[targetIndex].Center - Projectile.Center).ToRotation();
			}
			else
			{
				rot = rot.AngleLerp(Projectile.velocity.ToRotation(), 0.1f);
			}

			Projectile.rotation = rot;
			Projectile.direction = Projectile.spriteDirection = -1;
		}

		private void Regular_AI(ref int targetIndex)
		{
			targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 500);

			if (Main.myPlayer == Projectile.owner)
			{
				//safe random increase so the modulo still goes to 0 properly
				bool closeToAttackDelay = (AttackCounter % AttackDelay) == AttackDelay - 1;
				AttackCounter += Main.rand.Next(1, closeToAttackDelay ? 2 : 3);
				if (AttackCounter % AttackDelay == 0)
				{
					if (targetIndex != -1 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						if (AttackCounter == AttackDelay) AttackCounter += AttackDelay;
						Vector2 position = Projectile.Center;
						NPC target = Main.npc[targetIndex];
						Vector2 velocity = target.Center + target.velocity * 5f - Projectile.Center;
						velocity.Normalize();
						velocity *= 7f;
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<PetDestroyerDroneLaser>(), LaserDamage, 2f, Main.myPlayer, 0f, 0f);
						Projectile.netUpdate = true;
						//decide not to update parent, instead, parent updates itself
						//parent.netUpdate = true;
					}
					else
					{
						if (AttackCounter > AttackDelay)
						{
							AttackCounter -= AttackDelay;
							Projectile.netUpdate = true;
							//parent.netUpdate = true;
						}
					}
					AttackCounter -= AttackDelay;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PetDestroyerProbe").Value;
			Rectangle bounds = image.Bounds;
			bounds.Y = Projectile.frame * bounds.Height;
			Vector2 stupidOffset = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f - Projectile.gfxOffY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 origin = bounds.Size() / 2;
			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

			image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PetDestroyerProbe_Glowmask").Value;
			Main.EntitySpriteDraw(image, drawPos, bounds, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0);

			return false;
		}
	}
}
