using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Buffs.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class AnimatedTomeProj : SimplePetProjBase
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/AnimatedTomeProj_0"; //temp
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Animated Tome");
			Main.projFrames[Projectile.type] = 5;
			Main.projPet[Projectile.type] = true;

			AmuletOfManyMinionsApi.RegisterFlyingPet(this, ModContent.GetInstance<AnimatedTomeBuff_AoMM>(), ModContent.ProjectileType<AnimatedTomeShotProj>());
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.BabyHornet);
			Projectile.width = 22;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.AnimatedTome = false;
			}
			if (modPlayer.AnimatedTome)
			{
				Projectile.timeLeft = 2;
			}

			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			AssAI.ZephyrfishAI(Projectile);
			AssAI.ZephyrfishDraw(Projectile);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = Mod.Assets.Request<Texture2D>("Projectiles/Pets/AnimatedTomeProj_" + mPlayer.animatedTomeType).Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			Vector2 stupidOffset = new Vector2(Projectile.width / 2 - Projectile.direction * 3f, Projectile.height / 2 + Projectile.gfxOffY);

			Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0);

			return false;
		}
	}

	//Randomly picks an appearance based on one of the gem staff bolts, while using the SetDefaults/Texture of amethyst bolt
	[Content(ContentType.AommSupport | ContentType.HostileNPCs)]
	public class AnimatedTomeShotProj : MinionShotProj_AoMM
	{
		public override int ClonedType => ProjectileID.AmethystBolt; //Base stats of amethyst bolt, meaning it won't get the penetration when its in diamond bolt appearance

		public override SoundStyle? SpawnSound => SoundID.Item8;

		private int projectileToMimic = 0;

		public override void OnSpawn(IEntitySource source)
		{
			projectileToMimic = Main.rand.Next(ProjectileID.AmethystBolt, ProjectileID.DiamondBolt + 1); //The ids are neatly in order
			AIType = projectileToMimic;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = projectileToMimic;

			return true; //No base
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write7BitEncodedInt(AIType);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AIType = reader.Read7BitEncodedInt();
		}
	}
}
