using Terraria;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Buffs
{
	public class pet_pukebaby_01 : ModBuff
		{
			public override void SetDefaults()
				{
					DisplayName.SetDefault("Lil' Ichy");
					Description.SetDefault("A baby Ichor Sticker is following you.");
					Main.buffNoTimeDisplay[Type] = true;
					Main.vanityPet[Type] = true;
				}
			public override void Update(Player player, ref int buffIndex)
				{
					player.buffTime[buffIndex] = 18000;
					player.GetModPlayer<MyPlayer>(mod).pet_pukebaby_01 = true;
					bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("pet_pukebaby_01")] <= 0;
					if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
						{
							Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("pet_pukebaby_01"), 0, 0f, player.whoAmI, 0f, 0f);
						}
				}
		}
}
