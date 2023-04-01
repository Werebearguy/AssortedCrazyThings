using AssortedCrazyThings.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Mounts
{
	[Content(ContentType.Bosses)]
	public class SigilOfTheWingBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blessing of the Wing");
			Description.SetDefault("Your spirit hangs on by a thread");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.immune = true;
			player.immuneTime = 2;
			player.immuneNoBlink = true;

			AssPlayer assPlayer = player.GetModPlayer<AssPlayer>();
			player.mount.SetMount(ModContent.MountType<SigilOfTheWingMount>(), player);
			if (player.buffTime[buffIndex] > 600)
			{
				//For some reason this gets set to 3600 after AddBuff with any duration
				player.buffTime[buffIndex] = 600;
			}
			else if (player.buffTime[buffIndex] <= 2)
			{
				assPlayer.SigilOfTheWingStop(ref buffIndex);
			}

			player.RemoveAllGrapplingHooks();
			player.controlUseItem = false;
			player.controlHook = false;
			player.controlMount = false;
			player.releaseMount = false;

			assPlayer.hidePlayer = true;
			assPlayer.sigilOfTheWingOngoing = true;
		}
	}
}
