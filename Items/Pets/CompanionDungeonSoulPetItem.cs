using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CompanionDungeonSoulPetItem : CaughtDungeonSoulBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Tooltip.SetDefault("Summons a friendly Soul to light your way"
                               + "\nLight pet slot");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<CompanionDungeonSoulPetProj>(), ModContent.BuffType<CompanionDungeonSoulPetBuff>());
            frame2CounterCount = 8.0;
            animatedTextureSelect = 0;

            Item.width = 26;
            Item.height = 28;
            Item.rare = -11;

            Item.value = Item.sellPrice(silver: 50);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 3600);
            return false;
        }

        //hardmode recipe
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Bone, 2).AddTile(TileID.CrystalBall).Register();
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem2>(), 1).AddTile(TileID.CrystalBall).Register();
        }
    }
}
