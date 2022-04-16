using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Handlers.SpawnedNPCHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderling
{
	//Code mostly taken from Lucy's Axe, without the netcode
	public enum GoblinUnderlingMessageSource
	{
		Idle,
		FirstSummon,
		BossSpawnGeneric,
		MoonlordSpawn,
		BetsySpawn,
		BossDefeatGeneric,
		MoonlordDefeat,
		Attacking,
		PlayerHurt,
		OOAStarts,
		OOANewWave,
		OnValhallaArmorEquipped,

		Count
	}

	[Content(ContentType.Weapons)]
	public class GoblinUnderlingSystem : AssSystem
	{
		private static byte variation;

		private const int GlobalCooldownMax = 5 * 60;
		private static float globalCooldown; //So that messages can't be spawned at the same time (unrealistic)

		private static float[] messageCooldownsByType;

		private static Dictionary<GoblinUnderlingMessageSource, List<string>> sourceToMessages; //Must have all entries registered
		private static Dictionary<GoblinUnderlingMessageSource, Func<int>> sourceToCooldowns; //Anything not registered gets a one second default cooldown

		private static List<GoblinUnderlingTier> tiers = new();

		public static int TierCount => tiers.Count;
		public static int CurrentTierIndex { get; private set; }
		public static Asset<Texture2D>[] bodyAssets;
		public static Asset<Texture2D>[] weaponAssets;

		public static GoblinUnderlingTier GetCurrentTier()
		{
			return tiers[CurrentTierIndex];
		}

		private static void RegisterMessage(GoblinUnderlingMessageSource source, List<string> messages, Func<int> cooldown = null)
		{
			sourceToMessages[source] = messages;
			if (cooldown != null)
			{
				sourceToCooldowns[source] = cooldown;
			}
		}

		private static void LoadMessages()
		{
			messageCooldownsByType = new float[(int)GoblinUnderlingMessageSource.Count];

			sourceToMessages = new Dictionary<GoblinUnderlingMessageSource, List<string>>();
			sourceToCooldowns = new Dictionary<GoblinUnderlingMessageSource, Func<int>>();

			RegisterMessage(GoblinUnderlingMessageSource.Idle, new List<string>()
			{
				"I'm bored, boss!",
				"I like hangin' out, boss!",
				"Dis armor's too heavy...",
				"You're a good boss, boss!",
				"I look up to you, boss! Not because I'm short...well, mostly because I'm short!",
				" Dat cloud looks like you, boss!",
				"Da last boss I had was mean. You're nice!",
				"Boss, what's your favorite food? I like those curved yellow things!",
				"I got lotsa extra inventory space when I wear armor!",
				"I don't like slimes. Dey get inside my armor, and it's sticky!",
				"Boss, look! If I lay down, I can hide in da grass!",
				"Dose otha' goblins are dumb. Dey should follow you too!",
				"Do you like me, boss?",
			},
			cooldown: () => Main.rand.Next(20, 40) * 60);

			RegisterMessage(GoblinUnderlingMessageSource.FirstSummon, new List<string>()
			{
				"You da boss now, boss!",
			});

			//Always put that one on cooldown, as some bosses have fancy summoning ways where for example core and then parts spawn, all having boss
			RegisterMessage(GoblinUnderlingMessageSource.BossSpawnGeneric, new List<string>()
			{
				"Uh oh!", "Dat's a biggun!",
				"Boss, dat one's big!",
				"I ain't afraid of nothin!",
				"Boss, we got trouble!",
			},
			cooldown: () => 10 * 60);

			RegisterMessage(GoblinUnderlingMessageSource.MoonlordSpawn, new List<string>()
			{
				"We got dis, boss! Don't give up!",
			});

			RegisterMessage(GoblinUnderlingMessageSource.BetsySpawn, new List<string>()
			{
				"Dat one's big! Dat means it has big guts!",
			});

			//Always put that one on cooldown, as some bosses have fancy defeat ways
			RegisterMessage(GoblinUnderlingMessageSource.BossDefeatGeneric, new List<string>()
			{
				"We did it, boss!",
				"I knew you could do it!",
				"Easy. Next!",
			},
			cooldown: () => 10 * 60);

			RegisterMessage(GoblinUnderlingMessageSource.MoonlordDefeat, new List<string>()
			{
				"Dat one was tough, but we're tougher!",
				"I don't wanna see no more squid after dis...",
				"Boss, you da best!",
			});

			RegisterMessage(GoblinUnderlingMessageSource.Attacking, new List<string>()
			{
				"Gotcha!",
				"I got dis one, boss!",
				"I'm a goblin! If you beat me, you get 1 exp and 2 gold!",
				"No one touches boss!",
			},
			cooldown: () => 30 * 60);

			RegisterMessage(GoblinUnderlingMessageSource.PlayerHurt, new List<string>()
			{
				"Don't hurt the boss!",
				"You'll pay for dat!",
				"You good, boss?!",
			},
			cooldown: () => 60 * 60);

			RegisterMessage(GoblinUnderlingMessageSource.OOAStarts, new List<string>()
			{
				"Dey'd be smart to join you, boss!",
				"Let's beat deez chumps!",
			});

			RegisterMessage(GoblinUnderlingMessageSource.OOANewWave, new List<string>()
			{
				"Boss, more chumps are coming!",
				"More baddies!",
				"Protect the shiny gem!",
				"I won't let dem pass, boss!",
				"Where do dey come from?!",
			});

			RegisterMessage(GoblinUnderlingMessageSource.OnValhallaArmorEquipped, new List<string>()
			{
				"Lookin' spiffy, boss!",
			},
			cooldown: () => 10 * 60);

			SpawnedNPCSystem.OnSpawnedNPC += OnSpawnedBoss;

			AssPlayer.OnSlainBoss += OnSlainBoss;

			On.Terraria.Main.ReportInvasionProgress += OnOOAStarts;

			//MP handled in HijackGetData
			On.Terraria.GameContent.Events.DD2Event.SetEnemySpawningOnHold += OnOOANewWave_SP;
		}

		private static void OnOOANewWave_SP(On.Terraria.GameContent.Events.DD2Event.orig_SetEnemySpawningOnHold orig, int forHowLong)
		{
			orig(forHowLong);

			//Only singleplayer
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				return;
			}

			if (forHowLong == 1800)
			{
				foreach (var proj in GetLocalGoblinUnderlings())
				{
					TryCreate(proj, GoblinUnderlingMessageSource.OOANewWave);
				}
			}
		}

		private static void OnOOAStarts(On.Terraria.Main.orig_ReportInvasionProgress orig, int progress, int progressMax, int icon, int progressWave)
		{
			orig(progress, progressMax, icon, progressWave);

			if (Main.netMode != NetmodeID.Server)
			{
				//ReportInvasionProgress(0, 1, 3, 1) //3 is the dd2 event icon
				if (progress == 0 && progressMax == 1 && icon == 3 && progressWave == 1)
				{
					foreach (var proj in GetLocalGoblinUnderlings())
					{
						TryCreate(proj, GoblinUnderlingMessageSource.OOAStarts);
					}
				}
			}
		}

		private static void FinalizeMessageData()
		{
			foreach (var obj in Enum.GetValues(typeof(GoblinUnderlingMessageSource)))
			{
				if (obj.Equals(GoblinUnderlingMessageSource.Count))
				{
					continue;
				}

				GoblinUnderlingMessageSource source = (GoblinUnderlingMessageSource)obj;
				if (!sourceToMessages.ContainsKey(source))
				{
					throw new Exception($"{source} not registered to {nameof(sourceToMessages)}!");
				}

				//Default CD
				if (!sourceToCooldowns.ContainsKey(source))
				{
					sourceToCooldowns[source] = () => 1 * 60;
				}
			}
		}

		//Has to be after tiers are assigned
		private static void LoadTextures()
		{
			if (!Main.dedServ)
			{
				bodyAssets = new Asset<Texture2D>[TierCount];
				weaponAssets = new Asset<Texture2D>[TierCount];

				string body = "AssortedCrazyThings/Projectiles/Minions/GoblinUnderling/GoblinUnderlingProj_";
				string weapon = "AssortedCrazyThings/Projectiles/Minions/GoblinUnderling/GoblinUnderlingWeapon_";
				foreach (var tier in tiers)
				{
					int index = tier.texIndex;
					bodyAssets[index] = ModContent.Request<Texture2D>(body + index);
					weaponAssets[index] = ModContent.Request<Texture2D>(weapon + index);
				}
			}
		}

		private static void LoadTiers()
		{
			tiers = new List<GoblinUnderlingTier>
			{
				//Baseline values in Item/AI code                                                                                         //dmg    kb    ap  sp     m  hb  ran   ransp
                /*Baseline*/ new GoblinUnderlingTier(0, () => true                 , ModContent.ProjectileType<GoblinUnderlingDart_0>()   , 1f   , 1f  , 0 , 0.3f , 6, 0 , 1.5f, 8f ),
                /*EoC*/      new GoblinUnderlingTier(1, () => NPC.downedBoss1      , ModContent.ProjectileType<GoblinUnderlingDart_1>()   , 1.25f, 1.2f, 0 , 0.35f, 6, 2 , 1.5f, 9f ),
                /*EoW/BoC*/  new GoblinUnderlingTier(2, () => NPC.downedBoss2      , ModContent.ProjectileType<GoblinUnderlingDart_2>()   , 1.5f , 1.4f, 0 , 0.4f , 6, 4 , 1.5f, 10f),
                /*Skeletron*/new GoblinUnderlingTier(3, () => NPC.downedBoss3      , ModContent.ProjectileType<GoblinUnderlingDart_3>()   , 1.75f, 1.6f, 10, 0.45f, 5, 6 , 1.5f, 11f),
                /*Mechboss*/ new GoblinUnderlingTier(4, () => NPC.downedMechBossAny, ModContent.ProjectileType<GoblinUnderlingDart_4>()   , 3f   , 1.8f, 10, 0.6f , 5, 6 , 1.5f, 12f),
                /*Plantera*/ new GoblinUnderlingTier(5, () => NPC.downedPlantBoss  , ModContent.ProjectileType<GoblinUnderlingTerraBeam>(), 3.5f , 2f  , 10, 0.7f , 4, 10, 1f  , 14f , true),
			};
		}

		private static void OnSlainBoss(Player player, int type)
		{
			if (Main.myPlayer != player.whoAmI)
			{
				return;
			}

			foreach (var proj in GetLocalGoblinUnderlings())
			{
				if (type == NPCID.MoonLordCore)
				{
					TryCreate(proj, GoblinUnderlingMessageSource.MoonlordDefeat);
				}
				else
				{
					TryCreate(proj, GoblinUnderlingMessageSource.BossDefeatGeneric);
				}
			}
		}

		private static void OnSpawnedBoss(NPC npc)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			//Special check for Eater, as he is not a boss. So just check for "first spawned head"
			bool onlyOneEater = false;
			if (npc.type == NPCID.EaterofWorldsHead)
			{
				onlyOneEater = true;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC other = Main.npc[i];

					if (other.active && i != npc.whoAmI && other.type == NPCID.EaterofWorldsHead)
					{
						onlyOneEater = false;
						break;
					}
				}
			}

			if (!(npc.boss || onlyOneEater))
			{
				return;
			}

			//Only check for spawned bosses a few screens close
			float dist = npc.Distance(Main.LocalPlayer.Center);
			const int maxDist = 1920 * 3;
			if (dist > maxDist)
			{
				//Main.NewText("boss too far away: " + (int)dist + ">" + maxDist);
				return;
			}

			foreach (var proj in GetLocalGoblinUnderlings())
			{
				if (npc.type == NPCID.MoonLordCore)
				{
					TryCreate(proj, GoblinUnderlingMessageSource.MoonlordSpawn);
				}
				else if (npc.type == NPCID.DD2Betsy)
				{
					TryCreate(proj, GoblinUnderlingMessageSource.BetsySpawn);
				}
				else
				{
					TryCreate(proj, GoblinUnderlingMessageSource.BossSpawnGeneric);
				}
			}
		}

		public static IEnumerable<Projectile> GetLocalGoblinUnderlings()
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];

				if (proj.active && proj.owner == Main.myPlayer && proj.type == ModContent.ProjectileType<GoblinUnderlingProj>())
				{
					yield return proj;
				}
			}
		}

		private static void UpdateMessageCooldowns()
		{
			float reduceAmount = ClientConfig.Instance.SatchelofGoodiesChatterFreq / 100f;

			if (globalCooldown > 0)
			{
				globalCooldown -= reduceAmount;
				if (globalCooldown < 0)
				{
					globalCooldown = 0;
				}
			}

			for (int i = 0; i < messageCooldownsByType.Length; i++)
			{
				if (messageCooldownsByType[i] > 0)
				{
					messageCooldownsByType[i] -= reduceAmount;
					if (messageCooldownsByType[i] < 0)
					{
						messageCooldownsByType[i] = 0;
					}
				}
			}
		}

		private static void DetermineCurrentTier()
		{
			CurrentTierIndex = 0;
			for (int i = TierCount - 1; i >= 0; i--)
			{
				//Start from last tier, prioritize
				var tier = tiers[i];
				if (tier.condition())
				{
					CurrentTierIndex = i;
					break;
				}
			}
		}

		public static void OnEnterWorld(Player player)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}

			variation = (byte)Main.rand.Next(12);
			PutMessageTypeOnCooldown(GoblinUnderlingMessageSource.Idle);
		}

		public static void PutMessageTypeOnCooldown(GoblinUnderlingMessageSource source, int? cooldownOverride = null)
		{
			int cd = 0;
			if (cooldownOverride.HasValue)
			{
				cd = cooldownOverride.Value;
			}
			else if (sourceToCooldowns.TryGetValue(source, out Func<int> definedCD))
			{
				cd = definedCD();
			}
			messageCooldownsByType[(int)source] = cd;
		}

		/// <summary>
		/// Checks for cooldown aswell
		/// </summary>
		public static bool TryCreate(Projectile projectile, GoblinUnderlingMessageSource source, Vector2? position = null, Vector2? velocity = null, int? cooldownOverride = null, int? variantOverride = null)
		{
			if (globalCooldown <= 0 && messageCooldownsByType[(int)source] <= 0)
			{
				Create(projectile, source, position, velocity, cooldownOverride, variantOverride);
				return true;
			}

			return false;
		}

		public static void Create(Projectile projectile, GoblinUnderlingMessageSource source, Vector2? position = null, Vector2? velocity = null, int? cooldownOverride = null, int? variantOverride = null)
		{
			if (Main.myPlayer != projectile.owner)
			{
				return;
			}

			if (ClientConfig.Instance.SatchelofGoodiesDialogueDisabled)
			{
				return;
			}

			PutMessageTypeOnCooldown(GoblinUnderlingMessageSource.Idle); //Always give idle message a cooldown

			int variant = variation;
			bool hasOverride = variantOverride.HasValue;
			if (hasOverride)
			{
				variant = variantOverride.Value;
			}

			Vector2 pos = position ?? projectile.Top;
			Vector2 vel = velocity ?? new Vector2(Main.rand.NextFloat(1f, 3f) * -projectile.direction, Main.rand.NextFloat(-3.5f, -2f));

			SpawnPopupText(source, variant, pos, vel);

			if (!hasOverride)
			{
				variation++;
			}

			globalCooldown = GlobalCooldownMax;
			PutMessageTypeOnCooldown(source, cooldownOverride);
		}

		private static void SpawnPopupText(GoblinUnderlingMessageSource source, int variationUnwrapped, Vector2 position, Vector2 velocity)
		{
			string textForVariation = GetTextForVariation(source, variationUnwrapped);
			AdvancedPopupRequest request = default;
			request.Text = textForVariation;
			//Eight symbols should equal to one second => 8 / 60 = 7.5
			float seconds = Math.Min(textForVariation.Length / 7.5f, 4);
			request.DurationInFrames = (int)(seconds * 60);
			request.Velocity = velocity;
			request.Color = new Color(125, 217, 124);
			PopupText.NewText(request, position);
		}

		private static string GetTextForVariation(GoblinUnderlingMessageSource source, int variationUnwrapped)
		{
			var list = sourceToMessages[source];
			int index = variationUnwrapped % list.Count;
			return list[index];
		}

		/// <summary>
		/// Handles kb scaling and knocking away from player. Should be called from all projectiles
		/// </summary>
		public static void CommonModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref int hitDirection)
		{
			GoblinUnderlingTier tier = GetCurrentTier();
			//damage = (int)(damage * tier.damageMult); THIS IS DONE IN GOBLIN PREAI, OVERRIDING DEFAULT MINION SCALING

			knockback *= tier.knockbackMult;

			int armorPen = tier.armorPen;
			damage += target.checkArmorPenetration(armorPen);

			float fromPlayerToTargetX = target.Center.X - projectile.GetOwner().Center.X;
			if (Math.Abs(fromPlayerToTargetX) < 7 * 16)
			{
				//Hit away from player if target is close
				hitDirection = Math.Sign(fromPlayerToTargetX);
				knockback *= 2;
			}
		}

		public override void Load()
		{
			LoadMessages();
		}

		public override void PostSetupContent()
		{
			LoadTiers();
			LoadTextures();

			FinalizeMessageData();
		}

		public override void Unload()
		{
			tiers = null;
			bodyAssets = null;
			weaponAssets = null;

			sourceToMessages = null;
			sourceToCooldowns = null;
			messageCooldownsByType = null;

			SpawnedNPCSystem.OnSpawnedNPC -= OnSpawnedBoss;

			AssPlayer.OnSlainBoss -= OnSlainBoss;
		}

		public override void PostUpdatePlayers()
		{
			UpdateMessageCooldowns();

			DetermineCurrentTier();
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

					if (forHowLong == 1800)
					{
						foreach (var proj in GetLocalGoblinUnderlings())
						{
							TryCreate(proj, GoblinUnderlingMessageSource.OOANewWave);
						}
					}
				}
			}

			return ret;
		}
	}
}
