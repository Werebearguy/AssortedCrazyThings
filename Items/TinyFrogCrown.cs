using AssortedCrazyThings.Projectiles;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    public class TinyFrogCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Prince's Tiny Crown");
            Tooltip.SetDefault("'Give your Lifelike Mechanical Frog a Princelike Tiny Crown'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 18;
            item.height = 16;
            item.maxStack = 1;
            item.rare = -11;
            item.useAnimation = 16;
            item.useTime = 16;
            item.useStyle = 4;
            item.UseSound = SoundID.Item1;
            item.consumable = false;
            item.value = Item.sellPrice(silver:12);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 1);
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
                int frogIndex = -1;
                //find first occurence of a player owned frog pet
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active)
                    {
                        if (Main.projectile[i].modProjectile != null)
                        {
                            if (Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == mod.ProjectileType<LifelikeMechanicalFrog>())
                            {
                                frogIndex = i;
                                break;
                            }
                        }
                    }
                }

                if (frogIndex != -1 && Main.projectile[frogIndex].active && Main.projectile[frogIndex].owner == Main.myPlayer && Main.projectile[frogIndex].type == mod.ProjectileType<LifelikeMechanicalFrog>())
                {
                    //only client side
                    if (Main.netMode != NetmodeID.Server)
                    {
                        mPlayer.mechFrogCrown = !mPlayer.mechFrogCrown;
                        //pet projectile then checks in PreDraw for that bool

                        Dust dust;
                        float factor = 1f;
                        if (Main.projectile[frogIndex].velocity.Length() > 5) factor = 2f;
                        for (int i = 0; i < 14; i++)
                        {
                            dust = Main.dust[Dust.NewDust(Main.projectile[frogIndex].position, 18, 28, 204, Main.projectile[frogIndex].velocity.X * factor, Main.projectile[frogIndex].velocity.Y * factor, 0, new Color(255, 255, 255), 0.8f)];
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
