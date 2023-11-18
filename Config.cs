using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AssortedCrazyThings
{
	public class ContentConfig : ServerConfigBase
	{
		public static ContentConfig Instance => ModContent.GetInstance<ContentConfig>();

		internal ContentType FilterFlags { private set; get; }

		[Header("TogglesSpecial")]

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(152, 150, 113)]
		public bool Bosses { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		[BackgroundColor(123, 164, 255)]
		public bool CuteSlimes { get; set; }

		[Header("NPCs")]

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(227, 160, 147)]
		public bool HostileNPCs { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(227, 160, 147)]
		public bool FriendlyNPCs { get; set; }

		[Header("Items")]

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool DroppedPets { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool OtherPets { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool Weapons { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool Tools { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool PlaceablesFunctional { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool PlaceablesDecorative { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool Armor { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool VanityArmor { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool Accessories { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool VanityAccessories { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		public bool BossConsolation { get; set; }

		[Header("Other")]

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(89, 106, 159)]
		public bool AommSupport { get; set; }

		/// <summary>
		/// Affects the way Cute Slimes spawn and how the Jellied Ale works
		/// </summary>
		[DefaultValue(true)]
		public bool CuteSlimesPotionOnly { get; set; }

		[Header("HintClientConfig")]

		[JsonIgnore]
		[ShowDespiteJsonIgnore]
		public bool Hint => true;

		public override void OnChanged()
		{
			SetFlags();
		}

		private void SetFlags()
		{
			//Inverted, sets a flag if toggle is false
			FilterFlags = ContentType.Always;

			if (!Bosses)
			{
				FilterFlags |= ContentType.Bosses;
			}
			if (!CuteSlimes)
			{
				FilterFlags |= ContentType.CuteSlimes;
			}
			if (!HostileNPCs)
			{
				FilterFlags |= ContentType.HostileNPCs;
			}
			if (!FriendlyNPCs)
			{
				FilterFlags |= ContentType.FriendlyNPCs;
			}
			if (!DroppedPets)
			{
				FilterFlags |= ContentType.DroppedPets;
			}
			if (!OtherPets)
			{
				FilterFlags |= ContentType.OtherPets;
			}
			if (!Weapons)
			{
				FilterFlags |= ContentType.Weapons;
			}
			if (!Tools)
			{
				FilterFlags |= ContentType.Tools;
			}
			if (!PlaceablesFunctional)
			{
				FilterFlags |= ContentType.PlaceablesFunctional;
			}
			if (!PlaceablesDecorative)
			{
				FilterFlags |= ContentType.PlaceablesDecorative;
			}
			if (!Armor)
			{
				FilterFlags |= ContentType.Armor;
			}
			if (!VanityArmor)
			{
				FilterFlags |= ContentType.VanityArmor;
			}
			if (!Accessories)
			{
				FilterFlags |= ContentType.Accessories;
			}
			if (!VanityAccessories)
			{
				FilterFlags |= ContentType.VanityAccessories;
			}
			if (!BossConsolation)
			{
				FilterFlags |= ContentType.BossConsolation;
			}
			if (!AommSupport || !ModLoader.HasMod("AmuletOfManyMinions")) //Using instead of API.AommMod as that references the not-yet-registered config itself
			{
				//Automatically filter aomm content if the mod is disabled
				FilterFlags |= ContentType.AommSupport;
			}
		}
	}

	public class ClientConfig : ModConfig
	{
		public static ClientConfig Instance => ModContent.GetInstance<ClientConfig>();

		public override ConfigScope Mode => ConfigScope.ClientSide;

		//Old data
		[JsonExtensionData]
		private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

		[Header("GoblinUnderling")]
		[DefaultValue(true)]
		[BackgroundColor(125, 217, 124)]
		public bool GoblinUnderlingAutosummon { get; set; }

		[DefaultValue(true)]
		[BackgroundColor(125, 217, 124)]
		public bool GoblinUnderlingVisibleArmor { get; set; }

		public const int GoblinUnderlingChatterFreq_Min = 0;
		public const int GoblinUnderlingChatterFreq_Max = 500;
		[DefaultValue(100)]
		[BackgroundColor(125, 217, 124)]
		[Slider]
		[Increment(10)]
		[Range(GoblinUnderlingChatterFreq_Min, GoblinUnderlingChatterFreq_Max)]
		public int GoblinUnderlingChatterFreq { get; set; }

		internal bool GoblinUnderlingDialogueDisabled => GoblinUnderlingChatterFreq == 0;

		[Header("HintServerConfig")]
		[JsonIgnore]
		[ShowDespiteJsonIgnore]
		public bool Hint => true;

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			//port "SatchelofGoodiesAutosummon" -> GoblinUnderlingAutosummon
			//and "SatchelofGoodiesVisibleArmor" -> GoblinUnderlingVisibleArmor
			//and "SatchelofGoodiesChatterFreq" -> GoblinUnderlingChatterFreq
			JToken token;
			if (_additionalData.TryGetValue("SatchelofGoodiesAutosummon", out token))
			{
				GoblinUnderlingAutosummon = token.ToObject<bool>();
			}
			if (_additionalData.TryGetValue("SatchelofGoodiesVisibleArmor", out token))
			{
				GoblinUnderlingVisibleArmor = token.ToObject<bool>();
			}
			if (_additionalData.TryGetValue("SatchelofGoodiesChatterFreq", out token))
			{
				GoblinUnderlingChatterFreq = token.ToObject<int>();
			}

			_additionalData.Clear(); // Clear this or it'll crash.

			GoblinUnderlingChatterFreq = Utils.Clamp(GoblinUnderlingChatterFreq, GoblinUnderlingChatterFreq_Min, GoblinUnderlingChatterFreq_Max);
		}
	}

	public abstract class ServerConfigBase : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		public static bool IsPlayerLocalServerOwner(int whoAmI)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
			}

			return NetMessage.DoesPlayerSlotCountAsAHost(whoAmI);
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer) return true;
			else if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = AssLocalization.AcceptClientChangesText.ToString();
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
	}
}
