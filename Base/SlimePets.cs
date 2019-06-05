using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base
{
    public static class SlimePets
    {
        /// <summary>
        /// Contains every slime pet
        /// </summary>
        internal static List<SlimePet> slimePetList;

        /// <summary>
        /// slimePets.IndexOf(type) returns the indexed type
        /// </summary>
        public static List<int> slimePets;

        /// <summary>
        /// For the Cute Slime Statue, non-biome ones only
        /// </summary>
        public static List<int> slimePetNPCs;

        /// <summary>
        /// For the Jellied Ale bufftip
        /// </summary>
        public static List<List<string>> slimePetNPCsEnumToNames;


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
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && player.ZoneSnow && player.ZoneOverworldHeight/* && !AssUtils.EvilBiome(player)*/;
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
                    return Main.hardMode && !player.ZoneOverworldHeight && player.ZoneHoly && !(player.ZoneCorrupt || player.ZoneCrimson);
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
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) ? SpawnCondition.OverworldDaySlime.Chance * 0.01f : 0f;
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
                    return Main.hardMode && player.ZoneHoly && !(player.ZoneCorrupt || player.ZoneCrimson) && !player.ZoneOverworldHeight ? 0.015f : 0f;
                case SpawnConditionType.Dungeon:
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) ? SpawnCondition.DungeonNormal.Chance * 0.025f : 0f;
                case SpawnConditionType.Xmas:
                    return player.townNPCs < 3f && !AssUtils.EvilBiome(player) && Main.xMas ? SpawnCondition.OverworldDaySlime.Chance * 0.06f : 0f;
                default:
                    return 1f;
            }
        }

        /// <summary>
        /// Used in ModNPC.SpawnChance. Short way of specifying a spawn chance
        /// </summary>
        public static float CuteSlimeSpawnChance(NPCSpawnInfo spawnInfo, SpawnConditionType type, float customFactor = 1f)
        {
            float spawnChance = GetSpawnChance(spawnInfo.player, type) * customFactor;
            if (ModConf.CuteSlimesPotionOnly)
            {
                if (spawnInfo.player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable)
                {
                    //if flag active and potion, spawn normally
                    return spawnChance;
                }
                //if flag active and no potion, don't spawn
                return 0f;
            }
            else
            {
                if (spawnInfo.player.GetModPlayer<AssPlayer>().cuteSlimeSpawnEnable)
                {
                    //if no flag and potion active, spawn with higher chance
                    return spawnChance * 3;
                }
                //if no flag and no potion, spawn normally
                return spawnChance;
            }
        }

        /// <summary>
        /// Called in Mod.Load
        /// </summary>
        public static void Load()
        {
            slimePetList = new List<SlimePet>();
            slimePets = new List<int>(); //slimePets.IndexOf(type) returns the indexed type
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
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Hallow] = new List<string>() {"Illuminant" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Dungeon] = new List<string>() { "Dungeon" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Xmas] = new List<string>() { "Xmas" };

            slimePetNPCs = new List<int>
            {
                AssUtils.Instance.NPCType<CuteSlimeBlack>(),
                AssUtils.Instance.NPCType<CuteSlimeBlue>(),
                AssUtils.Instance.NPCType<CuteSlimeGreen>(),
                AssUtils.Instance.NPCType<CuteSlimePink>(),
                AssUtils.Instance.NPCType<CuteSlimePurple>(),
                AssUtils.Instance.NPCType<CuteSlimeRainbow>(),
                AssUtils.Instance.NPCType<CuteSlimeRed>(),
                AssUtils.Instance.NPCType<CuteSlimeYellow>()
            };

            //start list
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeBlackNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeBlueNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeCorruptNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeCrimsonNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeDungeonNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeGreenNewProj"
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeIceNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeIlluminantNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeLavaNewProj",
                hasNoHair: true,
                postAdditionSlot: (byte)SlotType.Hat
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeJungleNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimePrincessNewProj",
                hasNoHair: true,
                postAdditionSlot: (byte)SlotType.Hat
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimePurpleNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimePinkNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeRainbowNewProj"
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeRedNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeSandNewProj",
                hasNoHair: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeToxicNewProj"
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeXmasNewProj",
                hasNoHair: true,
                postAdditionSlot: (byte)SlotType.Body,
                carried: true,
                accessory: true
            ));
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeYellowNewProj",
                hasNoHair: true
            ));

            /*
            slimePetList.Add(SlimePet.NewSlimePet
            (
                name: "CuteSlimeColorNewProj",
                hasNoHair: false,
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
             * hasNoHair: if true, makes it so it uses the NoHair texture for a Hat accessory if specified
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

        /// <summary>
        /// Called in Mod.Unload
        /// </summary>
        public static void Unload()
        {
            slimePets = null;
            slimePetList = null;
            slimePetNPCsEnumToNames = null;
        }

        /// <summary>
        /// Called after Load. Indexes the slime pets
        /// </summary>
        public static void CreateMap()
        {
            slimePets = new List<int>(slimePetList.Count);
            for (int i = 0; i < slimePetList.Count; i++)
            {
                slimePets.Add(slimePetList[i].Type);
            }
        }

        /// <summary>
        /// Used to access the slime pet from just the projectile type
        /// </summary>
        public static SlimePet GetPet(int type)
        {
            return slimePetList[slimePets.IndexOf(type)];
        }
    }

    /// <summary>
    /// Contains data about a slime pet
    /// </summary>
    public class SlimePet
    {
        public string Name { private set; get; }
        public int Type { private set; get; }
        public bool HasNoHair { private set; get; }
        public byte PreAdditionSlot { private set; get; }
        public byte PostAdditionSlot { private set; get; }
        public bool[] IsSlotTypeBlacklisted { private set; get; }

        public SlimePet(string name, bool hasNoHair = false, byte preAdditionSlot = 0, byte postAdditionSlot = 0, List<bool> isSlotTypeBlacklisted = null)
        {
            Name = name;
            Type = AssUtils.Instance.ProjectileType(name);
            if (Type == 0) throw new Exception("Pet projectile called '" + name + "' doesn't exist, are you sure you spelled it correctly?");
            HasNoHair = hasNoHair;
            PreAdditionSlot = preAdditionSlot;
            PostAdditionSlot = postAdditionSlot;

            if (isSlotTypeBlacklisted != null)
            {
                if (isSlotTypeBlacklisted.Count != Enum.GetValues(typeof(SlotType)).Length - 1)
                    throw new Exception("isSlotTypeBlacklisted is longer than the amount of accessory slot types");
                else
                {
                    IsSlotTypeBlacklisted = new bool[5];
                    IsSlotTypeBlacklisted[0] = false; //maybe for later
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
        public static SlimePet NewSlimePet(string name, bool hasNoHair = false, byte preAdditionSlot = 0, byte postAdditionSlot = 0,
            bool body = false, bool hat = false, bool carried = false, bool accessory = false)
        {
            List<bool> isSlotTypeBlacklisted = new List<bool>(){ body, hat, carried, accessory };

            return new SlimePet(name, hasNoHair, preAdditionSlot, postAdditionSlot, isSlotTypeBlacklisted);
        }

        public override string ToString()
        {
            return "Name: " + Name
                +"; Type: " + Type
                + "; HasNoHair: " + (HasNoHair? "y":"n")
                + "; PreAdditionSlot: " + ((SlotType)PreAdditionSlot).ToString()
                + "; PostAdditionSlot: " + ((SlotType)PostAdditionSlot).ToString();
        }
    }
}
