using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    [Autoload]
    [LegacyName("AlienHornet")]
    public class AlienHornetItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<AlienHornetProj>();

        public override int BuffType => ModContent.BuffType<AlienHornetBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex Nectar");
            Tooltip.SetDefault("Summons a friendly Alien Hornet to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Nectar, 1).AddIngredient(ItemID.FragmentVortex, 10).AddTile(TileID.DemonAltar).Register();
        }
    }
}
