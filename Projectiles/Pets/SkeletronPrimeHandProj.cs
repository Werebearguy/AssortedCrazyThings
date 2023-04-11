using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SkeletronPrimeHandProj : SimplePetProjBase
	{
		private bool setAoMMVisualsThisTick = false;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/SkeletronPrimeHandProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			//Some forms spawn projectile
			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<SkeletronPrimeHandBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			AIType = ProjectileID.BabyEater;
			Projectile.aiStyle = -1;
			Projectile.width = 24;
			Projectile.height = 24;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.SkeletronPrimeHand = false;
			}
			if (modPlayer.SkeletronPrimeHand)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.BabyEaterAI(Projectile, sway: 0.8f);
			AssAI.BabyEaterDraw(Projectile);

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private int oldHandType = -1; //Reduces calls to TryGet/UpdateParamsDirect

		private void AoMM_AI()
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			int handType = mPlayer.skeletronPrimeHandType;

			if (oldHandType != handType)
			{
				oldHandType = handType;

				if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
				{
					//Dynamically set projectile
					if (handType == 0)
					{
						//Make the cannon variant shoot a bomb
						paras.FiredProjectileId = ModContent.ProjectileType<SkeletronPrimeHandBombShotProj>();
					}
					else if (handType == 3)
					{
						//Make the laser variant shoot a laser
						paras.FiredProjectileId = ModContent.ProjectileType<SkeletronPrimeHandLaserShotProj>();
					}
					else
					{
						paras.FiredProjectileId = null;
					}
					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}
			}

			//Need state to adjust rotation
			if (handType != 0 && handType != 3 || !AmuletOfManyMinionsApi.IsAttacking(this) ||
				!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) ||
				!state.IsInFiringRange || state.TargetNPC is not NPC targetNPC)
			{
				return;
			}

			Vector2 toTarget = targetNPC.Center - Projectile.Center;

			Projectile.spriteDirection = Projectile.direction = (toTarget.X > 0).ToDirectionInt();
			Projectile.rotation = toTarget.ToRotation() + MathHelper.PiOver2;
			setAoMMVisualsThisTick = true;
		}

		public override void PostAI()
		{
			if (!setAoMMVisualsThisTick)
			{
				Projectile.rotation = Projectile.velocity.X * -0.08f;
				Projectile.spriteDirection = Projectile.direction = 1;
			}
			setAoMMVisualsThisTick = false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Projectile.GetOwner();
			AssUtils.DrawSkeletronLikeArms("AssortedCrazyThings/Projectiles/Pets/SkeletronPrimeHand_Arm", Projectile.Center, player.Center + new Vector2(0, player.gfxOffY), centerPad: -20f, direction: 0);

			PetPlayer mPlayer = player.GetModPlayer<PetPlayer>();
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/SkeletronPrimeHandProj_" + mPlayer.skeletronPrimeHandType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2, Projectile.height / 2);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset;
			Vector2 drawOrigin = bounds.Size() / 2;
			drawOrigin.Y += Projectile.height / 2;

			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, Projectile.rotation, drawOrigin, 1f, Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SkeletronPrimeHandBombShotProj : AssProjectile
	{
		private bool spawned = false;
		private bool justCollided = false;
		private bool inflatedHitbox = false;
		private const int inflationAmount = 60;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Summon;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;

			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Projectile.position.X = Projectile.position.X + Projectile.width / 2;
			Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
			Projectile.width = inflationAmount;
			Projectile.height = inflationAmount;
			Projectile.position.X = Projectile.position.X - Projectile.width / 2;
			Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
			for (int i = 0; i < 10; i++) //40
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f);
				dust.velocity *= 2f; //3f
				if (Main.rand.NextBool(2))
				{
					dust.scale = 0.5f;
					dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
			for (int i = 0; i < 17; i++) //70
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
				dust.noGravity = true;
				dust.velocity *= 4f; //5f
				dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
				dust.velocity *= 2f;
			}
			for (int i = 0; i < 2; i++) //3
			{
				float scaleFactor10 = 0.33f;
				if (i == 1)
				{
					scaleFactor10 = 0.66f;
				}
				if (i == 2)
				{
					scaleFactor10 = 1f;
				}
				var entitySource = Projectile.GetSource_FromAI();
				Gore gore = Main.gore[Gore.NewGore(entitySource, new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += 1f;
				gore.velocity.Y += 1f;
				gore = Main.gore[Gore.NewGore(entitySource, new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += -1f;
				gore.velocity.Y += 1f;
				gore = Main.gore[Gore.NewGore(entitySource, new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += 1f;
				gore.velocity.Y += -1f;
				gore = Main.gore[Gore.NewGore(entitySource, new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += -1f;
				gore.velocity.Y += -1f;
			}
			Projectile.position.X = Projectile.position.X + Projectile.width / 2;
			Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.position.X = Projectile.position.X - Projectile.width / 2;
			Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			if (!inflatedHitbox) justCollided = true;
			return false;
		}

		//kind of a useless hack but I couldn't work out how vanilla makes the hitbox increase on tile collide
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (justCollided)
			{
				justCollided = false;
				inflatedHitbox = true;
				hitbox.Inflate(inflationAmount / 2, inflationAmount / 2);
				Projectile.timeLeft = 3;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3;
			}
			Projectile.direction = (target.Center.X < Projectile.Center.X).ToDirectionInt();

			Projectile.damage = (int)(Projectile.damage * 0.8f);
		}

		public override void AI()
		{
			if (!spawned)
			{
				spawned = true;

				SoundEngine.PlaySound(SoundID.Item14.WithVolumeScale(0.7f), Projectile.Center);
			}

			if (Projectile.alpha > 0 && Projectile.timeLeft > 3)
			{
				Projectile.alpha -= 40;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				Projectile.tileCollide = false;
				Projectile.alpha = 255;

				Projectile.position.X = Projectile.position.X + Projectile.width / 2;
				Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
				Projectile.width = inflationAmount;
				Projectile.height = inflationAmount;
				Projectile.position.X = Projectile.position.X - Projectile.width / 2;
				Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
			}

			Projectile.rotation += Projectile.velocity.X * 0.06f;

			Projectile.LoopAnimation(4);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class SkeletronPrimeHandLaserShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.MiniRetinaLaser; //Optic staff laser

		public override void OnSpawn(IEntitySource source)
		{
			//Due to increased extraUpdates (2), but too slow (0.25f) looks unnatural for a laser
			Projectile.velocity *= 0.5f;
		}
	}
}
