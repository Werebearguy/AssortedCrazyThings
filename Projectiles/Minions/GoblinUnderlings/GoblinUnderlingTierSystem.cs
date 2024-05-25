using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using AssortedCrazyThings.Base.Handlers.ProgressionTierHandler;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Weapons;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingTierSystem : ProgressionTierSystem
	{
		public static Dictionary<int, GoblinUnderlingChatterType> GoblinUnderlingProjs { get; private set; }

		private static Dictionary<GoblinUnderlingClass, Dictionary<ProgressionTierStage, GoblinUnderlingTierStats>> tierStats = new();

		public static ReadOnlySpan<ProgressionTierStage> GetTiers()
		{
			return ModContent.GetInstance<GoblinUnderlingTierSystem>().TierSet.Tiers;
		}

		public static IReadOnlyDictionary<ProgressionTierStage, GoblinUnderlingTierStats> GetTierStats(GoblinUnderlingClass @class)
		{
			return tierStats[@class];
		}

		public static void RegisterStats(GoblinUnderlingClass @class, Dictionary<ProgressionTierStage, GoblinUnderlingTierStats> stats)
		{
			tierStats[@class] = stats;
		}

		public static GoblinUnderlingTierStats GetCurrentTierStats(GoblinUnderlingClass @class)
		{
			return tierStats[@class][ModContent.GetInstance<GoblinUnderlingTierSystem>().CurrentTier];
		}

		protected override ProgressionTierSet GetTierSet()
		{
			return new ProgressionTierSet(new ProgressionTierStage[]
			{
				ProgressionTierStage.PreBoss,
				ProgressionTierStage.EoC,
				ProgressionTierStage.Evil,
				ProgressionTierStage.Skeletron,

				ProgressionTierStage.WoF,
				ProgressionTierStage.Mech,
				ProgressionTierStage.Plantera,
				ProgressionTierStage.Cultist
			});
		}

		public override void SafeOnModLoad()
		{
			GoblinUnderlingProjs = new();
		}

		public override void Unload()
		{
			GoblinUnderlingProjs = null;

			tierStats = null;
		}

		public override void PostSetupContent()
		{
			var tierStats = new Dictionary<ProgressionTierStage, GoblinUnderlingTierStats>
			{
				//PreBoss = Baseline values in Item/AI code																									   //dmg    kb    ap  sp     m  hb  ran   ransp ranmp
				{ ProgressionTierStage.PreBoss   , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponDart_0>()   , 1f   , 1f  , 0 , 0.3f , 6, 0 , 1.5f, 8f , 1f) },
				{ ProgressionTierStage.EoC       , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponDart_1>()   , 1.40f, 1.2f, 0 , 0.35f, 6, 2 , 1.5f, 9f , 1.2f) },
				{ ProgressionTierStage.Evil      , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponDart_2>()   , 1.60f, 1.4f, 5 , 0.4f , 6, 4 , 1.5f, 10f, 1.3f) },
				{ ProgressionTierStage.Skeletron , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponDart_3>()   , 1.70f, 1.6f, 5 , 0.45f, 5, 6 , 1.5f, 11f, 1.4f) },
				
				{ ProgressionTierStage.WoF       , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponDart_3>()   , 2.4f , 1.8f, 10, 0.45f, 5, 6 , 1.5f, 11f, 1.6f) }, //Mostly a copy of previous tier with more damage, same visuals too
				{ ProgressionTierStage.Mech      , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponDart_4>()   , 2.8f , 2.0f, 10, 0.6f , 5, 6 , 1.5f, 12f, 1.7f) },
				{ ProgressionTierStage.Plantera  , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponTerraBeam>(), 3.0f , 2.2f, 10, 0.7f , 4, 10, 1f  , 14f, 2f, -1, 0, showMeleeDuringRanged: true) },
				{ ProgressionTierStage.Cultist   , new GoblinUnderlingMeleeTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponDaybreak>() , 3.1f , 2.4f, 10, 0.8f , 4, 10, 1f  , 16f, 2f, showMeleeDuringRanged: false, rangedOnly: true) },
			};
			RegisterStats(GoblinUnderlingClass.Melee, tierStats);

			tierStats = new Dictionary<ProgressionTierStage, GoblinUnderlingTierStats>
			{
				//Keep in mind the dps numbers of the respective debuff inflicted:
				//onfire 4
				//frostburn 8
				//shadowflame 15
				//hellfire 15
				//frostbite 25
				//cursed inferno 24

				//PreBoss = Baseline values in Item/AI code																								   //dmg   kb    ap  sp     m  ransp ranmp dur rad
				{ ProgressionTierStage.PreBoss   , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_0>(), 1.8f, 1f  , 0 , 0.3f , 12, 8f , 1f  , 180, 48) },
				{ ProgressionTierStage.EoC       , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_0>(), 2.7f, 1.2f, 0 , 0.35f, 12, 9f , 1.2f, 240, 48 + 8) },
				{ ProgressionTierStage.Evil      , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_1>(), 2.7f, 1.4f, 5 , 0.4f , 11, 10f, 1.3f, 300, 48 + 16) },
				{ ProgressionTierStage.Skeletron , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_2>(), 2.6f, 1.6f, 5 , 0.45f, 11, 11f, 1.4f, 360, 48 + 24) },

				{ ProgressionTierStage.WoF       , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_3>(), 4.0f, 1.6f, 10, 0.45f, 10, 11f, 1.6f, 180, 48 + 48) },
				{ ProgressionTierStage.Mech      , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_4>(), 4.4f, 1.8f, 10, 0.6f , 10, 12f, 1.7f, 240, 48 + 56) },
				{ ProgressionTierStage.Plantera  , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_5>(), 5.2f, 2f  , 10, 0.7f , 9 , 14f, 2f  , 300, 48 + 64) },
				{ ProgressionTierStage.Cultist   , new GoblinUnderlingMagicTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponOrb_6>(), 5.8f, 2.2f, 10, 0.8f , 8 , 16f, 2f  , 360, 48 + 72) },
			};
			RegisterStats(GoblinUnderlingClass.Magic, tierStats);

			tierStats = new Dictionary<ProgressionTierStage, GoblinUnderlingTierStats>
			{
				//PreBoss = Baseline values in Item/AI code																									  //dmg   kb    ap  sp     m ransp ranmp
				{ ProgressionTierStage.PreBoss   , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponArrow_0>(), 1.2f , 1f  , 2 , 0.25f, 8, 9f  , 1.4f) },
				{ ProgressionTierStage.EoC       , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponArrow_1>(), 1.5f , 1.2f, 4 , 0.3f , 8, 10f , 1.6f) },
				{ ProgressionTierStage.Evil      , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponArrow_2>(), 1.65f, 1.4f, 6 , 0.35f, 7, 5.5f, 1.7f, -1, 0) },
				{ ProgressionTierStage.Skeletron , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponArrow_3>(), 1.8f , 1.6f, 8 , 0.4f , 7, 12f , 1.8f) },
				
				{ ProgressionTierStage.WoF       , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponArrow_3>(), 2.6f , 1.6f, 10, 0.4f , 6, 13f , 1.9f) }, //Mostly a copy of previous tier with more damage, same visuals too
				{ ProgressionTierStage.Mech      , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponArrow_4>(), 3.0f , 1.8f, 10, 0.5f , 5, 14f , 2.0f) },
				{ ProgressionTierStage.Plantera  , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponArrow_5>(), 3.0f , 2f  , 10, 0.6f , 4, 15f , 2.2f, GoblinUnderlingWeaponArrow_5.Gravity, GoblinUnderlingWeaponArrow_5.TicksWithoutGravity) },
				{ ProgressionTierStage.Cultist   , new GoblinUnderlingRangedTierStats(ModContent.ProjectileType<GoblinUnderlingWeaponBlaster>(), 3.4f , 2f  , 10, 0.7f , 3, 16f , 2.4f, -1, 0) },
			};
			RegisterStats(GoblinUnderlingClass.Ranged, tierStats);
		}
	}
}
