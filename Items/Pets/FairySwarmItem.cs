using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.DroppedPets)]
    public class FairySwarmItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<FairySwarmProj>();

        public override int BuffType => ModContent.BuffType<FairySwarmBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottle of Assorted Fairies");
            Tooltip.SetDefault("Summons several fairies to swarm around you");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.noUseGraphic = true;
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
