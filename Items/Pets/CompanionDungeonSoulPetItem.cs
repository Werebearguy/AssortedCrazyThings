using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class CompanionDungeonSoulPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Tooltip.SetDefault("Summons a friendly Soul to follow you.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.width = 14;
            item.height = 24;
            item.shoot = mod.ProjectileType<CompanionDungeonSoulPetProj>();
            item.buffType = mod.BuffType<CompanionDungeonSoulPetBuff>();
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
