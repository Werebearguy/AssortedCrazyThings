using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class aaaSoulLightPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("aaaSoulLightPet");
            Tooltip.SetDefault("Summons a friendly aaaSoulLightPet to follow you.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.width = 14;
            item.height = 24;
            item.shoot = mod.ProjectileType("aaaSoulLightPetProj");
            item.buffType = mod.BuffType("aaaSoulLightPetBuff");
            item.rare = -11;
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }


        //TODO add a recipe at the crystal ball (because its based on the DD2 ogre drop and thats kinda at that hardmode stage)
    }
}
