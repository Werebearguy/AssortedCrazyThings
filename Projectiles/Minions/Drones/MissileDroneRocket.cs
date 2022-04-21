using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
	[Content(ContentType.Weapons)]
	public class MissileDroneRocket : AssProjectile
	{
		private bool justCollided = false;
		private bool inflatedHitbox = false;
		private const int inflationAmount = 80;

		public override string Texture
		{
			get
			{
				return "Terraria/Images/Projectile_" + ProjectileID.RocketIII;
			}
		}

		public int FirstTarget
		{
			get
			{
				return (int)Projectile.localAI[1] - 1;
			}
			set
			{
				Projectile.localAI[1] = value + 1;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Drone Rocket");
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.RocketIII);
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 240;
			Projectile.DamageType = DamageClass.Summon;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
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
#if TML_2022_03
				Gore gore = Main.gore[Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += 1f;
				gore.velocity.Y += 1f;
				gore = Main.gore[Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += -1f;
				gore.velocity.Y += 1f;
				gore = Main.gore[Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += 1f;
				gore.velocity.Y += -1f;
				gore = Main.gore[Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
				gore.velocity *= scaleFactor10;
				gore.velocity.X += -1f;
				gore.velocity.Y += -1f;
#else
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
#endif
			}
			Projectile.position.X = Projectile.position.X + Projectile.width / 2;
			Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
			Projectile.width = 10;
			Projectile.height = 10;
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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3;
			}
			Projectile.direction = (target.Center.X < Projectile.Center.X).ToDirectionInt();
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0)
			{
				SoundEngine.PlaySound(SoundID.Item66, Projectile.Center); //62, 66, 82, 88
				Projectile.localAI[0]++;
			}
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				Projectile.tileCollide = false;
				Projectile.ai[1] = 0f;
				Projectile.alpha = 255;

				Projectile.position.X = Projectile.position.X + Projectile.width / 2;
				Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
				Projectile.width = inflationAmount;
				Projectile.height = inflationAmount;
				Projectile.position.X = Projectile.position.X - Projectile.width / 2;
				Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
				Projectile.knockBack = 8f;
			}
			else
			{
				//8f
				if (Projectile.ai[0] > 60 || Projectile.localAI[0] < 11)
				{
					Projectile.localAI[0]++;
					for (int i = 0; i < 2; i++)
					{
						float xOff = 0f;
						float yOff = 0f;
						if (i == 1)
						{
							xOff = Projectile.velocity.X * 0.5f;
							yOff = Projectile.velocity.Y * 0.5f;
						}
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + xOff, Projectile.position.Y + 3f + yOff) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, 6, 0f, 0f, 100, default(Color), 1f);
						dust.scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
						dust.velocity *= 0.2f;
						dust.noGravity = true;
						dust = Dust.NewDustDirect(new Vector2(Projectile.position.X + 3f + xOff, Projectile.position.Y + 3f + yOff) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, 31, 0f, 0f, 100, default(Color), 0.5f);
						dust.fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
						dust.velocity *= 0.05f;
					}
				}

				#region Find Target
				int targetIndex;
				if (Projectile.ai[0] > 60)
				{
					targetIndex = AssAI.FindTarget(Projectile, Projectile.Center, 1200, ignoreTiles: true);
					if (targetIndex != -1)
					{
						if (FirstTarget == -1) FirstTarget = targetIndex;
						//only home in on the first target it finds
						//(assuming during the rocket longevity there won't be another NPC spawning in distance with the same index)
						if (FirstTarget == targetIndex)
						{
							Vector2 velocity = Main.npc[targetIndex].Center + Main.npc[targetIndex].velocity * 5f - Projectile.Center;
							velocity.Normalize();
							velocity *= 6f;
							//for that nice initial curving
							//accel starts at 30, then goes down to 4
							float accel = Utils.Clamp(-(Projectile.ai[0] - 90), 4, 30);
							Projectile.velocity = (Projectile.velocity * (accel - 1) + velocity) / accel;
						}
					}
				}
				else
				{
					Projectile.velocity.Y += 0.1f; //0.015f;
				}
				#endregion

				//speedup
				if (Math.Abs(Projectile.velocity.X) < 15f && Math.Abs(Projectile.velocity.Y) < 15f)
				{
					if (Projectile.ai[0] > 60)
					{
						Projectile.velocity *= 1.1f;
					}
				}
			}
			Projectile.ai[0] += 1f;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
		}
	}
}
