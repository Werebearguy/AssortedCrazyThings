using AssortedCrazyThings.Items.Accessories.Useful;
using AssortedCrazyThings.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Mounts
{
	[Content(ContentType.Bosses)]
	public class SigilOfTheWingBuff : AssBuff
	{
		public static readonly int Duration = SigilOfTheWing.DurationSeconds * 60;

		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//This doesn't protect against spikes or similar environmental damage
			player.immune = true;
			player.immuneTime = 2;
			player.immuneNoBlink = true;
			player.aggro -= 600;

			AssPlayer assPlayer = player.GetModPlayer<AssPlayer>();
			player.mount.SetMount(ModContent.MountType<SigilOfTheWingMount>(), player);
			if (player.buffTime[buffIndex] > Duration)
			{
				//For some reason this gets set to 3600 after AddBuff with any duration
				player.buffTime[buffIndex] = Duration;
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
