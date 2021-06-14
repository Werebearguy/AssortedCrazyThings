using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [LegacyName("BabyIchorSticker")]
    public class BabyIchorStickerItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<BabyIchorStickerProj>();

        public override int BuffType => ModContent.BuffType<BabyIchorStickerBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sleeping Ichor Sticker");
            Tooltip.SetDefault("Summons a Baby Ichor Sticker to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 45, copper: 30);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Vertebrae, 15).AddIngredient(ItemID.Ichor, 5).AddTile(TileID.DemonAltar).Register();
        }
    }
}
