using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class GuideVoodoorangProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guide Voodoorang");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
            projectile.height = 22;
            projectile.width = 30;
        }

        public static int type = 0;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(1, projectile.position); //player hurt sound
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(1, projectile.position); //player hurt sound
        }

        public override void PostAI()
        {
            if (projectile.lavaWet)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && !NPC.AnyNPCs(NPCID.WallofFlesh))
                {
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && Main.npc[i].type == NPCID.Guide)
                        {
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(28, -1, -1, null, i, 9999f, 10f, 0f - (float)Main.npc[i].direction);
                            }
                            Main.npc[i].StrikeNPCNoInteraction(9999, 10f, -Main.npc[i].direction);
                            NPC.SpawnWOF(projectile.position);

                            projectile.Kill();
                            //item itself doesn't get deleted but only works when the guide is in the world anyway
                        }
                    }
                }
            }
        }
    }
}
