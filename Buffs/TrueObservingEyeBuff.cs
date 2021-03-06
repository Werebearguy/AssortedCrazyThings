﻿using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class TrueObservingEyeBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("True Observing Eye");
            Description.SetDefault("A tiny True Eye of Cthulhu is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().TrueObservingEye = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<TrueObservingEyeProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ModContent.ProjectileType<TrueObservingEyeProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
