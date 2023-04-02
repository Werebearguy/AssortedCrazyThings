using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs.Mounts;
using AssortedCrazyThings.Effects;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Accessories.Useful;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
	[Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
	//[LegacyName("AssPlayer")] Maybe rename later
	public sealed class AssPlayer : AssPlayerBase
	{
		public delegate void SlainBossDelegate(Player player, int type);
		public static event SlainBossDelegate OnSlainBoss; //Runs on all sides

		public bool everburningCandleBuff = false;
		public bool everburningCursedCandleBuff = false;
		public bool everfrozenCandleBuff = false;
		public bool everburningShadowflameCandleBuff = false;

		public bool sigilOfTheTalon = false;

		public bool sigilOfTheWingOngoing = false;
		public int sigilOfTheWingFinishCounter = 0;
		public bool sigilOfTheWing = false;
		public int sigilOfTheWingCooldown = 0; //gets saved when you relog so you can't cheese it
		public bool SigilOfTheWingReady => sigilOfTheWingCooldown <= 0;

		private int lastSlainBossTimerSeconds = -1; //-1: never slain a boss, otherwise starts at 0 and counts
		private int lastSlainBossTimerInternal = 0; //Used for incrementing the seconds timer
		private int lastSlainBossType = 0; //Does not save
		public bool HasBossSlainTimer => lastSlainBossTimerSeconds != -1;

		public bool needsNearbyEnemyNumber = false;
		public int nearbyEnemyNumber = 0; //Impl of vanilla player.accThirdEyeNumber which works for all clients, shorter range
		public int nearbyEnemyTimer = 0;

		public bool hidePlayer = false;

		//soul minion stuff
		public bool soulMinion = false;
		public Item tempSoulMinion = null;
		public SoulType selectedSoulMinionType = SoulType.Dungeon;

		public bool slimePackMinion = false;
		public byte selectedSlimePackMinionType = 0;

		public byte nextMagicSlimeSlingMinion = 0;

		//empowering buff stuff
		public bool empoweringBuff = false;
		private const short empoweringTimerMax = 60; //in seconds //one minute until it caps out (independent of buff duration)
		private short empoweringTimer = 0;
		public static float empoweringTotal = 0.5f; //this gets modified in AssWorld.PreUpdate()
		public float empoweringStep = 0f;

		//enhanced hunter potion stuff
		public bool enhancedHunterBuff = false;

		//cute slime spawn enable buff
		public bool cuteSlimeSpawnEnable = false;

		public bool soulSaviorArmor = false;

		public bool wyvernCampfire = false;

		public bool droneControllerMinion = false;

		public const byte shieldDroneReductionMax = 35;
		public const byte ShieldIncreaseAmount = 7;
		public byte shieldDroneReduction = 0; //percentage * 100
		public float shieldDroneLerpVisual = 0; //percentage

		private bool drawEffectsCalledOnce = false;

		public bool mouseoveredDresser = false;

		public int LastSelectedWeaponDamage { get; private set; } = 0;

		/// <summary>
		/// Bitfield. Use .HasFlag(DroneType.SomeType) to check if its there or not
		/// </summary>
		public DroneType droneControllerUnlocked = DroneType.None;

		/// <summary>
		/// Contains the DroneType value
		/// </summary>
		public DroneType selectedDroneControllerMinionType = DroneType.BasicLaser;

		public override void ResetEffects()
		{
			everburningCandleBuff = false;
			everburningCursedCandleBuff = false;
			everfrozenCandleBuff = false;
			everburningShadowflameCandleBuff = false;
			sigilOfTheTalon = false;
			sigilOfTheWingOngoing = false;
			sigilOfTheWing = false;
			hidePlayer = false;
			soulMinion = false;
			tempSoulMinion = null;
			slimePackMinion = false;
			empoweringBuff = false;
			enhancedHunterBuff = false;
			cuteSlimeSpawnEnable = false;
			soulSaviorArmor = false;
			droneControllerMinion = false;
			mouseoveredDresser = false;

			needsNearbyEnemyNumber = false;
		}

		public bool RightClickPressed { get { return PlayerInput.Triggers.JustPressed.MouseRight; } }

		public bool RightClickReleased { get { return PlayerInput.Triggers.JustReleased.MouseRight; } }

		public bool LeftClickPressed { get { return PlayerInput.Triggers.JustPressed.MouseLeft; } }

		public bool LeftClickReleased { get { return PlayerInput.Triggers.JustReleased.MouseLeft; } }

		public override void SaveData(TagCompound tag)
		{
			tag.Add("sigilOfTheWingCooldown", (int)sigilOfTheWingCooldown);
			tag.Add("lastSlainBossTimerSeconds", (int)lastSlainBossTimerSeconds);
			tag.Add("droneControllerUnlocked", (byte)droneControllerUnlocked);
		}

		public override void LoadData(TagCompound tag)
		{
			sigilOfTheWingCooldown = tag.GetInt("sigilOfTheWingCooldown");
			string timerKey = "lastSlainBossTimerSeconds";
			if (tag.ContainsKey(timerKey))
			{
				lastSlainBossTimerSeconds = tag.GetInt("lastSlainBossTimerSeconds");
			}
			droneControllerUnlocked = (DroneType)tag.GetByte("droneControllerUnlocked");
		}

		//TODO get rid of this, use manual packets since setting those values happens in a singular place
		public override void clientClone(ModPlayer clientClone)
		{
			AssPlayer clone = clientClone as AssPlayer;
			clone.shieldDroneReduction = shieldDroneReduction;
			//Needs syncing because spawning drone parts depends on this serverside (See GeneralGlobalNPC.NPCLoot)
			clone.droneControllerUnlocked = droneControllerUnlocked;
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			AssPlayer clone = clientPlayer as AssPlayer;
			if (clone.shieldDroneReduction != shieldDroneReduction ||
				clone.droneControllerUnlocked != droneControllerUnlocked)
			{
				SendClientChangesPacket();
			}
		}

		/// <summary>
		/// Things that are sent to the server that are needed on-change
		/// </summary>
		public void SendClientChangesPacket(int toClient = -1, int ignoreClient = -1)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)AssMessageType.ClientChangesAssPlayer);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shieldDroneReduction);
				packet.Write((byte)droneControllerUnlocked);
				packet.Send(toClient, ignoreClient);
			}
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)AssMessageType.SyncAssPlayer);
			packet.Write((byte)Player.whoAmI);

			//Actual data here
			packet.Write((byte)shieldDroneReduction);
			packet.Write7BitEncodedInt(lastSlainBossTimerSeconds);
			packet.Write7BitEncodedInt(lastSlainBossType);

			packet.Send(toWho, fromWho);
		}

		public void ReceiveSyncPlayer(BinaryReader reader)
		{
			shieldDroneReduction = reader.ReadByte();
			lastSlainBossTimerSeconds = reader.Read7BitEncodedInt();
			lastSlainBossType = reader.Read7BitEncodedInt();
		}

		public override void OnEnterWorld(Player player)
		{
			SendClientChangesPacket();
		}

		/// <summary>
		/// Resets the empowering timer from the Empowering Buff, spawns dust, sends sync
		/// </summary>
		public void ResetEmpoweringTimer(bool fromServer = false)
		{
			if (empoweringBuff && !Player.HasBuff(BuffID.ShadowDodge))
			{
				for (int i = 0; i < empoweringTimer; i++)
				{
					Dust dust = Dust.NewDustPerfect(Player.Center, 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)) + (new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)) * ((6 * empoweringTimer) / empoweringTimerMax)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
					dust.noLight = true;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(1f, 2.3f);
				}
				empoweringTimer = 0;

				if (Main.netMode == NetmodeID.MultiplayerClient && !fromServer)
				{
					ModPacket packet = Mod.GetPacket();
					packet.Write((byte)AssMessageType.ResetEmpoweringTimerpvp);
					packet.Write((byte)Player.whoAmI);
					packet.Send(); //send to server
				}
			}
		}

		/// <summary>
		/// Decreases damage based on current shield level from the Shield Drone, spawns dust
		/// </summary>
		/// <param name="damage"></param>
		public void DecreaseDroneShield(ref int damage)
		{
			if (shieldDroneReduction > 0)
			{
				for (int i = 0; i < shieldDroneReduction / 2; i++)
				{
					Dust dust = Dust.NewDustPerfect(Player.Center, 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)) + new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
					dust.noLight = true;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(1f, 2.3f);
				}

				damage = (int)(damage * ((100 - shieldDroneReduction) / 100f));
				if (Main.netMode != NetmodeID.Server && Main.myPlayer == Player.whoAmI) shieldDroneReduction -= ShieldIncreaseAmount; //since this is only set clientside by the projectile and synced by packets
			}
		}

		/// <summary>
		/// Sets the isTemp on the projectile so it behaves differently
		/// </summary>
		private void PreSyncSoulTemp(Projectile proj)
		{
			if (!ContentConfig.Instance.Bosses)
			{
				return;
			}

			if (proj.ModProjectile is CompanionDungeonSoulMinionBase soul)
			{
				soul.isTemp = true;
			}
		}

		/// <summary>
		/// Spawns the temporary soul when wearing the accessory that allows it
		/// </summary>
		private void SpawnSoulTemp()
		{
			if (!ContentConfig.Instance.Bosses)
			{
				return;
			}

			if (!(tempSoulMinion != null && !tempSoulMinion.IsAir && Player.whoAmI == Main.myPlayer))
			{
				return;
			}

			if (Player.statLife > Player.statLifeMax2 * 0.25f)
			{
				return;
			}

			bool checkIfAlive = false;
			int spawnedType = Main.hardMode ? ModContent.ProjectileType<CompanionDungeonSoulPostWOFMinion>() : ModContent.ProjectileType<CompanionDungeonSoulPreWOFMinion>();
			int spawnedDamage = Main.hardMode ? (int)(EverhallowedLantern.BaseDmg * 1.1f * 2f) : ((EverhallowedLantern.BaseDmg / 2 - 1) * 2);
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.owner == Player.whoAmI && projectile.type == spawnedType)
				{
					if (projectile.minionSlots == 0f) //criteria for temp, is set by isTemp
					{
						checkIfAlive = true;
						break;
					}
				}
			}

			if (!checkIfAlive)
			{
				int proj = Projectile.NewProjectile(Player.GetSource_Accessory(tempSoulMinion), Player.Center.X, Player.Center.Y, -Player.velocity.X, Player.velocity.Y - 6f, spawnedType, spawnedDamage, EverhallowedLantern.BaseKB, Main.myPlayer);
				Main.projectile[proj].originalDamage = spawnedDamage;
			}
		}

		/// <summary>
		/// Upon Soul Harvester death, convert all inert souls in inventory
		/// </summary>
		public void ConvertInertSoulsInventory()
		{
			if (!ContentConfig.Instance.Bosses)
			{
				return;
			}

			//this gets called once on server side for all players, and then each player calls it on itself client side
			int tempStackCount;
			int itemTypeOld = ModContent.ItemType<CaughtDungeonSoul>();
			int itemTypeNew = ModContent.ItemType<CaughtDungeonSoulFreed>(); //version that is used in crafting

			Item[][] inventoryArray = { Player.inventory, Player.bank.item, Player.bank2.item, Player.bank3.item, Player.bank4.item }; //go though player inv
			for (int y = 0; y < inventoryArray.Length; y++)
			{
				for (int e = 0; e < inventoryArray[y].Length; e++)
				{
					Item item = inventoryArray[y][e];
					if (item.type == itemTypeOld) //find inert soul
					{
						tempStackCount = item.stack;
						item.SetDefaults(itemTypeNew); //override with awakened
						item.stack = tempStackCount;
					}
				}
			}

			//trash slot
			Item trashItem = Player.trashItem;
			if (trashItem.type == itemTypeOld)
			{
				tempStackCount = trashItem.stack;
				trashItem.SetDefaults(itemTypeNew);
				trashItem.stack = tempStackCount;
			}

			//mouse item
			Item mouseItem = Main.mouseItem;
			if (Main.netMode != NetmodeID.Server && mouseItem.type == itemTypeOld)
			{
				tempStackCount = mouseItem.stack;
				mouseItem.SetDefaults(itemTypeNew);
				mouseItem.stack = tempStackCount;
			}
		}

		private void SigilOfTheWingCooldown()
		{
			//this code runs even when the accessory is not equipped
			if (sigilOfTheWingCooldown > 0)
			{
				sigilOfTheWingCooldown--;
				if (sigilOfTheWingCooldown == 0)
				{
					sigilOfTheWingFinishCounter = 0;
				}
			}
		}

		/// <summary>
		/// Sets some variables related to the Empowering Buff
		/// </summary>
		private void Empower()
		{
			if (empoweringBuff)
			{
				//TODO convert timer to ticks
				if (Main.GameUpdateCount % 60 == 0)
				{
					if (empoweringTimer < empoweringTimerMax)
					{
						empoweringTimer++;
						empoweringStep = (empoweringTimer * empoweringTotal) / empoweringTimerMax;
					}
				}
			}
			else
			{
				empoweringStep = 0f;
				empoweringTimer = 0;
			}
		}

		private bool SigilOfTheWingDeath(int hitDirection)
		{
			if (sigilOfTheWing && SigilOfTheWingReady)
			{
				Player.statLife = 5;
				Player.AddBuff(ModContent.BuffType<SigilOfTheWingBuff>(), SigilOfTheWing.DurationSeconds * 60);

				sigilOfTheWingCooldown = SigilOfTheWing.CooldownSeconds * 60;

				for (int j = 0; j < 100; j++)
				{
					if (Player.stoned)
					{
						Dust.NewDust(Player.position, Player.width, Player.height, 1, 2 * hitDirection, -2f);
					}
					else if (Player.frostArmor)
					{
						Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 135, 2 * hitDirection, -2f);
						dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
					}
					else if (Player.boneArmor)
					{
						Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 26, 2 * hitDirection, -2f);
						dust.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
					}
					else
					{
						Dust.NewDust(Player.position, Player.width, Player.height, 5, 2 * hitDirection, -2f);
					}
				}

				SoundEngine.PlaySound(SoundID.NPCDeath39, Player.Center);

				if (Player.stoned)
				{
					Player.stoned = false;
					Player.ClearBuff(BuffID.Stoned);
				}

				return false;
			}
			return true;
		}

		public void SigilOfTheWingStop(ref int buffIndex)
		{
			if (buffIndex > -1)
			{
				Player.DelBuff(buffIndex);
				buffIndex--;
			}
			sigilOfTheWingFinishCounter = 0;
			Player.mount.Dismount(Player);
			Player.immune = true;
			Player.immuneTime = 90;
			Player.immuneNoBlink = false;

			SoundEngine.PlaySound(SoundID.NPCHit36, Player.Center);
		}

		public void SigilOfTheWingStop()
		{
			int index = -1;
			if (Player.FindBuffIndex(ModContent.BuffType<SigilOfTheWingBuff>()) is int index2 && index2 > -1)
			{
				index = index2;
			}
			SigilOfTheWingStop(ref index);
		}

		public void SigilOfTheWingRegen(ref float regen)
		{
			if (!sigilOfTheWingOngoing)
			{
				return;
			}

			int targetLifeMax = (int)(Player.statLifeMax2 * 0.25f);
			if (Player.statLife > targetLifeMax)
			{
				Player.ClearBuff(ModContent.BuffType<SigilOfTheWingBuff>());
				SigilOfTheWingStop();
				return;
			}

			if (Player.lifeRegen < 0)
			{
				Player.lifeRegen = 0;
			}

			regen = 0; //Cancel existing natural regen
			int amount = targetLifeMax / (SigilOfTheWing.DurationSeconds / 2); //lifeRegen represents how much life every 2 seconds is regenerated
			Player.lifeRegen += Math.Max(1, amount);

			if (Main.myPlayer == Player.whoAmI)
			{
				int finishStep = (int)(targetLifeMax * 0.1f);
				const int totalSteps = 3;
				int remainingCounters = totalSteps - sigilOfTheWingFinishCounter;
				//When 70%, 80%, and 90%
				if (sigilOfTheWingFinishCounter < totalSteps && Player.statLife >= targetLifeMax - remainingCounters * finishStep)
				{
					float pitchOff = (sigilOfTheWingFinishCounter == totalSteps - 1) ? 0.05f : 0;
					SoundEngine.PlaySound(SoundID.MaxMana.WithVolumeScale(0.7f).WithPitchOffset(pitchOff), Player.Center);
					for (double i = 0; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 25)
					{
						Dust dust = Dust.NewDustPerfect(Player.Center - new Vector2(0f, 0), 135, new Vector2((float)-Math.Cos(i), (float)Math.Sin(i)) * 3f, 0, new Color(255, 255, 255), 1.8f);
						dust.noGravity = true;
					}

					sigilOfTheWingFinishCounter++;
				}
			}
		}

		private void ApplyCandleDebuffs(Entity victim)
		{
			if (victim is NPC npc)
			{
				if (everburningCandleBuff) npc.AddBuff(BuffID.OnFire, 120);
				if (everburningCursedCandleBuff) npc.AddBuff(BuffID.CursedInferno, 120);
				if (everfrozenCandleBuff) npc.AddBuff(BuffID.Frostburn, 120);
				if (everburningShadowflameCandleBuff) npc.AddBuff(BuffID.ShadowFlame, 60);
			}
			//else if (victim is Player)
		}

		#region CircleUI

		/// <summary>
		/// Contains a list of CircleUIHandlers that are used in CircleUIStart/End in Mod
		/// </summary>
		public List<CircleUIHandler> CircleUIList;

		public override void Initialize()
		{
			sigilOfTheWingCooldown = 0;
			lastSlainBossTimerSeconds = -1;
			selectedSoulMinionType = SoulType.Dungeon;
			selectedSlimePackMinionType = 0;
			nextMagicSlimeSlingMinion = 0;
			empoweringTimer = 0;
			empoweringStep = 0f;
			shieldDroneReduction = 0;
			shieldDroneLerpVisual = 0;
			drawEffectsCalledOnce = false;

			//needs to call new List() since Initialize() is called per player in the player select screen
			CircleUIList = new List<CircleUIHandler>();

			if (ContentConfig.Instance.Weapons)
			{
				CircleUIList.AddRange(new List<CircleUIHandler>
				{
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<SlimeHandlerKnapsack>(),
					condition: () => true,
					uiConf: SlimeHandlerKnapsack.GetUIConf,
					onUIStart: () => selectedSlimePackMinionType,
					onUIEnd: delegate
					{
						selectedSlimePackMinionType = (byte)CircleUI.returned;
						AssUtils.UIText("Selected: " + (selectedSlimePackMinionType == 0 ? "Default" : (selectedSlimePackMinionType == 1 ? "Assorted" : "Spiked")), CombatText.HealLife);
					},
					triggerLeft: false
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<DroneController>(),
					condition: () => true,
					uiConf: DroneController.GetUIConf,
					onUIStart: delegate
					{
						if (Utils.IsPowerOfTwo((int)selectedDroneControllerMinionType))
						{
							return (int)Math.Log((int)selectedDroneControllerMinionType, 2);
						}
						return 0;
					},
					onUIEnd: delegate
					{
						selectedDroneControllerMinionType = (DroneType)(byte)Math.Pow(2, CircleUI.returned);
						AssUtils.UIText("Selected: " + DroneController.GetDroneData(selectedDroneControllerMinionType).Name, CombatText.HealLife);
					},
					triggerLeft: false
				)}
				);
			}

			if (ContentConfig.Instance.Bosses)
			{
				CircleUIList.Add(new CircleUIHandler(
				triggerItem: ModContent.ItemType<EverhallowedLantern>(),
				condition: () => true,
				uiConf: EverhallowedLantern.GetUIConf,
				onUIStart: delegate
				{
					if (Utils.IsPowerOfTwo((int)selectedSoulMinionType))
					{
						return (int)Math.Log((int)selectedSoulMinionType, 2);
					}
					return 0;
				},
				onUIEnd: delegate
				{
					selectedSoulMinionType = (SoulType)(byte)Math.Pow(2, CircleUI.returned);
					AssUtils.UIText("Selected: " + EverhallowedLantern.GetSoulData(selectedSoulMinionType).Name, CombatText.HealLife);
				},
				triggerLeft: false
				));
			}

			// after filling the list, set the trigger list
			for (int i = 0; i < CircleUIList.Count; i++)
			{
				CircleUIList[i].AddTriggers();
			}
		}
		#endregion

		/// <summary>
		/// Get proper SpriteEffects flags based on player status
		/// </summary>
		private static SpriteEffects GetSpriteEffects(Player player)
		{
			if (player.gravDir == 1f)
			{
				if (player.direction == 1)
				{
					return SpriteEffects.None;
				}
				else
				{
					return SpriteEffects.FlipHorizontally;
				}
			}
			else
			{
				if (player.direction == 1)
				{
					return SpriteEffects.FlipVertically;
				}
				else
				{
					return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				}
			}
		}

		public void SlainBoss(int type)
		{
			lastSlainBossTimerSeconds = 0;
			lastSlainBossType = type;

			OnSlainBoss?.Invoke(Player, type);

			if (Main.netMode == NetmodeID.Server)
			{
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)AssMessageType.SlainBoss);
				packet.Write7BitEncodedInt(lastSlainBossType);
				packet.Send(Player.whoAmI);
			}
		}

		public bool HasSlainBossSecondsAgo(int timeInSeconds)
		{
			return HasBossSlainTimer && lastSlainBossTimerSeconds < timeInSeconds;
		}

		public void UpdateSlainBossTimer()
		{
			if (HasBossSlainTimer)
			{
				lastSlainBossTimerInternal++;
				if (lastSlainBossTimerInternal >= 60)
				{
					lastSlainBossTimerInternal = 0;

					lastSlainBossTimerSeconds++;
				}
			}
		}

		public void UpdateNearbyEnemies()
		{
			float distSQ = 600 * 600;
			if (nearbyEnemyTimer == 0)
			{
				nearbyEnemyNumber = 0;
				nearbyEnemyTimer = 15;
				for (int l = 0; l < Main.maxNPCs; l++)
				{
					NPC npc = Main.npc[l];
					if (npc.active && !npc.friendly && npc.damage > 0 && npc.lifeMax > 5 && !npc.dontCountMe && (npc.Center - Player.Center).LengthSquared() < distSQ)
					{
						nearbyEnemyNumber++;
					}
				}
			}
			else
			{
				nearbyEnemyTimer--;
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (!drawEffectsCalledOnce)
			{
				drawEffectsCalledOnce = true;
			}
			else
			{
				return;
			}
			if (Main.gameMenu) return;

			//Other code

			//if (!PlayerLayer.MiscEffectsBack.visible) return;

			if (shieldDroneReduction > 0)
			{
				Color outer = Color.White;
				Color inner = new Color(0x03, 0xFE, 0xFE);

				float ratio = shieldDroneReduction / 100f;
				if (shieldDroneLerpVisual < ratio)
				{
					shieldDroneLerpVisual += 0.01f;
				}
				if (shieldDroneLerpVisual > ratio) shieldDroneLerpVisual = ratio;

				outer *= shieldDroneLerpVisual;
				inner *= shieldDroneLerpVisual;
				Lighting.AddLight(drawPlayer.Center, inner.ToVector3());

				float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
				outer *= alpha;
				inner *= alpha;
				Effect shader = ShaderManager.SetupCircleEffect(new Vector2((int)drawPlayer.Center.X, (int)drawPlayer.Center.Y + drawPlayer.gfxOffY), 2 * 16, outer, inner);

				ShaderManager.ApplyToScreenOnce(Main.spriteBatch, shader);
			}
		}

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			if (!Main.rand.NextBool(5)) return;
			ApplyCandleDebuffs(target);
		}

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (!Main.rand.NextBool(5)) return;
			if (proj.minion && !Main.rand.NextBool(5)) return;
			ApplyCandleDebuffs(target);
		}

		public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
		{
			//ApplyCandleDebuffs(target);
			AssPlayer assPlayer = target.GetModPlayer<AssPlayer>();
			assPlayer.ResetEmpoweringTimer();

			assPlayer.SpawnSoulTemp();
		}

		public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
		{
			//ApplyCandleDebuffs(target);
			AssPlayer assPlayer = target.GetModPlayer<AssPlayer>();
			assPlayer.ResetEmpoweringTimer();

			assPlayer.SpawnSoulTemp();
		}

		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		{
			if (empoweringBuff && !item.CountsAsClass(DamageClass.Summon) && item.damage > 0) damage += empoweringStep; //summon damage gets handled in EmpoweringBuffGlobalProjectile
		}

		public override void ModifyWeaponCrit(Item item, ref float crit)
		{
			crit += 10 * empoweringStep;
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (!SigilOfTheWingDeath(hitDirection)) return false;

			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			DecreaseDroneShield(ref damage);

			if (wyvernCampfire && damageSource.SourceProjectileType == ProjectileID.HarpyFeather)
			{
				hitDirection = 0; //this cancels knockback
			}

			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
		}

		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			ResetEmpoweringTimer();

			SpawnSoulTemp();
		}

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			bool inWater = !attempt.inHoney && !attempt.inLava;

			if (attempt.waterTilesCount < attempt.waterNeededToFish)
			{
				return;
			}

			if (ContentConfig.Instance.OtherPets)
			{
				//Match Zephyr Fish conditions
				if (attempt.legendary && !attempt.crate && inWater)
				{
					if (((int)(Player.Center.X / 16) < Main.maxTilesX * 0.08f || (int)(Player.Center.X / 16) > Main.maxTilesX * 0.92f) && Main.rand.NextBool(5)) //2 times more likely than zephyr fish makes it about as rare as reaver shark
					{
						itemDrop = ModContent.ItemType<AnomalocarisItem>();
						return;
					}
				}
			}
		}

		public override void PostUpdateBuffs()
		{
			SigilOfTheWingCooldown();

			Empower();

			UpdateSlainBossTimer();
		}

		public override void PreUpdate()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				if (drawEffectsCalledOnce)
				{
					drawEffectsCalledOnce = false;
				}

				if (Main.myPlayer == Player.whoAmI && ContentConfig.Instance.Weapons)
				{
					if (Player.ownedProjectileCounts[DroneController.GetDroneData(DroneType.Shield).ProjType] < 1) shieldDroneReduction = 0;
				}
			}

			Item heldItem = Player.HeldItem;
			if (heldItem.damage > 0 && !heldItem.accessory)
			{
				LastSelectedWeaponDamage = Player.GetWeaponDamage(heldItem);
			}

			UpdateNearbyEnemies();
		}

		public static readonly PlayerDrawLayer[] WhitelistedByPlayerHiding = new[] {
			PlayerDrawLayers.MountBack,
			PlayerDrawLayers.MountFront,

			PlayerDrawLayers.WebbedDebuffBack,
			PlayerDrawLayers.FrozenOrWebbedDebuff,
			PlayerDrawLayers.ElectrifiedDebuffBack,
			PlayerDrawLayers.ElectrifiedDebuffFront,
		};

		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			HideDrawLayersForPlayer(drawInfo);
		}

		private void HideDrawLayersForPlayer(PlayerDrawSet drawInfo)
		{
			if (!hidePlayer)
			{
				return;
			}

			foreach (PlayerDrawLayer layer in PlayerDrawLayerLoader.Layers)
			{
				//If layer matches whitelist, or the layer it is parented to matches whitelist, don't hide
				if (Array.IndexOf(WhitelistedByPlayerHiding, layer) > -1)
				{
					continue;
				}

				var position = layer.GetDefaultPosition();
				if (position is PlayerDrawLayer.BeforeParent beforeParent && Array.IndexOf(WhitelistedByPlayerHiding, beforeParent.Parent) > -1 ||
					position is PlayerDrawLayer.AfterParent afterParent && Array.IndexOf(WhitelistedByPlayerHiding, afterParent.Parent) > -1)
				{
					continue;
				}

				layer.Hide();
			}
		}

		public override void NaturalLifeRegen(ref float regen)
		{
			//Used instead of other liferegen hooks because this runs after good/bad ones, just before application
			SigilOfTheWingRegen(ref regen);
		}
	}
}
