using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Pets;
using AssortedCrazyThings.UI;
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

        }

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
        internal static UserInterface CircleUIInterface;
        internal static CircleUI CircleUI;
        internal static ModHotKey CircleUIHotkey;
        
        internal static CircleUIConf DocileDemonEyeConf;
        internal static CircleUIConf LifeLikeMechFrogConf;
        internal static CircleUIConf CursedSkullConf;
        internal static CircleUIConf YoungWyvernConf;

        internal static UserInterface EverhallowedLanternSwapInterface;
        internal static EverhallowedLanternUI EverhallowedLanternSwapUI;

        //Mod Helpers compat
        public static string GithubUserName { get { return "Werebearguy"; } }
        public static string GithubProjectName { get { return "AssortedCrazyThings"; } }

        private void LoadPets()
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

        private void LoadSoulBuffBlacklist()
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
            //assuming this is called after InitSoulBuffBlacklist
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
            //has to be called after Load() because of the Main.projFrames[projectile.type] calls
            if (!Main.dedServ && Main.netMode != 2)
            {
                CircleUI = new CircleUI();
                CircleUI.Activate();
                CircleUIInterface = new UserInterface();
                CircleUIInterface.SetState(CircleUI);
                
                DocileDemonEyeConf = CircleUIConf.DocileDemonEyeConf();
                LifeLikeMechFrogConf = CircleUIConf.LifeLikeMechFrogConf();
                CursedSkullConf = CircleUIConf.CursedSkullConf();
                YoungWyvernConf = CircleUIConf.YoungWyvernConf();

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
                CircleUIInterface = null;
                CircleUI = null;
                CircleUIHotkey = null;

                DocileDemonEyeConf = null;
                LifeLikeMechFrogConf = null;
                CursedSkullConf = null;
                YoungWyvernConf = null;

                EverhallowedLanternSwapInterface = null;
                EverhallowedLanternSwapUI = null;
            }
        }

        private void LoadMisc()
        {
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

        private void UnloadMisc()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                animatedSoulTextures = null;

                sunPetTextures = null;
            }
        }

        public override void Load()
        {
            AssUtils.Instance = this;

            CircleUIHotkey = RegisterHotKey("CircleUI", "C");

            ModConf.Load();

            LoadPets();

            LoadSoulBuffBlacklist();

            //LoadUI();

            LoadMisc();
        }

        public override void Unload()
        {
            PetAccessory.Unload();

            UnloadUI();

            UnloadMisc();

            AssUtils.Instance = null;
        }

        public override void PostSetupContent()
        {
            AddToSoulBuffBlacklist();

            LoadUI();

            //https://forums.terraria.org/index.php?threads/boss-checklist-in-game-progression-checklist.50668/
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                //5.1f means just after skeletron
                bossChecklist.Call("AddMiniBossWithInfo", NPCs.DungeonBird.Harvester.name, 5.1f, (Func<bool>)(() => AssWorld.downedHarvester), "Use a [i:" + ItemType<Items.IdolOfDecay>() + "] in the dungeon after Skeletron has been defeated");
            }
        }

        private void CircleUIStart()
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();

            if (Main.LocalPlayer.HeldItem.type == ItemType<VanitySelector>())
            {
                if (pPlayer.DocileDemonEye)
                {
                    //set custom config with starting value
                    CircleUI.currentSelected = pPlayer.petEyeType;

                    CircleUI.UIConf = DocileDemonEyeConf;
                }
                else if (pPlayer.LifelikeMechanicalFrog)
                {
                    CircleUI.currentSelected = pPlayer.mechFrogCrown? 1: 0;

                    CircleUI.UIConf = LifeLikeMechFrogConf;
                }
                else if (pPlayer.CursedSkull)
                {
                    CircleUI.currentSelected = pPlayer.cursedSkullType;

                    CircleUI.UIConf = CursedSkullConf;
                }
                else if (pPlayer.YoungWyvern)
                {
                    CircleUI.currentSelected = pPlayer.youngWyvernType;

                    CircleUI.UIConf = YoungWyvernConf;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            // Spawn UI
            CircleUI.visible = true;
            CircleUI.spawnPosition = Main.MouseScreen;
            CircleUI.leftCorner = Main.MouseScreen - new Vector2(CircleUI.mainRadius, CircleUI.mainRadius);
            CircleUI.heldItemType = Main.LocalPlayer.HeldItem.type;
            Main.NewText("CircleUIStart " + CircleUI.heldItemType);
        }

        private void PoofVisual(int projType)
        {
            int projIndex = -1;
            //find first occurence of a player owned projectile
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active)
                {
                    if (Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == projType)
                    {
                        projIndex = i;
                        break;
                    }
                }
            }
            Dust dust;
            float factor = 1f;
            if (Main.projectile[projIndex].velocity.Length() > 5) factor = 2f;
            for (int i = 0; i < 14; i++)
            {
                dust = Main.dust[Dust.NewDust(Main.projectile[projIndex].position, 18, 28, 204, Main.projectile[projIndex].velocity.X * factor, Main.projectile[projIndex].velocity.Y * factor, 0, new Color(255, 255, 255), 0.8f)];
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        private void CircleUIEnd()
        {
            Main.NewText("CircleUIEnd " + CircleUI.heldItemType);
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();
            if (CircleUI.returned != -1 && CircleUI.returned != CircleUI.currentSelected)
            {
                //if something returned AND if the returned thing isn't the same as the current one

                if (CircleUI.heldItemType == ItemType<VanitySelector>())
                {
                    Main.PlaySound(SoundID.Item4.WithVolume(0.8f), Main.LocalPlayer.position);
                    PoofVisual(CircleUI.UIConf.additionalInfo);

                    if (pPlayer.DocileDemonEye)
                    {
                        pPlayer.petEyeType = (byte)CircleUI.returned;
                    }
                    else if (pPlayer.LifelikeMechanicalFrog)
                    {
                        pPlayer.mechFrogCrown = (CircleUI.returned > 0) ? true : false;
                    }
                    else if (pPlayer.CursedSkull)
                    {
                        pPlayer.cursedSkullType = (byte)CircleUI.returned;
                    }
                    else if (pPlayer.YoungWyvern)
                    {
                        pPlayer.youngWyvernType = (byte)CircleUI.returned;
                    }
                }
            }

            CircleUI.returned = -1;
            CircleUI.visible = false;
        }

        private void UpdateCircleUI(GameTime gameTime)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            if (CircleUIHotkey.JustPressed) Main.NewText("HotKeyPressed");
            if (CircleUIHotkey.JustReleased) Main.NewText("HotKeyReleased");
            //Main.NewText("visible: " + CircleUI.visible);

            if (mPlayer.RightClickPressed && CircleUIConf.TriggerList.Contains(Main.LocalPlayer.HeldItem.type))
            {
                CircleUIStart();
            }
            else if (mPlayer.RightClickReleased && CircleUI.visible)
            {
                CircleUIEnd();
            }
            else if (CircleUI.heldItemType != Main.LocalPlayer.HeldItem.type) //cancel the UI when you switch items
            {
                CircleUI.returned = -1;
                CircleUI.visible = false;
            }
        }

        private void UpdateEverhallowedLanternStats(int selectedSoulType)
        {
            for(int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
            {
                if(Main.LocalPlayer.inventory[i].type == ItemType<Items.Weapons.EverhallowedLantern>())
                {
                    var stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(selectedSoulType);
                    Main.LocalPlayer.inventory[i].damage = stats.Damage;
                    Main.LocalPlayer.inventory[i].shoot = stats.Type;
                    Main.LocalPlayer.inventory[i].knockBack = stats.Knockback;

                    var soulType = (CompanionDungeonSoulMinionBase.SoulType)stats.SoulType;
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

            if (mPlayer.RightClickPressed && Main.LocalPlayer.HeldItem.type == ItemType<EverhallowedLantern>())
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
                    //if something returned AND if the returned thing isnt the same as the current one
                    Main.PlaySound(SoundID.Item4.WithVolume(0.8f), Main.LocalPlayer.position);
                    mPlayer.selectedSoulMinionType = EverhallowedLanternUI.selectedSoulMinionType;
                    UpdateEverhallowedLanternStats(EverhallowedLanternUI.selectedSoulMinionType);
                }
                
                EverhallowedLanternUI.selectedSoulMinionType = -1;
                EverhallowedLanternUI.visible = false;
            }
            else if (EverhallowedLanternUI.heldItemType != Main.LocalPlayer.HeldItem.type) //cancel the UI when you switch items
            {
                EverhallowedLanternUI.selectedSoulMinionType = -1;
                EverhallowedLanternUI.visible = false;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UpdateCircleUI(gameTime);
            UpdateEverhallowedLanternUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
            if (InventoryIndex != -1)
            {
                layers.Insert(++InventoryIndex, new LegacyGameInterfaceLayer
                    (
                    "ACT: Appearance Selection",
                    delegate
                    {
                        if (CircleUI.visible) CircleUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

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

        //public void SyncAltTextureNPC(NPC npc)
        //{
        //    //server side only

        //    if (Main.netMode == NetmodeID.Server)
        //    {
        //        ModPacket packet = GetPacket();
        //        packet.Write((byte)AssMessageType.SyncAltTextureNPC);
        //        packet.Write((byte)npc.whoAmI);
        //        packet.Write((byte)npc.altTexture);
        //        packet.Send();
        //    }
        //}

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            AssMessageType msgType = (AssMessageType)reader.ReadByte();
            short knapSackSlimeIndex;
            int arrayLength;
            byte knapSackSlimeTexture;
            byte playernumber;
            AssPlayer mPlayer;
            PetPlayer petPlayer;
            //byte npcnumber;
            //byte npcAltTexture;

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

                case AssMessageType.SyncPlayer:
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
                case AssMessageType.SyncPlayerVanity:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        playernumber = reader.ReadByte();
                        petPlayer = Main.player[playernumber].GetModPlayer<PetPlayer>();
                        petPlayer.slots = reader.ReadUInt32();
                        petPlayer.petEyeType = reader.ReadByte();
                        petPlayer.mechFrogCrown = reader.ReadBoolean();
                    }
                    break;
                //case AssMessageType.SyncAltTextureNPC:
                //    if (Main.netMode == NetmodeID.MultiplayerClient)
                //    {
                //        npcnumber = reader.ReadByte();
                //        npcAltTexture = reader.ReadByte();
                //        Main.NewText("recv tex " + npcAltTexture + " from " + npcnumber);
                //        Main.NewText("type " + Main.npc[npcnumber].type);
                //        Main.NewText("extracount " + NPCID.Sets.ExtraTextureCount[Main.npc[npcnumber].type]);
                //        if (NPCID.Sets.ExtraTextureCount[Main.npc[npcnumber].type] > 0)
                //        {
                //            Main.NewText("set tex to" + npcAltTexture);
                //            Main.npc[npcnumber].altTexture = npcAltTexture;
                //        }
                //    }
                //    break;
                case AssMessageType.SendClientChangesVanity:
                    playernumber = reader.ReadByte();
                    petPlayer = Main.player[playernumber].GetModPlayer<PetPlayer>();
                    petPlayer.slots = reader.ReadUInt32();
                    petPlayer.petEyeType = reader.ReadByte();
                    petPlayer.mechFrogCrown = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)AssMessageType.SendClientChangesVanity);
                        packet.Write(playernumber);
                        packet.Write((uint)petPlayer.slots);
                        packet.Write((byte)petPlayer.petEyeType);
                        packet.Write((bool)petPlayer.mechFrogCrown);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case AssMessageType.ConvertInertSoulsInventory:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        //convert souls in local inventory
                        mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
                        mPlayer.ConvertInertSoulsInventory();
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
    }

    enum AssMessageType : byte
    {
        SendClientChangesVanity,
        SyncKnapSackSlimeTexture,
        SyncPlayer,
        SyncPlayerVanity,
        SyncAltTextureNPC,
        ConvertInertSoulsInventory
    }
}
