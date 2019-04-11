using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.PetAccessories;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

[assembly: AssemblyVersion("1.2.3.0")]
namespace AssortedCrazyThings
{
    class AssortedCrazyThings : Mod
    {
        //Sun pet textures
        public static Texture2D[] sunPetTextures;

        //Soul item animated textures
        public static Texture2D[] animatedSoulTextures;

        //Soul NPC spawn blacklist
        public static int[] soulBuffBlacklist;

        //Zoom level, (for UIs)
        public static Vector2 ZoomFactor; //0f == fully zoomed out, 1f == fully zoomed in

        //Loaded Mods
        public static bool BossAssistLoadedWithRadar;

        //UI stuff
        internal static UserInterface CircleUIInterface;
        internal static CircleUI CircleUI;

        internal static UserInterface HoverNPCUIInterface;
        internal static HoverNPCUI HoverNPCUI;

        internal static UserInterface HarvesterEdgeUIInterface;
        internal static HarvesterEdgeUI HarvesterEdgeUI;

        internal static UserInterface EnhancedHunterUIInterface;
        internal static EnhancedHunterUI EnhancedHunterUI;

        internal static UserInterface PetVanityUIInterface;
        internal static PetVanityUI PetVanityUI;

        //Mod Helpers compat
        public static string GithubUserName { get { return "Werebearguy"; } }
        public static string GithubProjectName { get { return "AssortedCrazyThings"; } }

        private void LoadPets()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                SlimePets.Load();

                PetAccessory.Load();
            }
        }

        private void UnloadPets()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                SlimePets.Unload();

                PetAccessory.Unload();
            }
        }

        private void LoadSoulBuffBlacklist()
        {
            List<int> tempList = new List<int>
            {
                NPCID.Bee,
                NPCID.BeeSmall,
                NPCID.BlueSlime,
                NPCID.BlazingWheel,
                NPCID.EaterofWorldsHead,
                NPCID.EaterofWorldsBody,
                NPCID.EaterofWorldsTail,
                NPCID.Creeper,
                NPCID.GolemFistLeft,
                NPCID.GolemFistRight,
                NPCID.PlanterasHook,
                NPCID.PlanterasTentacle,
                NPCID.Probe,
                NPCID.ServantofCthulhu,
                NPCID.SlimeSpiked,
                NPCID.SpikeBall,
                NPCID.TheHungry,
                NPCID.TheHungryII,

            };

            soulBuffBlacklist = tempList.ToArray();
        }

        private void AddToSoulBuffBlacklist()
        {
            //assuming this is called after InitSoulBuffBlacklist
            List<int> tempList = new List<int>(soulBuffBlacklist)
            {
                NPCType<DungeonSoul>(),
                NPCType<DungeonSoulFreed>()
            };

            soulBuffBlacklist = tempList.ToArray();
        }

        private void LoadWormList()
        {
            List<int> tempList = new List<int>();

            for (int i = 580; i < NPCLoader.NPCCount; i++)
            {
                ModNPC modNPC = NPCLoader.GetNPC(i);
                if (modNPC != null && (modNPC.GetType().Name.EndsWith("Body") || modNPC.GetType().Name.EndsWith("Tail")))
                {
                    tempList.Add(modNPC.npc.type);
                }
            }

            AssUtils.isModdedWormBodyOrTail = tempList.ToArray();
            Array.Sort(AssUtils.isModdedWormBodyOrTail);
        }

        private void LoadUI()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                CircleUI = new CircleUI();
                CircleUI.Activate();
                CircleUIInterface = new UserInterface();
                CircleUIInterface.SetState(CircleUI);

                HoverNPCUI = new HoverNPCUI();
                HoverNPCUI.Activate();
                HoverNPCUIInterface = new UserInterface();
                HoverNPCUIInterface.SetState(HoverNPCUI);

                HarvesterEdgeUI = new HarvesterEdgeUI();
                HarvesterEdgeUI.Activate();
                HarvesterEdgeUIInterface = new UserInterface();
                HarvesterEdgeUIInterface.SetState(HarvesterEdgeUI);

                EnhancedHunterUI = new EnhancedHunterUI();
                EnhancedHunterUI.Activate();
                EnhancedHunterUIInterface = new UserInterface();
                EnhancedHunterUIInterface.SetState(EnhancedHunterUI);

                PetVanityUI = new PetVanityUI();
                PetVanityUI.Activate();
                PetVanityUIInterface = new UserInterface();
                PetVanityUIInterface.SetState(PetVanityUI);

                CircleUIConf.AddItemAsTrigger(ItemType<EverhallowedLantern>(), false); //right click of Everhallowed Lantern
                CircleUIConf.AddItemAsTrigger(ItemType<SlimeHandlerKnapsack>(), false); //right click of Slime Handler Knapsack
                CircleUIConf.AddItemAsTrigger(ItemType<VanitySelector>()); //left click of Costume Suitcase
                CircleUIConf.AddItemAsTrigger(ItemType<VanitySelector>(), false); //right click of Costume Suitcase
            }
        }

        private void UnloadUI()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                CircleUIInterface = null;
                CircleUI = null;

                HoverNPCUIInterface = null;
                HoverNPCUI = null;

                HarvesterEdgeUIInterface = null;
                HarvesterEdgeUI = null;

                EnhancedHunterUIInterface = null;
                EnhancedHunterUI = null;

                PetVanityUIInterface = null;
                PetVanityUI = null;

                HarvesterEdgeUI.texture = null;
                EnhancedHunterUI.arrowTexture = null;
                PetVanityUI.redCrossTexture = null;
                CircleUIConf.TriggerListLeft.Clear();
                CircleUIConf.TriggerListRight.Clear();
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

                for (int i = 0; i < sunPetTextures.Length; i++)
                {
                    sunPetTextures[i] = GetTexture("Projectiles/Pets/PetSunProj_" + i);
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

            ModConf.Load();

            LoadPets();

            LoadSoulBuffBlacklist();

            LoadMisc();
        }

        public override void Unload()
        {
            UnloadPets();

            UnloadUI();

            UnloadMisc();

            GitgudData.Unload();

            AssUtils.Instance = null;
        }

        public override void PostSetupContent()
        {
            //for things that have to be called after Load() because of Main.projFrames[projectile.type] calls (and similar)
            LoadUI();

            LoadWormList();

            GitgudData.Load();

            AddToSoulBuffBlacklist();

            //https://forums.terraria.org/index.php?threads/boss-checklist-in-game-progression-checklist.50668/
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                //5.1f means just after skeletron
                bossChecklist.Call("AddMiniBossWithInfo", Harvester.name, 5.1f, (Func<bool>)(() => AssWorld.downedHarvester), "Use a [i:" + ItemType<IdolOfDecay>() + "] in the dungeon after Skeletron has been defeated");
            }

            Mod bossAssist = ModLoader.GetMod("BossAssist");
            if (bossAssist != null && bossAssist.Version > new Version(0, 2, 2))
            {
                BossAssistLoadedWithRadar = true;
            }
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

            if (projIndex != -1)
            {
                Dust dust;
                for (int i = 0; i < 14; i++)
                {
                    dust = Main.dust[Dust.NewDust(Main.projectile[projIndex].position, Main.projectile[projIndex].width, Main.projectile[projIndex].height, 204, Main.projectile[projIndex].velocity.X, Main.projectile[projIndex].velocity.Y, 0, new Color(255, 255, 255), 0.8f)];
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
        }

        private void UIText(string str, Color color)
        {
            CombatText.NewText(Main.LocalPlayer.getRect(), color, str);
        }

        private void CircleUIStart(int triggerType, bool triggerLeft = true)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();

            if (triggerLeft) //left click
            {
                if (triggerType == ItemType<VanitySelector>())
                {
                    if (pPlayer.DocileDemonEye)
                    {
                        //set custom config with starting value
                        CircleUI.currentSelected = pPlayer.petEyeType;

                        CircleUI.UIConf = CircleUIConf.DocileDemonEyeConf();
                    }
                    else if (pPlayer.LifelikeMechanicalFrog)
                    {
                        CircleUI.currentSelected = pPlayer.mechFrogCrown ? 1 : 0;

                        CircleUI.UIConf = CircleUIConf.LifeLikeMechFrogConf();
                    }
                    else if (pPlayer.CursedSkull)
                    {
                        CircleUI.currentSelected = pPlayer.cursedSkullType;

                        CircleUI.UIConf = CircleUIConf.CursedSkullConf();
                    }
                    else if (pPlayer.YoungWyvern)
                    {
                        CircleUI.currentSelected = pPlayer.youngWyvernType;

                        CircleUI.UIConf = CircleUIConf.YoungWyvernConf();
                    }
                    else if (pPlayer.PetFishron)
                    {
                        CircleUI.currentSelected = pPlayer.petFishronType;

                        CircleUI.UIConf = CircleUIConf.PetFishronConf();
                    }
                    else if (pPlayer.YoungHarpy)
                    {
                        CircleUI.currentSelected = pPlayer.youngHarpyType;

                        CircleUI.UIConf = CircleUIConf.YoungHarpyConf();
                    }
                    else if (pPlayer.Abeemination)
                    {
                        CircleUI.currentSelected = pPlayer.abeeminationType;

                        CircleUI.UIConf = CircleUIConf.AbeeminiationConf();
                    }
                    else if (pPlayer.LilWraps)
                    {
                        CircleUI.currentSelected = pPlayer.lilWrapsType;

                        CircleUI.UIConf = CircleUIConf.LilWrapsConf();
                    }
                    else if (pPlayer.VampireBat)
                    {
                        CircleUI.currentSelected = pPlayer.vampireBatType;

                        CircleUI.UIConf = CircleUIConf.VampireBatConf();
                    }
                    else if (pPlayer.Pigronata)
                    {
                        CircleUI.currentSelected = pPlayer.pigronataType;

                        CircleUI.UIConf = CircleUIConf.PigronataConf();
                    }
                    else if (pPlayer.QueenLarva)
                    {
                        CircleUI.currentSelected = pPlayer.queenLarvaType;

                        CircleUI.UIConf = CircleUIConf.QueenLarvaConf();
                    }
                    else if (pPlayer.OceanSlime)
                    {
                        CircleUI.currentSelected = pPlayer.oceanSlimeType;

                        CircleUI.UIConf = CircleUIConf.OceanSlimeConf();
                    }
                    else if (pPlayer.MiniAntlion)
                    {
                        CircleUI.currentSelected = pPlayer.miniAntlionType;

                        CircleUI.UIConf = CircleUIConf.MiniAntlionConf();
                    }
                    else if (pPlayer.PetGoldfish)
                    {
                        CircleUI.currentSelected = pPlayer.petGoldfishType;

                        CircleUI.UIConf = CircleUIConf.PetGoldfishConf();
                    }
                    else if (pPlayer.SkeletronHand)
                    {
                        CircleUI.currentSelected = pPlayer.skeletronHandType;

                        CircleUI.UIConf = CircleUIConf.SkeletronHandConf();
                    }
                    //FOR LEFT CLICK ONLY (REGULAR PET)
                    //ALTERNATE
                    //else if (pPlayer.ClassName)
                    //{
                    //    CircleUI.currentSelected = pPlayer.classNameType;

                    //    CircleUI.UIConf = CircleUIConf.ClassNameConf();
                    //}
                    else
                    {
                        UIText("No alt costumes found for pet", CombatText.DamagedFriendly);
                        return;
                    }
                }
            }
            else //right click
            {
                if (triggerType == ItemType<VanitySelector>())
                {
                    if (pPlayer.PetMoon)
                    {
                        CircleUI.currentSelected = pPlayer.petMoonType;

                        CircleUI.UIConf = CircleUIConf.PetMoonConf();
                    }
                    else if (pPlayer.PetCultist)
                    {
                        CircleUI.currentSelected = pPlayer.petCultistType;

                        CircleUI.UIConf = CircleUIConf.PetCultistConf();
                    }
                    //FOR RIGHT CLICK ONLY (LIGHT PET)
                    //ALTERNATE
                    //else if (pPlayer.ClassName)
                    //{
                    //    CircleUI.currentSelected = pPlayer.classNameType;

                    //    CircleUI.UIConf = CircleUIConf.ClassNameConf();
                    //}
                    else
                    {
                        UIText("No alt costumes found for light pet", CombatText.DamagedFriendly);
                        return;
                    }
                }
                else if (triggerType == ItemType<EverhallowedLantern>())
                {
                    CircleUI.currentSelected = mPlayer.selectedSoulMinionType;

                    //this one needs to be created anew because of the unlocked list
                    CircleUI.UIConf = CircleUIConf.EverhallowedLanternConf();
                }
                else if (triggerType == ItemType<SlimeHandlerKnapsack>())
                {
                    CircleUI.currentSelected = mPlayer.selectedSlimePackMinionType;

                    //this one needs to be created anew because of the unlocked list
                    CircleUI.UIConf = CircleUIConf.SlimeHandlerKnapsackConf();
                }
                else
                {
                    return;
                }
            }

            //Spawn UI
            CircleUI.visible = true;
            CircleUI.spawnPosition = Main.MouseScreen;
            CircleUI.leftCorner = Main.MouseScreen - new Vector2(CircleUI.mainRadius, CircleUI.mainRadius);
            CircleUI.heldItemType = triggerType;
            CircleUI.fadeIn = 0;
        }

        private void CircleUIEnd(bool triggerLeft = true)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();
            if (CircleUI.returned != -1 && CircleUI.returned != CircleUI.currentSelected)
            {
                //if something returned AND if the returned thing isn't the same as the current one

                Main.PlaySound(SoundID.Item4.WithVolume(0.6f), Main.LocalPlayer.position);

                if (triggerLeft) //left click
                {
                    if (CircleUI.heldItemType == ItemType<VanitySelector>())
                    {
                        PoofVisual(CircleUI.UIConf.AdditionalInfo);
                        UIText("Selected: " + CircleUI.UIConf.Tooltips[CircleUI.returned], CombatText.HealLife);
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
                        else if (pPlayer.PetFishron)
                        {
                            pPlayer.petFishronType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.YoungHarpy)
                        {
                            pPlayer.youngHarpyType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.Abeemination)
                        {
                            pPlayer.abeeminationType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.LilWraps)
                        {
                            pPlayer.lilWrapsType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.VampireBat)
                        {
                            pPlayer.vampireBatType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.Pigronata)
                        {
                            pPlayer.pigronataType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.QueenLarva)
                        {
                            pPlayer.queenLarvaType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.OceanSlime)
                        {
                            pPlayer.oceanSlimeType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.MiniAntlion)
                        {
                            pPlayer.miniAntlionType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.PetGoldfish)
                        {
                            pPlayer.petGoldfishType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.SkeletronHand)
                        {
                            pPlayer.skeletronHandType = (byte)CircleUI.returned;
                        }
                        //ALTERNATE
                        //else if (pPlayer.ClassName)
                        //{
                        //    pPlayer.classNameType = (byte)CircleUI.returned;
                        //}
                    }
                }
                else //right click
                {
                    if (CircleUI.heldItemType == ItemType<VanitySelector>())
                    {
                        PoofVisual(CircleUI.UIConf.AdditionalInfo);
                        UIText("Selected: " + CircleUI.UIConf.Tooltips[CircleUI.returned], CombatText.HealLife);
                        if (pPlayer.PetMoon)
                        {
                            pPlayer.petMoonType = (byte)CircleUI.returned;
                        }
                        else if (pPlayer.PetCultist)
                        {
                            pPlayer.petCultistType = (byte)CircleUI.returned;
                        }
                        //ALTERNATE
                        //else if (pPlayer.ClassName)
                        //{
                        //    pPlayer.classNameType = (byte)CircleUI.returned;
                        //}
                    }
                    else if (CircleUI.heldItemType == ItemType<EverhallowedLantern>())
                    {
                        mPlayer.selectedSoulMinionType = CircleUI.returned;

                        UpdateEverhallowedLanternStats(CircleUI.returned);
                    }
                    else if (CircleUI.heldItemType == ItemType<SlimeHandlerKnapsack>())
                    {
                        mPlayer.selectedSlimePackMinionType = (byte)CircleUI.returned;

                        UIText("Selected: " + (mPlayer.selectedSlimePackMinionType == 0 ? "Default" : (mPlayer.selectedSlimePackMinionType == 1? "Assorted" : "Spiked")), CombatText.HealLife);
                    }
                }
            }

            CircleUI.returned = -1;
            CircleUI.visible = false;
        }

        private void UpdateCircleUI(GameTime gameTime)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();

            bool? left = null;
            if (mPlayer.LeftClickPressed && CircleUIConf.TriggerListLeft.Contains(Main.LocalPlayer.HeldItem.type))
            {
                left = true;
            }
            else if (mPlayer.RightClickPressed && CircleUIConf.TriggerListRight.Contains(Main.LocalPlayer.HeldItem.type))
            {
                left = false;
            }

            if (left != null && AllowedToOpenUI()) CircleUIStart(Main.LocalPlayer.HeldItem.type, (bool)left);

            if (CircleUI.visible)
            {
                left = null;
                if (mPlayer.LeftClickReleased)
                {
                    left = true;
                }
                else if (mPlayer.RightClickReleased)
                {
                    left = false;
                }

                if (left != null) CircleUIEnd((bool)left);

                if (CircleUI.heldItemType != Main.LocalPlayer.HeldItem.type) //cancel the UI when you switch items
                {
                    CircleUI.returned = -1;
                    CircleUI.visible = false;
                }
            }
        }

        private void UpdateEverhallowedLanternStats(int selectedSoulType)
        {
            bool first = true;
            for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
            {
                if (Main.LocalPlayer.inventory[i].type == ItemType<EverhallowedLantern>())
                {
                    var stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(selectedSoulType);
                    //bad practice, don't do this
                    Main.LocalPlayer.inventory[i].damage = stats.Damage;
                    Main.LocalPlayer.inventory[i].shoot = stats.Type;
                    Main.LocalPlayer.inventory[i].knockBack = stats.Knockback;

                    var soulType = (CompanionDungeonSoulMinionBase.SoulType)stats.SoulType;
                    if (first && soulType == CompanionDungeonSoulMinionBase.SoulType.Dungeon)
                    {
                        CombatText.NewText(Main.LocalPlayer.getRect(),
                            CombatText.HealLife, "Selected: " + soulType.ToString() + " Soul");
                    }
                    else if (first)
                    {
                        CombatText.NewText(Main.LocalPlayer.getRect(),
                            CombatText.HealLife, "Selected: Soul of " + soulType.ToString());
                    }
                    first = false;
                }
            }
        }

        private void UpdatePetVanityUI(GameTime gameTime)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            PetPlayer pPlayer = Main.LocalPlayer.GetModPlayer<PetPlayer>();

            if (mPlayer.LeftClickPressed && AllowedToOpenUI() && PetAccessory.IsItemAPetVanity(Main.LocalPlayer.HeldItem.type, forUI: true))
            {
                PetAccessory petAccessory = PetAccessory.GetAccessoryFromType(Main.LocalPlayer.HeldItem.type);
                if(pPlayer.slimePetIndex != -1 &&
                    Main.projectile[pPlayer.slimePetIndex].active &&
                    Main.projectile[pPlayer.slimePetIndex].owner == Main.myPlayer &&
                    SlimePets.slimePets.Contains(Main.projectile[pPlayer.slimePetIndex].type) &&
                    !SlimePets.GetPet(Main.projectile[pPlayer.slimePetIndex].type).IsSlotTypeBlacklisted[(int)petAccessory.Slot])
                {
                    //Spawn UI
                    PetVanityUI.visible = true;
                    PetVanityUI.spawnPosition = Main.MouseScreen;
                    PetVanityUI.leftCorner = Main.MouseScreen - new Vector2(CircleUI.mainRadius, CircleUI.mainRadius);
                    PetVanityUI.petAccessory = petAccessory;
                    PetAccessory petAcc = Main.LocalPlayer.GetModPlayer<PetPlayer>().GetAccessoryInSlot((byte)PetVanityUI.petAccessory.Slot);
                    PetVanityUI.hasEquipped = petAcc != null && petAcc.Type == petAccessory.Type;
                    PetVanityUI.fadeIn = 0;
                }
            }

            if (PetVanityUI.visible)
            {
                if (mPlayer.LeftClickReleased)
                {
                    if (PetVanityUI.returned > -1)
                    {
                        //if something returned AND if the returned thing isn't the same as the current one

                        Main.PlaySound(SoundID.Item1.WithVolume(2f), Main.LocalPlayer.position);
                        //UIText("Selected: " + PetVanityUI.petAccessory.AltTextureSuffixes[PetVanityUI.returned], CombatText.HealLife);

                        PetVanityUI.petAccessory.Color = (byte)PetVanityUI.returned;
                        pPlayer.ToggleAccessory(PetVanityUI.petAccessory);
                    }
                    else if (PetVanityUI.hasEquipped && PetVanityUI.returned == -1)
                    {
                        //hovered over the middle and had something equipped: take accessory away
                        pPlayer.DelAccessory(PetVanityUI.petAccessory);
                    }
                    //else if (returned == -2) {nothing happens}

                    PetVanityUI.returned = -1;
                    PetVanityUI.visible = false;
                }

                if (PetVanityUI.petAccessory.Type != Main.LocalPlayer.HeldItem.type) //cancel the UI when you switch items
                {
                    PetVanityUI.returned = -1;
                    PetVanityUI.visible = false;
                }
            }
        }

        private void UpdateHoverNPCUI(GameTime gameTime)
        {
            //if (AssUtils.AnyNPCs(AssWorld.harvesterTypes))
            //{
            //    HoverNPCUI.visible = true;
            //}
            //else
            //{
            //    HoverNPCUI.visible = false;
            //}
            HoverNPCUI.Update(gameTime);
        }

        private void UpdateEnhancedHunterUI(GameTime gameTime)
        {
            if (Main.LocalPlayer.GetModPlayer<AssPlayer>().enhancedHunterBuff)
            {
                EnhancedHunterUI.visible = true;
            }
            else
            {
                EnhancedHunterUI.visible = false;
            }
            EnhancedHunterUI.Update(gameTime);
        }

        private void UpdateHarvesterEdgeUI(GameTime gameTime)
        {
            HarvesterEdgeUI.Update(gameTime);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UpdateCircleUI(gameTime);
            UpdateHoverNPCUI(gameTime);
            UpdateEnhancedHunterUI(gameTime);
            UpdateHarvesterEdgeUI(gameTime);
            UpdatePetVanityUI(gameTime);
        }

        private bool AllowedToOpenUI()
        {
            return Main.hasFocus &&
                !Main.gamePaused &&
                !Main.LocalPlayer.dead &&
                !Main.LocalPlayer.mouseInterface &&
                !Main.drawingPlayerChat &&
                !Main.editSign &&
                !Main.editChest &&
                !Main.blockInput &&
                !Main.mapFullscreen &&
                !Main.HoveringOverAnNPC &&
                !Main.LocalPlayer.showItemIcon &&
                Main.LocalPlayer.talkNPC == -1 &&
                !(Main.LocalPlayer.frozen || Main.LocalPlayer.webbed || Main.LocalPlayer.stoned);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
            if (inventoryIndex != -1)
            {
                if (CircleUI.visible)
                {
                    //remove the item icon when using the item while held outside the inventory
                    int mouseItemIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Item / NPC Head"));
                    if (mouseItemIndex != -1) layers.RemoveAt(mouseItemIndex);
                    layers.Insert(++inventoryIndex, new LegacyGameInterfaceLayer
                        (
                        "ACT: Appearance Select",
                        delegate
                        {
                            CircleUIInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }

                if (PetVanityUI.visible)
                {
                    //remove the item icon when using the item while held outside the inventory
                    int mouseItemIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Item / NPC Head"));
                    if (mouseItemIndex != -1) layers.RemoveAt(mouseItemIndex);
                    layers.Insert(++inventoryIndex, new LegacyGameInterfaceLayer
                        (
                        "ACT: Pet Vanity Select",
                        delegate
                        {
                            PetVanityUIInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }

            int mouseOverIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Over"));
            if (mouseOverIndex != -1)
            {
                layers.Insert(++mouseOverIndex, new LegacyGameInterfaceLayer
                    (
                    "ACT: NPC Mouse Over",
                    delegate
                    {
                        HoverNPCUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(++mouseOverIndex, new LegacyGameInterfaceLayer
                    (
                    "ACT: Enhanced Hunter",
                    delegate
                    {
                        if (EnhancedHunterUI.visible) EnhancedHunterUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(++mouseOverIndex, new LegacyGameInterfaceLayer
                    (
                    "ACT: Harvester Edge",
                    delegate
                    {
                        HarvesterEdgeUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            ZoomFactor = Transform.Zoom - (Vector2.UnitX + Vector2.UnitY);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            AssMessageType msgType = (AssMessageType)reader.ReadByte();
            byte playerNumber;
            AssPlayer mPlayer;
            PetPlayer petPlayer;
            byte changes;

            switch (msgType)
            {
                case AssMessageType.SyncPlayerVanity:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        playerNumber = reader.ReadByte();
                        petPlayer = Main.player[playerNumber].GetModPlayer<PetPlayer>();
                        //no "changes" packet
                        petPlayer.RecvSyncPlayerVanitySub(reader);
                    }
                    break;
                case AssMessageType.SendClientChangesVanity:
                    //client and server
                    playerNumber = reader.ReadByte();
                    petPlayer = Main.player[playerNumber].GetModPlayer<PetPlayer>();
                    changes = reader.ReadByte();
                    petPlayer.RecvClientChangesPacketSub(reader, changes);

                    //server transmits to others
                    if (Main.netMode == NetmodeID.Server)
                    {
                        petPlayer.SendClientChangesPacketSub(changes, -1, playerNumber);
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
                case AssMessageType.GitgudLoadCounters:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        GitgudData.RecvCounters(reader);
                    }
                    break;
                case AssMessageType.GitgudChangeCounters:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        //GitgudData.RecvReset(Main.myPlayer, reader);
                        GitgudData.RecvChangeCounter(reader);
                    }
                    break;
                case AssMessageType.ResetEmpoweringTimerpvp:
                    //client and server
                    playerNumber = reader.ReadByte();
                    mPlayer = Main.player[playerNumber].GetModPlayer<AssPlayer>();
                    mPlayer.ResetEmpoweringTimer(fromServer: true);

                    //server transmits to others
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)AssMessageType.ResetEmpoweringTimerpvp);
                        packet.Write((byte)playerNumber);
                        packet.Send(playerNumber); //send to client
                    }
                    break;
                default:
                    ErrorLogger.Log("AssortedCrazyThings: Unknown Message type: " + msgType);
                    break;
            }
        }

        //Credit to jopojelly
        //makes alpha on .png textures actually properly rendered
        public static void PremultiplyTexture(Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }
    }

    public enum AssMessageType : byte
    {
        SendClientChangesVanity,
        SyncPlayerVanity,
        ConvertInertSoulsInventory,
        GitgudLoadCounters,
        GitgudChangeCounters,
        ResetEmpoweringTimerpvp
    }

    public enum GitgudType : byte
    {
        None,
        KingSlime,
        EyeOfCthulhu,
        BrainOfCthulhu,
        EaterOfWorlds,
        QueenBee,
        Skeletron,
        WallOfFlesh,
        Plantera,
    }

    public enum PetPlayerChanges : byte
    {
        //easier to copypaste when it's not capitalized
        none,
        all,
        slots,
        mechFrogCrown,
        petEyeType,
        cursedSkullType,
        youngWyvernType,
        petFishronType,
        petMoonType,
        youngHarpyType,
        abeeminationType,
        lilWrapsType,
        vampireBatType,
        pigronataType,
        queenLarvaType,
        oceanSlimeType,
        miniAntlionType,
        petGoldfishType,
        skeletronHandType,
        petCultistType,
        //ALTERNATE
        //classNameType,
    }
}
