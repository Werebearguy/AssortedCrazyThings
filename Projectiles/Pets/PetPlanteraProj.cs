using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	//cannot be dyed since it counts as a minion and deals damage
	[Content(ContentType.DroppedPets)]
	public class PetPlanteraProj : SimplePetProjBase
	{
		public const int ContactDamage = 20;
		public const int ImmunityCooldown = 60;

		private const float STATE_IDLE = 0f;
		private const float STATE_ATTACK = 1f;

		public float AI_STATE
		{
			get
			{
				return Projectile.ai[0];
			}
			set
			{
				Projectile.ai[0] = value;
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
				.WithOffset(-10, -10f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<PetPlanteraBuff_AoMM>(), null);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyEater);
			Projectile.width = 46;
			Projectile.height = 46;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minion = false; //minion = false to prevent it from being "replaced" after casting other summons and then spawning its tentacles again
			Projectile.minionSlots = 0f;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = ImmunityCooldown;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetPlantera = false;
			}
			if (modPlayer.PetPlantera)
			{
				Projectile.timeLeft = 2;
			}

			#region Handle State
			int targetIndex = AssAI.FindTarget(Projectile, player.Center, 300); //check for player surrounding
			if (targetIndex == -1)
			{
				if (AI_STATE == STATE_ATTACK)
				{
					targetIndex = AssAI.FindTarget(Projectile, player.Center, 400); //check for player surrounding
					if (targetIndex == -1)
					{
						AI_STATE = STATE_IDLE;
						Projectile.netUpdate = true;
					}
				}
				else
				{
					//keep idling
				}
			}
			else //target found
			{
				if (AI_STATE == STATE_IDLE)
				{
					AI_STATE = STATE_ATTACK;
					Projectile.netUpdate = true;
				}
				else
				{
					//keep attacking
				}
			}
			#endregion

			#region Act Upon State
			if (AI_STATE == STATE_IDLE)
			{
				Projectile.friendly = false;
				AssAI.BabyEaterAI(Projectile, originOffset: new Vector2(0f, -60f));

				AssAI.BabyEaterDraw(Projectile, 5);
				Projectile.rotation += 3.14159f;
			}
			else //STATE_ATTACK
			{
				Projectile.friendly = true;

				if (targetIndex != -1)
				{
					NPC npc = Main.npc[targetIndex];
					Vector2 distanceToTargetVector = npc.Center - Projectile.Center;
					float distanceToTarget = distanceToTargetVector.Length();

					if (distanceToTarget > 30f)
					{
						distanceToTargetVector.Normalize();
						distanceToTargetVector *= 8f;
						Projectile.velocity = (Projectile.velocity * (16f - 1) + distanceToTargetVector) / 16f;

						Projectile.rotation = distanceToTargetVector.ToRotation() + 1.57f;
					}
				}

				AssAI.BabyEaterDraw(Projectile, 3);
			}
			#endregion
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.isAPreviewDummy)
			{
				return true;
			}

			int tentacleCount = 0;

			string chainPath = Texture + "_Chain";
			int tentacleType = ModContent.ProjectileType<PetPlanteraProjTentacle>();
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];
				if (other.active && Projectile.owner == other.owner && other.type == tentacleType)
				{
					AssUtils.DrawTether(chainPath, other.Center, Projectile.Center);
					tentacleCount++;
				}
				if (tentacleCount >= 4) break;
			}

			AssUtils.DrawTether(chainPath, Projectile.GetOwner().Center, Projectile.Center);
			return true;
		}
	}

	[Content(ContentType.DroppedPets)]
	public class PetPlanteraProjTentacle : SimplePetProjBase
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

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.aiStyle = -1;
			Projectile.width = 20;
			Projectile.height = 22;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.PetPlantera = false;
			}
			if (modPlayer.PetPlantera)
			{
				Projectile.timeLeft = 2;
			}

			#region Find Parent
			//set parent when spawned
			if (ParentIndex < 0)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<PetPlanteraProj>() && Projectile.owner == Main.projectile[i].owner)
					{
						ParentIndex = i;
						//projectile.netUpdate = true;
						break;
					}
				}
			}

			//if something goes wrong, abort mission
			if (ParentIndex < 0 || (ParentIndex > -1 && Main.projectile[ParentIndex].type != ModContent.ProjectileType<PetPlanteraProj>()))
			{
				Projectile.Kill();
				return;
			}
			#endregion

			//offsets so the tentacles are distributed evenly
			float offsetX = 0;
			float offsetY = 0;
			switch (Projectile.whoAmI % 4)
			{
				case 0:
					offsetX = -120 + Main.rand.Next(20);
					offsetY = 0;
					break;
				case 1:
					offsetX = -120 + Main.rand.Next(20);
					offsetY = 120;
					break;
				case 2:
					offsetX = 0 - Main.rand.Next(20);
					offsetY = 120;
					break;
				default: //case 3
					break;
			}

			Projectile parent = Main.projectile[ParentIndex];
			if (!parent.active)
			{
				Projectile.active = false;
				return;
			}

			//velocityFactor: 1.5f + (projectile.whoAmI % 4) * 0.8f so all tentacles don't share the same movement 
			AssAI.ZephyrfishAI(Projectile, parent: parent, velocityFactor: 1.5f + (Projectile.whoAmI % 4) * 0.8f, random: true, swapSides: 1, offsetX: offsetX, offsetY: offsetY);
			Vector2 between = parent.Center - Projectile.Center;
			Projectile.spriteDirection = 1;
			Projectile.rotation = between.ToRotation() - MathHelper.PiOver2;

			AssAI.ZephyrfishDraw(Projectile, 3 + Main.rand.Next(3));
		}
	}
}
