using AssortedCrazyThings.Base.Chatter.Conditions;
using AssortedCrazyThings.Base.Data;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter.GoblinUnderlings
{
	public enum GoblinUnderlingChatterType
	{
		None,
		Eager,
	}

	//TODO goblin Do proper dispatching here for multiple goblins. Currently just spawn on all active (which will spawn only 1 message on the first active one due to global cooldown)
	//Maybe global cooldown needs to be per generator

	//Special handler which has more than one generator, as it needs to manage all of them as a group
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingChatterHandler : ChatterHandler
	{
		private Dictionary<GoblinUnderlingChatterType, GoblinUnderlingChatterGenerator> GeneratorsPerType { get; init; }

		public override IEnumerable<ChatterGenerator> Generators => GeneratorsPerType.Values;

		public GoblinUnderlingChatterHandler() : base(ChatterType.GoblinUnderling)
		{
			GeneratorsPerType = new Dictionary<GoblinUnderlingChatterType, GoblinUnderlingChatterGenerator>()
			{
				{ GoblinUnderlingChatterType.Eager,
					new GoblinUnderlingChatterGenerator(GoblinUnderlingChatterType.Eager.ToString())
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
								}, () => Main.rand.Next(20, 40) * 60)
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

									//TODO goblin class condition
									//new ChatterMessage("StabStab", new melee(), true), //"Stab stab stab!"
									//new ChatterMessage("Boom", new mage(), true), //"Boom! Haha!" 
									//new ChatterMessage("Skewer", new ranged(), true), //"Gonna skewer ya!" 
								}, () => 30 * 60)
							},
							{ ChatterSource.PlayerHurt,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("DontHurt"),
									new ChatterMessage("YouWillPay"),
									new ChatterMessage("YouGood"),
									new ChatterMessage("ThatHurt"),
									new ChatterMessage("IsntWeak"),
								}, () => 60 * 60)
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
								}, () => 10 * 60)
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
								}, () => 10 * 60)
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
								}, () => 15 * 60)
							},
							{ ChatterSource.ItemSelected,
								new ChatterMessageGroup(new List<ChatterMessage>()
								{
									new ChatterMessage("IsAToy", new SelectPearlwoodSwordChatterCondition()),
								}, () => 15 * 60)
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
				}
			};
		}

		public override void OnPlayerHurt(Player player, Entity entity, Player.HurtInfo hurtInfo)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.PlayerHurt, new PlayerHurtChatterParams(entity, hurtInfo));
				}
			}
		}

		public override void OnArmorEquipped(Player player, EquipSnapshot equips, EquipSnapshot prevEquips)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.ArmorEquipped, new ArmorEquipChatterParams(equips, prevEquips));
				}
			}
		}

		public override void OnItemSelected(Player player, int itemType, int prevItemType)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.ItemSelected, new ItemSelectedChatterParams(itemType, prevItemType));
				}
			}
		}

		public override void OnInvasionChanged(int invasionType, int prevInvasionType)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.InvasionChanged, new InvasionChangedChatterParams(invasionType, prevInvasionType));
				}
			}
		}

		public override void OnBloodMoonChanged(bool bloodMoon, bool prevBloodMoon)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.BloodMoonChanged, new BloodMoonChangedChatterParams(bloodMoon, prevBloodMoon));
				}
			}
		}

		public override void OnSpawnedBoss(NPC npc, float distSQ)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.BossSpawn, new BossSpawnChatterParams(npc));
				}
			}
		}

		public override void OnSlainBoss(Player player, int type)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.BossDefeat, new BossDefeatChatterParams(type));
				}
			}
		}

		public override void OnOOAStarts(int forHowLong)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.OOAStarts);
				}
			}
		}

		public override void OnOOANewWave(int forHowLong)
		{
			foreach (var pair in GeneratorsPerType)
			{
				foreach (var proj in GoblinUnderlingHelperSystem.GetLocalGoblinUnderlings(pair.Key))
				{
					pair.Value.TryCreate(proj, ChatterSource.OOANewWave);
				}
			}
		}

		public bool OnAttacking(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			return GetGeneratorForType(proj).TryCreate(proj, ChatterSource.Attacking, new AttackingChatterParams(target, hit, damageDone));
		}

		public bool OnIdle(Projectile proj)
		{
			return GetGeneratorForType(proj).TryCreate(proj, ChatterSource.Idle);
		}

		public void PutIdleOnCooldown(Projectile proj)
		{
			GetGeneratorForType(proj).PutMessageTypeOnCooldown(ChatterSource.Idle);
		}

		public bool OnFirstSummon(Projectile proj)
		{
			return GetGeneratorForType(proj).TryCreate(proj, ChatterSource.FirstSummon);
		}

		public GoblinUnderlingChatterGenerator GetGeneratorForType(EagerUnderlingProj mProj)
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
