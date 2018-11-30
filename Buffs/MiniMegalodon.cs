using Terraria;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Buffs
{
	public class MiniMegalodon : ModBuff
		{
			public override void SetDefaults()
				{
					DisplayName.SetDefault("Mini Megalodon");
					Description.SetDefault("It recognizes your strength...for now.");
					Main.buffNoTimeDisplay[Type] = true;
					Main.vanityPet[Type] = true;
				}
			public override void Update(Player player, ref int buffIndex)
				{
					player.buffTime[buffIndex] = 18000;
					player.GetModPlayer<MyPlayer>(mod).MiniMegalodon = true;
					bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("MiniMegalodon")] <= 0;
					if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
						{
							Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("MiniMegalodon"), 0, 0f, player.whoAmI, 0f, 0f);
						}
				}
		}
}