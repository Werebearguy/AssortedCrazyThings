using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class FairySwarmProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1; //The texture is a dummy
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<FairySwarmBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			Projectile.alpha = 255;
			Projectile.hide = true;
			Projectile.aiStyle = -1;
		}

		private bool setAoMMParas = false;

		public override bool PreAI()
		{
			//Don't do any aomm movement
			AmuletOfManyMinionsApi.ReleaseControl(this);

			if (!setAoMMParas)
			{
				setAoMMParas = true;

				if (AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
				{
					paras.AttackFramesScaleFactor *= 0.75f;
					AmuletOfManyMinionsApi.UpdateParamsDirect(this, paras);
				}
			}

			return base.PreAI();
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.FairySwarm = false;
			}
			if (modPlayer.FairySwarm)
			{
				Projectile.timeLeft = 2;
			}

			Projectile.Center = player.Center;
			Projectile.gfxOffY = player.gfxOffY;

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private void AoMM_AI()
		{
			if (!AmuletOfManyMinionsApi.TryGetStateDirect(this, out var state) ||
				  !state.IsInFiringRange)
			{
				return;
			}

			int tier = AmuletOfManyMinionsApi.GetPetLevel(Projectile.GetOwner());

			int radius = 20 * tier + 120;

			int visualRadius = radius - 30;
			int dustAmount = (int)(visualRadius * MathHelper.TwoPi) / 360;
			for (int i = 0; i < dustAmount; i++)
			{
				Color value3 = Color.HotPink;
				Color value4 = Color.LightPink;
				if (Main.rand.NextFloat() < 0.333f)
				{
					value3 = Color.LimeGreen;
					value4 = Color.LightSeaGreen;
				}
				else if(Main.rand.NextFloat() < 0.333f)
				{
					value3 = Color.RoyalBlue;
					value4 = Color.LightBlue;
				}

				float distFactor = Main.rand.NextFloat();
				distFactor = 1f - distFactor * distFactor * distFactor; //More dust at edges
				Vector2 outwards = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * distFactor;
				Dust dust = Dust.NewDustPerfect(Projectile.Center + outwards * visualRadius, 278, new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)), 200, Color.Lerp(value3, value4, Main.rand.NextFloat()), 0.65f);
				dust.noGravity = true;
				dust.noLightEmittence = true;
				dust.noLight = true;
			}

			if (Main.myPlayer != Projectile.owner || !state.ShouldFireThisFrame)
			{
				return;
			}

			//Pick a random enemy in radius
			var targetNPCs = new List<NPC>();
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];

				if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < radius * radius && Collision.CanHitLine(Projectile.Center, 1, 1, npc.Center, 1, 1))
				{
					targetNPCs.Add(npc);
				}
			}

			if (targetNPCs.Count == 0)
			{
				return;
			}

			//make this spawn for all NPCs but with reduced damage
			int damage = Math.Max(1, (int)(1.25f * Projectile.damage / targetNPCs.Count));
			foreach (var targetNPC in targetNPCs)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), targetNPC.Center, Vector2.Zero, ModContent.ProjectileType<FairySwarmShotProj>(), damage, 2.5f + Projectile.knockBack, Main.myPlayer);
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class FairySwarmShotProj : AssProjectile
	{
		public override string Texture => "AssortedCrazyThings/Empty";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.timeLeft = 10;
			Projectile.alpha = 255;
			Projectile.hide = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			float fromPlayerToTargetX = target.Center.X - Projectile.GetOwner().Center.X;

			if (target.defense >= 20)
			{
				modifiers.ArmorPenetration += 10;
			}

			//Hit away from player
			modifiers.HitDirectionOverride = Math.Sign(fromPlayerToTargetX);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound((SoundID.Item27 with { MaxInstances = 1 }).WithVolumeScale(0.5f));

			var dustRect = Utils.CenteredRectangle(Projectile.Center, new Vector2(30));
			for (int i = 0; i < 10; i++)
			{
				//Some sparks
				Dust.NewDustDirect(dustRect.TopLeft(), dustRect.Width, dustRect.Height, 204, Scale: 0.8f);
			}
		}
	}
}
