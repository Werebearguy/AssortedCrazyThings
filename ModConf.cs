using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    public static class ModConf
    {
        public const int configVersion = 1;
        private readonly static string modName = "AssortedCrazyThings";

        private const string cuteSlimesField = "CuteSlimes";
        internal static bool cuteSlimes = true;
        public static bool CuteSlimes
        {
            get
            {
                return cuteSlimes;
            }
        }
        private const string walkingTombstonesField = "WalkingTombstones";
        internal static bool walkingTombstones = true;
        public static bool WalkingTombstones
        {
            get
            {
                return walkingTombstones;
            }
        }

        static readonly string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", modName + ".json");

        static Preferences ModConfig = new Preferences(ConfigPath);

        internal static void Load()
        {
            bool success = ReadConfig();
            if (!success)
            {
                ErrorLogger.Log("AssortedCrazyThings: Couldn't load config file, creating new file.");
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
                    //if (readVersion == 1)
                    //{
                    //    ErrorLogger.Log("AssortedCrazyThings: updated Version");
                    //    canUpdate = true;
                    //    ModConfig.Put("version", 2);
                    //    ModConfig.Save();
                    //}
                    if (!canUpdate) return false;
                }

                ModConfig.Get(cuteSlimesField, ref cuteSlimes);
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

            ModConfig.Put(cuteSlimesField, cuteSlimes);
            ModConfig.Put(walkingTombstonesField, walkingTombstones);

            ModConfig.Put("readme", "Check the mod homepage for a link to the wiki, and navigate to the 'Config' page for a readme");
            ModConfig.Save();
        }
    }
}
