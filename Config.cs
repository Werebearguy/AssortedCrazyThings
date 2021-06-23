using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AssortedCrazyThings
{
	[Label("Content Config")]
	public class ContentConfig : ServerConfigBase
	{
		public static ContentConfig Instance => ModContent.GetInstance<ContentConfig>();

		internal ContentType FilterFlags { private set; get; }

		[Header("The following toggles let you control content added by this mod" +
			"\n\nNPCs")]

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(152, 150, 113)]
		[Tooltip("Boss content")]
		[Label("Bosses")]
		public bool Bosses { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		[BackgroundColor(123, 164, 255)]
		[Tooltip("Cute slime content")]
		[Label("Cute Slimes")]
		public bool CuteSlimes { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(227, 160, 147)]
		[Tooltip("Harmful entities - This includes pets obtained by catching, and does not disable bosses!")]
		[Label("Hostile")]
		public bool HostileNPCs { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(227, 160, 147)]
		[Tooltip("Harmless entities - This does not disable cute slimes!")]
		[Label("Friendly")]
		public bool FriendlyNPCs { get; set; }

		[Header("Items")]

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Pets that drop from hostile entities")]
		[Label("Dropped Pets")]
		public bool DroppedPets { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Pets that are crafted or sold - This does not include cute slimes or NPCs caught as pets!")]
		[Label("Other Pets")]
		public bool OtherPets { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Items that deal damage (Pacifist path)")]
		[Label("Weapons")]
		public bool Weapons { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Tools and useful items")]
		[Label("Tools")]
		public bool Tools { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Placeable decorative tiles and objects")]
		[Label("Placeables")]
		public bool Placeables { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Armor with stats")]
		[Label("Armor")]
		public bool Armor { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Vanity armor")]
		[Label("Vanity")]
		public bool VanityArmor { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Accesssories with effects")]
		[Label("Accessories")]
		public bool Accessories { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Accessories without effects")]
		[Label("Vanity Accessories")]
		public bool VanityAccessories { get; set; }

		[ReloadRequired]
		[DefaultValue(true)]
		[BackgroundColor(194, 147, 227)]
		[Tooltip("Accessories that drop for failing to defeat a vanilla boss within 5 attempts")]
		[Label("Boss Consolation")]
		public bool BossConsolation { get; set; }

		[Header("Other")]

		/// <summary>
		/// Affects the way Cute Slimes spawn and how the Jellied Ale works
		/// </summary>
		[DefaultValue(true)]
		[Label("Cute Slimes Potion Only")]
		[Tooltip("Affects the way Cute Slimes spawn and how the Jellied Ale works")]
		public bool CuteSlimesPotionOnly { get; set; }

		public override void OnChanged()
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
			if (!Placeables)
			{
				FilterFlags |= ContentType.Placeables;
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

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				RemoteClient client = Netplay.Clients[i];
				if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
				{
					return true;
				}
			}
			return false;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer) return true;
			else if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = "You are not the server owner so you can not change this config";
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
	}
}
