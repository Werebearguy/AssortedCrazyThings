using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.Harvester;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
	[Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
	public class AssWorld : AssSystem
	{
		//basically "if they were alive last update"
		public bool megalodonAlive = false;
		public bool miniocramAlive = false;
		//"are they alive this update"
		bool isMegalodonSpawned;
		bool isMiniocramSpawned;
		//static names, in case you want to change them later
		public static string megalodonName = Megalodon.name;
		public static string miniocramName = SpawnOfOcram.name;
		public static string megalodonMessage = Megalodon.message;
		public static string miniocramMessage = SpawnOfOcram.message;
		//the megalodon messages are modified down below in the Disappear message

		//Soul stuff
		public static bool downedHarvester;

		public static bool slimeRainSky = false;

		public override void OnWorldLoad()
		{
			downedHarvester = false;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			var downed = new List<string>();
			if (downedHarvester)
			{
				downed.Add("harvester");
			}

			tag.Add("downed", downed);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedHarvester = downed.Contains("harvester");
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = downedHarvester;
			writer.Write(flags);

		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedHarvester = flags[0];
		}

		//small methods I made for myself to not make the code cluttered since I have to use these six times
		public static void AwakeningMessage(string message, Vector2 pos = default(Vector2), SoundStyle? soundStyle = null)
		{
			//Sound only in singleplayer
			if (soundStyle != null) SoundEngine.PlaySound(soundStyle.Value, pos);
			Message(message, new Color(175, 75, 255));
		}

		public static void Message(string message, Color color)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(message, color);
			}
			else if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
			}
		}

		public static void ToggleSlimeRainSky()
		{
			if (!Main.slimeRain && Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (!slimeRainSky)
				{
					SkyManager.Instance.Activate("Slime", default(Vector2));
					CombatText.NewText(Main.LocalPlayer.getRect(), CombatText.HealLife, "Background Activated");
					slimeRainSky = true;
				}
				else
				{
					SkyManager.Instance.Deactivate("Slime");
					CombatText.NewText(Main.LocalPlayer.getRect(), CombatText.DamagedFriendly, "Background Deactivated");
					slimeRainSky = false;
				}
			}
		}

		public static void DisableSlimeRainSky()
		{
			if (!Main.slimeRain && slimeRainSky && Main.netMode != NetmodeID.MultiplayerClient)
			{
				SkyManager.Instance.Deactivate("Slime");
				CombatText.NewText(Main.LocalPlayer.getRect(), CombatText.DamagedFriendly, "Background Deactivated");
				slimeRainSky = false;
			}
		}

		private void LimitSoulCount()
		{
			if (!ContentConfig.Instance.Bosses)
			{
				return;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (Main.GameUpdateCount % 30 == 15 && NPC.CountNPCS(ModContent.NPCType<DungeonSoul>()) > 10) //limit soul count in the world to 15
				{
					short oldest = 200;
					int timeleftmin = int.MaxValue;
					for (short j = 0; j < Main.maxNPCs; j++)
					{
						NPC npc = Main.npc[j];
						if (npc.active && npc.type == ModContent.NPCType<DungeonSoul>())
						{
							if (npc.timeLeft < timeleftmin)
							{
								timeleftmin = npc.timeLeft;
								oldest = j;
							}
						}
					}
					if (oldest != Main.maxNPCs)
					{
						NPC oldestnpc = Main.npc[oldest];
						oldestnpc.active = false;
						oldestnpc.netUpdate = true;
						if (Main.netMode == NetmodeID.Server && oldest < Main.maxNPCs)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, oldest);
						}
						//poof visual
						for (int i = 0; i < 15; i++)
						{
							Dust dust = Dust.NewDustPerfect(oldestnpc.Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
							dust.noLight = true;
							dust.noGravity = true;
							dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
						}
					}
				}
			}
		}

		private void UpdateEmpoweringFactor()
		{
			if (NPC.downedPlantBoss && AssPlayer.empoweringTotal < 1f)
				AssPlayer.empoweringTotal = 1f;
			else if (Main.hardMode && AssPlayer.empoweringTotal < 0.75f)
				AssPlayer.empoweringTotal = 0.75f;
		}

		public override void ResetNearbyTileEffects()
		{
			Main.LocalPlayer.GetModPlayer<AssPlayer>().wyvernCampfire = false;
		}

		public override void PostUpdateWorld()
		{
			CheckSpawns();
		}

		private void CheckSpawns()
		{
			if (!ContentConfig.Instance.HostileNPCs)
			{
				return;
			}

			//this code is when I first started modding, terrible stuff
			//those flags are checked for trueness each update
			isMegalodonSpawned = false;
			isMiniocramSpawned = false;
			for (short j = 0; j < Main.maxNPCs; j++)
			{
				NPC npc = Main.npc[j];
				if (npc.active)
				{
					if (npc.TypeName == megalodonName && !isMegalodonSpawned)
					{
						isMegalodonSpawned = true;
						//check if it wasnt alive in previous update
						if (!megalodonAlive)
						{
							AwakeningMessage(megalodonMessage, npc.Center, SoundID.Roar);
							megalodonAlive = true;
						}
					}
					if (npc.TypeName == miniocramName && !isMiniocramSpawned)
					{
						isMiniocramSpawned = true;
						if (!miniocramAlive)
						{
							AwakeningMessage(miniocramMessage, npc.Center, SoundID.Roar);
							miniocramAlive = true;
						}
					}
				}
			}
			//after this we know that either atleast one miniboss is active or not
			//if alive, but not active, print disappear message
			if (!isMegalodonSpawned && megalodonAlive)
			{
				megalodonAlive = false;
				Message("The " + megalodonName + " disappeared... for now", new Color(175, 255, 175));
			}
			if (!isMiniocramSpawned && miniocramAlive)
			{
				miniocramAlive = false;
				Message("The " + miniocramName + " disappeared... for now", new Color(175, 255, 175));
			}
		}

		public override void PreUpdateWorld()
		{
			LimitSoulCount();

			UpdateEmpoweringFactor();
		}
	}
}
