using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
	[Content(ContentType.Bosses)]
	[AutoloadEquip(EquipType.Head)]
	public class SoulSaviorHeaddress : AssItem
	{
		public static readonly int DamageIncrease = 10;
		public static readonly int MaxMinionsIncrease = 1;
		public static readonly int SetBonusThornsPerMinionSlot = 10;
		public static readonly int EverhallowedLanternDamageIncrease = 30;

		public static LocalizedText SetBonusText { get; private set; }

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DamageIncrease, MaxMinionsIncrease);

		public override void SetStaticDefaults()
		{
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = Item.sellPrice(gold: 2, silver: 80);
			Item.rare = 3;
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += MaxMinionsIncrease;
			player.GetDamage(DamageClass.Summon) += DamageIncrease / 100f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SoulSaviorPlate>() && legs.type == ModContent.ItemType<SoulSaviorRobe>();
		}

		public override void UpdateArmorSet(Player player)
		{
			Lighting.AddLight(player.Center, new Vector3(0.05f, 0.05f, 0.15f));

			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
			mPlayer.soulSaviorArmor = true;

			StatModifier summoner = player.GetDamage(DamageClass.Summon);
			float factor = summoner.ApplyTo(player.maxMinions / (100f / SetBonusThornsPerMinionSlot));

			player.thorns = factor;

			/*
             * HOW IT WORKS: thorns = 1f means that 100% of the damage an NPC does is reflected (absolute) (ONLY CONTACT DAMAGE)
             * aka if a zombie with 14 damage attacks you in your armor, you receive 1 damage (since you have 44 defense)  but the 
             * zombie receives 14 damage back
             * 
             * the factor is calculated above:
             * it uses maxMinions default, but if slotsMinions is modified by anything (like cheatsheet) it uses that instead
             * 
             * without any minion boosting equip, you deal 65% thorns damage (1 default slot and 4 more
             * through armor, then 130% minion damage (+10% each from each armor piece))
             * 
             * with 7 minion slots and 150% minion damage you deal 105% thorns damage
             */

			player.setBonus = SetBonusText.Format(SetBonusThornsPerMinionSlot, (int)(factor * 100), EverhallowedLanternDamageIncrease);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Ectoplasm, 3).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 16).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
