using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs.DungeonBird;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.DungeonBird
{
    [Content(ContentType.Bosses)]
    public class BabyHarvesterHandler : AssSystem
    {
        public static int WhoAmICache { get; private set; } = Main.maxProjectiles;
        public static bool HasWhoAmICache => WhoAmICache >= 0 && WhoAmICache < Main.maxProjectiles;

        public override void PostUpdateProjectiles()
        {
            TryFindBabyHarvester(out _, out int whoAmICache, fromCache: false);
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
                        //AssUtils.Print("deleted a duplicate");
                        proj.Kill();
                    }
                }

                //Do not spawn
                return;
            }

            //TODO add back after testing
            if (/*AssWorld.downedHarvester || */NPC.AnyNPCs(ModContent.NPCType<Harvester>()))
            {
                //Do not spawn another one if harvester is already slain or alive
                return;
            }

            //No alive baby harvester, spawn
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                if (player.active && !player.dead && ValidPlayer(player))
                {
                    //AssUtils.Print(Main.time + " spawning harvester");
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

            //If current player dead or not suitable anymore
            Player playerowner = babyHarvester.Player;
            if (!playerowner.dead && ValidPlayer(playerowner))
            {
                return;
            }

            //Find new suitable player, reassign owner
            bool found = false;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                if (player.active && !player.dead && ValidPlayer(player))
                {
                    //AssUtils.Print($"{Main.time} assign new player to harvester from {babyHarvester.PlayerOwner} to {i}");
                    babyHarvester.AssignPlayerOwner(i);
                    found = true;
                    break;
                }
            }

            //If not found, despawn
            if (!found)
            {
                //AssUtils.Print("despawning harvester");
                proj.Kill();
            }
        }
    }
}
