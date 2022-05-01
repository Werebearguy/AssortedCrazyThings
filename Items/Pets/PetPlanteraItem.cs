using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetPlanteraItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetPlanteraProj>();

		public override int BuffType => ModContent.BuffType<PetPlanteraBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Potted Plantera Seed");
			Tooltip.SetDefault("Summons a Plantera Sprout to watch over you"
				+ "\n'It's a mean and green'");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}

		public static void Spawn(Player player, int buffIndex = -1, Item item = null)
		{
			if (Main.myPlayer != player.whoAmI)
			{
				//Clientside only
				return;
			}

			IEntitySource source;
			if (buffIndex > -1)
			{
				source = player.GetSource_Buff(buffIndex);
			}
			else if (item != null)
			{
				source = player.GetSource_ItemUse(item);
			}
			else
			{
				return;
			}

			int tentacle = ModContent.ProjectileType<PetPlanteraProjTentacle>();
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == player.whoAmI && proj.type == tentacle)
				{
					proj.Kill();
				}
			}

			Projectile.NewProjectile(source, player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, ModContent.ProjectileType<PetPlanteraProj>(), PetPlanteraProj.ContactDamage, 1f, player.whoAmI, 0f, 0f);

			if (player.ownedProjectileCounts[tentacle] == 0)
			{
				for (int i = 0; i < 4; i++)
				{
					Projectile.NewProjectile(source, player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, tentacle, 1, 0f, player.whoAmI, 0f, 0f);
				}
			}
		}
	}
}
