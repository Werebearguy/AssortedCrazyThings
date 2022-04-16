using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetEaterofWorldsItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<PetEaterofWorldsHead>();

		public override int BuffType => ModContent.BuffType<PetEaterofWorldsBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Cracked Worm Egg");
			Tooltip.SetDefault("Summons a tiny Eater of Worlds to follow you");
		}

		public override void SafeSetDefaults()
		{

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
				source = player.GetProjectileSource_Buff(buffIndex);
			}
			else if (item != null)
			{
				source = player.GetProjectileSource_Item(item);
			}
			else
			{
				return;
			}

			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == player.whoAmI && Array.IndexOf(PetEaterofWorldsBase.wormTypes, proj.type) > -1)
				{
					proj.Kill();
				}
			}

			//prevIndex stuff only needed for when replacing/summoning the minion segments individually

			int index = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, player.direction, -player.gravDir, ModContent.ProjectileType<PetEaterofWorldsHead>(), 0, 0f, player.whoAmI, 0f, 0f);
			int prevIndex = index;
			int off = PetEaterofWorldsBase.DISTANCE_BETWEEN_SEGMENTS;

			for (int i = 1; i <= PetEaterofWorldsBase.NUMBER_OF_BODY_SEGMENTS; i++)
			{
				index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetEaterofWorldsBody1>(), 0, 0f, player.whoAmI, index, 0f);
				Main.projectile[prevIndex].localAI[1] = index;
				prevIndex = index;
				off += PetEaterofWorldsBase.DISTANCE_BETWEEN_SEGMENTS;
				index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetEaterofWorldsBody2>(), 0, 0f, player.whoAmI, index, 0f);
				Main.projectile[prevIndex].localAI[1] = index;
				prevIndex = index;
				off += PetEaterofWorldsBase.DISTANCE_BETWEEN_SEGMENTS;
			}
			index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetEaterofWorldsTail>(), 0, 0f, player.whoAmI, index, 0f);
			Main.projectile[prevIndex].localAI[1] = index;
		}
	}
}
