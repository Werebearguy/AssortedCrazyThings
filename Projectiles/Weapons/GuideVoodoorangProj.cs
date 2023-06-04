using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Weapons)]
	public class GuideVoodoorangProj : AssProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
			Projectile.height = 22;
			Projectile.width = 30;
		}

		public static int type = 0;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.PlayerHit, Projectile.position); //player hurt sound
			return true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(SoundID.PlayerHit, Projectile.position); //player hurt sound
		}

		public override void PostAI()
		{
			if (Projectile.lavaWet)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && !NPC.AnyNPCs(NPCID.WallofFlesh))
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.active && npc.type == NPCID.Guide)
						{
							if (npc.IsNPCValidForBestiaryKillCredit())
							{
								Main.BestiaryTracker.Kills.RegisterKill(npc);
							}

							var hit = new NPC.HitInfo
							{
								Knockback = 10,
								HitDirection = -npc.direction,
								InstantKill = true
							};
							npc.StrikeNPC(hit);
							if (Main.netMode != NetmodeID.SinglePlayer)
							{
								NetMessage.SendStrikeNPC(npc, hit);
							}

							NPC.SpawnWOF(Projectile.position);

							Projectile.Kill();
							break;
							//item itself doesn't get deleted but only works when the guide is in the world anyway
						}
					}
				}
			}
		}
	}
}
