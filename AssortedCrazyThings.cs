using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AssortedCrazyThings
{
    class AssortedCrazyThings : Mod
    {
        public AssortedCrazyThings()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        //Instance
        public static AssortedCrazyThings Instance;

        //Slime pet legacy
        public static int[] slimePetLegacy = new int[9];
        public static int[] slimePetNoHair = new int[6];

        //Sun pet textures
        public static Texture2D[] sunPetTextures;

        //Soul item animated textures
        public static Texture2D[] animatedSoulTextures;

        //Soul NPC spawn blacklist
        public static int[] soulBuffBlacklist;

        // UI stuff
        internal static UserInterface AmmoboxAmmoSwapInterface;
        internal static AmmoSelectorUI AmmoboxSwapUI;
        //internal static ModHotKey AmmoboxAmmoSwapHotkey;

        internal static UserInterface EverhallowedLanternSwapInterface;
        internal static EverhallowedLanternUI EverhallowedLanternSwapUI;

        //Mod Helpers compat
        public static string GithubUserName { get { return "Werebearguy"; } }
        public static string GithubProjectName { get { return "AssortedCrazyThings"; } }

        private void InitPets()
        {
            int index = 0;
            slimePetLegacy[index++] = ProjectileType<CuteSlimeBlackPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeBluePet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeGreenPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimePinkPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimePurplePet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeRainbowPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeRedPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeXmasPet>();
            slimePetLegacy[index++] = ProjectileType<CuteSlimeYellowPet>();

            index = 0;

            slimePetNoHair[index++] = ProjectileType<CuteSlimeBlackNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimeBlueNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimePurpleNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimePinkNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimeRedNewPet>();
            slimePetNoHair[index++] = ProjectileType<CuteSlimeYellowNewPet>();

            if (!Main.dedServ && Main.netMode != 2)
            {
                PetAccessory.Load();
            }
        }

        private void InitSoulBuffBlacklist()
        {
            soulBuffBlacklist = new int[40];
            int index = 0;
            soulBuffBlacklist[index++] = NPCID.GiantWormBody;
            soulBuffBlacklist[index++] = NPCID.GiantWormTail;
            soulBuffBlacklist[index++] = NPCID.DiggerBody;
            soulBuffBlacklist[index++] = NPCID.DiggerTail;
            soulBuffBlacklist[index++] = NPCID.DevourerBody;
            soulBuffBlacklist[index++] = NPCID.DevourerTail;
            soulBuffBlacklist[index++] = NPCID.EaterofWorldsBody;
            soulBuffBlacklist[index++] = NPCID.EaterofWorldsTail;
            soulBuffBlacklist[index++] = NPCID.SeekerBody;
            soulBuffBlacklist[index++] = NPCID.SeekerTail;
            soulBuffBlacklist[index++] = NPCID.TombCrawlerBody;
            soulBuffBlacklist[index++] = NPCID.TombCrawlerTail;
            soulBuffBlacklist[index++] = NPCID.LeechBody;
            soulBuffBlacklist[index++] = NPCID.LeechTail;
            soulBuffBlacklist[index++] = NPCID.BoneSerpentBody;
            soulBuffBlacklist[index++] = NPCID.BoneSerpentTail;
            soulBuffBlacklist[index++] = NPCID.DuneSplicerBody;
            soulBuffBlacklist[index++] = NPCID.DuneSplicerTail;
            soulBuffBlacklist[index++] = NPCID.SpikeBall;
            soulBuffBlacklist[index++] = NPCID.BlazingWheel;

            soulBuffBlacklist[index++] = NPCID.BlueSlime;
            soulBuffBlacklist[index++] = NPCID.SlimeSpiked;
            //immune to all debuffs anyway
            //soulBuffBlacklist[index++] = NPCID.TheDestroyerBody;
            //soulBuffBlacklist[index++] = NPCID.TheDestroyerTail;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody1;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody2;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody3;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonBody4;
            //soulBuffBlacklist[index++] = NPCID.CultistDragonTail;

            Array.Resize(ref soulBuffBlacklist, index + 1);
        }

        private void AddToSoulBuffBlacklist()
        {
            int index = soulBuffBlacklist.Length - 1; //last index

            Array.Resize(ref soulBuffBlacklist, index + 40); //buffer


            Mod pinkymod = ModLoader.GetMod("pinkymod");
            if (pinkymod != null)
            {
                soulBuffBlacklist[index++] = pinkymod.NPCType("BoneLeechBody");
                soulBuffBlacklist[index++] = pinkymod.NPCType("BoneLeechTail");
            }

            Array.Resize(ref soulBuffBlacklist, index + 1);
        }

        private void LoadUI()
        {
            //AmmoboxAmmoSwapHotkey = RegisterHotKey("Switch between ammo", "C");

            if (!Main.dedServ && Main.netMode != 2)
            {
                AmmoboxSwapUI = new AmmoSelectorUI();
                AmmoboxSwapUI.Activate();
                AmmoboxAmmoSwapInterface = new UserInterface();
                AmmoboxAmmoSwapInterface.SetState(AmmoboxSwapUI);

                EverhallowedLanternSwapUI = new EverhallowedLanternUI();
                EverhallowedLanternSwapUI.Activate();
                EverhallowedLanternSwapInterface = new UserInterface();
                EverhallowedLanternSwapInterface.SetState(EverhallowedLanternSwapUI);
            }
        }

        private void UnloadUI()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                AmmoboxAmmoSwapInterface = null;
                AmmoboxSwapUI = null;
                //AmmoboxAmmoSwapHotkey = null;

                EverhallowedLanternSwapInterface = null;
                EverhallowedLanternSwapUI = null;
            }
        }

        public override void Load()
        {
            Instance = this;

            InitPets();

            InitSoulBuffBlacklist();

            LoadUI();

            if (!Main.dedServ && Main.netMode != 2)
            {
                animatedSoulTextures = new Texture2D[2];

                animatedSoulTextures[0] = GetTexture("Items/CaughtDungeonSoulAnimated");
                animatedSoulTextures[1] = GetTexture("Items/CaughtDungeonSoulFreedAnimated");

                sunPetTextures = new Texture2D[3];

                for (int i = 0; i < 3; i++)
                {
                    sunPetTextures[i] = GetTexture("Projectiles/Pets/SunPetProj_" + i);
                    PremultiplyTexture(sunPetTextures[i]);
                }
            }
        }

        public override void Unload()
        {
            PetAccessory.Unload();

            UnloadUI();

            if (!Main.dedServ && Main.netMode != 2)
            {
                animatedSoulTextures = null;

                sunPetTextures = null;
            }

            Instance = null;
        }

        public override void PostSetupContent()
        {
            AddToSoulBuffBlacklist();

            //https://forums.terraria.org/index.php?threads/boss-checklist-in-game-progression-checklist.50668/
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                //5.1f means just after skeletron
                bossChecklist.Call("AddMiniBossWithInfo", NPCs.DungeonBird.Harvester.name, 5.1f, (Func<bool>)(() => AssWorld.downedHarvester), "Use a [i:" + ItemType<Items.IdolOfDecay>() + "] in the dungeon after Skeletron has been defeated");
            }
        }

        private void UpdateAmmoBoxUI(GameTime gameTime)
        {
        //    if (AmmoSelectorUI.visible && AmmoboxSwapUI != null)
        //    {
        //        AmmoboxSwapUI.Update(gameTime);
        //    }
        //    if (AmmoboxAmmoSwapHotkey.JustPressed) Main.NewText("HotKeyPressed");
        //    if (AmmoboxAmmoSwapHotkey.JustReleased) Main.NewText("HotKeyReleased");
        //    //Main.NewText("visible: " + AmmoSelectorUI.visible);

        //    if (AmmoboxAmmoSwapHotkey.JustPressed && true && AmmoSelectorUI.itemAllowed)
        //    {
        //        //  Spawn ammo selector
        //        //Main.NewText("test");
        //        AmmoboxSwapUI.UpdateAmmoTypeList();
        //        AmmoSelectorUI.currentFirstAmmoType = Main.LocalPlayer.inventory[54].type;
        //        AmmoSelectorUI.visible = true;
        //        AmmoSelectorUI.spawnPosition = Main.MouseScreen;
        //        AmmoSelectorUI.leftCorner = Main.MouseScreen - new Vector2(AmmoSelectorUI.mainRadius, AmmoSelectorUI.mainRadius);
        //    }
        //    else if (AmmoboxAmmoSwapHotkey.JustReleased && AmmoSelectorUI.visible)
        //    {
        //        //  Destroy ammo selector
        //        Main.NewText("ammo selector closing");
        //        //  Switch selected ammo
        //        if (AmmoSelectorUI.selectedAmmoType != -1)
        //        {
        //            List<Tuple<int, int>> available = new List<Tuple<int, int>>();
        //            //  Basic belt
        //            for (int i = 54; i <= 57; i++)
        //            {
        //                if (Main.LocalPlayer.inventory[i].type == AmmoSelectorUI.selectedAmmoType)
        //                {
        //                    //  Add pairs slotID - stackSize of chosen ammo
        //                    available.Add(new Tuple<int, int>(i, Main.LocalPlayer.inventory[i].stack));
        //                }
        //            }
        //            //  Lihzahrd belt
        //            if (false) //true
        //            {
        //                for (int j = 0; j < 54; j++)
        //                {
        //                    if (Main.LocalPlayer.inventory[j].type == AmmoSelectorUI.selectedAmmoType)
        //                    {
        //                        available.Add(new Tuple<int, int>(j, Main.LocalPlayer.inventory[j].stack));
        //                    }
        //                }
        //            }

        //            Tuple<int, int> chosen = available[0];
        //            //  Prioritize larger stacks for switching
        //            foreach (Tuple<int, int> tuple in available)
        //            {
        //                if (tuple.Item2 > chosen.Item2)
        //                {
        //                    chosen = tuple;
        //                }
        //            }

        //            //  Switch ammo stacks
        //            //  Save first stack
        //            Item temp = Main.LocalPlayer.inventory[54];
        //            Item chosenItem = Main.LocalPlayer.inventory[chosen.Item1];
        //            Main.LocalPlayer.inventory[54] = chosenItem;
        //            Main.LocalPlayer.inventory[chosen.Item1] = temp;
        //        }

        //        AmmoSelectorUI.currentFirstAmmoType = -1;
        //        AmmoSelectorUI.selectedAmmoType = -1;
        //        AmmoSelectorUI.visible = false;
        //        AmmoboxSwapUI.Update(gameTime);
        //        //  Set amount of circles drawn to -1
        //        AmmoSelectorUI.circleAmount = -1;
        //        //  Clear ammo types already in the list
        //        AmmoSelectorUI.ammoTypes.Clear();
        //        AmmoSelectorUI.ammoCount.Clear();
        //    }
        }

        private void UpdateEverhallowedLanternStats(int selectedSoulType)
        {
            for(int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
            {
                if(Main.LocalPlayer.inventory[i].type == ItemType<Items.Weapons.EverhallowedLantern>())
                {
                    CompanionDungeonSoulMinionBase.SoulStats stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(this, selectedSoulType);
                    Main.LocalPlayer.inventory[i].damage = stats.Damage;
                    Main.LocalPlayer.inventory[i].shoot = stats.Type;
                    Main.LocalPlayer.inventory[i].knockBack = stats.Knockback;

                    CompanionDungeonSoulMinionBase.SoulType soulType = (CompanionDungeonSoulMinionBase.SoulType)stats.SoulType;
                    if (soulType == CompanionDungeonSoulMinionBase.SoulType.Dungeon)
                    {
                        CombatText.NewText(Main.LocalPlayer.getRect(),
                            CombatText.HealLife, "Selected: " + soulType.ToString() + " Soul");
                    }
                    else
                    {
                        CombatText.NewText(Main.LocalPlayer.getRect(),
                            CombatText.HealLife, "Selected: Soul of " + soulType.ToString());
                    }
                }
            }
        }

        private void UpdateEverhallowedLanternUI(GameTime gameTime)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();

            if (mPlayer.RightClickPressed && Main.LocalPlayer.HeldItem.type == ItemType<Items.Weapons.EverhallowedLantern>())
            {
                EverhallowedLanternUI.visible = true;
                EverhallowedLanternUI.circleAmount = 4;
                EverhallowedLanternUI.heldItemType = Main.LocalPlayer.HeldItem.type;
                EverhallowedLanternUI.currentSoulMinionType = mPlayer.selectedSoulMinionType;
                EverhallowedLanternUI.spawnPosition = Main.MouseScreen;
                EverhallowedLanternUI.leftCorner = Main.MouseScreen - new Vector2(EverhallowedLanternUI.mainRadius, EverhallowedLanternUI.mainRadius);
            }
            else if (mPlayer.RightClickReleased && EverhallowedLanternUI.visible)
            {
                if (EverhallowedLanternUI.selectedSoulMinionType != -1 && mPlayer.selectedSoulMinionType != EverhallowedLanternUI.selectedSoulMinionType)
                {
                    Main.PlaySound(SoundID.Item4.WithVolume(0.8f), Main.LocalPlayer.position);
                    mPlayer.selectedSoulMinionType = EverhallowedLanternUI.selectedSoulMinionType;
                    UpdateEverhallowedLanternStats(EverhallowedLanternUI.selectedSoulMinionType);
                }
                
                EverhallowedLanternUI.selectedSoulMinionType = -1;
                EverhallowedLanternUI.visible = false;
            }
            else if (EverhallowedLanternUI.heldItemType != Main.LocalPlayer.HeldItem.type)
            {
                EverhallowedLanternUI.selectedSoulMinionType = -1;
                EverhallowedLanternUI.visible = false;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            //UpdateAmmoBoxUI(gameTime);
            UpdateEverhallowedLanternUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
            if (InventoryIndex != -1)
            {
                //layers.Insert(++InventoryIndex, new LegacyGameInterfaceLayer
                //    (
                //    "ACT: Ammo Swapping",
                //    delegate {
                //        if (AmmoSelectorUI.visible) AmmoboxAmmoSwapInterface.Draw(Main.spriteBatch, new GameTime());
                //        return true;
                //    },
                //    InterfaceScaleType.UI)
                //);

                layers.Insert(++InventoryIndex, new LegacyGameInterfaceLayer
                    (
                    "ACT: Everhallowed Lantern",
                    delegate {
                        if (EverhallowedLanternUI.visible) EverhallowedLanternSwapInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            AssMessageType msgType = (AssMessageType)reader.ReadByte();
            short knapSackSlimeIndex;
            int arrayLength;
            byte knapSackSlimeTexture;
            byte playernumber;
            AssPlayer mPlayer;
            PetPlayer petPlayer;

            switch (msgType)
            {
                case AssMessageType.SyncKnapSackSlimeTexture:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        knapSackSlimeIndex = reader.ReadInt16();
                        knapSackSlimeTexture = reader.ReadByte();
                        if (Main.projectile[knapSackSlimeIndex].type == ProjectileType<SlimePackMinion>())
                        {
                            Main.projectile[knapSackSlimeIndex].localAI[1] = knapSackSlimeTexture;
                        }
                    }
                    break;

                case AssMessageType.OnEnterWorld:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        playernumber = reader.ReadByte();
                        mPlayer = Main.player[playernumber].GetModPlayer<AssPlayer>();

                        arrayLength = reader.ReadInt16();
                        //Main.NewText(arrayLength);
                        short[] indexes = new short[arrayLength];
                        byte[] textures = new byte[arrayLength];

                        for (int i = 0; i < arrayLength; i++)
                        {
                            indexes[i] = reader.ReadInt16();
                            textures[i] = reader.ReadByte();
                        }
                        for (int i = 0; i < arrayLength; i++)
                        {
                            //Main.NewText("recv SyncKnapSackSlimeTextureOnEnterWorld with " + indexes[i] + " " + textures[i]);
                            if (Main.projectile[indexes[i]].type == ProjectileType<SlimePackMinion>())
                            {
                                Main.projectile[indexes[i]].localAI[1] = textures[i];
                            }
                        }
                    }
                    break;
                case AssMessageType.OnEnterWorldVanity:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        playernumber = reader.ReadByte();
                        petPlayer = Main.player[playernumber].GetModPlayer<PetPlayer>();
                        petPlayer.slots = reader.ReadUInt32();
                        petPlayer.petEyeType = reader.ReadByte();
                        petPlayer.mechFrogCrown = reader.ReadBoolean();
                    }
                    break;
                case AssMessageType.SendClientChangesVanity:
                    playernumber = reader.ReadByte();

                    //if (Main.netMode == NetmodeID.MultiplayerClient)
                    //{
                    //    Main.NewText("recv sendclientchanges from " + playernumber);
                    //}
                    petPlayer = Main.player[playernumber].GetModPlayer<PetPlayer>();
                    petPlayer.slots = reader.ReadUInt32();
                    petPlayer.petEyeType = reader.ReadByte();
                    petPlayer.mechFrogCrown = reader.ReadBoolean();
                    // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        //NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("server send SendClientChanges from " + playernumber), new Color(255, 25, 25));
                        ModPacket packet = GetPacket();
                        packet.Write((byte)AssMessageType.SendClientChangesVanity);
                        packet.Write(playernumber);
                        packet.Write((uint)petPlayer.slots);
                        packet.Write((byte)petPlayer.petEyeType);
                        packet.Write((bool)petPlayer.mechFrogCrown);
                        packet.Send(-1, playernumber);
                    }
                    break;
                default:
                    ErrorLogger.Log("AssortedCrazyThings: Unknown Message type: " + msgType);
                    break;
            }
        }

        //Credit to jopojelly
        public static void PremultiplyTexture(Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(
                        buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }

        public static string GetTimeAsString(bool accurate = true)
        {
            int num = 0;
            string text3 = Lang.inter[95].Value;
            string text4 = "AM";
            double num6 = Main.time;
            if (!Main.dayTime)
            {
                num6 += 54000.0;
            }
            num6 = num6 / 86400.0 * 24.0;
            double num7 = 7.5;
            num6 = num6 - num7 - 12.0;
            if (num6 < 0.0)
            {
                num6 += 24.0;
            }
            if (num6 >= 12.0)
            {
                text4 = "PM";
            }
            int num8 = (int)num6;
            double num9 = num6 - (double)num8;
            num9 = (double)(int)(num9 * 60.0);
            string text5 = string.Concat(num9);
            if (num9 < 10.0)
            {
                text5 = "0" + text5;
            }
            if (num8 > 12)
            {
                num8 -= 12;
            }
            if (num8 == 0)
            {
                num8 = 12;
            }
            if(!accurate) text5 = ((!(num9 < 30.0)) ? "30" : "00");
            return text3 + ": " + num8 + ":" + text5 + " " + text4;
        }

        public static string GetMoonPhaseAsString(bool showNumber = false)
        {
            string suffix = "";
            if (showNumber) suffix = " (" + (Main.moonPhase + 1) + ")";
            string text3 = Lang.inter[102].Value;
            if (Main.moonPhase == 0)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.FullMoon") + suffix;
            }
            else if (Main.moonPhase == 1)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaningGibbous") + suffix;
            }
            else if (Main.moonPhase == 2)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.ThirdQuarter") + suffix;
            }
            else if (Main.moonPhase == 3)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaningCrescent") + suffix;
            }
            else if (Main.moonPhase == 4)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.NewMoon") + suffix;
            }
            else if (Main.moonPhase == 5)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaxingCrescent") + suffix;
            }
            else if (Main.moonPhase == 6)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.FirstQuarter") + suffix;
            }
            else if (Main.moonPhase == 7)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaxingGibbous") + suffix;
            }
            return "";
        }
    }

    enum AssMessageType : byte
    {
        RedrawPetAccessories,
        SendClientChanges,
        SendClientChangesVanity,
        SyncKnapSackSlimeTexture,
        OnEnterWorld,
        OnEnterWorldVanity
    }
}
