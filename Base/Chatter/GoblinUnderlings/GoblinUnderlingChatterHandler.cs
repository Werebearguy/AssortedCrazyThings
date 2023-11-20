using AssortedCrazyThings.Base.Chatter.Conditions;
using AssortedCrazyThings.Base.Data;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter.GoblinUnderlings
{
	public enum GoblinUnderlingChatterType
	{
		None,
		Eager,
		Serious,
		Shy,
	}

	/*
	 * How it works:
	 * Global cooldown applies to all. The problem is that it can "drown out" anything that wants to chat in the same tick forever.
	 * Since multiple goblins can be out at once, they can drown eachother out, we need to manage currently active goblins and cycle through them
	 * On successful message: put all gobbos on that on cooldown, then randomize order that the generators are checked in. First in order gets the next message
	 */
	//Special handler which has more than one generator, as it needs to manage all of them as a group
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingChatterHandler : ChatterHandler
	{
		private Dictionary<GoblinUnderlingChatterType, GoblinUnderlingChatterGenerator> GeneratorsPerType { get; init; }
		private List<GoblinUnderlingChatterType> Order { get; set; }
		private ChatterTracker Tracker { get; set; } = new();

		public override IEnumerable<ChatterGenerator> Generators => GeneratorsPerType.Values;

		public GoblinUnderlingChatterHandler() : base(ChatterType.GoblinUnderling)
		{
			GeneratorsPerType = new Dictionary<GoblinUnderlingChatterType, GoblinUnderlingChatterGenerator>()
			{
				{ GoblinUnderlingChatterType.Eager,
					new GoblinUnderlingChatterGenerator(GoblinUnderlingChatterType.Eager.ToString(), new Color(125, 217, 124))
					{
						Chatters = new Dictionary<ChatterSource, ChatterMessageGroup>()
						{
							{ ChatterSource.Idle,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("Bored"),
									new ChatterMessage("WhenFighting"),
									new ChatterMessage("HangingOut"),
									new ChatterMessage("GoodBoss"),
									new ChatterMessage("BossSilent"),
									new ChatterMessage("HateSlimes"),
									new ChatterMessage("WhenMeal"),
									new ChatterMessage("DoYouLikeMe"),

									new ChatterMessage("GettingSleepy", new SurfaceNightChatterCondition(), true),
									new ChatterMessage("SeeBase", new SkyChatterCondition(), true),
									new ChatterMessage("SandInArmor", new DesertChatterCondition(), true),
									new ChatterMessage("GroundLooking", new CrimsonChatterCondition(), true),
									new ChatterMessage("BeatTheCold", new SnowChatterCondition(), true),
									new ChatterMessage("FaceLooksLikeRock", new UndergroundChatterCondition(), true),
									new ChatterMessage("RockLooksLikeBones", new UndergroundChatterCondition(), true),
									new ChatterMessage("FindShinyMetals", new UndergroundChatterCondition(), true),
									new ChatterMessage("NothingScares", new SurfaceNoLightChatterCondition(), true),
									new ChatterMessage("HeardRat", new UndergroundNoLightChatterCondition(), true),
									new ChatterMessage("CavesSpooky", new UndergroundNoLightChatterCondition(), true),
								})
							},
							{ ChatterSource.FirstSummon,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("YouTheBoss"),
								})
							},
							{ ChatterSource.Attacking,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("Gotcha"),
									new ChatterMessage("IGotThis"),
									new ChatterMessage("ExpGold"),
									new ChatterMessage("NoOneTouches"),
									new ChatterMessage("BadGuys"),
									new ChatterMessage("ComeHere"),
									new ChatterMessage("LevelOne"),

									new ChatterMessage("StabStab", new MeleeClassChatterCondition(), true),
									new ChatterMessage("Boom", new MagicClassChatterCondition(), true),
									new ChatterMessage("Skewer", new RangedClassChatterCondition(), true),
								})
							},
							{ ChatterSource.PlayerHurt,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("DontHurt"),
									new ChatterMessage("YouWillPay"),
									new ChatterMessage("YouGood"),
									new ChatterMessage("ThatHurt"),
									new ChatterMessage("IsntWeak"),
								})
							},
							{ ChatterSource.PlayerHurtByTrap,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WallsAttack", new DartTrapChatterCondition()),
									new ChatterMessage("WatchWalls", new DartTrapChatterCondition()),
									new ChatterMessage("BadWall", new DartTrapChatterCondition()),
									new ChatterMessage("GroundEnemy", new GeyserTrapChatterCondition()),
									new ChatterMessage("HotHot", new GeyserTrapChatterCondition()),
									new ChatterMessage("MissThat", new GeyserTrapChatterCondition()),
									new ChatterMessage("Dangerous", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("NoFair", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("BadPeople", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("BigBadBalls", new BoulderTrapChatterCondition()),
									new ChatterMessage("DontFlattened", new BoulderTrapChatterCondition()),
									new ChatterMessage("HadToHurt", new BoulderTrapChatterCondition()),
								})
							},
							{ ChatterSource.BossSpawn,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("UhOh"),
									new ChatterMessage("BigOne"),
									new ChatterMessage("ThatOnesBig"),
									new ChatterMessage("NotAfraid"),
									new ChatterMessage("WeGotTrouble"),

									new ChatterMessage("HateBees", new QueenBeeGenericChatterCondition()),
									new ChatterMessage("WontReadBook", new DarkMageT1GenericChatterCondition()),
									new ChatterMessage("OneEyedDummy", new OgreT2GenericChatterCondition()),
									new ChatterMessage("BigGuts", new BetsyGenericChatterCondition()),
									new ChatterMessage("WeGotThis", new MoonLordGenericChatterCondition()),

									new ChatterMessage("JigglyJellyfish", new QueenJellyfishGenericChatterCondition()),
									new ChatterMessage("FatBat", new ViscountGenericChatterCondition()),
								})
							},
							{ ChatterSource.BossDefeat,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WeDidIt"),
									new ChatterMessage("KnewYouCould"),
									new ChatterMessage("EasyNext"),
									new ChatterMessage("WontBeInSequel"),

									new ChatterMessage("NastyBees", new QueenBeeGenericChatterCondition()),
									new ChatterMessage("StupidBook", new DarkMageT1GenericChatterCondition()),
									new ChatterMessage("DumbWeakling",new OgreT2GenericChatterCondition()),
									new ChatterMessage("DumbDragon", new BetsyGenericChatterCondition()),
									new ChatterMessage("YouTheBest", new MoonLordGenericChatterCondition()),
									new ChatterMessage("ToughTougher", new MoonLordGenericChatterCondition()),
									new ChatterMessage("NoMoreSquid", new MoonLordGenericChatterCondition()),

									new ChatterMessage("JellyTastesBad", new QueenJellyfishGenericChatterCondition()),
									new ChatterMessage("SuckOnThat", new ViscountGenericChatterCondition()),
								})
							},
							{ ChatterSource.OOAStarts,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WhatDidYouDo"),
								})
							},
							{ ChatterSource.OOANewWave,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("HolesStillOpen"),
									new ChatterMessage("MoreBaddies"),
									new ChatterMessage("ProtectGem"),
									new ChatterMessage("WontLetPass"),
								})
							},
							{ ChatterSource.ArmorEquipped,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("LookingSpiffy", new EquipValhallaArmorChatterCondition()),
									new ChatterMessage("LookingFluffy", new EquipFlinxFurCoatChatterCondition()),
								})
							},
							{ ChatterSource.ItemSelected,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("IsAToy", new SelectPearlwoodSwordChatterCondition()),
								})
							},
							{ ChatterSource.InvasionChanged,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("OldGangBack", new GoblinArmyInvasionChangedChatterCondition()),
								})
							},
							{ ChatterSource.BloodMoonChanged,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("SkyIsRed", new BloodMoonChangedChatterCondition()),
								})
							},
						}
					}
				},
				{ GoblinUnderlingChatterType.Serious,
					new GoblinUnderlingChatterGenerator(GoblinUnderlingChatterType.Serious.ToString(), new Color(166, 156, 65))
					{
						Chatters = new Dictionary<ChatterSource, ChatterMessageGroup>()
						{
							{ ChatterSource.Idle,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("Peaceful"),
									new ChatterMessage("YouSeemCalm"),
									new ChatterMessage("ShallWeRest"),
									new ChatterMessage("PaceEasy"),
									new ChatterMessage("ShouldSleep", new SurfaceNightChatterCondition(), true),
									new ChatterMessage("WindNice", new WindyDayChatterCondition(), true),
									new ChatterMessage("WarmWithFire", new SnowChatterCondition(), true),
									new ChatterMessage("SandsClaimWeak", new DesertChatterCondition(), true),
									new ChatterMessage("UnnaturalPlace", new AnyEvilChatterCondition(), true),
									new ChatterMessage("NotDawdle", new UnderworldChatterCondition(), true),
									new ChatterMessage("SpiritsHide", new SurfaceNoLightChatterCondition(), true),
									new ChatterMessage("StonesHistory", new UndergroundChatterCondition(), true),
									new ChatterMessage("HiddenTreasure", new UndergroundChatterCondition(), true),
									new ChatterMessage("BeCareful", new UndergroundNoLightChatterCondition(), true),
									new ChatterMessage("TooFarAhead", new UndergroundNoLightChatterCondition(), true),
								})
							},
							{ ChatterSource.FirstSummon,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("Greetings"),
								})
							},
							{ ChatterSource.Attacking,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("IGotThis"),
									new ChatterMessage("NotDefy"),
									new ChatterMessage("NotSurvive"),

									new ChatterMessage("CutDown", new MeleeClassChatterCondition(), true),
									new ChatterMessage("StruckDown", new MagicClassChatterCondition(), true),
									new ChatterMessage("Burn", new MagicClassChatterCondition(), true),
									new ChatterMessage("InSights", new RangedClassChatterCondition(), true),
								})
							},
							{ ChatterSource.PlayerHurt,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("Alright"),
									new ChatterMessage("Unpleasant"),
									new ChatterMessage("Tough"),
									new ChatterMessage("Healing"),
								})
							},
							{ ChatterSource.PlayerHurtByTrap,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WatchStep", new DartTrapChatterCondition()),
									new ChatterMessage("PressurePlates", new DartTrapChatterCondition()),
									new ChatterMessage("CowardlyTrap", new DartTrapChatterCondition()),
									new ChatterMessage("BossNotSee", new GeyserTrapChatterCondition()),
									new ChatterMessage("NaturalTrap", new GeyserTrapChatterCondition()),
									new ChatterMessage("MinorBurn", new GeyserTrapChatterCondition()),
									new ChatterMessage("TrapCruel", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("TakeTime", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("CruelPeople", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("Weighed", new BoulderTrapChatterCondition()),
									new ChatterMessage("INotSee", new BoulderTrapChatterCondition()),
									new ChatterMessage("FromDistance", new BoulderTrapChatterCondition()),
								})
							},
							{ ChatterSource.BossSpawn,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("LargeEnemy"),
									new ChatterMessage("Formidable"),
									new ChatterMessage("Prepared"),
									new ChatterMessage("Trust"),

									new ChatterMessage("TerribleNight", new SkeletronGenericChatterCondition()),
									new ChatterMessage("MagicSuperior", new DarkMageT1GenericChatterCondition()),
									new ChatterMessage("SenseIntelligence", new OgreT2GenericChatterCondition()),
									new ChatterMessage("HidingDragon", new BetsyGenericChatterCondition()),
									new ChatterMessage("CantLose", new MoonLordGenericChatterCondition()),

									new ChatterMessage("WastedFirstLife", new LichGenericChatterCondition()),
									new ChatterMessage("ForceOvercome", new DreamEaterGenericChatterCondition()),
								})
							},
							{ ChatterSource.BossDefeat,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("MakeDoll"),
									new ChatterMessage("TooEasy"),
									new ChatterMessage("Overcame"),
									new ChatterMessage("WellDone"),

									new ChatterMessage("CurseLifted", new SkeletronGenericChatterCondition()),
									new ChatterMessage("BetterMage", new DarkMageT1GenericChatterCondition()),
									new ChatterMessage("Joke", new OgreT2GenericChatterCondition()),
									new ChatterMessage("ExpectedMore", new BetsyGenericChatterCondition()),
									new ChatterMessage("Incredible", new MoonLordGenericChatterCondition()),

									new ChatterMessage("WastedSecondLife", new LichDefeatedChatterCondition()),
									new ChatterMessage("Greatest", new DreamEaterGenericChatterCondition()),
								})
							},
							{ ChatterSource.OOAStarts,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WhatMagic"),
								})
							},
							{ ChatterSource.OOANewWave,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("NotOverYet"),
									new ChatterMessage("MoreFoes"),
									new ChatterMessage("ProtectGem"),
									new ChatterMessage("ArmyEndless"),
								})
							},
							{ ChatterSource.ArmorEquipped,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("OddThings", new EquipWeirdHeadwearArmorChatterCondition()),
								})
							},
							{ ChatterSource.ItemSelected,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("ThatDoll", new SelectAnyDollChatterCondition()),
								})
							},
							{ ChatterSource.InvasionChanged,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("FoolsReturned", new GoblinArmyInvasionChangedChatterCondition()),
								})
							},
							{ ChatterSource.BloodMoonChanged,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("HeadUnbearable", new BloodMoonChangedChatterCondition()),
								})
							},
						}
					}
				},
				{ GoblinUnderlingChatterType.Shy,
					new GoblinUnderlingChatterGenerator(GoblinUnderlingChatterType.Shy.ToString(), new Color(111, 158, 179))
					{
						Chatters = new Dictionary<ChatterSource, ChatterMessageGroup>()
						{
							{ ChatterSource.Idle,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("LikeNoFight"),
									new ChatterMessage("FeelSafe"),
									new ChatterMessage("RunIntoBad"),
									new ChatterMessage("WatchStep"),
									new ChatterMessage("Nevermind"),
									new ChatterMessage("ShouldRest"),

									new ChatterMessage("GoHome", new SurfaceNightChatterCondition(), true),
									new ChatterMessage("PleasantWind", new WindyDayChatterCondition(), true),
									new ChatterMessage("SnowCold", new SnowChatterCondition(), true),
									new ChatterMessage("PlaceScary", new AnyEvilChatterCondition(), true),
									new ChatterMessage("EveryoneNice", new InTownChatterCondition(), true),
									new ChatterMessage("SeeStars", new SurfaceNoLightChatterCondition(), true),
									new ChatterMessage("WatchFooting", new UndergroundChatterCondition(), true),
									new ChatterMessage("SawMouse", new UndergroundChatterCondition(), true),
									new ChatterMessage("DontLeave", new UndergroundNoLightChatterCondition(), true),
									new ChatterMessage("Scared", new UndergroundNoLightChatterCondition(), true),
								})
							},
							{ ChatterSource.FirstSummon,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("NiceToMeet"),
								})
							},
							{ ChatterSource.Attacking,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("NotEscape"),
									new ChatterMessage("SeeDead"),
									new ChatterMessage("TargetSighted"),
									new ChatterMessage("Eliminated"),

									new ChatterMessage("BossesBlade", new MeleeClassChatterCondition(), true),
									new ChatterMessage("Burn", new MagicClassChatterCondition(), true),
									new ChatterMessage("WontMiss", new RangedClassChatterCondition(), true),
									new ChatterMessage("PleaseLeave", new GoblinArmyInvasionOngoingChatterCondition(), true),
								})
							},
							{ ChatterSource.PlayerHurt,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("YouAlright"),
									new ChatterMessage("OhNo"),
									new ChatterMessage("DontDie"),
									new ChatterMessage("Boss"),
								})
							},
							{ ChatterSource.PlayerHurtByTrap,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									//One missing for DartTrapChatterCondition
									new ChatterMessage("HopeOkay", new DartTrapChatterCondition()),
									new ChatterMessage("TreatingWound", new DartTrapChatterCondition()),
									new ChatterMessage("CoolDown", new GeyserTrapChatterCondition()),
									new ChatterMessage("SeeSooner", new GeyserTrapChatterCondition()),
									new ChatterMessage("NatureCruel", new GeyserTrapChatterCondition()),
									new ChatterMessage("AwfulTraps", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("TreatWound", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("Barbaric", new SpikyOrFlameTrapChatterCondition()),
									new ChatterMessage("BadlyHurt", new BoulderTrapChatterCondition()),
									new ChatterMessage("MoreCautious", new BoulderTrapChatterCondition()),
									new ChatterMessage("DontLetHit", new BoulderTrapChatterCondition()),
								})
							},
							{ ChatterSource.BossSpawn,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WhatIsThat"),
									new ChatterMessage("LargeEnemy"),
									new ChatterMessage("DoMyBest"),

									new ChatterMessage("LooksDangerous", new DarkMageT1GenericChatterCondition()),
									new ChatterMessage("HesHuge", new OgreT2GenericChatterCondition()),
									new ChatterMessage("Dragon", new BetsyGenericChatterCondition()),
									new ChatterMessage("Pretty", new EoLGenericChatterCondition()),
									new ChatterMessage("NotAgain", new MartianSaucerGenericChatterCondition()),
									new ChatterMessage("WontBeAfraid", new MoonLordGenericChatterCondition()),

									new ChatterMessage("RocksMoving", new GraniteEnergyStormGenericChatterCondition()),
									new ChatterMessage("TheyAreBack", new StarScouterGenericChatterCondition()),
								})
							},
							{ ChatterSource.BossDefeat,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WasScary"),
									new ChatterMessage("WasntUseful"),
									new ChatterMessage("YouWereGreat"),
									new ChatterMessage("CouldDoIt"),

									new ChatterMessage("GladItsOver", new DarkMageT1GenericChatterCondition()),
									new ChatterMessage("TheBiggerTheyAre", new OgreT2GenericChatterCondition()),
									new ChatterMessage("DontFight", new BetsyGenericChatterCondition()),
									new ChatterMessage("Distracted", new EoLGenericChatterCondition()),
									new ChatterMessage("WontBeTaken", new MartianSaucerGenericChatterCondition()),
									new ChatterMessage("YouDidIt", new MoonLordGenericChatterCondition()),

									new ChatterMessage("WeirdFight", new GraniteEnergyStormGenericChatterCondition()),
									new ChatterMessage("ThankYou", new StarScouterGenericChatterCondition()),
								})
							},
							{ ChatterSource.OOAStarts,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("WhatHoles"),
								})
							},
							{ ChatterSource.OOANewWave,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("MoreOfThem"),
									new ChatterMessage("MoreFoes"),
									new ChatterMessage("WhenEnd"),
									new ChatterMessage("GemSafe"),
								})
							},
							{ ChatterSource.ArmorEquipped,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("ItsYou", new EquipAnyMartianArmorChatterCondition()),
									new ChatterMessage("SeeingFace", new EquipFamiliarWigChatterCondition()),
								})
							},
							//No ChatterSource.ItemSelected
							{ ChatterSource.InvasionChanged,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("MustFight", new GoblinArmyInvasionChangedChatterCondition()),
								})
							},
							{ ChatterSource.BloodMoonChanged,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("BodyHurts", new BloodMoonChangedChatterCondition()),
								})
							},
						}
					}
				},
			};

			Order = new();
			foreach (var pair in GeneratorsPerType)
			{
				Order.Add(pair.Key);
			}
		}

		private void SetCooldownsForAll(ChatterSource source)
		{
			foreach (var gen in Generators)
			{
				//If this is lower than 1f, it sets the cooldowns of _other_ goblins to expire faster, effectively increasing chatter frequency for groups. 1f means it's as if only 1 goblin was chatting
				gen.PutMessageTypeOnCooldown(source, factor: 1f);
			}
		}

		public class ChatterTracker
		{
			public int total = 0;
			public Dictionary<GoblinUnderlingChatterType, Ref<int>> per = new();

			public void Count(GoblinUnderlingChatterType type, ChatterSource source)
			{
				total++;
				if (!per.ContainsKey(type))
				{
					per[type] = new Ref<int>(0);
				}
				per[type].Value++;
			}

			public void Report()
			{
				AssUtils.Print("Chatter per type. Total: " + total);
				if (total > 0)
				{
					foreach (var pair in per)
					{
						AssUtils.Print($"{pair.Key}: {pair.Value.Value} | {(pair.Value.Value / (float)total) * 100:F0}%");
					}
				}
			}
		}

		private void HandleMessageForAll(ChatterSource source, IChatterParams param = null)
		{
			foreach (var guChatterType in Order)
			{
				var proj = GoblinUnderlingHelperSystem.GetFirstGoblinUnderling(guChatterType);
				if (proj != null && GeneratorsPerType[guChatterType].TryCreate(Tracker, proj, source, param))
				{
					SetCooldownsForAll(source);
					Order = Order.OrderBy(_ => Main.rand.Next(100)).ToList();
					return;
				}
			}
		}

		public override void OnPlayerHurt(Player player, Entity entity, Player.HurtInfo hurtInfo)
		{
			HandleMessageForAll(ChatterSource.PlayerHurt, new PlayerHurtChatterParams(entity, hurtInfo));
		}

		public override void OnPlayerHurtByTrap(Player player, Projectile projectile, Player.HurtInfo hurtInfo)
		{
			HandleMessageForAll(ChatterSource.PlayerHurtByTrap, new PlayerHurtByTrapChatterParams(projectile, hurtInfo));
		}

		public override void OnArmorEquipped(Player player, EquipSnapshot equips, EquipSnapshot prevEquips)
		{
			HandleMessageForAll(ChatterSource.ArmorEquipped, new ArmorEquipChatterParams(equips, prevEquips));
		}

		public override void OnItemSelected(Player player, int itemType, int prevItemType)
		{
			HandleMessageForAll(ChatterSource.ItemSelected, new ItemSelectedChatterParams(itemType, prevItemType));
		}

		public override void OnInvasionChanged(int invasionType, int prevInvasionType)
		{
			HandleMessageForAll(ChatterSource.InvasionChanged, new InvasionChangedChatterParams(invasionType, prevInvasionType));
		}

		public override void OnBloodMoonChanged(bool bloodMoon, bool prevBloodMoon)
		{
			HandleMessageForAll(ChatterSource.BloodMoonChanged, new BloodMoonChangedChatterParams(bloodMoon, prevBloodMoon));
		}

		public override void OnSpawnedBoss(NPC npc, float distSQ)
		{
			HandleMessageForAll(ChatterSource.BossSpawn, new BossSpawnChatterParams(npc));
		}

		public override void OnSlainBoss(Player player, int type)
		{
			HandleMessageForAll(ChatterSource.BossDefeat, new BossDefeatChatterParams(type));
		}

		public override void OnOOAStarts(int forHowLong)
		{
			HandleMessageForAll(ChatterSource.OOAStarts);
		}

		public override void OnOOANewWave(int forHowLong)
		{
			HandleMessageForAll(ChatterSource.OOANewWave);
		}

		public bool OnAttacking(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (GetGeneratorForType(proj).TryCreate(Tracker, proj, ChatterSource.Attacking, new AttackingChatterParams(proj, target, hit, damageDone)))
			{
				SetCooldownsForAll(ChatterSource.Attacking);
				return true;
			}
			return false;
		}

		public bool OnIdle(Projectile proj)
		{
			if (GetGeneratorForType(proj).TryCreate(Tracker, proj, ChatterSource.Idle))
			{
				SetCooldownsForAll(ChatterSource.Idle);
				return true;
			}
			return false;
		}

		public bool OnFirstSummon(Projectile proj)
		{
			if (GetGeneratorForType(proj).TryCreate(Tracker, proj, ChatterSource.FirstSummon))
			{
				SetCooldownsForAll(ChatterSource.FirstSummon);
				return true;
			}
			return false;
		}

		public void PutIdleOnCooldown(Projectile proj)
		{
			GetGeneratorForType(proj).PutMessageTypeOnCooldown(ChatterSource.Idle);
		}

		public GoblinUnderlingChatterGenerator GetGeneratorForType(GoblinUnderlingProj mProj)
		{
			return GetGeneratorForType(mProj.Projectile);
		}

		public GoblinUnderlingChatterGenerator GetGeneratorForType(Projectile proj)
		{
			return GetGeneratorForType(GoblinUnderlingTierSystem.GoblinUnderlingProjs[proj.type]);
		}

		public GoblinUnderlingChatterGenerator GetGeneratorForType(GoblinUnderlingChatterType guChatterType)
		{
			return GeneratorsPerType[guChatterType];
		}
	}
}
