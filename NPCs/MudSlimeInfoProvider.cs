using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AssortedCrazyThings.NPCs
{
	/// <summary>
	/// Special variation of CommonEnemyUICollectionInfoProvider which will count as 1 kill if chloro slime is unlocked aswell
	/// </summary>
	public class MudSlimeInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private string _persistentIdentifierToCheck;
		private bool _quickUnlock;
		private int _killCountNeededToFullyUnlock;

		public MudSlimeInfoProvider(string persistentId, bool quickUnlock)
		{
			_persistentIdentifierToCheck = persistentId;
			_quickUnlock = quickUnlock;
			_killCountNeededToFullyUnlock = GetKillCountNeeded(persistentId);
		}

		public static int GetKillCountNeeded(string persistentId)
		{
			int defaultKillsForBannerNeeded = ItemID.Sets.DefaultKillsForBannerNeeded;
			if (!ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(persistentId, out var value))
			{
				return defaultKillsForBannerNeeded;
			}

			if (!ContentSamples.NpcsByNetId.TryGetValue(value, out var value2))
			{
				return defaultKillsForBannerNeeded;
			}

			int num = Item.BannerToItem(Item.NPCtoBanner(value2.BannerID()));
			return ItemID.Sets.KillsToBanner[num];
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			int killCount = Main.BestiaryTracker.Kills.GetKillCount(_persistentIdentifierToCheck);
			BestiaryEntryUnlockState unlockStateByKillCount = GetUnlockStateByKillCount(killCount, _quickUnlock);

			if (unlockStateByKillCount == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				string chloroID = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<ChloroSlime>()];
				killCount = Main.BestiaryTracker.Kills.GetKillCount(chloroID);
				if (killCount > 0)
				{
					unlockStateByKillCount = BestiaryEntryUnlockState.CanShowPortraitOnly_1;
				}
			}
			BestiaryUICollectionInfo result = default(BestiaryUICollectionInfo);
			result.UnlockState = unlockStateByKillCount;

			return result;
		}

		public BestiaryEntryUnlockState GetUnlockStateByKillCount(int killCount, bool quickUnlock)
		{
			int killCountNeededToFullyUnlock = _killCountNeededToFullyUnlock;
			return GetUnlockStateByKillCount(killCount, quickUnlock, killCountNeededToFullyUnlock);
		}

		public static BestiaryEntryUnlockState GetUnlockStateByKillCount(int killCount, bool quickUnlock, int fullKillCountNeeded)
		{
			int num = fullKillCountNeeded / 2;
			int num2 = fullKillCountNeeded / 5;
			if (quickUnlock && killCount > 0)
				return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;

			if (killCount >= fullKillCountNeeded)
				return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;

			if (killCount >= num)
				return BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3;

			if (killCount >= num2)
				return BestiaryEntryUnlockState.CanShowStats_2;

			if (killCount >= 1)
				return BestiaryEntryUnlockState.CanShowPortraitOnly_1;

			return BestiaryEntryUnlockState.NotKnownAtAll_0;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => null;
	}
}
