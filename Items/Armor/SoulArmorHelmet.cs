using AssortedCrazyThings.Items.VanityArmor;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
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
                + "\n10% increased summon damage");
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
            return body.type == mod.ItemType<SoulArmorBreastplate>() && legs.type == mod.ItemType<SoulArmorLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Three companion souls are fighting for you!";
            player.minionDamage *= 1.1f; //here instead of updateequip 
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            mPlayer.soulArmorMinions = true;

            bool miniontProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<CompanionDungeonSoulMinion>()] < 3; //3
            if (miniontProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), player.direction * 0.5f, -0.5f, mod.ProjectileType<CompanionDungeonSoulMinion>(), CompanionDungeonSoulMinion.Damage, 0f, player.whoAmI, 0f, 0f);
            }

            //visual //this technically should be realized with UpdateVanitySet here and in AssPlayer, but cba
            if (Main.rand.NextBool(10)/* && some check if there is a head vanity equipped to not show the visual*/)
            {
                Vector2 randomVector = new Vector2(Main.rand.Next(16) - 7, Main.rand.Next(16) - 7); //random vector between -7 and 8
                Vector2 directionalVector = new Vector2(player.width/2 * (1 - player.direction), 0f);

                Vector2 position = player.position + directionalVector + randomVector;
                Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f) + player.direction / -2f, Main.rand.NextFloat(-1.0f, -0.5f)), 0, new Color(255, 255, 255), 1.2f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = Main.rand.NextFloat(0.8f, 1.2f);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulAwakened>(), 10);
            recipe.AddIngredient(mod.ItemType<SoulHarvesterMask>(), 1);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}