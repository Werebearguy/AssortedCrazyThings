using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoulArmorHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior Coronet");
            Tooltip.SetDefault("Soul Savior Garment"
                + "\n+10% summon damage");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(gold:1);
            item.rare = -11;
            item.defense = 6; //TODO ADJUST HERE
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SoulArmorBreastplate") && legs.type == mod.ItemType("SoulArmorLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Three companion souls are fighting for you!";
            player.minionDamage *= 1.1f; //here instead of updateequip 
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            mPlayer.soulArmorMinions = true;
        }

        public override void AddRecipes()
        {
            //TODO ADJUST HERE
        }
    }
}