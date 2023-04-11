using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class BabyOcramProj : SimplePetProjBase
	{
		public int SpawnTimer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			//0 will still go into "shooting" behavior but won't shoot anything
			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<BabyOcramBuff_AoMM>(), 0);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.BabyOcram = false;
			}
			if (modPlayer.BabyOcram)
			{
				Projectile.timeLeft = 2;
			}

			AssAI.ZephyrfishAI(Projectile);
			AssAI.ZephyrfishDraw(Projectile);
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			if (AmuletOfManyMinionsApi.IsActive(this))
			{
				AoMM_AI();
			}
		}

		private void AoMM_AI()
		{
			HandleEyes();
		}

		private void HandleEyes()
		{
			if (Projectile.owner != Main.myPlayer)
			{
				return;
			}

			if (!AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var paras))
			{
				return;
			}

			int spawnType = ModContent.ProjectileType<BabyOcramShotProj>();

			int countUnlaunched = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];

				if (proj.active && proj.owner == Projectile.owner && proj.type == spawnType &&
					proj.ModProjectile is BabyOcramShotProj shotProj && !shotProj.Launched)
				{
					countUnlaunched++;
					if (countUnlaunched >= 3)
					{
						return;
					}
				}
			}

			SpawnTimer++;
			if (SpawnTimer > paras.AttackFrames)
			{
				SpawnTimer = 0;

				var source = Projectile.GetSource_FromThis();
				Projectile.NewProjectile(source, Projectile.Center, Main.rand.NextVector2Unit(), spawnType, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class BabyOcramShotProj : AssProjectile
	{
		public int ParentIdentity
		{
			get => (int)Projectile.ai[0] - 1;
			set => Projectile.ai[0] = value + 1;
		}

		public bool Launched
		{
			get => Projectile.ai[1] == 1f;
			set => Projectile.ai[1] = value ? 1f : 0f;
		}

		//Since the index might be different between clients, using ai[] for it will break stuff
		public int ParentIndex
		{
			get => (int)Projectile.localAI[0] - 1;
			set => Projectile.localAI[0] = value + 1;
		}

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = true;
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 240; //Doesn't matter
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
				Projectile test = AssUtils.NetGetProjectile(Projectile.owner, ParentIdentity, ModContent.ProjectileType<BabyOcramProj>(), out int index);
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
			if (!parent.active || parent.type != ModContent.ProjectileType<BabyOcramProj>())
			{
				Projectile.Kill();
				return;
			}

			if (parent.ModProjectile is not BabyOcramProj babyOcram)
			{
				return;
			}

			Visuals();

			Projectile.rotation = 0f;

			Projectile.friendly = false;
			Projectile.tileCollide = false;
			if (Launched)
			{
				Projectile.alpha = 0;
				Projectile.friendly = true;
				Projectile.tileCollide = true;
				return;
			}
			else
			{
				Launch(babyOcram);
				if (Launched)
				{
					return;
				}
			}

			//Move around
			Projectile.timeLeft = 2;
			AssAI.BabyEaterAI(Projectile, parent: parent, velocityFactor: 1.5f, sway: 0.1f);
			AssAI.TeleportIfTooFar(Projectile, parent.Center);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, Math.Sign(oldVelocity.X), Math.Sign(oldVelocity.Y));
			}

			return base.OnTileCollide(oldVelocity);
		}

		private void Launch(BabyOcramProj babyOcram)
		{
			if (Main.myPlayer != Projectile.owner || !AmuletOfManyMinionsApi.IsActive(babyOcram) ||
				!AmuletOfManyMinionsApi.TryGetStateDirect(babyOcram, out var state) ||
				!AmuletOfManyMinionsApi.TryGetParamsDirect(babyOcram, out var paras) ||
				!state.IsInFiringRange || state.TargetNPC is not NPC targetNPC)
			{
				return;
			}

			Launched = true;
			Projectile.timeLeft = 180;
			var targetCenter = targetNPC.Center;
			Projectile.velocity = (targetCenter - Projectile.Center).SafeNormalize(Vector2.Zero) * paras.LaunchVelocity;
			Projectile.netUpdate = true;
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

			Projectile.LoopAnimation(4);
		}
	}
}
