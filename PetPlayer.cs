using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Data;
using AssortedCrazyThings.Base.SlimeHugs;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Pets;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
	[Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
	public class PetPlayer : AssPlayerBase
	{
		private bool enteredWorld = false;

		private const int altTextureCountLoaded = 25; //IMPORTANT TO INCREMENT THIS EACH TIME A NEW ALT TEXTURE IS ADDED 

		//Alt texture types
		public byte mechFrogType = 0;
		public byte petEyeType = 0;
		public byte cursedSkullType = 0;
		public byte youngWyvernType = 0;
		public byte petFishronType = 0;
		public byte petMoonType = 0;
		public byte abeeminationType = 0;
		public byte lilWrapsType = 0;
		public byte vampireBatType = 0;
		public byte pigronataType = 0;
		public byte queenLarvaType = 0;
		public byte miniAntlionType = 0;
		public byte petGoldfishType = 0;
		public byte skeletronHandType = 0;
		public byte skeletronPrimeHandType = 0;
		public byte petCultistType = 0;
		public byte petAnomalocarisType = 0;
		public byte dynamiteBunnyType = 0;
		public byte wallFragmentType = 0;
		public byte metroidPetType = 0;
		public byte cuteLamiaPetType = 0;

		public byte oceanSlimeType = 0;
		public byte stingSlimeType = 0;
		public byte animatedTomeType = 0;

		public byte youngHarpyType = 0;

		//ALTERNATE
		////name pet texture
		//public byte classNameType = 0;

		public bool CuteSlimeYellow = false;
		public bool CuteSlimeXmas = false;
		public bool CuteSlimeToxic = false;
		public bool CuteSlimeSand = false;
		public bool CuteSlimeRed = false;
		public bool CuteSlimeRainbow = false;
		public bool CuteSlimeQueen = false;
		public bool CuteSlimePurple = false;
		public bool CuteSlimePrincess = false;
		public bool CuteSlimePink = false;
		public bool CuteSlimeLava = false;
		public bool CuteSlimeJungle = false;
		public bool CuteSlimeIlluminant = false;
		public bool CuteSlimeIce = false;
		public bool CuteSlimeGreen = false;
		public bool CuteSlimeDungeon = false;
		public bool CuteSlimeCrimson = false;
		public bool CuteSlimeCorrupt = false;
		public bool CuteSlimeBlue = false;
		public bool CuteSlimeBlack = false;

		public bool DrumstickElemental = false;
		public bool SuspiciousNugget = false;
		public bool MiniAntlion = false;
		public bool LilWraps = false;
		public bool PetFishron = false;
		public bool RainbowSlime = false;
		public bool PrinceSlime = false;
		public bool IlluminantSlime = false;
		public bool ChunkySlime = false;
		public bool FairySlime = false;
		public bool HornedSlime = false;
		public bool JoyousSlime = false;
		public bool MeatballSlime = false;
		public bool OceanSlime = false;
		public bool StingSlime = false;
		public bool TurtleSlime = false;
		public bool Pigronata = false;
		public bool Abeemination = false;
		public bool MiniMegalodon = false;
		public bool YoungHarpy = false;
		public bool CuteGastropod = false;
		public bool YoungWyvern = false;
		public bool BabyIchorSticker = false;
		public bool LifelikeMechanicalFrog = false;
		public bool ChunkyandMeatball = false;
		public bool DemonHeart = false;
		public bool BrainofConfusion = false;
		public bool AlienHornet = false;
		public bool DetachedHungry = false;
		public bool BabyOcram = false;
		public bool CursedSkull = false;
		public bool BabyCrimera = false;
		public bool VampireBat = false;
		public bool TorturedSoul = false;
		public bool EnchantedSword = false;
		public bool Goblet = false;
		public bool SoulLightPet = false;
		public bool SoulLightPet2 = false;
		public bool DocileDemonEye = false;
		public bool QueenLarva = false;
		public bool PetSun = false;
		public bool PetMoon = false;
		public bool WallFragment = false;
		public bool TinyTwins = false;
		public bool PetGoldfish = false;
		public bool SkeletronHand = false;
		public bool SkeletronPrimeHand = false;
		public bool PetHarvester = false;
		public bool PetGolemHead = false;
		public bool TrueObservingEye = false;
		public bool PetCultist = false;
		public bool PetPlantera = false;
		public bool PetEaterofWorlds = false;
		public bool PetDestroyer = false;
		public bool AnimatedTome = false;
		public bool PetAnomalocaris = false;
		public bool DynamiteBunny = false;
		public bool FairySwarm = false;
		public bool SwarmofCthulhu = false;
		public bool PetQueenSlime = false;
		public bool FailureSlime = false;
		public bool GhostMartian = false;
		public bool NumberMuncher = false;
		public bool StrangeRobot = false;
		public bool MetroidPet = false;
		public bool CuteLamiaPet = false;
		public bool TorchGodLightPet = false;
		public bool Woby = false;
		//ALTERNATE
		//public bool ClassName = false;

		public override void ResetEffects()
		{
			CuteSlimeYellow = false;
			CuteSlimeXmas = false;
			CuteSlimeToxic = false;
			CuteSlimeSand = false;
			CuteSlimeRed = false;
			CuteSlimeRainbow = false;
			CuteSlimePrincess = false;
			CuteSlimeQueen = false;
			CuteSlimePurple = false;
			CuteSlimePink = false;
			CuteSlimeLava = false;
			CuteSlimeJungle = false;
			CuteSlimeIce = false;
			CuteSlimeIlluminant = false;
			CuteSlimeGreen = false;
			CuteSlimeDungeon = false;
			CuteSlimeCrimson = false;
			CuteSlimeCorrupt = false;
			CuteSlimeBlue = false;
			CuteSlimeBlack = false;

			DrumstickElemental = false;
			SuspiciousNugget = false;
			MiniAntlion = false;
			LilWraps = false;
			PetFishron = false;
			RainbowSlime = false;
			PrinceSlime = false;
			IlluminantSlime = false;
			ChunkySlime = false;
			FairySlime = false;
			HornedSlime = false;
			JoyousSlime = false;
			MeatballSlime = false;
			OceanSlime = false;
			StingSlime = false;
			TurtleSlime = false;
			Pigronata = false;
			Abeemination = false;
			MiniMegalodon = false;
			YoungHarpy = false;
			CuteGastropod = false;
			YoungWyvern = false;
			BabyIchorSticker = false;
			LifelikeMechanicalFrog = false;
			ChunkyandMeatball = false;
			DemonHeart = false;
			BrainofConfusion = false;
			AlienHornet = false;
			DetachedHungry = false;
			BabyOcram = false;
			CursedSkull = false;
			BabyCrimera = false;
			VampireBat = false;
			TorturedSoul = false;
			EnchantedSword = false;
			Goblet = false;
			SoulLightPet = false;
			SoulLightPet2 = false;
			DocileDemonEye = false;
			QueenLarva = false;
			PetSun = false;
			PetMoon = false;
			WallFragment = false;
			TinyTwins = false;
			PetGoldfish = false;
			SkeletronHand = false;
			SkeletronPrimeHand = false;
			PetHarvester = false;
			PetGolemHead = false;
			TrueObservingEye = false;
			PetCultist = false;
			PetPlantera = false;
			PetEaterofWorlds = false;
			PetDestroyer = false;
			AnimatedTome = false;
			PetAnomalocaris = false;
			DynamiteBunny = false;
			FairySwarm = false;
			SwarmofCthulhu = false;
			PetQueenSlime = false;
			FailureSlime = false;
			GhostMartian = false;
			NumberMuncher = false;
			StrangeRobot = false;
			MetroidPet = false;
			CuteLamiaPet = false;
			TorchGodLightPet = false;
			Woby = false;
			//ALTERNATE
			//ClassName = false;
		}

		/// <summary>
		/// Returns true if this has been called the third time after two successful calls within 80 ticks
		/// </summary>
		public bool ThreeTimesUseTime()
		{
			uint diff = (uint)Math.Abs(Main.GameUpdateCount - lastTime);
			if (diff > 40.0)
			{
				//19
				resetPetAccessories = false;
				lastTime = Main.GameUpdateCount;
				return false; //step one
			}

			//step two and three have to be done in 40 ticks each
			if (diff <= 40.0)
			{
				if (!resetPetAccessories)
				{
					lastTime = Main.GameUpdateCount;
					resetPetAccessories = true;
					return false; //step two
				}

				//if program gets to here, it is about to return true

				if (resetPetAccessories)
				{
					resetPetAccessories = false;
					return true; //step three
				}
			}
			//should never get here anyway
			return false;
		}

		public override void SaveData(TagCompound tag)
		{
			SavePetAccessories(tag);

			TagCompound petTags = new()
			{
				{ "mechFrogType", mechFrogType },
				{ "petEyeType", petEyeType },
				{ "cursedSkullType", cursedSkullType },
				{ "youngWyvernType", youngWyvernType },
				{ "petFishronType", petFishronType },
				{ "petMoonType", petMoonType },
				{ "abeeminationType", abeeminationType },
				{ "lilWrapsType", lilWrapsType },
				{ "vampireBatType", vampireBatType },
				{ "pigronataType", pigronataType },
				{ "queenLarvaType", queenLarvaType },
				{ "miniAntlionType", miniAntlionType },
				{ "petGoldfishType", petGoldfishType },
				{ "skeletronHandType", skeletronHandType },
				{ "skeletronPrimeHandType", skeletronPrimeHandType },
				{ "petCultistType", petCultistType },
				{ "petAnomalocarisType", petAnomalocarisType },
				{ "dynamiteBunnyType", dynamiteBunnyType },
				{ "wallFragmentType", wallFragmentType },
				{ "metroidPetType", metroidPetType },
				{ "cuteLamiaPetType", cuteLamiaPetType },

				{ "oceanSlimeType", oceanSlimeType },
				{ "stingSlimeType", stingSlimeType },
				{ "animatedTomeType", animatedTomeType },

				{ "youngHarpyType", youngHarpyType }
			};

			tag.Add("petTags", petTags);
		}

		private void SavePetAccessories(TagCompound tag)
		{
			var petAccessoryIdentities = new List<PetAccessoryIdentity>();

			for (int i = 0; i < petAccessoriesBySlots.Length; i++)
			{
				var (id, altTextureIndex) = petAccessoriesBySlots[i];
				var modName = string.Empty;
				var name = string.Empty;
				var index = (byte)0;
				if (id > 0)
				{
					var petAccessory = PetAccessory.GetAccessoryFromID((SlotType)(i + 1), id);

					modName = petAccessory.Mod.Name;
					name = petAccessory.Name;
					index = altTextureIndex;
				}

				petAccessoryIdentities.Add(new PetAccessoryIdentity(modName, name, index));
			}

			if (petAccessoryIdentities.Count > 0)
			{
				tag.Add("petAccessoryIdentities", petAccessoryIdentities);
			}

			if (unloadedPetAccessoryIdentities.Count > 0)
			{
				tag.Add("unloadedPetAccessoryIdentities", unloadedPetAccessoryIdentities);
			}
		}

		public override void LoadData(TagCompound tag)
		{
			string mainKey = "petAccessoryIdentities";
			string secondaryKey = "unloadedPetAccessoryIdentities";
			if (tag.ContainsKey(mainKey) || tag.ContainsKey(secondaryKey))
			{
				LoadPetAccessories(tag, tag.GetList<PetAccessoryIdentity>(mainKey).ToList(), tag.GetList<PetAccessoryIdentity>(secondaryKey).ToList());
			}

			mainKey = "petTags";
			if (tag.ContainsKey(mainKey))
			{
				var petTags = tag.Get<TagCompound>(mainKey);

				mechFrogType = petTags.GetByte("mechFrogType");
				petEyeType = petTags.GetByte("petEyeType");
				cursedSkullType = petTags.GetByte("cursedSkullType");
				youngWyvernType = petTags.GetByte("youngWyvernType");
				petFishronType = petTags.GetByte("petFishronType");
				petMoonType = petTags.GetByte("petMoonType");
				abeeminationType = petTags.GetByte("abeeminationType");
				lilWrapsType = petTags.GetByte("lilWrapsType");
				vampireBatType = petTags.GetByte("vampireBatType");
				pigronataType = petTags.GetByte("pigronataType");
				queenLarvaType = petTags.GetByte("queenLarvaType");
				miniAntlionType = petTags.GetByte("miniAntlionType");
				petGoldfishType = petTags.GetByte("petGoldfishType");
				skeletronHandType = petTags.GetByte("skeletronHandType");
				skeletronPrimeHandType = petTags.GetByte("skeletronPrimeHandType");
				petCultistType = petTags.GetByte("petCultistType");
				petAnomalocarisType = petTags.GetByte("petAnomalocarisType");
				dynamiteBunnyType = petTags.GetByte("dynamiteBunnyType");
				wallFragmentType = petTags.GetByte("wallFragmentType");
				metroidPetType = petTags.GetByte("metroidPetType");
				cuteLamiaPetType = petTags.GetByte("cuteLamiaPetType");

				oceanSlimeType = petTags.GetByte("oceanSlimeType");
				stingSlimeType = petTags.GetByte("stingSlimeType");
				animatedTomeType = petTags.GetByte("animatedTomeType");

				youngHarpyType = petTags.GetByte("youngHarpyType");
			}
		}

		private void LoadPetAccessories(TagCompound tag, List<PetAccessoryIdentity> petAccessories, List<PetAccessoryIdentity> unloadedPetAccessories)
		{
			//Keep same size as the loaded list
			var emptyPetAccessory = new PetAccessoryIdentity(string.Empty, string.Empty, 0);
			unloadedPetAccessoryIdentities = new();
			for (int i = 0; i < petAccessories.Count; i++)
			{
				unloadedPetAccessoryIdentities.Add(emptyPetAccessory);
			}

			for (int i = 0; i < petAccessoriesBySlots.Length; i++)
			{
				var loadedPetAccessory = petAccessories[i];
				(string modName, string name, byte index) = loadedPetAccessory;

				bool hasUnloaded = unloadedPetAccessories.Count > 0;

				if (modName != string.Empty && name != string.Empty)
				{
					//Prioritize handling existing one
					if (ModContent.TryFind(modName, name, out PetAccessory petAccessory))
					{
						var id = petAccessory.ID;
						petAccessoriesBySlots[i] = (id, index);
					}
					else
					{
						unloadedPetAccessoryIdentities[i] = loadedPetAccessory;
					}
				}
				else if (hasUnloaded)
				{
					var unloadedPetAccessory = unloadedPetAccessories[i];
					(string unloadedModName, string unloadedName, byte unloadedIndex) = unloadedPetAccessory;

					if (unloadedModName != string.Empty && unloadedName != string.Empty)
					{
						//Fall back to unloaded one
						if (ModContent.TryFind(unloadedModName, unloadedName, out PetAccessory petAccessory))
						{
							var id = petAccessory.ID;
							petAccessoriesBySlots[i] = (id, unloadedIndex);
						}
						else
						{
							unloadedPetAccessoryIdentities[i] = unloadedPetAccessory;
						}
					}
				}
			}
		}

		public override void clientClone(ModPlayer clientClone)
		{
			PetPlayer clone = clientClone as PetPlayer;

			Array.Copy(petAccessoriesBySlots, clone.petAccessoriesBySlots, petAccessoriesBySlots.Length);
			Array.Copy(lastPetAccessoriesBySlots, clone.lastPetAccessoriesBySlots, lastPetAccessoriesBySlots.Length);
			Array.Copy(ClonedTypes, clone.ClonedTypes, ClonedTypes.Length);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			PetPlayer clone = clientPlayer as PetPlayer;
			PetPlayerChanges changes = PetPlayerChanges.None;
			int index = 255;

			bool slotChanges = false;
			for (int i = 0; i < petAccessoriesBySlots.Length; i++)
			{
				if (clone.petAccessoriesBySlots[i] != petAccessoriesBySlots[i])
				{
					slotChanges = true;
					break;
				}
			}

			if (slotChanges)
			{
				changes = PetPlayerChanges.Slots;
			}
			else
			{
				for (int i = 0; i < ClonedTypes.Length; i++)
				{
					if (clone.ClonedTypes[i] != ClonedTypes[i])
					{
						changes = PetPlayerChanges.PetTypes;
						index = i;
						break;
					}
				}
			}

			if (changes != PetPlayerChanges.None) SendClientChangesPacket(changes, index);
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)AssMessageType.SyncPlayerVanity);
			packet.Write((byte)Player.whoAmI);
			//no "changes" packet
			SendFieldValues(packet);
			packet.Send(toWho, fromWho);
		}

		private void SendFieldValues(ModPacket packet)
		{
			for (int i = 0; i < petAccessoriesBySlots.Length; i++)
			{
				var (id, altTextureIndex) = petAccessoriesBySlots[i];
				packet.Write((byte)id);
				packet.Write((byte)altTextureIndex);
			}

			for (int i = 0; i < ClonedTypes.Length; i++)
			{
				packet.Write((byte)ClonedTypes[i]);
			}
		}

		public void RecvSyncPlayerVanitySub(BinaryReader reader)
		{
			for (int i = 0; i < petAccessoriesBySlots.Length; i++)
			{
				byte id = reader.ReadByte();
				byte altTextureIndex = reader.ReadByte();
				petAccessoriesBySlots[i] = (id, altTextureIndex);
			}

			for (int i = 0; i < ClonedTypes.Length; i++)
			{
				ClonedTypes[i] = reader.ReadByte();
			}
			GetFromClonedTypes();
		}

		/// <summary>
		/// Reads from reader to assign the player fields (Called in Mod.HandlePacket())
		/// </summary>
		public void RecvClientChangesPacketSub(BinaryReader reader, byte changes, int index)
		{
			//AssUtils.Print("RecvClientChangesPacketSub " + changes + " index " + index + " from p " + player.whoAmI);
			switch (changes)
			{
				case (byte)PetPlayerChanges.All:
					RecvSyncPlayerVanitySub(reader);
					break;
				case (byte)PetPlayerChanges.Slots:
					for (int i = 0; i < petAccessoriesBySlots.Length; i++)
					{
						byte id = reader.ReadByte();
						byte altTextureIndex = reader.ReadByte();
						petAccessoriesBySlots[i] = (id, altTextureIndex);
					}
					break;
				case (byte)PetPlayerChanges.PetTypes:
					byte type = reader.ReadByte();
					if (index >= 0 && index < ClonedTypes.Length) ClonedTypes[index] = type;
					break;
				default: //shouldn't get there hopefully
					Mod.Logger.Debug("Received unspecified PetPlayerChanges Packet " + changes);
					break;
			}
			GetFromClonedTypes();
		}

		/// <summary>
		/// Sends the player fields either to clients or to the server. Called in Mod.HandlePacket()
		/// (forwarding data from the server to other players)
		/// </summary>
		public void SendClientChangesPacketSub(byte changes, int index, int toClient = -1, int ignoreClient = -1)
		{
			//AssUtils.Print("SendClientChangesPacketSub " + changes + " index " + index + " from p " + player.whoAmI + ((Main.netMode == NetmodeID.MultiplayerClient)? " client":" server"));
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)AssMessageType.ClientChangesVanity);
			packet.Write((byte)Player.whoAmI);
			packet.Write((byte)changes);
			packet.Write((byte)index);

			switch (changes)
			{
				case (byte)PetPlayerChanges.All:
					SendFieldValues(packet);
					break;
				case (byte)PetPlayerChanges.Slots:
					for (int i = 0; i < petAccessoriesBySlots.Length; i++)
					{
						var (id, altTextureIndex) = petAccessoriesBySlots[i];
						packet.Write((byte)id);
						packet.Write((byte)altTextureIndex);
					}
					break;
				case (byte)PetPlayerChanges.PetTypes:
					packet.Write((byte)ClonedTypes[index]);
					break;
				default: //shouldn't get there hopefully
					Mod.Logger.Debug("Sending unspecified PetPlayerChanges " + changes);
					break;
			}

			packet.Send(toClient, ignoreClient);
		}

		/// <summary>
		/// Cliendside method to send the chances specified to the server.
		/// Called in SendClientChanges()
		/// </summary>
		private void SendClientChangesPacket(PetPlayerChanges changes, int index = 255)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				SendClientChangesPacketSub((byte)changes, index);
			}
		}

		public override void OnEnterWorld(Player player)
		{
			enteredWorld = true;
		}

		public override void PreUpdate()
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				SetClonedTypes();
			}

			if (ContentConfig.Instance.CuteSlimes)
			{
				ValidateSlimePetIndex();

				slimeHugsUpdatedThisTick = false;

				if (!createdSlimeHugs)
				{
					createdSlimeHugs = true;

					SlimeHugs = new List<SlimeHug>();
					SlimeHugs.AddRange(Mod.GetContent<SlimeHug>().Select(h => (SlimeHug)h.Clone())); //Fetch every loaded hug, create a new instance of it to use for this player
					SlimeHugs.Sort((s1, s2) => s1.CompareTo(s2)); //Sort by cooldown, takes priority from high cooldown
				}
			}
		}

		/// <summary>
		/// Returns true if the player has a cute slime pet spawned
		/// </summary>
		public bool HasValidSlimePet(out SlimePet slimePet)
		{
			slimePet = null;

			if (!ContentConfig.Instance.CuteSlimes)
			{
				return false;
			}

			if (slimePetIndex >= 0 && slimePetIndex < Main.maxProjectiles)
			{
				if (Main.projectile[slimePetIndex] is Projectile projectile)
				{
					if (projectile.active && projectile.owner == Player.whoAmI && SlimePets.TryGetPetFromProj(projectile.type, out slimePet))
					{
						return true;
					}
				}
			}
			return false;
		}

		#region Slime Pet Hug
		public bool slimeHugsUpdatedThisTick = false; //Protection against multiple slime pets updating it at the same time
		public const int HugDelaySuccess = 60 * 60 * 5;
		public const int HugDelayFail = 60 * 60 * 3;
		public int slimeHugTimer = -HugDelaySuccess / 2; //Global hug timer
		public bool IsHugging => slimeHugTimer > 0;

		public List<SlimeHug> SlimeHugs { get; private set; } = new List<SlimeHug>();

		public bool createdSlimeHugs = false;
		public int slimePetIndex = -1;

		/// <summary>
		/// Returns true if a slime hug of this type exists and is set to <paramref name="slimeHug"/>
		/// </summary>
		public bool TryGetSlimeHug(int type, out SlimeHug slimeHug)
		{
			slimeHug = null;
			if (type == -1) return false;
			slimeHug = SlimeHugs.FirstOrDefault(h => h.Type == type);

			return slimeHug != null;
		}

		/// <summary>
		/// Global condition under which hug cooldowns will be handled
		/// </summary>
		public static bool CanHandleCooldown(CuteSlimeBaseProj slime)
		{
			return slime.OnGround;
		}

		/// <summary>
		/// Check active state of the player for if he is legible to receive a hug
		/// </summary>
		public static bool IsHuggable(Player player)
		{
			return !player.mount.Active && !player.pulley && player.velocity.Y == 0f && player.itemAnimation == 0;
		}

		/// <summary>
		/// Resets the slime pet index if necessary
		/// </summary>
		private void ValidateSlimePetIndex()
		{
			if (!HasValidSlimePet(out _))
			{
				slimePetIndex = -1;
			}
		}

		//In player:
		//1. handle each hug timer in loop separately
		//2. If conditions met for initiating a hug, choose a hug
		//2a. If no hug chosen, return
		//3. If hug chosen but conditions no longer met, fail and return
		//4. Assign hug type

		//In projectile:
		//5. Apply "appoach to player" AI, play prehug emote
		//6. If close enough to player, set hug timer to hug duration
		//7. Change AI to hugging, cancelling any other AI
		//8. Decrement slimeHugTimer, cancel when 0
		//- Active hug is stored on the projectile (set through SetHugType)
		public void UpdateSlimeHugs(CuteSlimeBaseProj slime)
		{
			if (!ContentConfig.Instance.CuteSlimes)
			{
				return;
			}

			//This is called from within the current slime pet, as it is intertwined with its AI
			if (slimePetIndex < 0)
			{
				//Bad call, reset timer
				slimeHugTimer = -HugDelayFail;
				return;
			}

			if (slimeHugsUpdatedThisTick)
			{
				return;
			}

			slime.oldHugType = slime.hugType;

			slimeHugsUpdatedThisTick = true;

			if (IsHugging)
			{
				//Handle timer during a hug
				slimeHugTimer--;
				if (slimeHugTimer == 0)
				{
					//Hug succeeded, set delay
					slime.SetHugType(-1);
					slimeHugTimer = -HugDelaySuccess;
				}
			}
			else
			{
				if (slimeHugTimer < 0)
				{
					slimeHugTimer++;
				}
			}

			SlimeHug newHug = null;
			foreach (var hug in SlimeHugs)
			{
				if (CanHandleCooldown(slime) && hug.HandleCooldown())
				{
					if (hug.IsAvailable(slime, this))
					{
						newHug = hug;
					}
				}
			}

			if (slimeHugTimer != 0 || !IsHuggable(Player) || !slime.CanChooseHug(Player))
			{
				return;
			}

			if (newHug != null) //Atleast one off cooldown
			{
				if (slime.hugType != newHug.Type)
				{
					slime.SetHugType(newHug.Type);
					newHug.ApplyCooldown();
				}

				if (!IsHuggable(Player))
				{
					//Cancel sequence prematurely
					slime.SetHugType(-1);
					slimeHugTimer = -HugDelayFail;
					return;
				}
			}
		}
		#endregion

		#region Slime Pet Vanity
		private bool resetPetAccessories = false;
		private uint lastTime = 0;

		public (byte id, byte altTextureIndex)[] petAccessoriesBySlots;
		public (byte id, byte altTextureIndex)[] lastPetAccessoriesBySlots;

		public List<PetAccessoryIdentity> unloadedPetAccessoryIdentities;

		public bool HasPetAccessories
		{
			get
			{
				for (int i = 0; i < petAccessoriesBySlots.Length; i++)
				{
					if (petAccessoriesBySlots[i].id > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Adds the pet vanity accessory to the current pet
		/// </summary>
		public bool AddAccessory(PetAccessory petAccessory)
		{
			byte slotNumber = (byte)(petAccessory.Slot - 1);

			//id is between 0 and 255
			byte id = petAccessory.ID;
			byte altTextureIndex = petAccessory.AltTextureIndex;

			(byte currentID, byte currentAltTextureIndex) = petAccessoriesBySlots[slotNumber];

			//returns false if accessory was already equipped
			if (id == currentID && altTextureIndex == currentAltTextureIndex)
			{
				return false;
			}

			//else sets new
			petAccessoriesBySlots[slotNumber] = (id, altTextureIndex);

			return true;
		}

		/// <summary>
		/// Deletes the pet vanity accessory on the current pet
		/// </summary>
		public void DelAccessory(PetAccessory petAccessory)
		{
			byte slotNumber = (byte)(petAccessory.Slot - 1);
			petAccessoriesBySlots[slotNumber] = (0, 0);
		}

		/// <summary>
		/// Toggles the pet vanity acessory on the current pet
		/// </summary>
		public void ToggleAccessory(PetAccessory petAccessory)
		{
			if (petAccessory.Slot == SlotType.None) throw new Exception("Can't toggle accessory on reserved slot");
			if (!AddAccessory(petAccessory)) DelAccessory(petAccessory);
		}

		/// <summary>
		/// Returns the pet vanity accessory equipped in the specified SlotType of the current pet
		/// </summary>
		public bool TryGetAccessoryInSlot(byte slotNumber, out PetAccessory petAccessory)
		{
			petAccessory = null;
			byte slot = slotNumber;
			slotNumber -= 1;

			(byte id, byte altTextureIndex) = petAccessoriesBySlots[slotNumber];

			if (id == 0)
			{
				return false;
			}

			petAccessory = PetAccessory.GetAccessoryFromID((SlotType)slot, id);
			petAccessory.AltTextureIndex = altTextureIndex;

			return petAccessory != null;
		}
		#endregion

		#region CircleUI
		/// <summary>
		/// Contains a list of CircleUIHandlers that are used in CircleUIStart/End in Mod
		/// </summary>
		public List<CircleUIHandler> CircleUIList;

		/// <summary>
		/// Contains a list of pet type fields (assigned during Load() and every tick in PreUpdate(),
		/// and read from in in Multiplayer whenever data is received).
		/// Simplifies the saving/loading of tags
		/// </summary>
		public byte[] ClonedTypes;

		public static CircleUIConf GetLifelikeMechanicalFrogConf()
		{
			List<Asset<Texture2D>> assets = new List<Asset<Texture2D>>() {
						AssUtils.Instance.Assets.Request<Texture2D>("Projectiles/Pets/LifelikeMechanicalFrogProj"),
						AssUtils.Instance.Assets.Request<Texture2D>("Projectiles/Pets/LifelikeMechanicalFrogProjCrown") };

			List<string> tooltips = new List<string>() { "Default", "Crowned" };

			//no need for unlocked + toUnlock
			return new CircleUIConf(
				Main.projFrames[ModContent.ProjectileType<LifelikeMechanicalFrogProj>()],
				ModContent.ProjectileType<LifelikeMechanicalFrogProj>(),
				assets, null, tooltips, null);
		}

		public static CircleUIConf GetDocileDemonEyeConf()
		{
			List<string> tooltips = new List<string>() { "Red", "Green", "Purple",
				"Red Fractured", "Green Fractured", "Purple Fractured",
				"Red Mechanical", "Green Mechanical", "Purple Mechanical",
				"Red Laser", "Green Laser", "Purple Laser" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<DocileDemonEyeProj>(), tooltips);
		}

		public static CircleUIConf GetCursedSkullConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Dragon" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<CursedSkullProj>(), tooltips);
		}

		public static CircleUIConf GetYoungWyvernConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Mythical", "Arch", "Arch (Legacy)" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<YoungWyvernProj>(), tooltips);
		}

		public static CircleUIConf GetPetFishronConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Sharkron", "Sharknado" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<PetFishronProj>(), tooltips);
		}

		public static CircleUIConf GetPetMoonConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Yellow", "Ringed", "Mythril", "Bright Blue", "Green", "Pink", "Orange", "Purple", }; //9 10 11 are contextual

			return CircleUIHandler.PetConf(ModContent.ProjectileType<PetMoonProj>(), tooltips);
		}

		public static CircleUIConf GetYoungHarpyConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Eagle", "Raven", "Dove", "Default (Legacy)", "Eagle (Legacy)", "Raven (Legacy)", "Dove (Legacy)" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<YoungHarpyProj>(), tooltips, new Vector2(0f, 4f));
		}

		public static CircleUIConf GetAbeeminationConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Snow Bee", "Oil Spill", "Missing Ingredients" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<AbeeminationProj>(), tooltips, new Vector2(0f, -4f));
		}

		public static CircleUIConf GetLilWrapsConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Dark", "Light", "Shadow", "Spectral" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<LilWrapsProj>(), tooltips);
		}

		public static CircleUIConf GetVampireBatConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Werebat" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<VampireBatProj>(), tooltips);
		}

		public static CircleUIConf GetPigronataConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Winter", "Autumn", "Spring", "Summer", "Halloween", "Christmas" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<PigronataProj>(), tooltips);
		}

		public static CircleUIConf GetQueenLarvaConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Prawn Larva", "Unexpected Seed", "Big Kid Larva", "Where's The Baby?" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<QueenLarvaProj>(), tooltips);
		}

		public static CircleUIConf GetOceanSlimeConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Stupid Hat", "Gnarly Grin", "Flipped Jelly" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<OceanSlimeProj>(), tooltips, new Vector2(1f, -4f));
		}

		public static CircleUIConf GetStingSlimeConf()
		{
			List<string> tooltips = new List<string>() { "Black", "Orange" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<StingSlimeProj>(), tooltips, new Vector2(1f, -4f));
		}

		public static CircleUIConf GetMiniAntlionConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Albino", "Larval" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<MiniAntlionProj>(), tooltips, new Vector2(0f, -4f));
		}

		public static CircleUIConf PetGoldfishConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Crimson", "Corruption", "Bunny" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<PetGoldfishProj>(), tooltips);
		}

		public static CircleUIConf GetSkeletronHandConf()
		{
			List<string> tooltips = new List<string>() { "Default", "OK-Hand", "Peace", "Rock It", "Fist" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<SkeletronHandProj>(), tooltips, new Vector2(2, -4f));
		}

		public static CircleUIConf GetSkeletronPrimeHandConf()
		{
			List<string> tooltips = new List<string>() { "Cannon", "Saw", "Vice", "Laser" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<SkeletronPrimeHandProj>(), tooltips, new Vector2(0f, -4f));
		}

		public static CircleUIConf GetPetCultistConf()
		{
			List<string> tooltips = new List<string>() { "Lunar", "Solar" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<PetCultistProj>(), tooltips, new Vector2(1f, 0f));
		}

		public static CircleUIConf GetAnimatedTomeConf()
		{
			List<string> tooltips = new List<string>() { "Green", "Blue", "Purple", "Pink", "Yellow", "Spell" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<AnimatedTomeProj>(), tooltips);
		}

		public static CircleUIConf GetAnomalocarisConf()
		{
			List<string> tooltips = new List<string>() { "Wild", "Shrimpy", "Snippy" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<AnomalocarisProj>(), tooltips);
		}

		public static CircleUIConf GetDynamiteBunnyConf()
		{
			List<string> tooltips = new List<string>() { "White", "Corrupt", "Crimtane", "Angora", "Dutch", "Flemish", "Lop", "Silver", "Caerbannog" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<DynamiteBunnyProj>(), tooltips, new Vector2(0f, -7f));
		}

		public static CircleUIConf GetWallFragmentConf()
		{
			List<string> tooltips = new List<string>() { "Default", "Chinese" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<WallFragmentMouth>(), tooltips);
		}

		public static CircleUIConf GetMetroidPetConf()
		{
			List<string> tooltips = new List<string>() { "Metroid", "Failed Clone", "Convergent", "Irradiated", "Corrupted", "The Baby" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<MetroidPetProj>(), tooltips, new Vector2(0f, -2f));
		}

		public static CircleUIConf GetCuteLamiaPetConf()
		{
			List<string> tooltips = new List<string>() { "Dark", "Light", "Dropkick" };

			return CircleUIHandler.PetConf(ModContent.ProjectileType<CuteLamiaPetProj>(), tooltips, new Vector2(0f, 0f));
		}

		//ALTERNATE
		//public static CircleUIConf GetClassNameConf()
		//{
		//    List<string> tooltips = new List<string>() { "Default", "AltName1", "AltName2" };

		//    return CircleUIHandler.PetConf(ModContent.ProjectileType<ClassNameProj>(), tooltips);
		//}

		public override void Initialize()
		{
			ClonedTypes = new byte[altTextureCountLoaded];

			mechFrogType = 0;
			petEyeType = 0;
			cursedSkullType = 0;
			youngWyvernType = 0;
			petFishronType = 0;
			petMoonType = 0;
			abeeminationType = 0;
			lilWrapsType = 0;
			vampireBatType = 0;
			pigronataType = 0;
			queenLarvaType = 0;
			miniAntlionType = 0;
			petGoldfishType = 0;
			skeletronHandType = 0;
			skeletronPrimeHandType = 0;
			petCultistType = 0;
			petAnomalocarisType = 0;
			dynamiteBunnyType = 0;
			wallFragmentType = 0;
			metroidPetType = 0;
			cuteLamiaPetType = 0;

			oceanSlimeType = 0;
			stingSlimeType = 0;
			animatedTomeType = 0;

			youngHarpyType = 0;

			petAccessoriesBySlots = new (byte, byte)[4]; //C# initializes the array as (0,0)
			lastPetAccessoriesBySlots = new (byte, byte)[4];

			unloadedPetAccessoryIdentities = new List<PetAccessoryIdentity>();

			SlimeHugs = new List<SlimeHug>();

			//called before Load()
			//needs to call new List() since Initialize() is called per player in the player select screen

			CircleUIList = new List<CircleUIHandler>();

			if (ContentConfig.Instance.OtherPets)
			{
				CircleUIList.AddRange(new List<CircleUIHandler>
				{
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => LifelikeMechanicalFrog,
					uiConf: GetLifelikeMechanicalFrogConf,
					onUIStart: () => mechFrogType,
					onUIEnd: () => mechFrogType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => DocileDemonEye,
					uiConf: GetDocileDemonEyeConf,
					onUIStart: () => petEyeType,
					onUIEnd: () => petEyeType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => CursedSkull,
					uiConf: GetCursedSkullConf,
					onUIStart: () => cursedSkullType,
					onUIEnd: () => cursedSkullType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => YoungWyvern,
					uiConf: GetYoungWyvernConf,
					onUIStart: () => youngWyvernType,
					onUIEnd: () => youngWyvernType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => PetMoon,
					uiConf: GetPetMoonConf,
					onUIStart: () => petMoonType,
					onUIEnd: () => petMoonType = (byte)CircleUI.returned,
					triggerLeft: false
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => Abeemination,
					uiConf: GetAbeeminationConf,
					onUIStart: () => abeeminationType,
					onUIEnd: () => abeeminationType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => VampireBat,
					uiConf: GetVampireBatConf,
					onUIStart: () => vampireBatType,
					onUIEnd: () => vampireBatType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => Pigronata,
					uiConf: GetPigronataConf,
					onUIStart: () => pigronataType,
					onUIEnd: () => pigronataType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => PetGoldfish,
					uiConf: PetGoldfishConf,
					onUIStart: () => petGoldfishType,
					onUIEnd: () => petGoldfishType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => PetAnomalocaris,
					uiConf: GetAnomalocarisConf,
					onUIStart: () => petAnomalocarisType,
					onUIEnd: () => petAnomalocarisType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => DynamiteBunny,
					uiConf: GetDynamiteBunnyConf,
					onUIStart: () => dynamiteBunnyType,
					onUIEnd: () => dynamiteBunnyType = (byte)CircleUI.returned
				),
                //ALTERNATE
                //    new CircleUIHandler(
                //    triggerItem: ModContent.ItemType<VanitySelector>(),
                //    condition: () => ClassName,
                //    uiConf: GetClassNameConf,
                //    onUIStart: () => classNameType,
                //    onUIEnd: () => classNameType = (byte)CircleUI.returned,
                //    needsSaving: true
                //),
                });
			}

			if (ContentConfig.Instance.DroppedPets)
			{
				int vanitySelector = ModContent.ItemType<VanitySelector>();
				CircleUIList.AddRange(new List<CircleUIHandler>()
				{
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => QueenLarva,
					uiConf: GetQueenLarvaConf,
					onUIStart: () => queenLarvaType,
					onUIEnd: () => queenLarvaType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => MiniAntlion,
					uiConf: GetMiniAntlionConf,
					onUIStart: () => miniAntlionType,
					onUIEnd: () => miniAntlionType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => LilWraps,
					uiConf: GetLilWrapsConf,
					onUIStart: () => lilWrapsType,
					onUIEnd: () => lilWrapsType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => WallFragment,
					uiConf: GetWallFragmentConf,
					onUIStart: () => wallFragmentType,
					onUIEnd: () => wallFragmentType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => MetroidPet,
					uiConf: GetMetroidPetConf,
					onUIStart: () => metroidPetType,
					onUIEnd: () => metroidPetType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => CuteLamiaPet,
					uiConf: GetCuteLamiaPetConf,
					onUIStart: () => cuteLamiaPetType,
					onUIEnd: () => cuteLamiaPetType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => SkeletronHand,
					uiConf: GetSkeletronHandConf,
					onUIStart: () => skeletronHandType,
					onUIEnd: () => skeletronHandType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => SkeletronPrimeHand,
					uiConf: GetSkeletronPrimeHandConf,
					onUIStart: () => skeletronPrimeHandType,
					onUIEnd: () => skeletronPrimeHandType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => PetCultist,
					uiConf: GetPetCultistConf,
					onUIStart: () => petCultistType,
					onUIEnd: () => petCultistType = (byte)CircleUI.returned,
					triggerLeft: false
				),
					new CircleUIHandler(
					triggerItem: vanitySelector,
					condition: () => PetFishron,
					uiConf: GetPetFishronConf,
					onUIStart: () => petFishronType,
					onUIEnd: () => petFishronType = (byte)CircleUI.returned
				)
				});
			}

			if (ContentConfig.Instance.HostileNPCs)
			{
				CircleUIList.AddRange(new List<CircleUIHandler>()
				{
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => OceanSlime,
					uiConf: GetOceanSlimeConf,
					onUIStart: () => oceanSlimeType,
					onUIEnd: () => oceanSlimeType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => StingSlime,
					uiConf: GetStingSlimeConf,
					onUIStart: () => stingSlimeType,
					onUIEnd: () => stingSlimeType = (byte)CircleUI.returned
				),
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => AnimatedTome,
					uiConf: GetAnimatedTomeConf,
					onUIStart: () => animatedTomeType,
					onUIEnd: () => animatedTomeType = (byte)CircleUI.returned
				)
				});
			}

			if (ContentConfig.Instance.FriendlyNPCs)
			{
				CircleUIList.AddRange(new List<CircleUIHandler>()
				{
					new CircleUIHandler(
					triggerItem: ModContent.ItemType<VanitySelector>(),
					condition: () => YoungHarpy,
					uiConf: GetYoungHarpyConf,
					onUIStart: () => youngHarpyType,
					onUIEnd: () => youngHarpyType = (byte)CircleUI.returned
				),
				});
			}

			//after filling the list
			for (int i = 0; i < CircleUIList.Count; i++)
			{
				CircleUIHandler circleUIHandler = CircleUIList[i];

				//set the trigger list
				circleUIHandler.AddTriggers();
			}
		}

		/// <summary>
		/// Called whenever something is received.
		/// Sets the pet type of the corresponding entry of ClonedTypes
		/// </summary>
		public void GetFromClonedTypes()
		{
			//AssUtils.Print("set getfromclonedtypes p " + player.whoAmI + " " + mp);
			int index = 0;

			mechFrogType = ClonedTypes[index++];
			petEyeType = ClonedTypes[index++];
			cursedSkullType = ClonedTypes[index++];
			youngWyvernType = ClonedTypes[index++];
			petFishronType = ClonedTypes[index++];
			petMoonType = ClonedTypes[index++];
			abeeminationType = ClonedTypes[index++];
			lilWrapsType = ClonedTypes[index++];
			vampireBatType = ClonedTypes[index++];
			pigronataType = ClonedTypes[index++];
			queenLarvaType = ClonedTypes[index++];
			miniAntlionType = ClonedTypes[index++];
			petGoldfishType = ClonedTypes[index++];
			skeletronHandType = ClonedTypes[index++];
			skeletronPrimeHandType = ClonedTypes[index++];
			petCultistType = ClonedTypes[index++];
			petAnomalocarisType = ClonedTypes[index++];
			dynamiteBunnyType = ClonedTypes[index++];
			wallFragmentType = ClonedTypes[index++];
			metroidPetType = ClonedTypes[index++];
			cuteLamiaPetType = ClonedTypes[index++];
			//ALTERNATE
			//classNameType = ClonedTypes[index++];

			oceanSlimeType = ClonedTypes[index++];
			stingSlimeType = ClonedTypes[index++];
			animatedTomeType = ClonedTypes[index++];

			youngHarpyType = ClonedTypes[index++];
		}

		/// <summary>
		/// Called in PreUpdate (which runs before OnEnterWorld, hence the check).
		/// Sets each entry of ClonedTypes to the corresponding pet type
		/// </summary>
		public void SetClonedTypes()
		{
			if (enteredWorld)
			{
				int index = -1;
				ClonedTypes[++index] = mechFrogType;
				ClonedTypes[++index] = petEyeType;
				ClonedTypes[++index] = cursedSkullType;
				ClonedTypes[++index] = youngWyvernType;
				ClonedTypes[++index] = petFishronType;
				ClonedTypes[++index] = petMoonType;
				ClonedTypes[++index] = abeeminationType;
				ClonedTypes[++index] = lilWrapsType;
				ClonedTypes[++index] = vampireBatType;
				ClonedTypes[++index] = pigronataType;
				ClonedTypes[++index] = queenLarvaType;
				ClonedTypes[++index] = miniAntlionType;
				ClonedTypes[++index] = petGoldfishType;
				ClonedTypes[++index] = skeletronHandType;
				ClonedTypes[++index] = skeletronPrimeHandType;
				ClonedTypes[++index] = petCultistType;
				ClonedTypes[++index] = petAnomalocarisType;
				ClonedTypes[++index] = dynamiteBunnyType;
				ClonedTypes[++index] = wallFragmentType;
				ClonedTypes[++index] = metroidPetType;
				ClonedTypes[++index] = cuteLamiaPetType;

				ClonedTypes[++index] = oceanSlimeType;
				ClonedTypes[++index] = stingSlimeType;
				ClonedTypes[++index] = animatedTomeType;

				ClonedTypes[++index] = youngHarpyType;
				//ALTERNATE
				//ClonedTypes[++index] = classNameType;
			}
		}

		#endregion
	}
}
