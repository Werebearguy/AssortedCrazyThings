using AssortedCrazyThings.Base.Data;
using AssortedCrazyThings.Base.Handlers.SpawnedNPCHandler;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AssortedCrazyThings.Base.Chatter
{
	[Content(ConfigurationSystem.AllFlags)]
	public class ChatterSystem : AssSystem
	{
		public const int DefaultCooldown = 2 * 60;
		public const int GlobalCooldownMax = 5 * 60;
		public static float GlobalCooldown { get; private set; } //So that messages can't be spawned at the same time (unrealistic)

		private static int prevInvasionType = 0;
		private static bool prevBloodMoon = false;

		public static Dictionary<ChatterType, ChatterHandler> Dict => ChatterHandlerLoader.Dict;

		public static Dictionary<ChatterSource, Type> SourceToParamTypes { get; private set; }

		public static void SetGlobalCooldown()
		{
			GlobalCooldown = GlobalCooldownMax;
		}

		private static void LoadSourceToParamTypes()
		{
			//Important to only use params specified for each source when spawning it
			SourceToParamTypes = new();
			foreach (var source in Enum.GetValues<ChatterSource>())
			{
				SourceToParamTypes[source] = typeof(DefaultChatterParams);
			}

			//Overrides here
			SourceToParamTypes[ChatterSource.Attacking] = typeof(AttackingChatterParams);
			SourceToParamTypes[ChatterSource.PlayerHurt] = typeof(PlayerHurtChatterParams);
			SourceToParamTypes[ChatterSource.BossSpawn] = typeof(BossSpawnChatterParams);
			SourceToParamTypes[ChatterSource.BossDefeat] = typeof(BossDefeatChatterParams);
			SourceToParamTypes[ChatterSource.ArmorEquipped] = typeof(ArmorEquipChatterParams);
			SourceToParamTypes[ChatterSource.ItemSelected] = typeof(ItemSelectedChatterParams);
			SourceToParamTypes[ChatterSource.InvasionChanged] = typeof(InvasionChangedChatterParams);
			SourceToParamTypes[ChatterSource.BloodMoonChanged] = typeof(BloodMoonChangedChatterParams);
		}

		public override void OnModLoad()
		{
			LoadSourceToParamTypes();

			SpawnedNPCSystem.OnSpawnedNPC += OnSpawnedBossHook;

			AssPlayer.OnSlainBoss += OnSlainBossHook;

			On_Main.ReportInvasionProgress += OnOOAStarts;

			//MP handled in HijackGetData
			Terraria.GameContent.Events.On_DD2Event.SetEnemySpawningOnHold += OnOOANewWave_SP;
		}

		public override void OnModUnload()
		{
			SourceToParamTypes = null;
			ChatterHandlerLoader.Unload();

			SpawnedNPCSystem.OnSpawnedNPC -= OnSpawnedBossHook;

			AssPlayer.OnSlainBoss -= OnSlainBossHook;
		}

		public static void OnSpawnedBoss(NPC npc, float distSQ)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnSpawnedBoss(npc, distSQ);
			}
		}

		public static void OnSlainBoss(Player player, int type)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnSlainBoss(player, type);
			}
		}

		public static void OnOOAStarts(int forHowLong)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnOOAStarts(forHowLong);
			}
		}

		public static void OnOOANewWave(int forHowLong)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnOOANewWave(forHowLong);
			}
		}

		public static void OnEnterWorld(Player player)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnEnterWorld(player);
			}
		}

		public static void OnItemSelected(Player player, int currentItemType, int prevItemType)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnItemSelected(player, currentItemType, prevItemType);
			}
		}

		public static void OnArmorEquipped(Player player, EquipSnapshot currentEquips, EquipSnapshot prevEquips)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnArmorEquipped(player, currentEquips, prevEquips);
			}
		}

		public static void OnPlayerHurt(Player player, Entity entity, Player.HurtInfo hurtInfo)
		{
			foreach (var handler in Dict.Values)
			{
				handler.OnPlayerHurt(player, entity, hurtInfo);
			}
		}

		public override void PostUpdatePlayers()
		{
			float reduceAmount = ClientConfig.Instance.GoblinUnderlingChatterFreq / 100f;

			if (GlobalCooldown > 0)
			{
				GlobalCooldown -= reduceAmount;
				if (GlobalCooldown < 0)
				{
					GlobalCooldown = 0;
				}
			}

			foreach (var handler in Dict.Values)
			{
				foreach (var gen in handler.Generators)
				{
					gen.UpdateCooldowns(reduceAmount);
				}

				if (Main.invasionProgressNearInvasion && Main.invasionType != prevInvasionType)
				{
					handler.OnInvasionChanged(Main.invasionType, prevInvasionType);
				}
				if (Main.bloodMoon != prevBloodMoon)
				{
					handler.OnBloodMoonChanged(Main.bloodMoon, prevBloodMoon);
				}
			}

			if (Main.invasionProgressNearInvasion)
			{
				prevInvasionType = Main.invasionType;
			}
			prevBloodMoon = Main.bloodMoon;
		}

		public override void PostSetupContent()
		{
			//Localization structure:
			//Chatter: { ChatterHandler1: { ChatterGenerator1: { ChatterSource1: { Message1: Value1, Message2: Value2 }, ChatterSource2: { Message1: Value1, Message2: Value2 }, } } }
			foreach (var pair in Dict)
			{
				//ChatterHandler1
				foreach (var gen in pair.Value.Generators)
				{
					foreach (var chatter in gen.Chatters)
					{
						//ChatterSource1
						var registeredKeys = new List<string>();
						foreach (var message in chatter.Value.Messages)
						{
							//Message1: Value1
							string key = pair.Value.Mod.GetLocalizationKey($"Chatter.{pair.Key}.{gen.Key}.{chatter.Key}.{message.Key}");
							if (registeredKeys.Contains(message.Key))
							{
								throw new Exception($"'{key}' is already registered. Make sure there are no duplicate {nameof(ChatterMessage)}.{nameof(ChatterMessage.Key)}");
							}
							message.Text = Language.GetOrRegister(key, () => message.Key);
							registeredKeys.Add(message.Key);
						}
					}
				}
			}
		}

		private static void OnOOANewWave_SP(Terraria.GameContent.Events.On_DD2Event.orig_SetEnemySpawningOnHold orig, int forHowLong)
		{
			orig(forHowLong);

			//SP handle for OOANewWave
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				return;
			}

			DetectOOAWave(forHowLong);
		}

		private static void OnOOAStarts(On_Main.orig_ReportInvasionProgress orig, int progress, int progressMax, int icon, int progressWave)
		{
			orig(progress, progressMax, icon, progressWave);
			//inlined? Hook doesn't get called
			/*

			if (Main.netMode != NetmodeID.Server)
			{
				//ReportInvasionProgress(0, 1, 3, 1) //3 is the dd2 event icon
				if (progress == 0 && progressMax == 1 && icon == 3 && progressWave == 1)
				{
					OnOOAStarts(forHowLong);
				}
			}*/
		}

		private static void OnSlainBossHook(Player player, int type)
		{
			if (Main.myPlayer != player.whoAmI)
			{
				return;
			}

			OnSlainBoss(player, type);
		}

		private static void OnSpawnedBossHook(NPC npc)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (!SpawnedNPCSystem.IsABoss(npc))
			{
				return;
			}

			//Only check for spawned bosses a few screens close
			float distSQ = npc.DistanceSQ(Main.LocalPlayer.Center);
			const int maxDist = 1920 * 3;
			if (distSQ > maxDist * maxDist)
			{
				//Main.NewText("boss too far away: " + (int)dist + ">" + maxDist);
				return;
			}

			OnSpawnedBoss(npc, distSQ);
		}

		public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
		{
			bool ret = base.HijackGetData(ref messageType, ref reader, playerNumber);

			//MP client handle for OOANewWave
			if (messageType == MessageID.CrystalInvasionSendWaitTime)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					int forHowLong = reader.ReadInt32();

					DetectOOAWave(forHowLong);
				}
			}

			return ret;
		}

		private static void DetectOOAWave(int forHowLong)
		{
			if (forHowLong == 1800/* || forHowLong == 60 the "quick skip" time, but it would show up twice then*/)
			{
				OnOOANewWave(forHowLong);
			}
			else if (forHowLong == 300)
			{
				OnOOAStarts(forHowLong);
			}
		}
	}
}
