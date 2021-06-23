using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    [Autoload]
    public class EverburningGlobalItem : AssGlobalItem
    {
        public override bool InstancePerEntity => false;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            EverburningGlobalItem myClone = (EverburningGlobalItem)base.Clone(item, itemClone);
            return myClone;
        }

        //public override void OpenVanillaBag(string context, Player player, int arg)
        //{
        //    if (context == "bossBag" && arg == ItemID.DestroyerBossBag)
        //    {
        //        player.QuickSpawnItem(ModContent.ItemType<DroneParts>());
        //    }
        //}

        public override bool CanUseItem(Item item, Player player)
        {
            //IS ACTUALLY CALLED EVERY TICK WHENEVER YOU USE THE ITEM ON THE SERVER; BUT ONLY ONCE ON THE CLIENT
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

            if (!player.CCed && (mPlayer.everburningCandleBuff || mPlayer.everfrozenCandleBuff || mPlayer.everburningShadowflameCandleBuff || mPlayer.everburningCursedCandleBuff))
            {
                if (item.active && item.damage >= 0)
                {
                    if (item.CountsAsClass<SummonDamageClass>())
                    {
                        //TODO do something with auto-fire boomerangs
                        if (item.shoot > ProjectileID.None && item.shootSpeed > 0)
                        {
                            ShootCandleDust(item, mPlayer);
                        }
                    }

                    else if (item.CountsAsClass<RangedDamageClass>())
                    {
                        if (player.HasAmmo(item, true))
                        {
                            ShootCandleDust(item, mPlayer);
                        }
                    }

                    else if (item.CountsAsClass<MagicDamageClass>() && item.mana <= player.statMana)
                    {
                        ShootCandleDust(item, mPlayer);
                    }

                    else if (item.CountsAsClass<ThrowingDamageClass>())
                    {
                        ShootCandleDust(item, mPlayer);
                    }
                }
            }
            return true;
        }

        private void SpawnMeleeDust(int type, Color color, Rectangle hitbox, Player player)
        {
            //6 is the default fire particle type
            if (player.HeldItem.damage >= 0)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, type, player.velocity.X * 0.2f + (player.direction * 3), player.velocity.Y * 0.2f, 100, color, 2f);
                dust.noGravity = true;
                dust.velocity.X *= 2f;
                dust.velocity.Y *= 2f;
            }
        }

        private void ShootCandleDust(Item item, AssPlayer mPlayer)
        {
            Player player = mPlayer.Player;
            Vector2 cm = new Vector2(Main.MouseWorld.X - player.Center.X, Main.MouseWorld.Y - player.Center.Y);
            float rand = Main.rand.NextFloat(0.7f, 1.3f);
            float velox = ((cm.X * item.shootSpeed * rand) / cm.Length());// rand makes it so it has different velocity factor (how far it flies)
            float veloy = ((cm.Y * item.shootSpeed * rand) / cm.Length());
            Vector2 velo = new Vector2(velox, veloy);
            Vector2 pos = new Vector2(player.Center.X, player.Center.Y + 8f);

            //reduce but not prevent spam from boomerang related weapons or modded damage classes
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CandleDustDummy>()] < 2)
                Projectile.NewProjectile(player.GetProjectileSource_Item(item), pos, velo + player.velocity, ModContent.ProjectileType<CandleDustDummy>(), 0, 0f, player.whoAmI);
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            if (mPlayer.everburningCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnMeleeDust(6, color, hitbox, player);
            }
            if (mPlayer.everburningCursedCandleBuff)
            {
                Color color = new Color(255, 255, 255); //(196, 255, 0);
                SpawnMeleeDust(75, color, hitbox, player);
            }
            if (mPlayer.everfrozenCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnMeleeDust(59, color, hitbox, player);
            }
            if (mPlayer.everburningShadowflameCandleBuff)
            {
                Color color = new Color(196, 0, 255);
                SpawnMeleeDust(62, color, hitbox, player);
            }
            //type 64 is ichor
        }
    }
}
