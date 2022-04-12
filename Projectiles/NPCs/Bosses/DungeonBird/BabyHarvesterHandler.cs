using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.DungeonBird
{
    //TODO test in MP
    [Content(ContentType.Bosses)]
    public class BabyHarvesterHandler : AssSystem
    {
        public static int WhoAmICache { get; private set; } = Main.maxProjectiles;
        public static bool HasWhoAmICache => WhoAmICache >= 0 && WhoAmICache < Main.maxProjectiles;

        public override void PostUpdateProjectiles()
        {
            int whoAmICache;
            TryFindBabyHarvester(out _, out whoAmICache, fromCache: false);
            WhoAmICache = whoAmICache;

            TrySpawnBabyHarvester();

            ValidateBabyHarvester();
        }

        public static bool TryFindBabyHarvester(out Projectile proj, out int index, bool fromCache = true)
        {
            proj = null;
            index = Main.maxProjectiles;

            if (!fromCache)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile other = Main.projectile[i];

                    if (other.active && ValidProjectile(other))
                    {
                        proj = other;
                        index = i;
                        break;
                    }
                }
            }
            else if (HasWhoAmICache)
            {
                proj = Main.projectile[WhoAmICache];
                index = WhoAmICache;
            }

            return proj != null;
        }

        private static bool ValidProjectile(Projectile proj)
        {
            return proj.ModProjectile is BabyHarvesterProj;
        }

        private static bool ValidPlayer(Player player)
        {
            return player.ZoneSkyHeight; //TODO change to Dungeon
        }

        private static void TrySpawnBabyHarvester()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            if (HasWhoAmICache)
            {
                //Delete any possible duplicates
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];

                    if (proj.active && ValidProjectile(proj) && i != WhoAmICache)
                    {
                        AssUtils.Print("deleted a duplicate");
                        proj.Kill();
                    }
                }

                //Do not spawn
                return;
            }

            //No alive baby harvester, spawn
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                if (player.active && !player.dead && ValidPlayer(player))
                {
                    AssUtils.Print("spawning harvester");
                    BabyHarvesterProj.Spawn(player);
                    break;
                }
            }
        }

        private static void ValidateBabyHarvester()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            if (!TryFindBabyHarvester(out Projectile proj, out _))
            {
                return;
            }

            if (!(proj.ModProjectile is BabyHarvesterProj babyHarvester && babyHarvester.HasValidPlayerOwner))
            {
                return;
            }

            //If current player dead or not in dungeon anymore
            if (!babyHarvester.Player.dead && ValidPlayer(babyHarvester.Player))
            {
                return;
            }

            //Find new player in dungeon, reassign player
            bool found = false;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                if (player.active && !player.dead && ValidPlayer(player))
                {
                    AssUtils.Print($"assign new player to harvester from {babyHarvester.PlayerOwner} to {i}");
                    babyHarvester.AssignPlayerOwner(i);
                    found = true;
                    break;
                }
            }

            //If not found, despawn
            if (!found)
            {
                AssUtils.Print("despawning harvester");
                proj.Kill();
            }
        }
    }
}
