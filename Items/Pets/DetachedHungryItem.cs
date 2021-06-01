using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [LegacyName("DetachedHungry")]
    public class DetachedHungryItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<DetachedHungryProj>();

        public override int BuffType => ModContent.BuffType<DetachedHungryBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unconscious Hungry");
            Tooltip.SetDefault("Summons a detached Hungry to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 4).AddIngredient(ItemID.RottenChunk, 10).AddTile(TileID.DemonAltar).Register();
        }
    }
}
