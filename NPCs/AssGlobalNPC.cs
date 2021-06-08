using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.DroneUnlockables;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using AssortedCrazyThings.Items.Pets.CuteSlimes;

namespace AssortedCrazyThings.NPCs
{
    public class AssGlobalNPC : GlobalNPC
    {
        public bool shouldSoulDrop = false;
        public bool sentWyvernPacket = false;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            shouldSoulDrop = false;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //Other pets
            if (npc.type == NPCID.Antlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.WalkingAntlion)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MiniAntlionItem>(), chanceDenominator: 75));
            }
            else if (npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GobletItem>(), chanceDenominator: 200));
            }
            else if (npc.type == NPCID.DarkMummy || npc.type == NPCID.LightMummy || npc.type == NPCID.Mummy)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LilWrapsItem>(), chanceDenominator: 75));
            }
            else if (npc.type == NPCID.RainbowSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RainbowSlimeItem>(), chanceDenominator: 4));
            }
            else if (npc.type == NPCID.IlluminantSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IlluminantSlimeItem>(), chanceDenominator: 100));
            }

            //Boss pets

            else if (npc.type == NPCID.KingSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrinceSlimeItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObservingEyeItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.BrainofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrainofConfusionItem>(), chanceDenominator: 10));
            }
            else if (npc.boss && Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetEaterofWorldsItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.QueenBee)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QueenLarvaItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkeletronHandItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WallFragmentItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.TheDestroyer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetDestroyerItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.SkeletronPrime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkeletronPrimeHandItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
            {
                LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
                leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TinyTwinsItem>(), chanceDenominator: 10));
                npcLoot.Add(leadingConditionRule);
            }
            else if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CuteSlimeQueenItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.Plantera)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetPlanteraItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.Golem)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetGolemHeadItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetFishronItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.HallowBoss)
            {
                //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetEmpressItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.CultistBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetCultistItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TrueObservingEyeItem>(), chanceDenominator: 10));
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.TheDestroyer)
            {
                AssUtils.DropItemInstanced(npc, npc.Center, npc.Size, ModContent.ItemType<DroneParts>(),
                    condition: delegate (NPC n, Player player)
                    {
                        return !DroneController.AllUnlocked(player);
                    });
            }

            //Soul spawn from dead enemies while harvester alive

            if (shouldSoulDrop)
            {
                if (npc.type != ModContent.NPCType<DungeonSoul>())
                {
                    int soulType = ModContent.NPCType<DungeonSoul>();

                    //NewNPC starts looking for the first !active from 0 to 200
                    int soulID = NPC.NewNPC((int)npc.position.X + DungeonSoulBase.wid / 2, (int)npc.position.Y + DungeonSoulBase.hei / 2, soulType); //Spawn coords are actually the tile where its supposed to spawn on
                    Main.npc[soulID].timeLeft = DungeonSoulBase.SoulActiveTime;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, soulID);
                    }
                }
            }

            //Other

            if (!AssWorld.downedHarvester && !AssWorld.droppedHarvesterSpawnItemThisSession)
            {
                int index = npc.FindClosestPlayer();
                if (index != -1)
                {
                    Player player = Main.player[index];
                    if (player.ZoneDungeon && !player.HasItem(ModContent.ItemType<IdolOfDecay>()) && !AssUtils.AnyNPCs(AssortedCrazyThings.harvesterTypes))
                    {
                        if (Main.rand.NextBool(200))
                        {
                            Item.NewItem(npc.getRect(), ModContent.ItemType<IdolOfDecay>());
                            //To prevent the item dropping more than once in a single game instance if boss is not defeated
                            AssWorld.droppedHarvesterSpawnItemThisSession = true;
                        }
                    }
                }
            }

            GitgudData.Reset(npc);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.PartyGirl && NPC.downedSlimeKing)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<SlimeBeaconItem>());
                nextSlot++;
            }
        }

        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (Main.rand.NextBool(4))
            {
                shop[nextSlot] = ModContent.ItemType<SuspiciousNuggetItem>();
                nextSlot++;
            }
        }

        #region Wyvern Campfire

        public const short fadeTimer = 254;
        public short fadeTimerCount = 0;

        private void KillInstantly(NPC npc)
        {
            // These 3 lines instantly kill the npc without showing damage numbers, dropping loot, or playing DeathSound. Use this for instant deaths
            npc.life = 0;
            //npc.HitEffect();
            npc.active = false;
            npc.netUpdate = true;
            SoundEngine.PlaySound(SoundID.NPCDeath16, npc.position); // plays a fizzle sound
        }

        private bool SlowDown(ref NPC npc)
        {
            //returns true if after SlowDown it still moves, false if it stands still
            bool isMoving = !(npc.velocity.X == 0 && npc.velocity.Y == 0);
            if (isMoving)
            {
                if (npc.velocity.X > 4f || npc.velocity.X < -4f)
                {
                    npc.velocity.X *= 0.9f;
                }
                else
                {
                    if (npc.velocity.X > 0.3f)
                    {
                        npc.velocity.X -= 0.5f;
                    }
                    else if (npc.velocity.X < -0.3f)
                    {
                        npc.velocity.X += 0.5f;
                    }
                }

                if (npc.velocity.Y > 4f || npc.velocity.Y < -4f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                else
                {
                    if (npc.velocity.Y > 0.3f)
                    {
                        npc.velocity.Y -= 0.5f;
                    }
                    else if (npc.velocity.Y < -0.3f)
                    {
                        npc.velocity.Y += 0.5f;
                    }
                }

                if ((npc.velocity.X > 0f && npc.velocity.X < 0.3f) || (npc.velocity.X < 0f && npc.velocity.X > -0.3f))
                {
                    npc.velocity.X = 0f;
                }
                if ((npc.velocity.Y > 0f && npc.velocity.Y < 0.3f) || (npc.velocity.Y < 0f && npc.velocity.Y > -0.3f))
                {
                    npc.velocity.Y = 0f;
                }
                //Main.NewText("X" + npc.velocity.X);
                //Main.NewText("Y" + npc.velocity.Y);

                //update isMoving again
                isMoving = !(npc.velocity.X == 0 && npc.velocity.Y == 0);
            }

            return isMoving;
        }

        private void QuickWyvernDust(Vector2 pos, Color color, float fadeIn, float chance)
        {
            if (Main.rand.NextFloat() < chance)
            {
                int type = 15;
                Dust dust = Dust.NewDustDirect(pos, 4, 4, type, 0f, 0f, 120, color, 2f);
                dust.position = pos;
                dust.velocity = new Vector2(Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7));
                dust.fadeIn = fadeIn; //3f
                dust.noLight = true;
                dust.noGravity = true;
            }
        }

        public Vector2 RotToNormal(float rotation)
        {
            return new Vector2((float)Math.Sin(rotation), (float)-Math.Cos(rotation));
        }

        public void FadeAway(ref short fadeTimerCount, ref NPC npc)
        {
            //14 segments each 42 coordinates apart == 580x580 rect around center of wyvern head
            Rectangle rect = new Rectangle((int)npc.Center.X - 580, (int)npc.Center.Y - 580, 2 * 580, 2 * 580);
            fadeTimerCount++;
            for (short j = 0; j < Main.maxNPCs; j++)
            {
                NPC other = Main.npc[j];
                if (other.active && (other.type > NPCID.WyvernHead && other.type <= NPCID.WyvernTail)/* && NPCID.WyvernHead != Main.npc[j].type*/)
                {
                    if (rect.Intersects(other.Hitbox))
                    {
                        QuickWyvernDust(other.Center, Color.White, (float)(fadeTimerCount / 50f), 0.5f);
                        other.GetGlobalNPC<AssGlobalNPC>().fadeTimerCount = fadeTimerCount;
                    }
                }
            }

            //dust infront of the head (because it so long)
            Vector2 normal = 60 * RotToNormal(npc.rotation);
            QuickWyvernDust(npc.Center + normal, Color.White, (float)(fadeTimerCount / 50f), 0.5f);
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (fadeTimerCount > 0)
            {
                if (npc.type >= NPCID.WyvernHead && npc.type <= NPCID.WyvernTail)
                {
                    return drawColor * ((float)(fadeTimer - fadeTimerCount) / 255f);
                }
            }
            return base.GetAlpha(npc, drawColor);
        }

        public override bool PreAI(NPC npc)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                //this section of code doesn't run on the server anyway cause wyvernCampfire only gets set on LocalPlayer
                if (npc.type == NPCID.WyvernHead && Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].GetModPlayer<AssPlayer>().wyvernCampfire)
                {
                    if (!sentWyvernPacket && Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        sentWyvernPacket = true;
                        ModPacket packet = Mod.GetPacket();
                        packet.Write((byte)AssMessageType.WyvernCampfireKill);
                        packet.Write(npc.whoAmI);
                        packet.Send();
                    }
                    else if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        if (!SlowDown(ref npc))
                        {
                            if (fadeTimerCount <= fadeTimer)
                            {
                                FadeAway(ref fadeTimerCount, ref npc);
                            }
                            else
                            {
                                fadeTimerCount = 0;
                                KillInstantly(npc);
                            }
                            return false;
                        }
                        else
                        {
                            fadeTimerCount = 0;
                        }
                    }
                    //id 87 == head -> id 92 == tail
                }
            }
            return true;
        }
        #endregion
    }
}
