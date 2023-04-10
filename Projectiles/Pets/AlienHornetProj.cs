using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class AlienHornetProj : SimplePetProjBase
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alien Hornet");
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<AlienHornetBuff_AoMM>(), ModContent.ProjectileType<AlienHornetShotProj>());
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyHornet);
			AIType = ProjectileID.BabyHornet;
			Projectile.width = 38;
			Projectile.height = 36;
			Projectile.alpha = 0;
		}

		public override void PostDraw(Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Pets/AlienHornetProj_Glowmask").Value;
			Vector2 drawPos = Projectile.position - Main.screenPosition;
			Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 4);
			frame.Y = Projectile.frameCounter % 60;
			if (frame.Y > 24)
			{
				frame.Y = 24;
			}
			frame.Y *= Projectile.height;
			Main.EntitySpriteDraw(texture, drawPos, frame, Color.White * 0.7f, 0f, Vector2.Zero, 1f, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.hornet = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.AlienHornet = false;
			}
			if (modPlayer.AlienHornet)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
		}
	}

	[Content(ContentType.AommSupport | ContentType.OtherPets)]
	public class AlienHornetShotProj : AssProjectile
	{
		private bool spawned = false;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.HornetStinger); //The bee minion one
			Projectile.scale = 0.6f;
			Projectile.aiStyle = 1;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * Projectile.Opacity;
		}

		public override void AI()
		{
			if (!spawned)
			{
				spawned = true;

				SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
			}

			if (Main.rand.NextFloat() < 0.8f)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex);
				dust.noGravity = true;
				dust.scale = 1.2f;
				dust.velocity *= 0.2f;
			}
		}
	}
}
