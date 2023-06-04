using AssortedCrazyThings.Base;
using AssortedCrazyThings.BossBars;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Accessories.Useful;
using AssortedCrazyThings.Items.Consumables;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items.VanityArmor;
using AssortedCrazyThings.NPCs.DropConditions;
using AssortedCrazyThings.NPCs.DropRules;
using AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.NPCs.Harvester
{
	[AutoloadBossHead]
	[Content(ContentType.Bosses)]
	[LegacyName("Harvester")]
	public class HarvesterBoss : AssNPC
	{
		public class AIStats
		{
			public float MaxHP { get; init; }

			public int FireballInterval { get; init; } = 50;

			public int FireballDuration { get; init; } = 300;

			public float FireballSpeed { get; init; } = 18;

			public bool AlwaysShootFireballs { get; init; }

			public int SwoopWaitTime { get; init; } = 80;

			public AIStats()
			{

			}
		}

		public const int SpawnedSoulCount = 25;
		public const int FrameCountHorizontal = 4;
		public const int FrameCountVertical = 5;
		public const int FrameWidth = 314; //Old sprite 470
		public const int FrameHeight = 196; //Old sprite 254

		public readonly static int talonDamage = 36;
		public readonly static int wid = 96;
		public readonly static int hei = 96;

		//Offsets from the center
		public float talonOffsetLeftX = 0;
		public float talonOffsetRightX = 0;
		public float talonOffsetY = 0;

		public static int talonDirectionalOffset = 10;

		public Color overlayColor = Color.White;

		public static Asset<Texture2D> SheetAsset { get; private set; }
		public static Asset<Texture2D> WingsAsset { get; private set; }
		public static Asset<Texture2D> MeleeIndicatorAsset { get; private set; }

		public static Dictionary<int, AIStats> reviveToAIStats { get; private set; }

		public AIStats GetAIStats()
		{
			if (reviveToAIStats.TryGetValue(RevivesDone, out AIStats stats))
			{
				return stats;
			}

			return reviveToAIStats[0];
		}

		public override void Load()
		{
			if (!Main.dedServ)
			{
				SheetAsset = ModContent.Request<Texture2D>(Texture + "_Sheet");
				WingsAsset = ModContent.Request<Texture2D>(Texture + "_Wings");

				const short warriorEmblem = ItemID.WarriorEmblem;
				Main.instance.LoadItem(warriorEmblem);
				MeleeIndicatorAsset = TextureAssets.Item[warriorEmblem];
			}

			reviveToAIStats = new Dictionary<int, AIStats>()
			{
				[0] = new AIStats
				{
					MaxHP = 1f,
				},
				[1] = new AIStats
				{
					MaxHP = 0.6f,
					FireballDuration = 600,
					FireballSpeed = 15,
					FireballInterval = 35,
				},
				[2] = new AIStats
				{
					MaxHP = 0.3f,
					FireballDuration = 0,
					FireballInterval = 60,
					SwoopWaitTime = 34,
					AlwaysShootFireballs = true,
				},
			};
		}

		public override void Unload()
		{
			SheetAsset = null;
			WingsAsset = null;
			MeleeIndicatorAsset = null;

			reviveToAIStats = null;
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1; //Dummy texture and frame count, real sheet is two-dimensional

			NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
			NPCID.Sets.SpawnFromLastEmptySlot[NPC.type] = true;
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true; //Allows NPC.SpawnOnPlayer to work

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				CustomTexturePath = "AssortedCrazyThings/NPCs/Harvester/HarvesterBoss_Bestiary",
				Position = new Vector2(-9, 18), //Position on the icon
				PortraitPositionXOverride = 0, //Position on the portrait when clicked on
				PortraitPositionYOverride = 20,
				SpriteDirection = -1,
				PortraitScale = 1.4f,
				Frame = 0,
			};
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;

			NPCID.Sets.DebuffImmunitySets[NPC.type] = new NPCDebuffImmunityData()
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Confused,
					BuffID.Poisoned,
					BuffID.Venom,
					BuffID.OnFire,
					BuffID.CursedInferno,
				}
			};
		}

		public override void SetDefaults()
		{
			//npc.SetDefaults(NPCID.QueenBee);
			NPC.boss = true;
			NPC.npcSlots = 10f; //takes 10 npc slots, so no other npcs can spawn during the fight
								//actual body hitbox
			NPC.width = wid;
			NPC.height = hei;
			NPC.damage = talonDamage; //just a dummy value, it deals no contact damage. Used for projectile spawns
			NPC.defense = 15;
			NPC.lifeMax = 1750;
			NPC.scale = 1f;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 10);
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1; //91;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.lavaImmune = true;
			NPC.alpha = 255;
			NPC.SpawnWithHigherTime(30);

			NPC.BossBar = ModContent.GetInstance<HarvesterBossBar>();

			DrawOffsetY = 36; //By default, hitbox aligns with bottom center of the frame. This pushes it up
			Music = MusicID.Boss1;
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment);
			NPC.damage = (int)(NPC.damage * 1.1f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write7BitEncodedInt(AI_Counter);
			writer.Write(activeTalonIndex);
			writer.Write(RevivesDone);

			BitsByte flags = new BitsByte();
			flags[0] = allowAnimationSpeedAdjustment;
			writer.Write(flags);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AI_Counter = reader.Read7BitEncodedInt();
			activeTalonIndex = reader.ReadByte();
			RevivesDone = reader.ReadByte();

			BitsByte flags = reader.ReadByte();
			allowAnimationSpeedAdjustment = flags[0];
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			//Body won't deal contact damage to players
			return false;
		}

		public override bool CanHitNPC(NPC target)
		{
			//Body won't deal contact damage to townies/critters
			return false;
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			if (IsReviving)
			{
				return false;
			}

			return base.CanBeHitByItem(player, item);
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (IsReviving)
			{
				return false;
			}

			return base.CanBeHitByProjectile(projectile);
		}

		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (VulnerableToMelee)
			{
				//Take three times the damage from melee swings
				modifiers.FinalDamage *= 3;
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (VulnerableToMelee && projectile.ownerHitCheck && projectile.CountsAsClass(DamageClass.Melee))
			{
				//Take two times the damage from melee projectiles that require direct line of sight (things like arkhalis, spears, etc)
				modifiers.FinalDamage *= 2;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = -NPC.direction;

			NPC.frame.Width = FrameWidth;
			NPC.frame.Height = FrameHeight;
			NPC.frame.X = AI_Animation * FrameWidth;
			int lastFrame = GetLastFrame(AI_Animation);

			if (AI_Intro < 255 && !NPC.IsABestiaryIconDummy)
			{
				//Fixed transform frame
				NPC.frame.Y = FrameHeight * lastFrame;
				NPC.frameCounter = 8;
				return;
			}

			if (AI_Animation != Animation_Swoop)
			{
				NPC.LoopAnimation(FrameHeight, 6, endFrame: lastFrame);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			drawColor = NPC.GetAlpha(drawColor);

			//Vanilla draw code
			float someNumModdedNPCsArentUsing = 0f;
			float drawOffsetY = Main.NPCAddHeight(NPC);
			Asset<Texture2D> asset = SheetAsset;
			Texture2D texture = asset.Value;

			Vector2 halfSize = new Vector2(asset.Width() / FrameCountHorizontal / 2, asset.Height() / FrameCountVertical / 2);

			SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 halfSizeOff = halfSize * NPC.scale;
			Vector2 textureOff = new Vector2(asset.Width() * NPC.scale / FrameCountHorizontal / 2f, asset.Height() * NPC.scale / (float)FrameCountVertical);
			Vector2 drawPosition = new Vector2(NPC.Center.X, NPC.Bottom.Y + drawOffsetY + someNumModdedNPCsArentUsing + NPC.gfxOffY + 4f);
			Vector2 finalDrawPos = drawPosition + halfSizeOff - textureOff - screenPos;
			spriteBatch.Draw(texture, finalDrawPos, NPC.frame, drawColor, NPC.rotation, halfSize, NPC.scale, effects, 0f);

			asset = WingsAsset;
			texture = asset.Value;
			Color glowmaskColor = NPC.GetAlpha(Color.White);
			spriteBatch.Draw(texture, finalDrawPos, NPC.frame, glowmaskColor, NPC.rotation, halfSize, NPC.scale, effects, 0f);

			if (fadingAuraTimer > 0 && fadingAuraTimerMax > 0)
			{
				asset = SheetAsset;
				texture = asset.Value;
				float scale = fadingAuraTimer / (float)fadingAuraTimerMax;
				float scaleInverse = 1f - scale;
				Color auraColor = drawColor * scale * fadingAuraAlphaIntensity;
				Color auraWingColor = glowmaskColor * scale * fadingAuraAlphaIntensity;
				float auraScale = NPC.scale + scaleInverse * fadingAuraScaleIntensity;

				spriteBatch.Draw(texture, finalDrawPos, NPC.frame, auraColor, NPC.rotation, halfSize, auraScale, effects, 0f);

				asset = WingsAsset;
				texture = asset.Value;
				spriteBatch.Draw(texture, finalDrawPos, NPC.frame, auraWingColor, NPC.rotation, halfSize, auraScale, effects, 0f);
			}

			//TODO figure out a better way to display it
			/*
            if (!displayedMeleeIndicatorOnce && displayMeleeIndicator && StateToDisplayMeleeIndicator)
            {
                asset = MeleeIndicatorAsset;
                texture = asset.Value;
                Rectangle frame = asset.Frame();
                Vector2 origin = frame.Size() / 2;
                Vector2 dirOff = new Vector2(NPC.direction * (int)(((NPC.width / 2) * NPC.scale) + 30), 0); //Show left/right of the center

                Color color = Color.White;

                if (VulnerableToMelee)
                {
                    spriteBatch.Draw(texture, NPC.Center + origin + dirOff - screenPos, frame, Color.Red, 0, origin, 1.6f, effects, 0f);
                }
                else
                {
                    float sin = (float)(Math.Sin((Main.GameUpdateCount % 60 / 60f) * MathHelper.TwoPi) + 1) * 0.5f;
                    color.A = (byte)(255 * sin);
                }

                spriteBatch.Draw(texture, NPC.Center + origin + dirOff - screenPos, frame, color, 0, origin, 1.5f, effects, 0f);
            }
            */

			return false;
		}

		public override Color? GetAlpha(Color drawColor)
		{
			if (NPC.IsABestiaryIconDummy)
			{
				//This is required because we have NPC.alpha = 255 in SetDefaults, in the bestiary it would look transparent
				return NPC.GetBestiaryEntryColor();
			}
			return Color.Lerp(drawColor, Color.White, 0.4f).MultiplyRGBA(overlayColor) * NPC.Opacity;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			int idolOfDecay = ModContent.ItemType<IdolOfDecay>();
			LeadingConditionRule noHasItemWithBankRule = new LeadingConditionRule(new NoHasItemWithBankCondition(idolOfDecay));
			noHasItemWithBankRule.OnSuccess(ItemDropRule.Common(idolOfDecay));
			npcLoot.Add(noHasItemWithBankRule);

			//Souls are NOT spawned in the bag
			int itemType = ModContent.ItemType<CaughtDungeonSoulFreed>();
			var parameters = new DropOneByOne.Parameters()
			{
				ChanceNumerator = 1,
				ChanceDenominator = 1,
				MinimumStackPerChunkBase = 1,
				MaximumStackPerChunkBase = 1,
				MinimumItemDropsCount = SpawnedSoulCount,
				MaximumItemDropsCount = SpawnedSoulCount,
			};
			npcLoot.Add(new DropOneByOne(itemType, parameters));

			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HarvesterTreasureBag>()));

			//Relic and trophy are NOT spawned in the bag
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HarvesterTrophyItem>(), chanceDenominator: 10));
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<HarvesterRelicItem>()));

			npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<PetHarvesterItem>(), 4));

			//Drop one of three sigils, one random one per player
			var sigils = new int[] { ModContent.ItemType<SigilOfTheTalon>(), ModContent.ItemType<SigilOfTheBeak>(), ModContent.ItemType<SigilOfTheWing>() };
			var sigilRule = new OneFromOptionsPerPlayerOnPlayerRule(options: sigils);
			npcLoot.Add(sigilRule);

			//All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.Bone, minimumDropped: 40, maximumDropped: 60));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DesiccatedLeather>(), minimumDropped: 2, maximumDropped: 2));

			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SoulHarvesterMask>(), chanceDenominator: 7));

			//Finally add the leading rule
			npcLoot.Add(notExpertRule);
		}

		public override void OnKill()
		{
			NPC.SetEventFlagCleared(ref AssWorld.downedHarvester, -1);

			AssWorld.SoulHarvesterDeath.Announce();

			//"convert" NPC souls
			ConvertSouls();
		}

		public override bool CheckDead()
		{
			if (RevivesDone < Revive_Count)
			{
				//Main.NewText(Main.GameUpdateCount + " checkdead false " + (revivesDone < Revive_Count));
				NPC.life = NPC.lifeMax;
				NPC.defense = Revive_Defense;

				if (AI_State != State_Weakened)
				{
					NPC.dontTakeDamage = true; //Set for one tick to prevent damage in same tick from applying
					RevivesDone++;
					//Main.NewText("revive #" + revivesDone);

					if (Main.netMode != NetmodeID.Server)
					{
						var entitySource = NPC.GetSource_Death();
						for (int i = 0; i < 6; i++)
						{
							Gore.NewGore(entitySource, Main.rand.NextVector2FromRectangle(NPC.getRect()), NPC.velocity, Mod.Find<ModGore>("SoulHarvesterGore_05").Type, 1f);
						}
					}

					SetState(State_Weakened);
					SetFrame(0);

					for (int i = 0; i < NPC.maxBuffs; i++)
					{
						//Clear potential DoT debuffs
						NPC.DelBuff(i);
					}
				}

				return false;
			}

			//Main.NewText("checkdead skip " + (revivesDone < Revive_Count));
			return base.CheckDead();
		}

		private void ConvertSouls()
		{
			int npcTypeOld = ModContent.NPCType<DungeonSoul>();
			int npcTypeNew = ModContent.NPCType<DungeonSoulFreed>();  //version that doesn't get eaten by harvesters

			int itemTypeOld = ModContent.ItemType<CaughtDungeonSoul>();
			int itemTypeNew = ModContent.ItemType<CaughtDungeonSoulFreed>(); //version that is used in crafting
			for (short j = 0; j < Main.maxNPCs; j++)
			{
				NPC other = Main.npc[j];
				if (other.active && other.type == npcTypeOld)
				{
					other.active = false;
					int index = NPC.NewNPC(NPC.GetSource_Death(), (int)other.position.X, (int)other.position.Y, npcTypeNew);
					NPC npcnew = Main.npc[index];
					npcnew.ai[2] = Main.rand.Next(1, DungeonSoulBase.offsetYPeriod); //doesnt get synced properly to clients idk
					npcnew.timeLeft = 3600;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.SyncNPC, number: index);
					}

					//poof visual works only in singleplayer
					for (int i = 0; i < 15; i++)
					{
						Dust dust = Dust.NewDustPerfect(npcnew.Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
						dust.noLight = true;
						dust.noGravity = true;
						dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
					}
				}
			}

			//"convert" Item souls that got dropped for some reason
			int tempStackCount;
			for (int j = 0; j < Main.maxItems; j++)
			{
				Item item = Main.item[j];
				if (item.active && item.type == itemTypeOld)
				{
					tempStackCount = item.stack;
					item.SetDefaults(itemTypeNew);
					item.stack = tempStackCount;

					//poof visual
					for (int i = 0; i < 15; i++)
					{
						Dust dust = Dust.NewDustPerfect(item.Center, 59, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 1.5f)), 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
						dust.noLight = true;
						dust.noGravity = true;
						dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
					}
				}
			}

			//"convert" Item souls in inventory
			for (int j = 0; j < Main.maxPlayers; j++)
			{
				Player player = Main.player[j];
				if (player.active/* && !Main.player[j].dead*/)
				{
					AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

					if (Main.netMode == NetmodeID.Server)
					{
						SendConvertInertSoulsInventory(j);
					}
					else //singleplayer
					{
						mPlayer.ConvertInertSoulsInventory();
					}
				}
			}
		}

		private void SendConvertInertSoulsInventory(int toWho)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)AssMessageType.ConvertInertSoulsInventory);
				packet.Send(toClient: toWho);
			}
		}

		/// <summary>
		/// Sets the y on the spritesheet. use AI_Animation to set x
		/// </summary>
		/// <param name="frameY">The y</param>
		public void SetFrame(int frameY)
		{
			NPC.frame.Y = frameY * FrameHeight;
			NPC.frameCounter = 0;
		}

		/// <summary>
		/// (Counter/Timer only resets on hard state change)
		/// </summary>
		/// <param name="state">State to change to. By default, no change</param>
		/// <param name="subState">Sub-State to change to.</param>
		public void SetState(float state = State_KeepCurrent, float subState = 0f)
		{
			if (state != State_KeepCurrent)
			{
				AI_State = state;
				AI_Counter = 0;
				AI_Timer = 0;
			}

			AI_SubState = subState;
			NPC.netUpdate = true;
		}

		public static int GetLastFrame(int animation)
		{
			return animation switch
			{
				Animation_Swoop => 2,
				_ => FrameCountVertical - 1
			};
		}

		public const float State_KeepCurrent = -1f;
		public const float State_Fireball = 0f; //Basic flying, homing attack
		public const float State_SpawnedByBaby = 1f; //Special state set on spawn
		public const float State_Swoop = 2f;
		public const float State_Bombing = 3f;
		public const float State_Weakened = 4f; //Can enter from any state, special conditions

		//Substates:
		public const int Swoop_SeekStart = 0;//Second half the "U"
		public const int Swoop_Swooping = 1; //First half the "U"

		public const int Swooping_Distance = 500;
		public const int Swooping_Height = 280;
		public const int Swoop_Count = 3;
		public const int Revive_Count = 2;
		public const float Revive_MinHP = 0.1f;
		public const int Revive_Duration = 180;
		public const int Revive_Defense = 999;

		public const int Bombing_Distance = Swooping_Distance + 300;
		public const int Bombing_Height = 200;

		public const int Animation_NoHorizontal = 0;
		public const int Animation_Flight = 1;
		public const int Animation_Swoop = 2;
		public const int Animation_Bombing = 3;

		public ref float AI_State => ref NPC.ai[0];

		public ref float AI_SubState => ref NPC.ai[1];

		public ref float AI_Timer => ref NPC.ai[2];

		public int AI_Animation
		{
			get => (int)NPC.ai[3];
			set => NPC.ai[3] = value;
		}

		public ref float AI_Intro => ref NPC.localAI[0];

		public bool Initialized
		{
			get => NPC.localAI[1] == 1f;
			set => NPC.localAI[1] = value ? 1f : 0f;
		}

		public ref float Animation_Timer => ref NPC.localAI[2];

		//Synced
		public int AI_Counter
		{
			get => (int)NPC.localAI[3];
			set => NPC.localAI[3] = value;
		}

		//Synced
		private byte activeTalonIndex = byte.MaxValue;

		//Synced
		//0: no revives yet
		//1: first revive initiated/ongoing
		//...
		//Revive_Count: final possible revive initiated/ongoing
		public byte RevivesDone { get; private set; }

		public ref float ReviveProgress => ref AI_Timer;

		public bool IsReviving => AI_State == State_Weakened;

		public bool StateToDisplayMeleeIndicator => AI_State == State_Swoop && AI_Timer < 0 || VulnerableToMelee;

		public bool VulnerableToMelee => AI_State == State_Swoop && AI_SubState == Swoop_Swooping;

		//Not synced, serverside
		public int FireballTimer { get; private set; }

		//Not synced, should be clientside
		private bool displayMeleeIndicator = false;
		private bool displayedMeleeIndicatorOnce = false;

		private bool allowAnimationSpeedAdjustment = true;

		private float fadingAuraAlphaIntensity = 0f;
		private float fadingAuraScaleIntensity = 0f;
		private int fadingAuraTimerMax = 0;
		private int fadingAuraTimer = 0;

		private void SetFadingAura(int alphaTimer, float alphaIntensity = 0.8f, float scaleIntensity = 0.5f)
		{
			fadingAuraTimerMax = fadingAuraTimer = alphaTimer;
			fadingAuraAlphaIntensity = alphaIntensity;
			fadingAuraScaleIntensity = scaleIntensity;
		}

		private void HandleFadingAura()
		{
			if (fadingAuraTimer > 0)
			{
				fadingAuraTimer--;
			}
			else
			{
				fadingAuraTimerMax = 0;
				fadingAuraAlphaIntensity = 0f;
				fadingAuraScaleIntensity = 0f;
			}
		}

		public override void AI()
		{
			Lighting.AddLight(NPC.Center, 0.3f, 0.3f, 0.7f);

			if (NPC.target < 0 || NPC.target >= 255 || !Main.player[NPC.target].active || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest();
			}

			Player target = Main.player[NPC.target];

			if (target.DistanceSQ(NPC.Center) > 2000 * 2000)
			{
				NPC.TargetClosest();
			}

			HandleMeleeIndicator(target);

			HandleAnimation();

			HandleReviveVisuals();

			HandleFadingAura();

			HandleReviveTrigger();

			List<HarvesterTalon> talons = GetTalons();

			MonitorExtendedTalons(talons);

			NPC.chaseable = !IsReviving;
			NPC.dontTakeDamage = IsReviving;
			NPC.scale = 1f;

			if (!IsReviving && NPC.defense == Revive_Defense)
			{
				//Restore defense after revived
				NPC.defense = NPC.defDefense;
			}

			if (target.dead && !IsReviving)
			{
				AI_Animation = Animation_Flight;
				SetState(State_Fireball);
				NPC.velocity.Y += 0.06f;
				NPC.velocity.X *= 0.97f;
				if (NPC.timeLeft > 10)
				{
					NPC.timeLeft = 10;
				}
				return;
			}

			NPC.rotation = NPC.velocity.X * 0.02f;

			int diff = 38; //Distance offset between two talons
			int offX = -10; //Offset from the center for the left talon
			talonOffsetLeftX = offX * NPC.direction + NPC.velocity.X/* * TalonDirectionalOffset*/;
			talonOffsetRightX = (diff + offX) * NPC.direction + NPC.velocity.X/* * TalonDirectionalOffset*/;
			talonOffsetY = hei / 2 + 4 + NPC.velocity.Y;
			//Due to update order (boss updates after talons), add velocity to the offsets aswell

			if (!Initialized)
			{
				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int y = (int)(NPC.Center.Y + talonOffsetY);

					(int x, int type)[] talonTypes = new (int, int)[]
					{
						((int)(NPC.Center.X + talonOffsetLeftX), AssortedCrazyThings.harvesterTalonLeft),
						((int)(NPC.Center.X + talonOffsetRightX), AssortedCrazyThings.harvesterTalonRight)
					};

					var source = NPC.GetSource_FromAI();
					foreach (var (x, type) in talonTypes)
					{
						int index = NPC.NewNPC(source, x, y, type);
						if (index < Main.maxNPCs && Main.npc[index] is NPC talonNPC && talonNPC.ModNPC is HarvesterTalon talon)
						{
							talon.ParentWhoAmI = NPC.whoAmI;
							if (Main.netMode == NetmodeID.Server)
							{
								NetMessage.SendData(MessageID.SyncNPC, number: index);
							}
						}
					}
				}

				AI_Timer = 0;
				NPC.TargetClosest();

				if (AI_State != State_SpawnedByBaby)
				{
					SetState(State_Fireball);
				}
				Initialized = true;
			}

			//Spawned through summon item/cheated in
			if (AI_State != State_SpawnedByBaby && AI_Intro < 255)
			{
				AI_Intro += 5;
				NPC.alpha -= 5;
				if (AI_Intro >= 255)
				{
					AI_Intro = 255;
					NPC.alpha = 0;
					SetFadingAura(20, 0.8f, 0.5f);
					NPC.netUpdate = true;
				}
				return;
			}
			else
			{
				AI_Intro = 255;
				NPC.alpha = 0;
			}

			//Spawned by baby, which shows the transformation before spawning
			if (AI_State == State_SpawnedByBaby)
			{
				AI_Intro = 255;
				NPC.alpha = 0;
				SetFadingAura(20, 0.8f, 0.5f);
				SetState(State_Fireball);
				NPC.netUpdate = true;
			}

			HandleAI(target, talons);
		}

		private void MonitorExtendedTalons(List<HarvesterTalon> talons)
		{
			//Monitor talons that extend too far
			for (int i = 0; i < talons.Count; i++)
			{
				HarvesterTalon talon = talons[i];
				if (talon.AI_State == HarvesterTalon.State_Punching)
				{
					if (talon.NPC.Top.Y > NPC.Center.Y + Bombing_Height)
					{
						//Main.NewText("manually retract talon " + i);
						talon.AI_State = HarvesterTalon.State_Seek_Retract;
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendData(MessageID.SyncNPC, number: i);
						}
					}
				}
			}
		}

		private List<HarvesterTalon> GetTalons()
		{
			List<HarvesterTalon> talons = new List<HarvesterTalon>();

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];

				//If talon and belongs to self
				if (npc.active && npc.ModNPC is HarvesterTalon talon && talon.ParentWhoAmI == NPC.whoAmI)
				{
					talons.Add(talon);
				}
			}

			return talons;
		}

		bool lastEnraged = false;

		private void HandleAI(Player target, List<HarvesterTalon> talons)
		{
			float lifeRatio = NPC.life / (float)NPC.lifeMax;
			var aiStats = GetAIStats();

			bool enraged = !BabyHarvesterHandler.ValidPlayer(target); //This is delayed as per method, but player won't see it coming
			if (enraged && !lastEnraged)
			{
				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
			}
			lastEnraged = enraged;

			if (AI_State == State_Fireball || aiStats.AlwaysShootFireballs || enraged)
			{
				//Generate souls in a half circle above the boss and have them home to whatever the nearest player to them is
				FireballTimer++;
				int fireballInterval = aiStats.FireballInterval;
				float fireballSpeed = aiStats.FireballSpeed;
				if (enraged)
				{
					fireballInterval /= 3;
					fireballSpeed *= 1.5f;
				}

				if (FireballTimer % fireballInterval == 0)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 random = (-Vector2.UnitY).RotatedByRandom(MathHelper.PiOver2) * 160;
						Vector2 pos = NPC.Center + random;
						Vector2 toPlayer = target.DirectionFrom(pos);
						int damage = (int)((NPC.damage / (float)NPC.defDamage) * 16); //Fixed damage based on default
						damage = NPC.GetAttackDamage_ForProjectiles(damage, damage * 0.33f); //To compensate expert mode NPC damage + projectile damage increase
						Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, toPlayer * 1, ModContent.ProjectileType<HarvesterFracturedSoul>(), damage, 0f, Main.myPlayer, fireballSpeed);
					}
				}
			}

			if (AI_State == State_Fireball)
			{
				if (AI_Animation != Animation_Flight && AI_Animation != Animation_NoHorizontal)
				{
					AI_Animation = Animation_Flight;
				}

				AI_Timer++;

				float acceleration = 0.05f;

				Vector2 origin = new Vector2(NPC.Center.X + 6 * NPC.direction, NPC.position.Y + NPC.height * 0.8f);
				float diffX = target.Center.X - NPC.Center.X;
				float diffY = target.Center.Y - 200f - NPC.Center.Y; //300f
				float length = (float)Math.Sqrt(diffX * diffX + diffY * diffY);

				if (!Collision.CanHit(new Vector2(origin.X, origin.Y - 30f), 1, 1, target.position, target.width, target.height))
				{
					acceleration = 0.1f;
					diffX = target.Center.X - NPC.Center.X;
					diffY = target.Center.Y - NPC.Center.Y;

					//WHEN NO DIRECT CAN HIT LINE

					if (Math.Abs(NPC.velocity.X) < 32)
					{
						if (NPC.velocity.X < diffX)
						{
							NPC.velocity.X += acceleration;
							if (NPC.velocity.X < 0f && diffX > 0f)
							{
								NPC.velocity.X += acceleration * 2.5f; //1f all
							}
						}
						else if (NPC.velocity.X > diffX)
						{
							NPC.velocity.X -= acceleration;
							if (NPC.velocity.X > 0f && diffX < 0f)
							{
								NPC.velocity.X -= acceleration * 2.5f;
							}
						}
					}
					if (Math.Abs(NPC.velocity.Y) < 32)
					{
						if (NPC.velocity.Y < diffY)
						{
							NPC.velocity.Y += acceleration;
							if (NPC.velocity.Y < 0f && diffY > 0f)
							{
								NPC.velocity.Y += acceleration * 2.5f;
							}
						}
						else if (NPC.velocity.Y > diffY)
						{
							NPC.velocity.Y -= acceleration;
							if (NPC.velocity.Y > 0f && diffY < 0f)
							{
								NPC.velocity.Y -= acceleration * 2.5f;
							}
						}
					}
				}
				else if (length > 100f)
				{
					NPC.TargetClosest();
					if (Math.Abs(NPC.velocity.X) < 32)
					{
						if (NPC.velocity.X < diffX)
						{
							NPC.velocity.X += acceleration;
							if (NPC.velocity.X < 0f && diffX > 0f)
							{
								NPC.velocity.X += acceleration * 2f; //2f all
							}
						}
						else if (NPC.velocity.X > diffX)
						{
							NPC.velocity.X -= acceleration;
							if (NPC.velocity.X > 0f && diffX < 0f)
							{
								NPC.velocity.X -= acceleration * 2f;
							}
						}
					}
					if (Math.Abs(NPC.velocity.Y) < 32)
					{
						if (NPC.velocity.Y < diffY)
						{
							NPC.velocity.Y += acceleration;
							if (NPC.velocity.Y < 0f && diffY > 0f)
							{
								NPC.velocity.Y += acceleration * 2f;
							}
						}
						else if (NPC.velocity.Y > diffY)
						{
							NPC.velocity.Y -= acceleration;
							if (NPC.velocity.Y > 0f && diffY < 0f)
							{
								NPC.velocity.Y -= acceleration * 2f;
							}
						}
					}
				}

				float lifeRatioClamped = Utils.Remap(lifeRatio, 0, 1, 0.666f, 1);
				float timeToNextState = aiStats.FireballDuration * lifeRatioClamped;

				if (Main.expertMode && RevivesDone == Revive_Count) //Final revive
				{
					timeToNextState = 0;
				}

				if (AI_Timer > timeToNextState)
				{
					AI_Timer = 0;
					SetState(State_Swoop);
					NPC.TargetClosest(false);
				}
			}
			else if (AI_State == State_Swoop)
			{
				//Main.NewText("substat: " + AI_SubState);
				//Main.NewText("counter: " + AI_Counter);
				//Main.NewText("timer: " + AI_Timer);

				//AI_Counter meaning:
				//0: first swoop: decide direction (sign) and set 1
				//absolute value: swoop count
				//sign: swoop direction

				//After passing the player during start, change to seekStart

				int count = Math.Abs(AI_Counter);
				bool keepSwooping = count <= Swoop_Count;

				if (AI_SubState == Swoop_SeekStart)
				{
					float speed = 10;
					const float minInertia = 6;
					float inertia = minInertia;
					float lifeRatioClamped = Utils.Remap(lifeRatio, 0, 1, 0.6f, 1);

					if (AI_Counter != 0 && AI_Animation == Animation_Swoop)
					{
						float swoopPostTime = aiStats.SwoopWaitTime * lifeRatioClamped;

						//This means a swoop has already taken place, and the "post-swoop" frame should be displayed for a while
						AI_Timer++;
						inertia += (swoopPostTime - AI_Timer) / swoopPostTime * 16; //Increase inertia during the "post-swoop" so that it follows the U shape for a bit at the bottom

						inertia = Math.Max(inertia, minInertia);

						if (AI_Timer > 8 * lifeRatioClamped)
						{
							SetFrame(2); //Post-swoop
						}
						else if (AI_Timer > swoopPostTime)
						{
							AI_Timer = 0;
							AI_Animation = Animation_Flight;
							SetFrame(4); //Regular flight, frame after the post-swoop in regards to wing position
						}
					}

					//Find location to start swoop on the side of the player closer to the boss
					Vector2 toPlayer = target.DirectionFrom(NPC.Bottom); //Talons should hit center of player
					bool leftOfPlayer = toPlayer.X > 0;

					int targetY = keepSwooping ? Swooping_Height : Bombing_Height;

					Vector2 start = new Vector2(Swooping_Distance, -targetY);
					if (AI_Counter == 0)
					{
						//First swoop
						//Initial start is "right of player" meaning initial dir should be "left towards player"
						AI_Counter = -1;
						if (!leftOfPlayer)
						{
							//Flip both if NPC is "left of player"
							start.X *= -1;
							AI_Counter *= -1;
						}
					}
					else
					{
						//Consecutive swoops
						if (AI_Counter < 0) //current dir is "left towards player"
						{
							//Flip
							start.X *= -1;
						}
						//No flip required otherwise
					}

					//Apply absolute vector
					start += target.Center;

					Vector2 toStart = start - NPC.Center;

					if (toStart.LengthSquared() <= speed * speed)
					{
						if (AI_Timer >= 0)
						{
							//Kickstart timer
							AI_Timer = -1;
						}

						NPC.direction = leftOfPlayer.ToDirectionInt();
						AI_Animation = Animation_NoHorizontal;
						NPC.Center = start;
						if (NPC.velocity.LengthSquared() > 1)
						{
							NPC.velocity = NPC.velocity.SafeNormalize(Vector2.UnitY);
						}
						NPC.velocity *= 0.95f;
					}
					else
					{
						toStart = toStart.SafeNormalize(Vector2.UnitY);
						toStart *= speed;

						NPC.velocity = (NPC.velocity * (inertia - 1) + toStart) / inertia;

						if (NPC.direction == 1 && NPC.velocity.X < 0 ||
							NPC.direction == -1 && NPC.velocity.X > 0)
						{
							//Swap direction if not locked into position yet but flew past it
							NPC.direction *= -1;
						}
					}

					if (AI_Timer < 0)
					{
						AI_Timer--;

						if (!keepSwooping)
						{
							//Last seek stage before bombing: put talons out
							AI_Animation = Animation_Bombing;
						}

						float swoopPostTime = aiStats.SwoopWaitTime * lifeRatioClamped;
						if (AI_Timer < -swoopPostTime)
						{
							AI_Timer = 0;

							if (keepSwooping)
							{
								//This does keep whatever animation was previously happening, but changes it on swoop
								SetState(subState: Swoop_Swooping);
								NPC.direction = leftOfPlayer.ToDirectionInt();
								AI_Animation = Animation_Swoop;
								SetFrame(0);
							}
							else
							{
								SetState(State_Bombing);
								NPC.direction = leftOfPlayer.ToDirectionInt();
								AI_Animation = Animation_Bombing;
								//Frames match, so don't need to call SetFrame
							}
						}
					}
				}
				else if (AI_SubState == Swoop_Swooping)
				{
					if (AI_Timer == 0)
					{
						SoundEngine.PlaySound(SoundID.Roar with { Pitch = 0.15f }, NPC.Center);
					}

					int xSpeed = 12;
					AI_Timer += xSpeed; //AI_Timer is memory of distance traversed

					//Keep constant speed towards direction (not player)
					float xDir = NPC.direction * xSpeed;

					//Go down towards straight line smoothly, fast at first (to create half of "u" shape)
					//Number from 0 (start) to 1 (end, reached swoop location)

					float xProgress = AI_Timer / Swooping_Distance;
					float yProgress = EaseOutLinear(xProgress, 1f);

					//Used on VELOCITY, meaning this is a parabola position wise
					float EaseOutLinear(float x, float y0 = 1f)
					{
						//Start at y0, finish at 0
						float m = -y0;
						return m * x + y0;
					}

					//float EaseOutCirc(float x)
					//{
					//    //Start fast, but slow down slowly 30% through
					//    return (float)Math.Sqrt(1 - Math.Pow((double)x - 1, 2));
					//}

					int ySpeed = 10;
					float yDir = ySpeed * yProgress;

					if (xProgress >= 0.95f)
					{
						//About to hit player/finish swoop down
						SetFrame(1);
					}

					//bool passedPlayer = (NPC.Center.X > target.Center.X).ToDirectionInt() == NPC.direction;
					if (xProgress >= 1f/* || passedPlayer*/)
					{
						AI_Timer = 0;

						//Increment count for next swoop
						//Flip dir
						count++;
						int dir = Math.Sign(AI_Counter);
						dir *= -1;
						AI_Counter = dir * count;

						if (keepSwooping)
						{
							SetState(subState: Swoop_SeekStart);
						}
						else
						{
							SetState(State_Fireball);

							//Go into the state that picks an attack
						}
					}
					else
					{
						NPC.velocity = new Vector2(xDir, yDir);

						if (xProgress >= 0.5f)
						{
							//Course correct on second half of dive. For players trying to easily dodge the talons by stepping left/right 
							Vector2 toPlayer = target.Center - NPC.Center;
							toPlayer = toPlayer.SafeNormalize(Vector2.UnitX * NPC.direction) * 20;
							float inertia = 10;
							NPC.velocity = (NPC.velocity * (inertia - 1) + toPlayer) / inertia;
						}
					}
				}
			}
			else if (AI_State == State_Bombing)
			{
				//This gets entered
				if (AI_Timer == 0)
				{
					SetFadingAura(20, 1.4f, 0.5f);
					SoundEngine.PlaySound(SoundID.Roar, target.Center);
				}

				//Lock rotation, talons do not move based on rotation
				NPC.rotation = 0f;

				float xSpeed = 7.5f; //9
				AI_Timer += xSpeed; //AI_Timer is memory of distance traversed

				//Keep constant speed towards direction (not player)
				float xDir = NPC.direction * xSpeed;

				float xProgress = AI_Timer / Bombing_Distance;

				//Adjust Y level to keep same level to targeted player if not reached player yet
				Vector2 targetCenter = target.Center;
				Vector2 toTarget = targetCenter - NPC.Center;
				float yDir = NPC.velocity.Y;
				if (NPC.direction * toTarget.X > 0)
				{
					targetCenter += new Vector2(0, -Bombing_Height);
					toTarget = targetCenter - NPC.Center;
					int ySpeed = 20;
					if (toTarget.LengthSquared() > ySpeed * ySpeed)
					{
						toTarget = toTarget.SafeNormalize(Vector2.UnitY) * ySpeed;
					}
					float inertia = 10;
					yDir = (NPC.velocity.Y * (inertia - 1) + toTarget.Y) / inertia;
				}

				if (toTarget.Y > Bombing_Height)
				{
					//player below targeted bombing line, move directly with player to catch up
					//yDir *= 1;
				}
				else
				{
					//if distance smaller (i.e player jumps) only catch up with slower speed
					yDir *= 0.75f;
				}

				NPC.velocity = new Vector2(xDir + target.velocity.X / 2, yDir);

				if (xProgress >= 1)
				{
					NPC.TargetClosest(true);
					SetState(State_Fireball);
				}

				HandleTalonBombing(target, talons);
			}
			else if (AI_State == State_Weakened)
			{
				if (AI_Animation != Animation_Flight && AI_Animation != Animation_NoHorizontal)
				{
					AI_Animation = Animation_Flight;
				}

				NPC.velocity *= 0.98f;

				float len = NPC.velocity.Length();
				if (len <= 0.5f)
				{
					NPC.velocity *= 0;
				}

				int period = 15;
				if (Main.netMode != NetmodeID.Server)
				{
					if (ReviveProgress % (3 * period) == 0)
					{
						SetFadingAura(20, 0.3f + 0.5f * (ReviveProgress / Revive_Duration));
						SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
						overlayColor = new Color(195, 247, 255) * 0.4f;
						overlayColor.A = 255;

						Vector2 dustOffset = new Vector2(0, (NPC.height + NPC.width) / 2 * 0.5f);
						Vector2 center = NPC.Center;
						float amount = 0.5f + 0.5f * (ReviveProgress / Revive_Duration);
						for (int i = 0; i < 30 * amount; i++)
						{
							Vector2 dustOffset2 = dustOffset.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.1f, 1f);
							Dust dust = Dust.NewDustPerfect(center + dustOffset2, 59, newColor: Color.White, Scale: 2.1f);
							dust.noLight = true;
							dust.noGravity = true;
							dust.fadeIn = Main.rand.NextFloat(0.2f, 0.8f);
							dust.velocity = Utils.SafeNormalize(dust.position - center, Vector2.Zero) * 3;
						}
					}
				}

				ReviveProgress++;

				if (ReviveProgress >= Revive_Duration)
				{
					NPC.TargetClosest();

					NPC.life = (int)(NPC.lifeMax * aiStats.MaxHP);

					SetFadingAura(20, 0.8f, 0.5f);
					SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
					AI_Animation = Animation_NoHorizontal;
					SetState(State_Fireball);
				}
			}
		}

		private void HandleReviveVisuals()
		{
			overlayColor = Color.Lerp(overlayColor, Color.White, 0.1f);
		}

		private void HandleReviveTrigger()
		{
			if (RevivesDone < Revive_Count && NPC.life < NPC.lifeMax * Revive_MinHP)
			{
				NPC.life = 0;
				NPC.checkDead();
			}
		}

		private void HandleTalonBombing(Player target, List<HarvesterTalon> talons)
		{
			//Handle synchronised talon punches
			if (talons.Count == 0)
			{
				return;
			}

			HarvesterTalon activeTalon = null;
			if (activeTalonIndex == byte.MaxValue)
			{
				for (int i = 0; i < talons.Count; i++)
				{
					HarvesterTalon talon = talons[i];
					if (talon.Idle)
					{
						if (activeTalonIndex != byte.MaxValue)
						{
							//Already has an idle talon, choose the one further away from player
							if (Math.Abs(talons[activeTalonIndex].NPC.Center.X - target.Center.X) <= Math.Abs(talon.NPC.Center.X - target.Center.X))
							{
								activeTalonIndex = (byte)i;
							}
						}
						else
						{
							activeTalonIndex = (byte)i;
						}
					}
				}

				//During kickstart found one idle talon: turn it active
				if (activeTalonIndex != byte.MaxValue)
				{
					activeTalon = talons[activeTalonIndex];
					ActivateTalon(talons[activeTalonIndex]);
				}
			}
			else
			{
				activeTalon = talons[activeTalonIndex];
			}

			if (activeTalon != null)
			{
				//If one is active, monitor it and detect when it retracts
				if (activeTalon.AI_State == HarvesterTalon.State_Seek_Retract)
				{
					//Swap to next talon
					for (int i = 0; i < talons.Count; i++)
					{
						HarvesterTalon talon = talons[i];
						if (talon.Idle && i != activeTalonIndex)
						{
							activeTalonIndex = (byte)i;
							ActivateTalon(talon);
							break;
						}
					}
				}
			}

			//Main.NewText("active: " + activeTalon.NPC.whoAmI);
		}

		private void ActivateTalon(HarvesterTalon talon)
		{
			if (talon != null && talon.Idle)
			{
				//Main.NewText("manually fire talon " + talon.NPC.whoAmI);
				talon.AI_State = HarvesterTalon.State_Punch;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.SyncNPC, number: talon.NPC.whoAmI);
				}
			}
		}

		private void HandleMeleeIndicator(Player target)
		{
			return; //TODO disabled for now

			if (Main.myPlayer == target.whoAmI)
			{
				if (!displayMeleeIndicator && StateToDisplayMeleeIndicator && !displayedMeleeIndicatorOnce)
				{
					Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y,
						Main.screenWidth, Main.screenHeight); //Lazy solution, does not respond to zoom

					Rectangle npcRect = NPC.getRect();
					npcRect.Inflate(-20, -20);

					if (screenRect.Contains(npcRect))
					{
						//Main.NewText("display indic");
						displayMeleeIndicator = true;
					}
				}

				if (displayMeleeIndicator && !StateToDisplayMeleeIndicator)
				{
					//Main.NewText("stop displaying indic");
					//Reset after displaying once
					displayMeleeIndicator = false;
					displayedMeleeIndicatorOnce = true;
				}
			}
		}

		private void HandleAnimation()
		{
			if (allowAnimationSpeedAdjustment)
			{
				//"Global" animation rule
				float speedX = Math.Abs(NPC.velocity.X);
				//Main.NewText("AI_Animation: " + AI_Animation);
				//Main.NewText("Animation_Timer: " + Animation_Timer);
				//Main.NewText("speedX: " + speedX);

				if (AI_Animation == Animation_Flight && speedX < 0.75f)
				{
					//The timer usage is for not immediately spazzing with the talons when changing directions
					if (Animation_Timer++ >= 20)
					{
						Animation_Timer = 0;
						//If not flight, and speed less than low threshold, change after 20 ticks
						AI_Animation = Animation_NoHorizontal;
					}
				}
				else if (AI_Animation == Animation_NoHorizontal && speedX > 1f)
				{
					if (Animation_Timer++ >= 20 || speedX > 3)
					{
						Animation_Timer = 0;
						//If not in horizontal, and speed more than high threshold, change after 20 ticks, or if really fast already
						AI_Animation = Animation_Flight;
					}
				}
			}
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			return null;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			//Gore 5 is a broken rib. For use when reviving

			if (NPC.life <= 0 && !NPC.active) //!active is important due to CheckDead shenanigans
			{
				var entitySource = NPC.GetSource_Death();

				int first = 1; //Head
				int second = 13 + first; //"Feather"
				int third = 2 + second; //Large wing bone
				int fourth = 2 + third; //Talon
				int total = fourth;
				for (int i = 0; i < total; i++)
				{
					int name = 4;
					if (i < first) name = 1;
					else if (i < second) name = 2;
					else if (i < third) name = 3;
					Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("SoulHarvesterGore_0" + name).Type, 1f);
				}
			}
		}
	}
}
