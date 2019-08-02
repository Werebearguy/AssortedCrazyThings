using AssortedCrazyThings.Base;
using Newtonsoft.Json;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AssortedCrazyThings
{
    /// <summary>
    /// The class name is what is shown in the filename as AssortedCrazyThings_ACTConfig
    /// </summary>
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        /// <summary>
        /// Affects the way Cute Slimes spawn and how the Jellied Ale works
        /// </summary>
        [DefaultValue(true)]
        [Label("Cute Slimes Potion Only")]
        [Tooltip("Affects the way Cute Slimes spawn and how the Jellied Ale works")]
        public bool CuteSlimesPotionOnly;


        /// <summary>
        /// Enable/Disable Walking Tombstones spawning
        /// </summary>
        [DefaultValue(true)]
        [Label("Walking Tombstones")]
        [Tooltip("Enable/Disable Walking Tombstone spawning")]
        public bool WalkingTombstones;

        [Header("Resulting Behavior")]

        [JsonIgnore]
        [Label("Cute Slimes Potion Only Status")]
        public string CuteSlimesPotionOnlyStatus => GetStatus(CuteSlimesPotionOnly);

        [JsonIgnore]
        [Label("Walking Tombstones Status")]
        public string WalkingTombstonesStatus => GetStatus(WalkingTombstones);

        public override void OnLoaded()
        {
            AssUtils.AssConfig = this;
        }

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            message = "Only the host of this world can change the config! Do so in singleplayer.";
            return false;
        }

        private string GetStatus(bool b)
        {
            return b ? "Enabled" : "Disabled";
        }
    }
}
