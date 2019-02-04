using AssortedCrazyThings.Projectiles;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class DemonEyeContactCase : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Eye Contact Case");
            Tooltip.SetDefault("Use to switch your docile Demon Eye pet's appearance");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 28;
            item.height = 28;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 16;
            item.useTime = 16;
            item.useStyle = 4;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = Item.sellPrice(silver:1);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Lens, 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            //IS ACTUALLY CALLED EVERY TICK WHENEVER YOU USE THE ITEM ON THE SERVER; BUT ONLY ONCE ON THE CLIENT
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                if (mPlayer.eyePetIndex == -1)
                {
                    //find first occurence of a player owned eye pet
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active)
                        {
                            if (Main.projectile[i].modProjectile != null)
                            {
                                if (Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == mod.ProjectileType<DocileDemonEyeProj>())
                                {
                                    ErrorLogger.Log("had to change index of eye pet of" + player.name + " because it was -1");
                                    mPlayer.eyePetIndex = i;
                                    return true;
                                }
                            }
                        }
                    }
                }

                if (mPlayer.eyePetIndex != -1 && Main.projectile[mPlayer.eyePetIndex].active && Main.projectile[mPlayer.eyePetIndex].owner == Main.myPlayer && Main.projectile[mPlayer.eyePetIndex].type == mod.ProjectileType<DocileDemonEyeProj>())
                {
                    MiscGlobalProj gProjectile = Main.projectile[mPlayer.eyePetIndex].GetGlobalProjectile<MiscGlobalProj>(mod);

                    //only client side
                    if (Main.netMode != NetmodeID.Server)
                    {
                        mPlayer.eyePetType = gProjectile.CycleType();

                        Dust dust;
                        float factor = 1f;
                        if (Main.projectile[mPlayer.eyePetIndex].velocity.Length() > 5) factor = 2f;
                        for (int i = 0; i < 14; i++)
                        {
                            dust = Main.dust[Dust.NewDust(Main.projectile[mPlayer.eyePetIndex].position, 30, 30, 204, Main.projectile[mPlayer.eyePetIndex].velocity.X * factor, Main.projectile[mPlayer.eyePetIndex].velocity.Y * factor, 0, new Color(255, 255, 255), 0.8f)];
                            dust.noGravity = true;
                            dust.noLight = true;
                        }
                    }
                }
            }
            return true;
        }
    }
}
