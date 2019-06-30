using AssortedCrazyThings.Base;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    public static class ModConf
    {
        public const int configVersion = 2;

        private const string cuteSlimesPotionOnlyField = "CuteSlimesPotionOnly";
        internal static bool cuteSlimesPotionOnly = true;
        /// <summary>
        /// Affects the way Cute Slimes spawn and how the Jellied Ale works
        /// </summary>
        public static bool CuteSlimesPotionOnly
        {
            get
            {
                return cuteSlimesPotionOnly;
            }
        }

        private const string walkingTombstonesField = "WalkingTombstones";
        internal static bool walkingTombstones = true;
        /// <summary>
        /// Enable/Disable Walking Tombstones spawning
        /// </summary>
        public static bool WalkingTombstones
        {
            get
            {
                return walkingTombstones;
            }
        }

        static readonly string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", AssUtils.Instance.Name + ".json");

        /// <summary>
        /// The config that holds the data
        /// </summary>
        static readonly Preferences ModConfig = new Preferences(ConfigPath);

        internal static void Load()
        {
            bool success = ReadConfig();
            if (!success)
            {
                AssUtils.Instance.Logger.Info("Couldn't load config file, creating new file.");
                CreateConfig();
            }
        }

        // true if loaded successfully
        internal static bool ReadConfig()
        {
            if (ModConfig.Load())
            {
                int readVersion = 0;
                ModConfig.Get("version", ref readVersion);
                if (readVersion != configVersion)
                {
                    bool canUpdate = false;
                    if (readVersion == 1)
                    {
                        AssUtils.Instance.Logger.Info("Updated Config Version");
                        canUpdate = true;
                        
                        ModConfig.Get(walkingTombstonesField, ref walkingTombstones);
                        CreateConfig();
                    }
                    if (!canUpdate) return false;
                }

                ModConfig.Get(cuteSlimesPotionOnlyField, ref cuteSlimesPotionOnly);
                ModConfig.Get(walkingTombstonesField, ref walkingTombstones);
                return true;
            }
            return false;
        }

        // Create a new config file for the player to edit. 
        internal static void CreateConfig()
        {
            ModConfig.Clear();
            ModConfig.Put("version", configVersion);

            ModConfig.Put(cuteSlimesPotionOnlyField, cuteSlimesPotionOnly);
            ModConfig.Put(walkingTombstonesField, walkingTombstones);

            ModConfig.Put("readme", "Check the mod homepage for a link to the wiki, and navigate to the 'Config' page for a readme");
            ModConfig.Save();
        }
    }
}
