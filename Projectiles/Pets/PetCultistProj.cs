using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetCultistProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/PetCultistProj_0"; //temp
			}
		}

		private float sinY; //depends on projectile.ai[1], no need to sync

		private ref float Sincounter => ref Projectile.ai[1];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dwarf Cultist");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
			Projectile.width = 20;
			Projectile.height = 26;
			Projectile.tileCollide = false;
		}

		private void CustomDraw()
		{
			//frame 0: idle
			//frame 0 to 3: loop back and forth while healing
			Player player = Projectile.GetOwner();

			if (player.statLife < player.statLifeMax2 / 2)
			{
				Lighting.AddLight(player.Center, Color.White.ToVector3() * 0.5f);
				Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.5f);
			}

			if (Projectile.velocity.Length() < 6f && player.statLife < player.statLifeMax2 / 2)
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter <= 30)
				{
					Projectile.frame = 0;
				}
				else if (Projectile.frameCounter <= 35)
				{
					Projectile.frame = 1;
				}
				else if (Projectile.frameCounter <= 40)
				{
					Projectile.frame = 2;
				}
				else if (Projectile.frameCounter <= 70)
				{
					Projectile.frame = 3;
				}
				else if (Projectile.frameCounter <= 75)
				{
					Projectile.frame = 2;
				}
				else if (Projectile.frameCounter <= 80)
				{
					Projectile.frame = 1;
				}
				else
				{
					Projectile.frameCounter = 0;
				}
			}
			else
			{
				Projectile.frameCounter = 0;
				Projectile.frame = 0;
			}
		}

		private void CustomAI()
		{
			Player player = Projectile.GetOwner();

			Sincounter = Sincounter >= 240 ? 0 : Sincounter + 1;
			sinY = (float)((Math.Sin((Sincounter / 120f) * MathHelper.TwoPi) - 1) * 4);

			if (Projectile.velocity.LengthSquared() < 6f * 6f && player.statLife < player.statLifeMax2 / 2)
			{
				if (Sincounter % 80 == 30)
				{
					int heal = 1;
					player.statLife += heal;
					player.HealEffect(heal, false);
				}
				Vector2 spawnOffset = new Vector2(Projectile.width * 0.5f, -20f + Projectile.height * 0.5f);
				Vector2 spawnPos = Projectile.position + spawnOffset;

				Dust dust = Dust.NewDustPerfect(spawnPos, 175, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)));
				dust.noGravity = true;
				dust.fadeIn = 1.2f;

				//basically remaps player health from (max / 2) to 0 => 0.1f to 0.9f
				float halfLife = player.statLifeMax2 / 2;
				float complicatedFormula = ((halfLife - player.statLife) * 0.8f) / (halfLife / 2) + 0.1f;

				if (Main.rand.NextFloat() < complicatedFormula)
				{
					spawnOffset = new Vector2(0f, -20f);
					spawnPos = Projectile.position + spawnOffset;
					dust = Dust.NewDustDirect(new Vector2(spawnPos.X, spawnPos.Y), Projectile.width, Projectile.height, 175, 0f, 0f, 0, default(Color), 1.5f);
					dust.noGravity = true;
					dust.fadeIn = 1f;
					dust.velocity = Vector2.Normalize(player.MountedCenter - new Vector2(0f, player.height / 2) - (Projectile.Center + spawnOffset)) * (Main.rand.NextFloat() + 5f) + Projectile.velocity * 1.5f;
				}
			}
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetCultist = false;
			}
			if (modPlayer.PetCultist)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, reverseSide: true, veloXToRotationFactor: 0.5f, offsetX: 16f, offsetY: (player.statLife < player.statLifeMax2 / 2) ? -26f : 2f);

			CustomAI();
			CustomDraw();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection != 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/PetCultistProj_" + mPlayer.petCultistType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
			Vector2 stupidOffset = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f - Projectile.gfxOffY + sinY);
			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}
}
