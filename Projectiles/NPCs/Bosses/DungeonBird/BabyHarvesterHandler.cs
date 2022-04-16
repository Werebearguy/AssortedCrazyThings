using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using System;
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

            TryGiveSoulBuffToEnemies();
        }

        /// <summary>
        /// Returns true if NPC isn't in soulbuffblacklist or is a worm body or tail
        /// </summary>
        private static bool EligibleToReceiveSoulBuff(NPC npc)
        {
            if (Array.BinarySearch(AssortedCrazyThings.soulBuffBlacklist, npc.type) >= 0)
            {
                return false;
            }
            return !AssUtils.IsWormBodyOrTail(npc);
        }

        private static void TryGiveSoulBuffToEnemies()
        {
            if (TryFindBabyHarvester(out Projectile proj, out _) &&
                proj.ModProjectile is BabyHarvesterProj babyHarvester && babyHarvester.HasValidPlayerOwner)
            {
                GiveSoulBuffToNearbyNPCs(babyHarvester.Player, proj.Center);
            }
            else
            {
                int index = NPC.FindFirstNPC(AssortedCrazyThings.harvester);
                if (index <= -1)
                {
                    return;
                }
                else
                {
                    var harvesterCenter = Main.npc[index].Center;
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player player = Main.player[i];

                        if (player.active && !player.dead)
                        {
                            GiveSoulBuffToNearbyNPCs(player, harvesterCenter);
                        }
                    }
                }
            }
        }

        private static void GiveSoulBuffToNearbyNPCs(Player player, Vector2 position)
        {
            if (!IsTurningInvalidPlayer(player, out _) && (ValidPlayer(player) || player.DistanceSQ(position) < 2880 * 2880)) //one and a half screens or in suitable location
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && !npc.SpawnedFromStatue &&
                        npc.type != AssortedCrazyThings.harvester && npc.DistanceSQ(player.Center) < 2880 * 2880 &&
                        !npc.GetGlobalNPC<HarvesterGlobalNPC>().shouldSoulDrop)
                    {
                        if (EligibleToReceiveSoulBuff(npc))
                        {
                            npc.AddBuff(ModContent.BuffType<SoulBuff>(), 60, true);
                        }
                    }
                }
            }
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

        /// <summary>
        /// This handles spawning/despawning, utilizing a delayed condition check to handle edge cases
        /// </summary>
        public static bool ValidPlayer(Player player)
        {
            return player.GetModPlayer<BabyHarvesterPlayer>().Valid;
        }

        /// <summary>
        /// True if the condition is about to turn false
        /// </summary>
        public static bool IsTurningInvalidPlayer(Player player, out int timeLeft)
        {
            return player.GetModPlayer<BabyHarvesterPlayer>().IsTurningInvalid(out timeLeft);
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

            if (AssWorld.downedHarvester || NPC.AnyNPCs(AssortedCrazyThings.harvester))
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
                    AssWorld.Message("You hear a faint cawing from the dungeon.", Harvester.deathColor);

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
