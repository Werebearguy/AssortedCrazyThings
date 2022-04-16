using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs.NPCs.Bosses.DungeonBird;
using AssortedCrazyThings.NPCs.DungeonBird;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.DungeonBird
{
	//This projectile represents all 3 stages of the pre-harvester boss encounter
	[Content(ContentType.Bosses)]
	public class BabyHarvesterProj : AssProjectile
	{
		public class TierData
		{
			public int FrameSpeed { get; init; } = 4;
			public int SoulsToNextTier { get; init; } = 1;
			public int TransformationFrameCount { get; init; } = 1;

			public TierData()
			{

			}
		}

		//Synced
		public int PlayerOwner { get; private set; } = Main.maxPlayers;

		public bool HasValidPlayerOwner => PlayerOwner >= 0 && PlayerOwner < Main.maxPlayers;

		public Player Player => Main.player[PlayerOwner];

		public int SoulsEaten
		{
			get => (int)Projectile.ai[0];
			private set => Projectile.ai[0] = value;
		}

		//Not synced, used for MP application of visuals
		public int PrevSoulsEaten { get; private set; }

		public const int MaxTier = 3;
		public int Tier
		{
			get
			{
				foreach (var item in SoulsEatenToTier)
				{
					if (SoulsEaten < item.Key)
					{
						return Math.Min(item.Value, MaxTier);
					}
				}

				return MaxTier;
			}
		}

		//Not synced, used for sequencing soul eating behavior and transformation
		public float Timer { get; private set; }

		//If below 0, currently cooling down from previous transformation. above 0, current transformation ongoing
		public int CurrentTrafoTier
		{
			get => (int)Projectile.ai[1];
			private set => Projectile.ai[1] = value;
		}

		public bool TrafoInProgress => CurrentTrafoTier > 0;

		public bool TrafoInProgressVisual => TrafoInProgress && TrafoTimer >= Time_TrafoStart;

		public bool TrafoInCooldown => CurrentTrafoTier < 0;

		//Not synced, only used by the server
		public int PrevTier
		{
			get => (int)Projectile.localAI[0] + 1; //Since ai is initialized as 0, tier should be 1
			private set => Projectile.localAI[0] = value - 1;
		}

		//Not synced
		public int TrafoFrameY
		{
			get => (int)Projectile.localAI[1];
			private set => Projectile.localAI[1] = value;
		}

		//Not synced
		public int TrafoTimer { get; private set; }

		/*
        - Fly to soul, pause next to it for quarter of a second. Keeps flapping.
        - Absorb soul, remain stationary for 1 second. Keeps flapping.
        - When enough souls are absorbed to trigger transforming, remain stationary for 3 seconds before transforming.
        - After transforming, stay stationary for 2 seconds and prevents eating in that time. Keeps flapping.
        */
		public const int Time_PreSoulEat = 15;
		public const int Time_PostSoulEat = Time_PreSoulEat + 60;
		public const int Time_TrafoStart = 150;
		public const int Time_TrafoCooldown = 120;

		public Color overlayColor = Color.White;

		private float fadingAuraAlphaIntensity = 0f;
		private float fadingAuraScaleIntensity = 0f;
		private int fadingAuraTimerMax = 0;
		private int fadingAuraTimer = 0;

		private bool drawBehind = true;
		private bool firstEmote = false;
		private int hungryEmoteTimer = 0;
		private int hungryEmoteTimerMax = 400;

		private int oldSoulTarget = Main.maxNPCs;

		public static Dictionary<int, Asset<Texture2D>> SheetAssets;
		public static Dictionary<int, Asset<Texture2D>> WingAssets;
		public static Dictionary<int, Asset<Texture2D>> TransformationAssets;
		public static Dictionary<int, Asset<Texture2D>> TransformationWingAssets;

		//Transformations are indexed by the "from" tier in "from -> to"
		public static Dictionary<int, TierData> TierDatas;
		private static Dictionary<int, int> TierToSoulsEaten;
		private static Dictionary<int, int> SoulsEatenToTier;

		public static int Spawn(Player player)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return Main.maxProjectiles;
			}

			Vector2 position = player.Center;
			position.X += Main.rand.NextFloat(-1980, 1980) / 2;
			position.Y += 1000;

			return AssUtils.NewProjectile(new EntitySource_WorldEvent(), position, Vector2.Zero, ModContent.ProjectileType<BabyHarvesterProj>(), 0, 0,
				preSync: (Projectile proj) => (proj.ModProjectile as BabyHarvesterProj).AssignPlayerOwner(player.whoAmI));
		}

		public override void Load()
		{
			if (!Main.dedServ)
			{
				SheetAssets = new Dictionary<int, Asset<Texture2D>>();
				WingAssets = new Dictionary<int, Asset<Texture2D>>();
				TransformationAssets = new Dictionary<int, Asset<Texture2D>>();
				TransformationWingAssets = new Dictionary<int, Asset<Texture2D>>();

				for (int i = 1; i <= MaxTier; i++)
				{
					SheetAssets.Add(i, ModContent.Request<Texture2D>(Texture + i + "_Sheet"));
				}

				//1 has no wing glowmask
				for (int i = 2; i <= MaxTier; i++)
				{
					WingAssets.Add(i, ModContent.Request<Texture2D>(Texture + i + "_Sheet_Wings"));
				}

				for (int i = 1; i <= MaxTier; i++)
				{
					TransformationAssets.Add(i, ModContent.Request<Texture2D>(Texture + "_Transformation" + i));
					TransformationWingAssets.Add(i, ModContent.Request<Texture2D>(Texture + "_Transformation" + i + "_Glowmask"));
				}
			}

			TierDatas = new Dictionary<int, TierData>()
			{
				[1] = new TierData
				{
					FrameSpeed = 3,
					SoulsToNextTier = 1,
					TransformationFrameCount = 7,
				},
				[2] = new TierData
				{
					FrameSpeed = 4,
					SoulsToNextTier = 5,
					TransformationFrameCount = 9,
				},
				[3] = new TierData
				{
					FrameSpeed = 4,
					SoulsToNextTier = 10,
					TransformationFrameCount = 10,
				},
				[4] = new TierData //Dummy tier, used to detect last tier switch into main form
				{
					SoulsToNextTier = 999
				},
			};

			SoulsEatenToTier = new Dictionary<int, int>();
			TierToSoulsEaten = new Dictionary<int, int>();
			int soulsAccumulated = 0;
			foreach (var data in TierDatas)
			{
				soulsAccumulated += data.Value.SoulsToNextTier;
				SoulsEatenToTier.Add(soulsAccumulated, data.Key);
				TierToSoulsEaten.Add(data.Key, soulsAccumulated);
			}
		}

		public override void Unload()
		{
			SheetAssets = null;
			WingAssets = null;
			TransformationAssets = null;
			TransformationWingAssets = null;

			TierDatas = null;
			SoulsEatenToTier = null;
			TierToSoulsEaten = null;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baby Bird");
			Main.projFrames[Projectile.type] = 5;

			ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			Projectile.hide = true;
			Projectile.aiStyle = -1;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.netImportant = true;
			Projectile.timeLeft = int.MaxValue / 2;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)PlayerOwner);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			PlayerOwner = reader.ReadByte();
		}

		public void AssignPlayerOwner(int newPlayer)
		{
			bool sync = HasValidPlayerOwner;

			PlayerOwner = newPlayer;

			if (sync)
			{
				//If switching player, sync immediately
				Projectile.NetSync();
			}
		}

		public void AddSoulsEaten()
		{
			if (Main.myPlayer == Projectile.owner)
			{
				//AssUtils.Print(Main.time + " increased souls eaten");
				SoulsEaten++;

				//Has to be after SoulsEaten is modified
				if (PrevTier < Tier)
				{
					//Initiate transformation
					CurrentTrafoTier = PrevTier;
				}
				PrevTier = Tier;

				if (TierToSoulsEaten.TryGetValue(MaxTier, out var v))
				{
					if (SoulsEaten >= v)
					{
						//Transform into last stage
						CurrentTrafoTier = PrevTier;
					}
				}

				Projectile.NetSync();
			}
		}

		private void HandleSoulEatingVisuals()
		{
			overlayColor = Color.Lerp(overlayColor, Color.White, 0.1f);
		}

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

		private void HandleWingDust()
		{
			if (TrafoInProgressVisual)
			{
				return;
			}

			if (Tier == 1 || CurrentTrafoTier == 1)
			{
				return;
			}

			if (!TierToSoulsEaten.TryGetValue(MaxTier, out var maxSoulsEaten))
			{
				return;
			}

			Vector2 offset = Vector2.Zero;
			Vector2 size = Projectile.Size;

			if (CurrentTrafoTier != 2 && Tier == 3)
			{
				offset.X += Projectile.width;
				float y = Projectile.frame switch
				{
					0 => -Projectile.height / 1.5f,
					2 => Projectile.height / 1.5f,
					_ => 0,
				};
				offset.Y += y;
			}
			else
			{
				size += -Vector2.One * 6;

				offset.X += Projectile.width / 2f;
				float y = Projectile.frame switch
				{
					0 => -Projectile.height / 2.5f,
					2 => Projectile.height / 2.5f,
					_ => 0,
				};
				offset.Y += y;
			}

			for (int i = -1; i <= 1; i += 2)
			{
				Vector2 dustCenter = Projectile.Center + new Vector2(offset.X * i, offset.Y).RotatedBy(Projectile.rotation);
				Rectangle dustBox = Utils.CenteredRectangle(dustCenter, size);

				if (Main.rand.NextFloat() < 0.5f * (float)SoulsEaten / maxSoulsEaten)
				{
					Dust dust = Dust.NewDustDirect(dustBox.TopLeft(), dustBox.Width, dustBox.Height, 135, 0f, 0f, 0, default(Color), 1.5f);
					dust.noGravity = true;
					dust.noLight = true;
					dust.velocity *= 0.3f;
					if (Main.rand.NextBool(5))
					{
						dust.fadeIn = 1f;
					}
				}
			}
		}

		private void HandleLeavingCondition()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int time = 100;
				if (BabyHarvesterHandler.IsTurningInvalidPlayer(Player, out int timeLeft))
				{
					if (timeLeft % time == time - 1) //So it triggers at the start, and not close at the end
					{
						EmoteBubble.NewBubble(EmoteID.EmotionCry, new WorldUIAnchor(Projectile), time);
					}
				}
			}
		}

		private int SoulTargetClosest(float maxDistance = 1000f)
		{
			short closest = Main.maxNPCs;
			Vector2 toSoul;
			float oldDistance = maxDistance;
			float newDistance;
			for (short j = 0; j < Main.maxNPCs; j++)
			{
				//ignore souls if they are noclipping
				NPC other = Main.npc[j];
				if (other.active && other.type == ModContent.NPCType<DungeonSoul>())
				{
					toSoul = other.Center - Projectile.Center;
					newDistance = toSoul.Length();
					if (newDistance < oldDistance)
					{
						oldDistance = newDistance;
						closest = j;
					}
				}
			}
			return closest;
		}

		public override void AI()
		{
			if (!HasValidPlayerOwner)
			{
				Projectile.Kill();
				return;
			}
			//Player is now valid here

			if (!Player.active)
			{
				//Wait for revalidation (player disconnecting causes position of the player to be reset to 0,0, despawning the projectiles because its oob)
				//If no other suitable player is found, it gets despawned by the handler
				return;
			}

			//Client the bird is attached to gives himself the buff
			//Buff is cleared automatically from any player the bird is not following
			if (Main.myPlayer == Player.whoAmI)
			{
				int buffType = ModContent.BuffType<BabyHarvesterBuff>();
				if (!Player.HasBuff(buffType))
				{
					Player.AddBuff(buffType, 1600);
				}
			}

			HandleSoulEatingVisuals();

			HandleFadingAura();

			HandleWingDust();

			HandleLeavingCondition();

			//Main.NewText("t: " + Tier);
			//Main.NewText("c: " + CurrentTrafoTier);

			bool animate = true;
			if (!TrafoInProgress)
			{
				if (!TrafoInCooldown)
				{
					RegularAI();
				}
				else
				{
					//Main.NewText("in trafo cooldown " + CurrentTrafoTier);
					CurrentTrafoTier++;

					if (CurrentTrafoTier < -30)
					{
						Projectile.velocity.Y = -0.025f;
					}
				}
			}
			else
			{
				animate = TransformationAI();
			}

			if (animate)
			{
				int frameSpeed = 4;
				if (TierDatas.TryGetValue(Tier, out var data))
				{
					frameSpeed = data.FrameSpeed;
				}

				if (Projectile.velocity.LengthSquared() < 2 * 2)
				{
					frameSpeed++;
				}
				AssAI.ZephyrfishDraw(Projectile, frameSpeed);
			}
		}

		private void RegularAI()
		{
			drawBehind = true;
			int soulTarget = Main.maxNPCs;
			if (Timer <= Time_PreSoulEat)
			{
				//Main.NewText("in pre soul eat " + Timer);
				soulTarget = SoulTargetClosest();
			}
			if (soulTarget != Main.maxNPCs)
			{
				if (soulTarget != oldSoulTarget)
				{
					if (Tier == 1)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							EmoteBubble.NewBubble(EmoteID.ItemBugNet, new WorldUIAnchor(Projectile), 120);
						}
					}
				}
				oldSoulTarget = soulTarget;

				NPC soul = Main.npc[soulTarget];

				Vector2 toSoul = soul.Center - Projectile.Center;
				Vector2 offsetTier = Tier switch
				{
					1 => new Vector2(30, -20),
					2 => new Vector2(40, -25),
					3 => new Vector2(50, -30),
					_ => Vector2.Zero,
				};
				offsetTier.X *= -Math.Sign(soul.Center.X - Player.Center.X);

				Vector2 soulPos = soul.Center + offsetTier;
				Vector2 toSoulResting = soulPos - Projectile.Center;
				float distanceSQ = toSoulResting.LengthSquared();

				if (distanceSQ > 10 * 10)
				{
					//Approach soul
					Timer = 0;
					float speed = 8f;
					float inertia = 16;

					if (distanceSQ < 100 * 100)
					{
						speed = 3f;
						inertia = 3;
					}

					Vector2 toSoulSpeed = toSoulResting.SafeNormalize(Vector2.UnitY) * speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + toSoulSpeed) / inertia;
					Projectile.direction = Math.Sign(toSoul.X);
					Projectile.spriteDirection = Projectile.direction;
					Projectile.rotation = Projectile.velocity.X * 0.05f;
				}
				else
				{
					//Next to soul
					if (Tier == 1)
					{
						Timer += 0.2f;
					}
					else
					{
						Timer++;
					}
					Projectile.velocity *= 0.8f;
					Projectile.rotation *= 0.8f;

					if (Math.Abs(Projectile.velocity.X) < 0.1f)
					{
						Projectile.netUpdate = true;
						Projectile.velocity *= 0f;
						Projectile.rotation = 0;
					}

					if (Timer > Time_PreSoulEat)
					{
						//Destroy soul and spawn a homing projectile which increases the soul count
						if (Main.myPlayer == Projectile.owner)
						{
							Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), soul.Center, Vector2.Zero, ModContent.ProjectileType<HarvesterAbsorbedSoul>(), 0, 0f, Main.myPlayer);

							//AssUtils.Print(Main.time + " spawned homing soul");
						}

						soul.life = 0;
						soul.active = false;
						soul.netUpdate = true;

						Projectile.netUpdate = true;
					}
				}

			}
			else if (Timer == 0)
			{
				Vector2 offset = Vector2.Zero;
				float veloFactor = 0.9f;
				float sway = 1f;
				bool random = true;

				if (Tier == 1)
				{
					if (!firstEmote)
					{
						firstEmote = true;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							EmoteBubble.NewBubble(EmoteID.BossSkeletron, new WorldUIAnchor(Projectile), 240);
						}
					}

					hungryEmoteTimer++;
					if (hungryEmoteTimer > hungryEmoteTimerMax)
					{
						hungryEmoteTimer = 0;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							EmoteBubble.NewBubble(EmoteID.EmotionAlert, new WorldUIAnchor(Projectile), 120);
						}
					}

					offset = new Vector2(-60, 40);
					veloFactor = 1f;
					sway = 1.3f;

					drawBehind = Projectile.direction == 1;

					//if something: drawBehind = false
					float rot = Projectile.velocity.ToRotation();
					if (Projectile.direction == 1)
					{
						rot = MathHelper.Pi - Math.Abs(rot);
					}
					//The above normalizes rot to number always around 0
					float thres = MathHelper.PiOver4 / 2;

					//attempt to "flatten" velocity, prefer moving horizontally
					Rectangle flattenRect = Player.Hitbox;
					flattenRect.Inflate(0, 20);
					if (Projectile.Hitbox.Intersects(flattenRect) && rot > thres)
					{
						float by = 1 / 50f;
						by *= Projectile.direction * Math.Sign(rot);
						if (Projectile.direction == 1)
						{
							by *= -1;
						}
						Projectile.velocity = Projectile.velocity.RotatedBy(by);

						//Always keep min speed
						float minSpeed = 2.4f;
						if (Projectile.velocity.LengthSquared() < minSpeed * minSpeed)
						{
							Projectile.velocity = Utils.SafeNormalize(Projectile.velocity, Vector2.UnitY);
							Projectile.velocity *= minSpeed;
						}
					}
				}

				AssAI.ZephyrfishAI(Projectile, Player, veloFactor, sway, random, offsetX: offset.X, offsetY: offset.Y);
				Projectile.spriteDirection = -Projectile.spriteDirection;
			}
			else
			{
				Timer++;
				if (Timer > Time_PostSoulEat)
				{
					Timer = 0;
					Projectile.netUpdate = true;
				}
				else
				{
					//Main.NewText("in post soul eat " + Timer);
				}
			}
		}

		//Returns true if it should keep animating
		private bool TransformationAI()
		{
			//Runs on all clients, does not need any syncing to stop
			Projectile.velocity *= 0.4f;
			Projectile.rotation *= 0.8f;

			if (CurrentTrafoTier == MaxTier)
			{
				Projectile.rotation = 0f;
			}

			if (!TierDatas.TryGetValue(CurrentTrafoTier, out var data))
			{
				return true;
			}

			TrafoTimer++;
			if (TrafoTimer <= Time_TrafoStart)
			{
				int thres = 50;
				if (TrafoTimer % thres == 0)
				{
					int step = TrafoTimer / thres;
					overlayColor = new Color(195, 247, 255) * (0.6f - 0.2f * step);
					overlayColor.A = 255;
				}
				//Main.NewText("in trafo start " + TrafoTimer);
				return true;
			}

			int threshold = Time_TrafoStart;
			if (TrafoTimer > 3 + threshold)
			{
				TrafoFrameY++;
				TrafoTimer = threshold;
			}
			if (TrafoFrameY >= data.TransformationFrameCount)
			{
				Projectile.frame = 0;
				Projectile.frameCounter = 0;
				TrafoTimer = 0;
				TrafoFrameY = 0;

				if (CurrentTrafoTier == MaxTier)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int type = AssortedCrazyThings.harvester;
						if (!NPC.AnyNPCs(type))
						{
							int yOffset = 62; //Manually adjusted
							int index = NPC.NewNPC(new EntitySource_BossSpawn(Player), (int)Projectile.Center.X, (int)Projectile.Center.Y + yOffset, type);
							if (index < Main.maxNPCs && Main.npc[index] is NPC npc && npc.ModNPC is Harvester harvester)
							{
								harvester.AI_State = Harvester.State_SpawnedByBaby;
								if (Main.netMode == NetmodeID.Server)
								{
									NetMessage.SendData(MessageID.SyncNPC, number: index);
								}
							}

							AssWorld.AwakeningMessage(Harvester.name + " has been awakened!");
						}

						Projectile.Kill();
					}
				}
				else
				{
					Vector2 dustOffset = new Vector2(0, 20);
					Vector2 center = Projectile.Center;
					for (int i = 0; i < 10 + CurrentTrafoTier * 10; i++)
					{
						Vector2 dustOffset2 = dustOffset.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.1f, 1f);
						Dust dust = Dust.NewDustPerfect(center + dustOffset2, 59, newColor: Color.White, Scale: 2.1f);
						dust.noLight = true;
						dust.noGravity = true;
						dust.fadeIn = Main.rand.NextFloat(0.2f, 0.8f);
						dust.velocity = Utils.SafeNormalize(dust.position - center, Vector2.Zero) * 3;
					}

					SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
				}

				CurrentTrafoTier = -Time_TrafoCooldown; //Reset trafo
				Projectile.netUpdate = true;
			}

			return false;
		}

		public override void PostAI()
		{
			if (PrevSoulsEaten < SoulsEaten)
			{
				//AssUtils.Print(Main.time + " souls eaten visuals");

				overlayColor = new Color(195, 247, 255) * 0.7f;
				overlayColor.A = 255;
				SetFadingAura(30);

				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
			}

			PrevSoulsEaten = SoulsEaten;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 100);
				Dust dust = Main.dust[d];
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale = 1.3f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color.White, 0.4f).MultiplyRGBA(overlayColor) * Projectile.Opacity;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			(drawBehind ? behindNPCs : overPlayers).Add(index);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//Custom draw to just center on the hitbox, as all birds share the same frame size (for seamless transformation)
			Texture2D texture = null;
			int frameCount = 1;
			int frameY = 0;
			int tier = TrafoInProgress ? CurrentTrafoTier : Tier;
			if (TrafoInProgressVisual && TransformationAssets.TryGetValue(tier, out var trafoAsset))
			{
				texture = trafoAsset.Value;

				if (TierDatas.TryGetValue(tier, out var data))
				{
					frameCount = data.TransformationFrameCount;
				}
				frameY = TrafoFrameY;
			}
			else if (SheetAssets.TryGetValue(tier, out var asset))
			{
				texture = asset.Value;
				frameCount = Main.projFrames[Projectile.type];
				frameY = Projectile.frame;
			}

			if (texture == null)
			{
				return false;
			}

			Rectangle sourceRect = texture.Frame(1, frameCount, frameY: frameY);
			Vector2 drawOrigin = sourceRect.Size() / 2f;

			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Vector2 drawPos = Projectile.position + Projectile.Size / 2f + new Vector2(0, Projectile.gfxOffY + 4f - 1f) - Main.screenPosition;
			Color color = Projectile.GetAlpha(lightColor);

			float rotation = Projectile.rotation;
			float scale = Projectile.scale;
			Main.EntitySpriteDraw(texture, drawPos, sourceRect, color, rotation, drawOrigin, scale, spriteEffects, 0);

			Texture2D wingTexture = null;
			if (TrafoInProgressVisual && TransformationWingAssets.TryGetValue(tier, out var trafoWingAsset))
			{
				wingTexture = trafoWingAsset.Value;
			}
			else if (WingAssets.TryGetValue(tier, out var wingAsset))
			{
				wingTexture = wingAsset.Value;
			}

			Color wingColor = Projectile.GetAlpha(Color.White);
			if (wingTexture != null)
			{
				Main.EntitySpriteDraw(wingTexture, drawPos, sourceRect, wingColor, rotation, drawOrigin, scale, spriteEffects, 0);
			}

			if (fadingAuraTimer > 0 && fadingAuraTimerMax > 0)
			{
				float fadeScale = fadingAuraTimer / (float)fadingAuraTimerMax;
				float scaleInverse = 1f - fadeScale;
				Color auraColor = color * fadeScale * fadingAuraAlphaIntensity;
				Color auraWingColor = wingColor * fadeScale * fadingAuraAlphaIntensity;
				float auraScale = scale + scaleInverse * fadingAuraScaleIntensity;

				Main.EntitySpriteDraw(texture, drawPos, sourceRect, auraColor, rotation, drawOrigin, scale, spriteEffects, 0);

				if (wingTexture != null)
				{
					Main.EntitySpriteDraw(wingTexture, drawPos, sourceRect, auraWingColor, rotation, drawOrigin, auraScale, spriteEffects, 0);
				}
			}

			return false;
		}
	}
}
