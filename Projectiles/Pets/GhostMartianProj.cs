using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class GhostMartianProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Martian");
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
			if (AmuletOfManyMinionsApi.IsActive(this) && PostAttacking)
			{
				float num318 = PostAttackingTimer / (float)25;
				float scale22 = 1f - num318;
				Color value122 = Projectile.GetAlpha(new Color(255, 220, 220)) * scale22 * scale22 * 0.8f * Projectile.Opacity;
				value122.A = 0;
				short num319 = ProjectileID.MedusaHeadRay;
				Main.instance.LoadProjectile(num319);
				Texture2D value123 = TextureAssets.Projectile[num319].Value;
				Vector2 origin25 = value123.Size() * new Vector2(0.5f, 1f);
				float num320 = 9f;
				float num321 = Projectile.velocity.ToRotation();
				if (Projectile.velocity.Length() < 0.1f)
				{
					num321 = Projectile.direction == 1 ? 0f : MathHelper.Pi;
				}

				Vector2 vector50 = Projectile.position + Projectile.Size / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
				vector50.Y += -2f + (float)(Math.Cos(Main.mouseTextColor / 255f * MathHelper.TwoPi * 2f) * 4.0);
				Vector2 value124 = (num321 + MathHelper.PiOver2).ToRotationVector2();
				for (int num322 = 0; (float)num322 < num320; num322++)
				{
					float num323 = (num322 % 2 != 0).ToDirectionInt();
					float num324 = ((float)num322 + 1f) * num323 * 0.2f * (0.2f + 2f * num318) + num321 + MathHelper.PiOver2;
					float num325 = Utils.Remap(Vector2.Dot(num324.ToRotationVector2(), value124), -1f, 1f, 0f, 1f);
					float num326 = Projectile.scale * (0.15f + 0.6f * (float)Math.Sin(Main.GlobalTimeWrappedHourly + num322 * 0.739f)) * num325;
					Main.EntitySpriteDraw(value123, vector50 + Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.TwoPi * (1f / num320) * num322 + Main.GlobalTimeWrappedHourly) * 4f * Projectile.scale, null, value122 * num325, num324, origin25, new Vector2(num326 * 1.5f, num326), SpriteEffects.None, 0);
				}
			}

			Texture2D image = TextureAssets.Projectile[Type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			float sinY = (float)((Math.Sin(((float)sincounter / sincounterMax) * MathHelper.TwoPi) - 1) * 2);

			Vector2 stupidOffset = new Vector2(image.Width / 2, Projectile.height / 2 + sinY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;

			Main.EntitySpriteDraw(image, drawPos, bounds, Projectile.GetAlpha(lightColor), 0f, bounds.Size() / 2, 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}

		private bool setAoMMParas = false;

		public override bool PreAI()
		{
			if (!setAoMMParas)
			{
				setAoMMParas = true;

				if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
				{
					paras.PreferredTargetDistance = 30;

					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}
			}

			return true;
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

				bool defaultAI = true;
				if (AmuletOfManyMinionsApi.IsActive(this))
				{
					defaultAI = AoMM_AI();
				}

				if (defaultAI)
				{
					AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, offsetX: -6f, offsetY: 6f);

					Projectile.spriteDirection = (player.Center.X <= Projectile.Center.X).ToDirectionInt();

					AssAI.FlickerwickPetDraw(Projectile, frameCounterMaxFar: 4, frameCounterMaxClose: 10);
				}

				if (AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) &&
						state.IsInFiringRange && state.TargetNPC is NPC targetNPC)
				{
					Projectile.spriteDirection = ((targetNPC.Center.X - Projectile.Center.X) < 0).ToDirectionInt();
				}
			}
		}

		public const int FireRate = 20;

		public const int AttackTick = 1;

		public int AttackTimer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int PostAttackingTimer
		{
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public bool AttackThisTick => AttackTimer == AttackTick;

		public bool PostAttacking => PostAttackingTimer > 0;

		private bool atleastOneEnemyNextToMe = false;

		private bool AoMM_AI()
		{
			atleastOneEnemyNextToMe = false;
			int radius = 50 + AmuletOfManyMinionsApi.GetPetLevel(Projectile.GetOwner()) * 10;

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];

				if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < radius * radius)
				{
					atleastOneEnemyNextToMe = true;
					break;
				}
			}

			if (atleastOneEnemyNextToMe)
			{
				AttackTimer++;
				if (AttackTimer >= AttackTick + FireRate)
				{
					AttackTimer = AttackTick;
				}
			}
			else
			{
				AttackTimer = 0;
			}

			if (PostAttackingTimer > 0)
			{
				PostAttackingTimer++;
				if (PostAttackingTimer >= 25)
				{
					PostAttackingTimer = 0;
				}
			}

			if (PostAttackingTimer == 0 && AttackTimer > 0)
			{
				//Kickstart
				PostAttackingTimer = 1;
			}

			if (PostAttacking)
			{
				if (Main.rand.NextBool(2))
				{
					float scale = 1.4f + Main.rand.NextFloat() * 0.4f;
					float sizeOffset = 1.1f + Main.rand.NextFloat() * 0.3f;
					Vector2 offset = Main.rand.NextVector2CircularEdge(Projectile.width * sizeOffset, -Projectile.height * 0.25f * sizeOffset);
					float rotation = offset.ToRotation() + MathHelper.PiOver2;
					Dust dust = Dust.NewDustDirect(Projectile.Bottom + offset, 1, 1, DustID.SteampunkSteam, 0f, 0f, 50, Color.GhostWhite, scale);
					dust.velocity = offset * 0.0125f + Vector2.UnitX.RotatedBy(rotation);
					dust.noGravity = true;
				}

				AssAI.FlickerwickPetDraw(Projectile, frameCounterMaxFar: 10, frameCounterMaxClose: 10);

				return false;
			}

			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return AttackThisTick; //forcing true ignores friendly check
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (AttackThisTick)
			{
				int radius = 30 + AmuletOfManyMinionsApi.GetPetLevel(Projectile.GetOwner()) * 4;
				hitbox.Inflate(radius, radius);
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback *= 0.25f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			if (AmuletOfManyMinionsApi.IsActive(this) && PostAttacking)
			{
				lightColor.G = (byte)(lightColor.G * 0.7f);
				lightColor.B = (byte)(lightColor.B * 0.7f);

				return lightColor;
			}

			return base.GetAlpha(lightColor);
		}
	}
}
