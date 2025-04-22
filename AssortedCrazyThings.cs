using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport;
using AssortedCrazyThings.Base.Netcode;
using AssortedCrazyThings.Effects;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.NPCs.DropConditions;
using AssortedCrazyThings.NPCs.Harvester;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
	public class AssortedCrazyThings : Mod
	{
		//Soul item animated textures
		public const int animatedSoulFrameCount = 6;
		public static Asset<Texture2D>[] animatedSoulTextures;

		/// <summary>
		/// Soul NPC spawn blacklist
		/// </summary>
		public static int[] soulBuffBlacklist;

		/// <summary>
		/// The cached type of the Harvester boss, 0 if not loaded
		/// </summary>
		public static int harvester;
		/// <summary>
		/// The cached type of the left talon of the Harvester boss, 0 if not loaded
		/// </summary>
		public static int harvesterTalonLeft;
		/// <summary>
		/// The cached type of the right talon of the Harvester boss, 0 if not loaded
		/// </summary>
		public static int harvesterTalonRight;

		//Mod Helpers compat
		public static string GithubUserName { get { return "Werebearguy"; } }
		public static string GithubProjectName { get { return "AssortedCrazyThings"; } }

		private void LoadSoulBuffBlacklist()
		{
			List<int> tempList = new List<int>
			{
				NPCID.Bee,
				NPCID.BeeSmall,
				NPCID.BlueSlime,
				NPCID.BlazingWheel,
				NPCID.EaterofWorldsHead,
				NPCID.EaterofWorldsBody,
				NPCID.EaterofWorldsTail,
				NPCID.Creeper,
				NPCID.GolemFistLeft,
				NPCID.GolemFistRight,
				NPCID.PlanterasHook,
				NPCID.PlanterasTentacle,
				NPCID.Probe,
				NPCID.ServantofCthulhu,
				NPCID.SlimeSpiked,
				NPCID.SpikeBall,
				NPCID.TheHungry,
				NPCID.TheHungryII,
			};

			soulBuffBlacklist = tempList.ToArray();
		}

		/// <summary>
		/// Assuming this is called after InitSoulBuffBlacklist.
		/// Adds NPC types to soulBuffBlacklist manually
		/// </summary>
		private void AddToSoulBuffBlacklist()
		{
			if (!ContentConfig.Instance.Bosses)
			{
				return;
			}

			//assuming this is called after InitSoulBuffBlacklist
			List<int> tempList = new List<int>(soulBuffBlacklist)
			{
				ModContent.NPCType<DungeonSoul>(),
				ModContent.NPCType<DungeonSoulFreed>(),
			};

			soulBuffBlacklist = tempList.ToArray();
			Array.Sort(soulBuffBlacklist);
		}

		/// <summary>
		/// Fills isModdedWormBodyOrTail with types of modded NPCs which names are ending with Body or Tail (indicating they are part of something)
		/// </summary>
		private void LoadWormList()
		{
			List<int> tempList = new List<int>();

			for (int i = NPCID.Count; i < NPCLoader.NPCCount; i++)
			{
				ModNPC modNPC = NPCLoader.GetNPC(i);
				if (modNPC != null && (modNPC.GetType().Name.EndsWith("Body") || modNPC.GetType().Name.EndsWith("Tail")))
				{
					tempList.Add(modNPC.NPC.type);
				}
			}

			AssUtils.isModdedWormBodyOrTail = tempList.ToArray();
			Array.Sort(AssUtils.isModdedWormBodyOrTail);
		}

		private void LoadHarvesterTypes()
		{
			if (!ContentConfig.Instance.Bosses)
			{
				return;
			}

			harvester = ModContent.NPCType<HarvesterBoss>();
			harvesterTalonLeft = ModContent.NPCType<HarvesterTalonLeft>();
			harvesterTalonRight = ModContent.NPCType<HarvesterTalonRight>();
		}

		private void LoadMisc()
		{
			if (!Main.dedServ && ContentConfig.Instance.Bosses)
			{
				animatedSoulTextures = new Asset<Texture2D>[2];

				animatedSoulTextures[0] = Assets.Request<Texture2D>("Items/CaughtDungeonSoulAnimated");
				animatedSoulTextures[1] = Assets.Request<Texture2D>("Items/CaughtDungeonSoulFreedAnimated");
			}

			//Needs to be initialized early since it's used outside of its own condition
			MatchAppearanceCondition.DescriptionText = AssUtils.GetDropConditionDescription(nameof(MatchAppearanceCondition));
		}

		private void UnloadMisc()
		{
			animatedSoulTextures = null;

			PetEaterofWorldsBase.wormTypes = null;

			PetDestroyerBase.wormTypes = null;
		}

		public override void Load()
		{
			ConfigurationSystem.Load();

			ShaderManager.Load();

			NetHandler.Load();

			LoadHarvesterTypes();

			LoadSoulBuffBlacklist();

			LoadMisc();
		}

		public override void Unload()
		{
			ConfigurationSystem.Unload();

			ShaderManager.Unload();

			NetHandler.Unload();

			UnloadMisc();

			GitgudData.Unload();

			EverhallowedLantern.DoUnload();
		}

		public override void PostSetupContent()
		{
			//for things that have to be called after Load() because of Main.projFrames[projectile.type] calls (and similar)
			LoadWormList();

			GitgudData.Load();

			DroneController.DoLoad();

			EverhallowedLantern.DoLoad();

			AddToSoulBuffBlacklist();

			if (ContentConfig.Instance.DroppedPets)
			{
				PetEaterofWorldsBase.wormTypes = new int[]
				{
				ModContent.ProjectileType<PetEaterofWorldsHead>(),
				ModContent.ProjectileType<PetEaterofWorldsBody1>(),
				ModContent.ProjectileType<PetEaterofWorldsBody2>(),
				ModContent.ProjectileType<PetEaterofWorldsTail>()
				};

				PetDestroyerBase.wormTypes = new int[]
				{
				ModContent.ProjectileType<PetDestroyerHead>(),
				ModContent.ProjectileType<PetDestroyerBody1>(),
				ModContent.ProjectileType<PetDestroyerBody2>(),
				ModContent.ProjectileType<PetDestroyerTail>()
				};
			}
		}

		public override object Call(params object[] args)
		{
			return ModCallHandler.Call(args);
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			NetHandler.HandlePackets(reader, whoAmI);
		}

		//Credit to jopojelly
		/// <summary>
		/// Makes alpha on .png textures actually properly rendered
		/// </summary>
		public static void PremultiplyTexture(Texture2D texture)
		{
			Color[] buffer = new Color[texture.Width * texture.Height];
			texture.GetData(buffer);
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
			}
			texture.SetData(buffer);
		}
	}

	public enum PetPlayerChanges : byte
	{
		None,
		All,
		Slots,
		PetTypes
	}
}
