using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    [Content(ContentType.Weapons)]
    public class GuideVoodoorangProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guide Voodoorang");
        }

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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
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
                        if (Main.npc[i].active && Main.npc[i].type == NPCID.Guide)
                        {
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, i, 9999f, 10f, -Main.npc[i].direction);
                            }
                            Main.npc[i].StrikeNPCNoInteraction(9999, 10f, -Main.npc[i].direction);
                            NPC.SpawnWOF(Projectile.position);

                            Projectile.Kill();
                            //item itself doesn't get deleted but only works when the guide is in the world anyway
                        }
                    }
                }
            }
        }
    }
}
