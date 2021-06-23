using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.NPCs.CuteSlimes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using AssortedCrazyThings.Base.SlimeHugs;

namespace AssortedCrazyThings.Base
{
    [Content(ContentType.CuteSlimes)]
    [Autoload(false)]
    public class SlimePets : ModSystem
    {
        /// <summary>
        /// Contains every slime pet
        /// </summary>
        internal static List<SlimePet> slimePetList;

        /// <summary>
        /// Look-up table where the key is proj ID and it returns the corresponding SlimePet
        /// </summary>
        private static Dictionary<int, SlimePet> slimePetsByProj;

        /// <summary>
        /// For the Cute Slime Statue, non-biome ones only
        /// </summary>
        public static List<int> slimePetRegularNPCs;

        /// <summary>
        /// For the Jellied Ale bufftip
        /// </summary>
        public static List<List<string>> slimePetNPCsEnumToNames;

        /// <summary>
        /// To increase cute slime spawns when mods are present and its used with Jellied Ale
        /// </summary>
        public static float spawnIncreaseBasedOnOtherModNPCs = 0f;

        /// <summary>
        /// Used in CuteSlimeSpawnEnableBuff.ModifyTooltips
        /// Since SpawnCondition is unresponsive, implement the conditions manually
        /// </summary>
        public static bool CanSpawn(Player player, SpawnConditionType type)
        {
            switch (type)
            {
                case SpawnConditionType.Overworld:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Desert:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && player.ZoneDesert && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Tundra:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && player.ZoneSnow/* && !AssUtils.EvilBiome(player)*/;
                case SpawnConditionType.Jungle:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && player.ZoneJungle/* && !AssUtils.EvilBiome(player)*/;
                case SpawnConditionType.Underground:
                    return Main.hardMode && player.ZoneDirtLayerHeight && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Hell:
                    return player.ZoneUnderworldHeight && player.townNPCs < 3f/* && !AssUtils.EvilBiome(player)*/;
                case SpawnConditionType.Corruption:
                    return player.ZoneOverworldHeight && Main.hardMode && (!Main.bloodMoon ? player.ZoneCorrupt : player.ZoneCrimson);
                case SpawnConditionType.Crimson:
                    return player.ZoneOverworldHeight && Main.hardMode && (!Main.bloodMoon ? player.ZoneCrimson : player.ZoneCorrupt);
                case SpawnConditionType.Hallow:
                    return Main.hardMode && !player.ZoneOverworldHeight && player.ZoneHallow && !(player.ZoneCorrupt || player.ZoneCrimson);
                case SpawnConditionType.Dungeon:
                    return player.ZoneDungeon && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Xmas:
                    return Main.xMas && Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Used in CuteSlimeSpawnChance, returns the spawn chance based on the SpawnConditionType
        /// </summary>
        public static float GetSpawnChance(Player player, SpawnConditionType type = SpawnConditionType.None)
        {
            switch (type)
            {
                case SpawnConditionType.Overworld:
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) ? SpawnCondition.OverworldDaySlime.Chance * 0.0075f : 0f;
                case SpawnConditionType.Desert:
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) ? SpawnCondition.OverworldDayDesert.Chance * 0.12f : 0f;
                case SpawnConditionType.Tundra:
                    return player.townNPCs < 3f && player.ZoneSnow ? SpawnCondition.OverworldDaySlime.Chance * 0.06f : 0f;
                case SpawnConditionType.Jungle:
                    return player.townNPCs < 3f && Main.dayTime ? SpawnCondition.SurfaceJungle.Chance * 0.06f : 0f;
                case SpawnConditionType.Underground:
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) && Main.hardMode && player.ZoneDirtLayerHeight ? 0.015f : 0f;
                case SpawnConditionType.Hell:
                    return player.townNPCs < 3f ? SpawnCondition.Underworld.Chance * 0.015f : 0f;
                case SpawnConditionType.Corruption:
                    return Main.hardMode && player.ZoneOverworldHeight ? (!Main.bloodMoon ? SpawnCondition.Corruption.Chance * 0.025f : SpawnCondition.Crimson.Chance * 0.025f) : 0f;
                case SpawnConditionType.Crimson:
                    return Main.hardMode && player.ZoneOverworldHeight ? (!Main.bloodMoon ? SpawnCondition.Crimson.Chance * 0.025f : SpawnCondition.Corruption.Chance * 0.025f) : 0f;
                case SpawnConditionType.Hallow:
                    return Main.hardMode && player.ZoneHallow && !(player.ZoneCorrupt || player.ZoneCrimson) && !player.ZoneOverworldHeight ? 0.015f : 0f;
                case SpawnConditionType.Dungeon:
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) ? SpawnCondition.DungeonNormal.Chance * 0.025f : 0f;
                case SpawnConditionType.Xmas:
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) && Main.xMas ? SpawnCondition.OverworldDaySlime.Chance * 0.05f : 0f;
                default:
                    return 1f;
            }
        }

        /// <summary>
        /// Used in ModNPC.SpawnChance. Short way of specifying a spawn chance
        /// </summary>
        public static float CuteSlimeSpawnChance(NPCSpawnInfo spawnInfo, SpawnConditionType type, float customFactor = 1f)
        {
            //AssUtils.Print("spawn chance at " + (Main.netMode == NetmodeID.Server ? "Server" : "Client"));
            float spawnChance = GetSpawnChance(spawnInfo.player, type) * customFactor;
            if (AssUtils.AnyNPCs(x => x.ModNPC is CuteSlimeBaseNPC)) spawnChance *= 0.5f;
            //AssUtils.Print(spawnChance);
            if (Config.Instance.CuteSlimesPotionOnly)
            {
                if (spawnInfo.player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable)
                {
                    //if flag active and potion, spawn normally
                    //AssUtils.Print("potiononly and has potion");
                    return spawnChance * 1.2f * (1f + spawnIncreaseBasedOnOtherModNPCs);
                }
                //AssUtils.Print("potiononly and has no potion");
                //if flag active and no potion, don't spawn
                return 0f;
            }
            else
            {
                if (spawnInfo.player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable)
                {
                    //if no flag and potion active, spawn with higher chance
                    //AssUtils.Print("no potiononly and has potion");
                    return spawnChance * 3 * 1.2f * (1f + spawnIncreaseBasedOnOtherModNPCs);
                }
                //AssUtils.Print("no potiononly and has no potion");
                //if no flag and no potion, spawn normally
                return spawnChance;
            }
        }

        public override void OnModLoad()
        {
            LoadPets();

            PetAccessory.Load();
        }

        private static void LoadPets()
        {
            slimePetList = new List<SlimePet>();
            slimePetsByProj = new Dictionary<int, SlimePet>();
            //in all these lists, insert stuff in alphabetic order please

            Array enumArray = Enum.GetValues(typeof(SpawnConditionType));

            AssUtils.FillWithDefault(ref slimePetNPCsEnumToNames, null, enumArray.Length);
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Overworld] = new List<string>() { "Black", "Blue", "Green", "Pink", "Purple", "Rainbow", "Red", "Yellow" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Desert] = new List<string>() { "Sand" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Tundra] = new List<string>() { "Ice" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Jungle] = new List<string>() { "Jungle" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Underground] = new List<string>() { "Toxic" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Hell] = new List<string>() { "Lava" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Corruption] = new List<string>() { "Corrupt" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Crimson] = new List<string>() { "Crimson" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Hallow] = new List<string>() { "Illuminant" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Dungeon] = new List<string>() { "Dungeon" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Xmas] = new List<string>() { "Xmas" };

            slimePetRegularNPCs = new List<int>
            {
                ModContent.NPCType<CuteSlimeBlack>(),
                ModContent.NPCType<CuteSlimeBlue>(),
                ModContent.NPCType<CuteSlimeGreen>(),
                ModContent.NPCType<CuteSlimePink>(),
                ModContent.NPCType<CuteSlimePurple>(),
                ModContent.NPCType<CuteSlimeRainbow>(),
                ModContent.NPCType<CuteSlimeRed>(),
                ModContent.NPCType<CuteSlimeYellow>()
            };

            //start list
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeBlackProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeBlueProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeCorruptProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeCrimsonProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeDungeonProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeGreenProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeIceProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeIlluminantProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeLavaProj>(),
                postAdditionSlot: (byte)SlotType.Hat
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeJungleProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimePrincessProj>(),
                postAdditionSlot: (byte)SlotType.Hat
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimePurpleProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimePinkProj>()
            ));

            if (AConfigurationConfig.Instance.DroppedPets)
            {
                Add(SlimePet.NewSlimePet
                (
                    type: ModContent.ProjectileType<CuteSlimeQueenProj>(),
                    postAdditionSlot: (byte)SlotType.Hat
                ));
            }

            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeRainbowProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeRedProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeSandProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeToxicProj>()
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeXmasProj>(),
                postAdditionSlot: (byte)SlotType.Body,
                carried: true,
                accessory: true
            ));
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeYellowProj>()
            ));

            /*
            Add(SlimePet.NewSlimePet
            (
                type: ModContent.ProjectileType<CuteSlimeColorProj>(),
                preAdditionSlot: (byte)SlotType.None,
                postAdditionSlot: (byte)SlotType.None,
                body: false,
                hat: false,
                carried: false,
                accessory: false
                //last entry here needs to have no comma ","
            ));
            
             * TEMPLATE EXPLANATION:
             * name: name of the projectile class
             * preAdditionSlot: SlotType that causes the Addition texture to not be drawn when equipped (used for drawing behind the pet)
             * postAdditionSlot: SlotType that causes the Addition texture to not be drawn when equipped (used for drawing infront of the pet) (Example: Xmas)
             * //Note: don't bother including those two, ask me instead if there is an "Addition" that needs to be drawn
             * body, hat etc: if true, makes it so said accessory type can't be equipped and won't be drawn on the pet
             *
             * The things listed there are defaults (besides the name and color), so you can omit those if the pet is basic
             */

            //end list, don't write anything new after this line

            CreateMap();
        }

        public static void Add(SlimePet aSlimePet)
        {
            for (int i = 0; i < slimePetList.Count; i++)
            {
                SlimePet slimePet = slimePetList[i];
                if (slimePet.Name == aSlimePet.Name)
                    throw new Exception("Added Pet '" + aSlimePet.Name + "' already exists");
            }

            slimePetList.Add(aSlimePet);
        }

        public override void PostSetupContent()
        {
            int actcount = 0;
            foreach (var item in AssUtils.Instance.GetContent<ModNPC>())
            {
                actcount++;
            }

            int vanillaCount = NPCID.Count;
            int diff = NPCLoader.NPCCount - vanillaCount - actcount;

            spawnIncreaseBasedOnOtherModNPCs = diff / (float)vanillaCount;
        }

        public override void Unload()
        {
            UnloadPets();

            PetAccessory.Unload();

            SlimeHugLoader.Unload();
        }

        private static void UnloadPets()
        {
            slimePetList = null;
            slimePetsByProj = null;
            slimePetNPCsEnumToNames = null;
            spawnIncreaseBasedOnOtherModNPCs = 0f;
        }

        /// <summary>
        /// Called after Load. Indexes the slime pets
        /// </summary>
        public static void CreateMap()
        {
            for (int i = 0; i < slimePetList.Count; i++)
            {
                SlimePet slimePet = slimePetList[i];
                slimePetsByProj[slimePet.Type] = slimePet;
            }
        }

        /// <summary>
        /// Used to access the slime pet from just the projectile type
        /// </summary>
        public static bool TryGetPetFromProj(int type, out SlimePet slimePet)
        {
            return slimePetsByProj.TryGetValue(type, out slimePet);
        }
    }

    public enum SpawnConditionType : byte
    {
        None,
        Overworld,
        Desert,
        Tundra,
        Jungle,
        Underground,
        Hell,
        Corruption,
        Crimson,
        Hallow,
        Dungeon,
        Xmas
    }

    /// <summary>
    /// Contains data about a slime pet
    /// </summary>
    public class SlimePet
    {
        public string Name { private set; get; }
        public int Type { private set; get; }
        public byte PreAdditionSlot { private set; get; }
        public byte PostAdditionSlot { private set; get; }
        public bool[] IsSlotTypeBlacklisted { private set; get; }

        public SlimePet(string name, byte preAdditionSlot = 0, byte postAdditionSlot = 0, List<bool> isSlotTypeBlacklisted = null)
        {
            Name = name;
            Type = AssUtils.Instance.Find<ModProjectile>(name).Type;
            if (Type == 0) throw new Exception("Pet projectile called '" + name + "' doesn't exist, are you sure you spelled it correctly?");
            PreAdditionSlot = preAdditionSlot;
            PostAdditionSlot = postAdditionSlot;

            if (isSlotTypeBlacklisted != null)
            {
                if (isSlotTypeBlacklisted.Count != Enum.GetValues(typeof(SlotType)).Length - 1)
                    throw new Exception("'isSlotTypeBlacklisted' is longer than the amount of accessory slot types");
                else
                {
                    IsSlotTypeBlacklisted = new bool[5];
                    IsSlotTypeBlacklisted[(int)SlotType.None] = false; //maybe for later
                    IsSlotTypeBlacklisted[1] = isSlotTypeBlacklisted[0];
                    IsSlotTypeBlacklisted[2] = isSlotTypeBlacklisted[1];
                    IsSlotTypeBlacklisted[3] = isSlotTypeBlacklisted[2];
                    IsSlotTypeBlacklisted[4] = isSlotTypeBlacklisted[3];
                }
            }
            else
            {
                IsSlotTypeBlacklisted = new bool[5]; //all false
            }
        }

        /// <summary>
        /// "Sort of" constructor, compresses each blacklist field into a list
        /// </summary>
        public static SlimePet NewSlimePet(string name, byte preAdditionSlot = 0, byte postAdditionSlot = 0,
            bool body = false, bool hat = false, bool carried = false, bool accessory = false)
        {
            List<bool> isSlotTypeBlacklisted = new List<bool>() { body, hat, carried, accessory };

            return new SlimePet(name, preAdditionSlot, postAdditionSlot, isSlotTypeBlacklisted);
        }

        public static SlimePet NewSlimePet(int type, byte preAdditionSlot = 0, byte postAdditionSlot = 0,
            bool body = false, bool hat = false, bool carried = false, bool accessory = false)
        {
            return NewSlimePet(ModContent.GetModProjectile(type).Name, preAdditionSlot, postAdditionSlot, body, hat, carried, accessory);
        }

        public override string ToString()
        {
            return "Name: " + Name
                + "; Type: " + Type
                + "; PreAdditionSlot: " + ((SlotType)PreAdditionSlot).ToString()
                + "; PostAdditionSlot: " + ((SlotType)PostAdditionSlot).ToString();
        }
    }
}
