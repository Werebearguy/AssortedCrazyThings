using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Pets;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    public static class SlimePets
    {
        internal static List<SlimePet> slimePetList = new List<SlimePet>();

        public static List<int> slimePets = new List<int>(); //slimePets.IndexOf(type) returns the indexed type
        public static List<int> slimePetLegacy = new List<int>();
        public static List<List<string>> slimePetNPCsEnumToNames = new List<List<string>>();

        public enum SpawnConditionType : byte
        {
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

        public static bool CanSpawn(Player player, SpawnConditionType type)
        {
            switch (type)
            {
                case SpawnConditionType.Overworld:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Desert:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && player.ZoneDesert && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Tundra:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && player.ZoneSnow && player.ZoneOverworldHeight && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Jungle:
                    return Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && player.ZoneJungle && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Underground:
                    return Main.hardMode && player.ZoneDirtLayerHeight && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Hell:
                    return player.ZoneUnderworldHeight && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Corruption:
                    return Main.dayTime && player.ZoneOverworldHeight && Main.hardMode && (!Main.bloodMoon ? player.ZoneCorrupt : player.ZoneCrimson);
                case SpawnConditionType.Crimson:
                    return Main.dayTime && player.ZoneOverworldHeight && Main.hardMode && (!Main.bloodMoon ? player.ZoneCrimson : player.ZoneCorrupt);
                case SpawnConditionType.Hallow:
                    return Main.dayTime && player.ZoneOverworldHeight && Main.hardMode && player.ZoneHoly && !(player.ZoneCorrupt || player.ZoneCrimson);
                case SpawnConditionType.Dungeon:
                    return player.ZoneDungeon && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                case SpawnConditionType.Xmas:
                    return Main.xMas && Main.dayTime && player.ZoneOverworldHeight && player.townNPCs < 3f && !AssUtils.EvilBiome(player);
                default:
                    return false;
            }
        }

        public static float GetSpawnChance(Player player, SpawnConditionType type)
        {
            switch (type)
            {
                case SpawnConditionType.Overworld:
                    return SpawnCondition.OverworldDaySlime.Chance * 0.0125f;
                case SpawnConditionType.Desert:
                    return SpawnCondition.OverworldDayDesert.Chance * 0.05f;
                case SpawnConditionType.Tundra:
                    return player.ZoneSnow ? SpawnCondition.OverworldDaySlime.Chance * 0.05f : 0f;
                case SpawnConditionType.Jungle:
                    return SpawnCondition.SurfaceJungle.Chance * 0.05f;
                case SpawnConditionType.Underground:
                    return Main.hardMode && player.ZoneDirtLayerHeight ? 0.0125f : 0f;
                case SpawnConditionType.Hell:
                    return SpawnCondition.Underworld.Chance * 0.0125f;
                case SpawnConditionType.Corruption:
                    return Main.hardMode ? (!Main.bloodMoon ? SpawnCondition.Corruption.Chance * 0.05f : SpawnCondition.Crimson.Chance * 0.05f) : 0f;
                case SpawnConditionType.Crimson:
                    return Main.hardMode ? (!Main.bloodMoon ? SpawnCondition.Crimson.Chance * 0.05f : SpawnCondition.Corruption.Chance * 0.05f) : 0f;
                case SpawnConditionType.Hallow:
                    return SpawnCondition.OverworldHallow.Chance * 0.05f;
                case SpawnConditionType.Dungeon:
                    return SpawnCondition.DungeonNormal.Chance * 0.02f;
                case SpawnConditionType.Xmas:
                    return Main.xMas ? SpawnCondition.OverworldDaySlime.Chance * 0.025f : 0f;
                default:
                    return 0f;
            }
        }

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

        public static void Load()
        {
            //in all these lists, insert stuff in alphabetic order please

            //legacy, no need to adjust
            slimePetLegacy.AddRange(new List<int>
            {
                AssUtils.Instance.ProjectileType<CuteSlimeBlackProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeBlueProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeGreenProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimePinkProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimePurpleProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeRainbowProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeRedProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeXmasProj>(),
                AssUtils.Instance.ProjectileType<CuteSlimeYellowProj>(),
            });

            Array enumArray = Enum.GetValues(typeof(SpawnConditionType));
            slimePetNPCsEnumToNames = new List<List<string>>(enumArray.Length);
            for (int i = 0; i < enumArray.Length; i++)
            {
                slimePetNPCsEnumToNames.Add(null);
            }
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Overworld] = new List<string>() { "Black", "Blue", "Green", "Pink", "Purple", "Rainbow", "Red", "Yellow" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Desert] = new List<string>() { "Sand" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Tundra] = new List<string>() { "Ice" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Jungle] = new List<string>() { "Jungle" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Underground] = new List<string>() { "Toxic" };
            //slimePetNPCsEnumToNames[(int)SpawnConditionType.Hell] = new List<string>() { "Lava" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Corruption] = new List<string>() { "Corrupt" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Crimson] = new List<string>() { "Crimson" };
            //slimePetNPCsEnumToNames[(int)SpawnConditionType.Hallow] = new List<string>() {"Illuminant" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Dungeon] = new List<string>() { "Dungeon" };
            slimePetNPCsEnumToNames[(int)SpawnConditionType.Xmas] = new List<string>() { "Xmas" };

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

        public static void Unload()
        {
            slimePets.Clear();
            slimePetList.Clear();
            slimePetLegacy.Clear();
            slimePetNPCsEnumToNames.Clear();
        }

        public static void CreateMap()
        {
            slimePets = new List<int>(slimePetList.Count);
            for (int i = 0; i < slimePetList.Count; i++)
            {
                slimePets.Add(slimePetList[i].Type);
            }
        }

        public static SlimePet GetPet(int type)
        {
            return slimePetList[slimePets.IndexOf(type)];
        }
    }

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
