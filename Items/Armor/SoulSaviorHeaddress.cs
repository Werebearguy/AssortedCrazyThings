using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoulSaviorHeaddress : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior Headdress");
            Tooltip.SetDefault("Increases minion damage by 10%"
                + "\nIncreases your max number of minions");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 28;
            item.value = Item.sellPrice(gold: 2, silver: 80);
            item.rare = -11;
            item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.minionDamage += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType<SoulSaviorPlate>() && legs.type == mod.ItemType<SoulSaviorRobe>();
        }

        public override void UpdateArmorSet(Player player)
        {
            Lighting.AddLight(player.Center, new Vector3(0.05f, 0.05f, 0.15f));

            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            mPlayer.soulSaviorArmor = true;

            float minionFactor = (player.maxMinions >= player.slotsMinions) ? player.maxMinions : player.slotsMinions;
            float factor = (minionFactor / 10f) * player.minionDamage;

            player.thorns = factor;

            /*
             * HOW IT WORKS: thorns = 1f means that 100% of the damage an NPC does is reflected (absolute) (ONLY CONTACT DAMAGE)
             * aka if a zombie with 14 damage attacks you in your armor, you recieve 1 damage (since you have 44 defense)  but the 
             * zombie recieves 14 damage back
             * 
             * the factor is calculated above:
             * it uses maxMinions default, but if slotsMinions is modified by anything (like cheatsheet) it uses that instead
             * 
             * without any minion boosting equip, you deal 65% thorns damage (1 default slot and 4 more
             * through armor, then 130% minion damage (+10% each from each armor piece))
             * 
             * with 7 minion slots and 150% minion damage you deal 105% thorns damage (for example when equipping sigil of emergency
             * and harvester wings)
             */
             
            player.setBonus = "Empowers minions summoned by Everhallowed Lantern"
                + "\nReflects 10% contact damage per available minion slot"
                + "\nReflected damage further increased by effects that increase minion damage"
                + "\nCurrent reflected damage: " + (int)(factor * 100) + "%"
                + "\nMinions summoned by the Everhallowed Lantern become 'empowered' and gain 30% more damage";
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 3);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
