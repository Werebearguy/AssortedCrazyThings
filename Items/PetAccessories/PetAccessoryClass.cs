using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.PetAccessories
{
	//Add new classes here in alphabetic order

	public class PetAccessoryBowtie : PetAccessoryItem
	{

	}

	public class PetAccessoryBunnyEars : PetAccessoryItem
	{

	}

	public class PetAccessoryBunnySuit : PetAccessoryItem
	{

	}

	public class PetAccessoryCrown : PetAccessoryItem
	{

	}

	public class PetAccessoryMetalHelmet : PetAccessoryItem
	{

	}

	public class PetAccessoryKitchenKnife : PetAccessoryItem
	{

	}

	public class PetAccessoryHairBow : PetAccessoryItem
	{

	}

	public class PetAccessoryMittens : PetAccessoryItem
	{

	}

	public class PetAccessoryPartyHat : PetAccessoryItem
	{

	}

	public class PetAccessoryPumpkinMask : PetAccessoryItem
	{

	}

	public class PetAccessorySlimeHead : PetAccessoryItem
	{

	}

	public class PetAccessoryStaff : PetAccessoryItem
	{

	}

	public class PetAccessorySwallowedKey : PetAccessoryItem
	{
		protected override bool UseDefaultRecipe => false;

		protected override void SafeAddRecipes()
		{
			Recipe recipe = Recipe.Create(ItemID.GoldenKey);
			recipe.AddIngredient(ModContent.ItemType<PetAccessorySwallowedKey>());
			recipe.Register();
		}
	}

	public class PetAccessoryTophat : PetAccessoryItem
	{

	}

	public class PetAccessoryToyBreastplate : PetAccessoryItem
	{

	}

	public class PetAccessoryToyShield : PetAccessoryItem
	{

	}

	public class PetAccessoryToyMace : PetAccessoryItem
	{

	}

	public class PetAccessoryToySpear : PetAccessoryItem
	{

	}

	public class PetAccessoryToySword : PetAccessoryItem
	{

	}

	public class PetAccessoryWizardHat : PetAccessoryItem
	{

	}

	public class PetAccessoryXmasHat : PetAccessoryItem
	{

	}

	/// <summary>
	/// The type of pet vanity it is. Used for rendering, has separate lists
	/// </summary>
	public enum SlotType : byte
	{
		None = 0, //reserved
		Body = 1,
		Hat = 2,
		Carried = 3,
		Accessory = 4

		//for Carried, it's actually only the front facing hand. For something like gloves or dual wielding, use Accessory instead

		//also, keep the sprite dimensions the same as the cute slimes

		//also they will be rendered in this order aswell (means that Carried can overlap with Body)

		//Each slot can have up to 255 accessories (total of 1020)
	}

	/// <summary>
	/// Contains data about all pet vanity accessories
	/// </summary>
	[Autoload(false)]
	public class PetAccessory : ModType
	{
		private static Dictionary<SlotType, List<PetAccessory>> petAccessoriesByType;
		private static Dictionary<SlotType, List<int>> petAccessoryIdsByType;

		/// <summary>
		/// Look-up table where the key is item ID and it returns the corresponding PetAccessory
		/// </summary>
		private static Dictionary<int, PetAccessory> petAccessoriesByItem;

		/// <summary>
		/// Contains all pet vanity accessories
		/// </summary>
		private static List<PetAccessory> petAccessoryList;

		/// <summary>
		/// Unique ID for this accessory (unique in the scope of a single SlotType)
		/// </summary>
		public byte ID { private set; get; }

		private readonly string _name;
		public override string Name => _name; //The item name associated with the accessory

		public int Type { private set; get; } //The item type associated with the accessory

		public SlotType Slot { private set; get; }

		private byte _altTextureIndex;
		/// <summary>
		/// Index for AltTextureSuffixes. No private setter, since it is set directly when used
		/// </summary>
		public byte AltTextureIndex
		{
			set => _altTextureIndex = value;
			get => Utils.Clamp(_altTextureIndex, (byte)0, (byte)(AltTextureSuffixes.Count - 1));
		}

		public Vector2 Offset { private set; get; }
		public bool PreDraw { private set; get; }
		public byte Alpha { private set; get; }
		public float Opacity => (255 - Alpha) / 255f;
		public bool UseNoHair { private set; get; }

		/// <summary>
		/// For deciding if it has a UI or not
		/// </summary>
		public bool HasAlts => AltTextureSuffixes.Count > 0;

		/// <summary>
		/// For the texture names
		/// </summary>
		public List<string> AltTextureSuffixes { private set; get; }

		/// <summary>
		/// For UI tooltips
		/// </summary>
		public List<LocalizedText> AltTextureDisplayNames { private set; get; }

		/// <summary>
		/// For UI only, the _Draw{number} stuff is done manually
		/// </summary>
		public List<Asset<Texture2D>> AltTextures { private set; get; }

		public PetAccessory(Mod mod, SlotType slot, string name, float offsetX = 0f, float offsetY = 0f, bool preDraw = false, byte alpha = 0, bool useNoHair = false, List<string> altTextures = null, List<string> altTextureNameOverrides = null)
		{
			_name = "PetAccessory" + name;
			if (!mod.TryFind(Name, out ModItem modItem))
			{
				throw new Exception($"Item called '{Name}' doesn't exist, are you sure you spelled it correctly?");
			}

			if (slot == SlotType.None)
			{
				throw new Exception($"Pet Accessory '{Name}' cannot have {nameof(SlotType.None)} as the slot!");
			}
			Slot = slot;
			Type = modItem.Type;

			AltTextureIndex = 0;

			Offset = new Vector2(offsetX, offsetY);
			PreDraw = preDraw;
			Alpha = alpha;
			UseNoHair = useNoHair;

			AltTextureSuffixes = new List<string>();
			AltTextureDisplayNames = new List<LocalizedText>();
			if (altTextures != null)
			{
				AltTextureSuffixes.AddRange(altTextures);
				string category = $"Items.{Name}.AccessoryNames.";
				for (int i = 0; i < altTextures.Count; i++)
				{
					string altTexture = altTextures[i];
					LocalizedText text = Language.GetOrRegister(mod.GetLocalizationKey($"{category}{altTexture}"), () => altTexture);
					AltTextureDisplayNames.Add(text);
				}
			}

			//add icons for UI
			AltTextures = new List<Asset<Texture2D>>(AltTextureSuffixes.Count);
			for (int i = 0; i < AltTextureSuffixes.Count; i++)
			{
				AltTextures.Add(Main.dedServ ? null : AssUtils.Instance.Assets.Request<Texture2D>("Items/PetAccessories/" + Name + AltTextureSuffixes[i]));
			}
		}

		/// <summary>
		/// Has to be static so we can decide when to load it (has to be after content from this mod finished loading)
		/// </summary>
		public static void RegisterAccessories()
		{
			petAccessoriesByType = new Dictionary<SlotType, List<PetAccessory>>();
			petAccessoryIdsByType = new Dictionary<SlotType, List<int>>();
			petAccessoriesByItem = new Dictionary<int, PetAccessory>();

			petAccessoryList = new List<PetAccessory>();

			//-------------------------------------------------------------------
			//------------ADD PET ACCESSORY PROPERTIES HERE----------------------
			//-------------------------------------------------------------------
			/*
             * How to:
             * - call Add() with the appropriate parameters
             * - The first object is the mod instance this PetAccessory belongs to. In our case, our mod.
             * - The second object is a PetAccessory, which has the following parameters:
             *   * slot: the SlotType associated with it, 
             *   * name: the name of the item class without the 'PetAccessory' infront, to save space
             *   * offsetX/Y: self explanatory, remember, negative X is left, negative Y is up
             *   * preDraw: decides if that accessory should be drawn "behind" the actual slime (false means it will draw infront)
             *   * alpha: says by how much it should be transparent (0 is fully opaque, 255 fully transparent)
             *   * useNoHair: used for SlotType.Hat, if the accessory should cover the hair and should use a NoHair texture of the slime if available
             *   * altTextures: a List of names that denotes the selection options for the UI
             *   * For each `altTexture` thing you specify in the list, you need to add a texture (for the UI) suffixed with that thing,
             *     and a `_Draw` texture (there will be a duplicate icon for the default item)
             *     
             * - after you've done that, add a `_Draw` texture with the same name as the item you add, now the item should work
             *
             * - if you want to remove certain accessories from being usable for the system, comment the Add() call out with //
             */
			Mod mod = AssUtils.Instance;

			//BODY SLOT ACCESSORIES GO HERE
			//------------------------------------------------
			Add(mod, new PetAccessory(mod, SlotType.Body, name: "Bowtie", altTextures: new List<string>() { "Red", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
			Add(mod, new PetAccessory(mod, SlotType.Body, name: "ToyBreastplate", altTextures: new List<string>() { "Iron", "Gold" }));
			Add(mod, new PetAccessory(mod, SlotType.Body, name: "BunnySuit", altTextures: new List<string>() { "Black", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Red" }));

			//HAT SLOT ACCESSORIES GO HERE
			//------------------------------------------------
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "Crown", altTextures: new List<string>() { "Gold", "Platinum" }));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "HairBow", altTextures: new List<string>() { "Red", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "MetalHelmet", useNoHair: true, altTextures: new List<string>() { "Iron", "Gold" }));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "SlimeHead", alpha: 56, altTextures: new List<string>() { "Blue", "Purple", "Pink", "Pinky", "Red", "Yellow", "Green", "Black" }));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "WizardHat", useNoHair: true));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "XmasHat", useNoHair: true, altTextures: new List<string>() { "Red", "Green" }));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "BunnyEars", preDraw: true));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "Tophat"));
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "PartyHat"));
			//Override is just the value, otherwise value = key
			Add(mod, new PetAccessory(mod, SlotType.Hat, name: "PumpkinMask", useNoHair: true,
				altTextures: new List<string> { "SOrange", "IOrange", "SGreen", "IGreen", "SWhite", "IWhite", "SPurple", "IPurple", "SMelon", "IMelon" },
				altTextureNameOverrides: new List<string> { "Sinister Orange", "Innocent Orange", "Sinister Green", "Innocent Green", "Sinister White", "Innocent White", "Sinister Purple", "Innocent Purple", "Sinister Melon", "Innocent Melon" }));

			//CARRIED SLOT ACCESSORIES GO HERE
			//------------------------------------------------
			Add(mod, new PetAccessory(mod, SlotType.Carried, name: "KitchenKnife", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
			Add(mod, new PetAccessory(mod, SlotType.Carried, name: "Staff", preDraw: true, altTextures: new List<string>() { "Amethyst", "Sapphire", "Emerald", "Ruby", "Amber", "Topaz", "Diamond", "Magic" }));
			Add(mod, new PetAccessory(mod, SlotType.Carried, name: "ToyMace", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
			Add(mod, new PetAccessory(mod, SlotType.Carried, name: "ToySpear", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));
			Add(mod, new PetAccessory(mod, SlotType.Carried, name: "ToySword", preDraw: true, altTextures: new List<string>() { "Iron", "Gold" }));

			//ACCESSORY SLOT ACCESSORIES GO HERE
			//------------------------------------------------
			Add(mod, new PetAccessory(mod, SlotType.Accessory, name: "Mittens", altTextures: new List<string>() { "Red", "Orange", "Gold", "Yellow", "Green", "Blue", "Purple", "Pink", "White", "Gray", "Black" }));
			Add(mod, new PetAccessory(mod, SlotType.Accessory, name: "SwallowedKey", preDraw: true));
			Add(mod, new PetAccessory(mod, SlotType.Accessory, name: "ToyShield", altTextures: new List<string>() { "Iron", "Gold" }));

			CreateMaps();
		}

		public static void UnloadAccessories()
		{
			petAccessoriesByType = null;
			petAccessoryIdsByType = null;
			petAccessoriesByItem = null;

			petAccessoryList = null;
		}

		/// <summary>
		/// Called after <see cref="RegisterAccessories"/>, creates a map of pet vanity accessory -> ID for each SlotType
		/// </summary>
		public static void CreateMaps()
		{
			foreach (SlotType slotType in Enum.GetValues(typeof(SlotType)))
			{
				if (slotType != SlotType.None)
				{
					List<PetAccessory> tempAccessoryList = petAccessoriesByType[slotType];

					if (!petAccessoryIdsByType.ContainsKey(slotType))
					{
						petAccessoryIdsByType[slotType] = new List<int>();
					}

					List<int> tempIdList = petAccessoryIdsByType[slotType];
					for (int i = 0; i < tempAccessoryList.Count; i++)
					{
						tempIdList.Add(tempAccessoryList[i].ID);
					}
				}
			}

			for (int i = 0; i < petAccessoryList.Count; i++)
			{
				PetAccessory petAccessory = petAccessoryList[i];
				petAccessoriesByItem[petAccessory.Type] = petAccessory;
			}
		}

		/// <summary>
		/// Add a new pet vanity accessory to the game
		/// </summary>
		public static void Add(Mod mod, PetAccessory aPetAccessory)
		{
			mod.AddContent(aPetAccessory); //Makes ModContent.GetInstance and .Mod work
		}

		/// <summary>
		/// Called in <see cref="PetPlayer.GetAccessoryInSlot"/>.
		/// Used to retreive the pet vanity accessory in the current slot of that current ID
		/// </summary>
		public static PetAccessory GetAccessoryFromID(SlotType slotType, byte id) //if something has the id, it always has the slottype available
		{
			return petAccessoriesByType[slotType][petAccessoryIdsByType[slotType].IndexOf(id)];
		}

		/// <summary>
		/// Returns the pet vanity accessory corresponding to the item type.
		/// </summary>
		public static bool TryGetAccessoryFromItem(int type, out PetAccessory petAccessory) //since item types are unique, just look up in the global list
		{
			if (!ContentConfig.Instance.CuteSlimes)
			{
				petAccessory = null;
				return false;
			}

			return petAccessoriesByItem.TryGetValue(type, out petAccessory);
		}

		public bool IsSameAs(PetAccessory other)
		{
			return ID == other.ID && Slot == other.Slot;
		}

		public override string ToString()
		{
			return "ID: " + ID
				+ "; Col: " + AltTextureIndex
				+ "; Slo: " + Slot
				+ "; Nam: " + Name
				+ "; Typ: " + Type
				+ "; Off: " + Offset
				+ "; Pre: " + (PreDraw ? "y" : "n")
				+ "; Alp: " + Alpha
				+ "; NoH: " + (UseNoHair ? "y" : "n")
				+ "; Alt: " + (HasAlts ? "y" : "n");
		}

		protected sealed override void Register()
		{
			SlotType slotType = this.Slot;

			if (!petAccessoriesByType.ContainsKey(slotType))
			{
				petAccessoriesByType[slotType] = new List<PetAccessory>();
			}

			//everything fine
			List<PetAccessory> petAccessories = petAccessoriesByType[slotType];
			petAccessoriesByType[slotType].Add(this);
			this.ID = (byte)petAccessories.Count; //Assign ID post-adding (so 0 represents "no accessory")

			petAccessoryList.Add(this);

			ModTypeLookup<PetAccessory>.Register(this); //Makes ModType related ModContent methods work
		}

		public sealed override void SetupContent() => SetStaticDefaults();
	}

	/// <summary>
	/// Class that all vanity accessories inherit from. Provides the functionality.
	/// Has a default recipe which can be changed
	/// </summary>
	[Content(ContentType.CuteSlimes)]
	public abstract class PetAccessoryItem : AssItem
	{
		protected override bool CloneNewInstances => true;

		public static LocalizedText BlacklistedText { get; private set; }
		public static LocalizedText NoUseText { get; private set; }
		public static LocalizedText DisabledText { get; private set; }

		[field: CloneByReference]
		public LocalizedText ShortNameText { get; private set; }

		public sealed override void SetStaticDefaults()
		{
			ShortNameText = this.GetLocalization("ShortName");

			string category = "Items.PetAccessory.";
			BlacklistedText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}Blacklisted"));
			NoUseText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}NoUse"));
			DisabledText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}Disabled"));

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.rare = 2;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item1;
			Item.consumable = false;
			Item.value = Item.sellPrice(silver: 30);
		}

		private static LocalizedText Enum2string(SlotType e)
		{
			string category = "Items.PetAccessory.";
			return Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{category}{Enum.GetName(typeof(SlotType), e)}"));
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (PetAccessory.TryGetAccessoryFromItem(Item.type, out PetAccessory petAccessory))
			{
				tooltips.Add(new TooltipLine(Mod, "Slot", Enum2string(petAccessory.Slot).ToString()));

				Player player = Main.LocalPlayer;
				PetPlayer pPlayer = player.GetModPlayer<PetPlayer>();

				if (pPlayer.HasValidSlimePet(out SlimePet slimePet))
				{
					if (slimePet.IsSlotTypeBlacklisted[(byte)petAccessory.Slot])
					{
						tooltips.Add(new TooltipLine(Mod, nameof(BlacklistedText), BlacklistedText.ToString()));
					}
				}
				else if (player.HasItem(Item.type))
				{
					tooltips.Add(new TooltipLine(Mod, nameof(NoUseText), NoUseText.ToString())
					{
						OverrideColor = Color.OrangeRed
					});
				}
			}
			else
			{
				tooltips.Add(new TooltipLine(Mod, nameof(DisabledText), DisabledText.ToString()));
			}
		}

		protected virtual bool UseDefaultRecipe => true;

		public sealed override void AddRecipes()
		{
			if (UseDefaultRecipe)
			{
				Recipe recipe = CreateRecipe();
				recipe.AddIngredient(ModContent.ItemType<KnittingSet>());
				recipe.AddTile(TileID.Loom);
				recipe.Register();
			}

			SafeAddRecipes();
		}

		protected virtual void SafeAddRecipes()
		{

		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			//item not registered
			if (!PetAccessory.TryGetAccessoryFromItem(Item.type, out PetAccessory petAccessory)) return false;

			PetPlayer pPlayer = player.GetModPlayer<PetPlayer>();
			//no valid slime pet found
			if (!pPlayer.HasValidSlimePet(out SlimePet slimePet)) return false;

			//if a right click, enable usage
			if (player.altFunctionUse == 2) return true;
			//if a left click and no alts, enable usage
			else if (!petAccessory.HasAlts && !slimePet.IsSlotTypeBlacklisted[(int)petAccessory.Slot]) return true;
			//else disable (if it has alts when left clicked, as that opens a UI instead)
			return false;
		}

		public override bool? UseItem(Player player)
		{
			PetPlayer pPlayer = player.GetModPlayer<PetPlayer>();

			if (pPlayer.slimePetIndex < 0)
			{
				return true;
			}

			if (player.whoAmI == Main.myPlayer && player.itemTime == 0 && PetAccessory.TryGetAccessoryFromItem(Item.type, out PetAccessory petAccessory))
			{
				bool shouldReset = false;
				if (player.altFunctionUse == 2) //right click use
				{
					if (pPlayer.ThreeTimesUseTime()) //true after three right clicks in 60 ticks
					{
						shouldReset = true;
					}
				}
				//else normal left click use

				if (pPlayer.HasValidSlimePet(out SlimePet slimePet))
				{
					//only client side
					if (Main.netMode != NetmodeID.Server)
					{
						if (shouldReset && player.altFunctionUse == 2)
						{
							if (pPlayer.HasPetAccessories)
							{
								Array.Copy(pPlayer.petAccessoriesBySlots, pPlayer.lastPetAccessoriesBySlots, pPlayer.petAccessoriesBySlots.Length);
								pPlayer.petAccessoriesBySlots = new (byte, byte)[pPlayer.petAccessoriesBySlots.Length];
							}
							else
							{
								Array.Copy(pPlayer.lastPetAccessoriesBySlots, pPlayer.petAccessoriesBySlots, pPlayer.lastPetAccessoriesBySlots.Length);
								pPlayer.lastPetAccessoriesBySlots = new (byte, byte)[pPlayer.lastPetAccessoriesBySlots.Length];
							}

							Projectile projectile = Main.projectile[pPlayer.slimePetIndex];
							//"dust" originating from the center, forming a circle and going outwards
							for (double angle = 0; angle < MathHelper.TwoPi; angle += Math.PI / 6)
							{
								Dust.NewDustPerfect(projectile.Center - new Vector2(0f, projectile.height / 4), 16, new Vector2((float)-Math.Cos(angle), (float)Math.Sin(angle)) * 1.2f, 0, new Color(255, 255, 255), 1.6f);
							}
						}
						else if (player.altFunctionUse != 2)
						{
							if (!slimePet.IsSlotTypeBlacklisted[(int)petAccessory.Slot])
							{
								pPlayer.ToggleAccessory(petAccessory);
							}
						}
					}
				}
			}
			return true;
		}
	}
}
