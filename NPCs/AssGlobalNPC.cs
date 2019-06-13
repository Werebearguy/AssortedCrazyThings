using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items;
using Microsoft.Xna.Framework;
using System;
using AssortedCrazyThings.Base;

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

        public override void NPCLoot(NPC npc)
        {
            //Other pets

            if (npc.type == NPCID.Antlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.WalkingAntlion)
            {
                if (Main.rand.NextBool(75)) Item.NewItem(npc.getRect(), mod.ItemType<MiniAntlionItem>());
            }

            if (npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior)
            {
                if (Main.rand.NextBool(200)) Item.NewItem(npc.getRect(), mod.ItemType<GobletItem>());
            }

            if (npc.type == NPCID.DarkMummy || npc.type == NPCID.LightMummy || npc.type == NPCID.Mummy)
            {
                if (Main.rand.NextBool(75)) Item.NewItem(npc.getRect(), mod.ItemType<LilWrapsItem>());
            }

            if (npc.type == NPCID.RainbowSlime)
            {
                if (Main.rand.NextBool(4)) Item.NewItem(npc.getRect(), mod.ItemType<RainbowSlimeItem>());
            }

            if (npc.type == NPCID.IlluminantSlime)
            {
                if (Main.rand.NextBool(100)) Item.NewItem(npc.getRect(), mod.ItemType<IlluminantSlimeItem>());
            }

            //Boss pets

            if (npc.type == NPCID.KingSlime)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PrinceSlimeItem>());
            }

            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<ObservingEyeItem>());
            }

            if (npc.type == NPCID.BrainofCthulhu)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<BrainofConfusion>());
            }

            if (Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) != -1 && npc.boss)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetEaterofWorldsItem>());
            }

            if (npc.type == NPCID.QueenBee)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<QueenLarvaItem>());
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<SkeletronHandItem>());
            }

            if (npc.type == NPCID.WallofFlesh)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<WallFragmentItem>());
            }

            if (npc.type == NPCID.TheDestroyer)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetDestroyerItem>());

                npc.DropItemInstanced(npc.Center, npc.Size, mod.ItemType<DroneParts>());
            }

            if (npc.type == NPCID.SkeletronPrime)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<SkeletronPrimeHandItem>());
            }

            if (npc.type == NPCID.Retinazer && !NPC.AnyNPCs(NPCID.Spazmatism) || npc.type == NPCID.Spazmatism && !NPC.AnyNPCs(NPCID.Retinazer))
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<TinyTwinsItem>());
            }

            if (npc.type == NPCID.Plantera)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetPlanteraItem>());
            }

            if (npc.type == NPCID.Golem)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetGolemHeadItem>());
            }

            if (npc.type == NPCID.DukeFishron)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetFishronItem>());
            }

            if (npc.type == NPCID.CultistBoss)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetCultistItem>());
            }

            if (npc.type == NPCID.MoonLordCore)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<TrueObservingEyeItem>());
            }

            //Soul spawn from dead enemies while harvester alive

            if (shouldSoulDrop)
            {
                if (npc.type != mod.NPCType<DungeonSoul>())
                {
                    int soulType = mod.NPCType<DungeonSoul>();

                    //NewNPC starts looking for the first !active from 0 to 200
                    int soulID = NPC.NewNPC((int)npc.position.X + DungeonSoulBase.wid / 2, (int)npc.position.Y + DungeonSoulBase.hei / 2, soulType); //Spawn coords are actually the tile where its supposed to spawn on
                    Main.npc[soulID].timeLeft = DungeonSoulBase.SoulActiveTime;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(23, -1, -1, null, soulID);
                    }
                }
            }

            //Other

            if (!AssWorld.downedHarvester && !AssWorld.droppedHarvesterSpawnItemThisSession)
            {
                Player player = Main.player[npc.FindClosestPlayer()];
                if (player.ZoneDungeon && !player.HasItem(mod.ItemType<IdolOfDecay>()) && !AssUtils.AnyNPCs(AssWorld.harvesterTypes))
                {
                    if (Main.rand.NextBool(200))
                    {
                        Item.NewItem(npc.getRect(), mod.ItemType<IdolOfDecay>());
                        //To prevent the item dropping more than once in a single game instance if boss is not defeated
                        AssWorld.droppedHarvesterSpawnItemThisSession = true;
                    }
                }
            }

            GitgudData.Reset(npc);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.PartyGirl && NPC.downedSlimeKing)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType<SlimeBeaconItem>());
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
            Main.PlaySound(SoundID.NPCDeath16, npc.position); // plays a fizzle sound
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
                Dust dust = Main.dust[Dust.NewDust(pos, 4, 4, type, 0f, 0f, 120, color, 2f)];
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
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && (Main.npc[j].type > NPCID.WyvernHead && Main.npc[j].type <= NPCID.WyvernTail)/* && NPCID.WyvernHead != Main.npc[j].type*/)
                {
                    if (rect.Intersects(Main.npc[j].Hitbox))
                    {
                        QuickWyvernDust(Main.npc[j].Center, Color.White, (float)(fadeTimerCount / 50f), 0.5f);
                        Main.npc[j].GetGlobalNPC<AssGlobalNPC>().fadeTimerCount = fadeTimerCount;
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
                        ModPacket packet = mod.GetPacket();
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
