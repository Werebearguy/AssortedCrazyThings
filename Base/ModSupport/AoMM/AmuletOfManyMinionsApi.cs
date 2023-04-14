using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.ModSupport.AoMM
{
	// Copy this file for your mod, change the namespace above to yours, and read the comments
	/// <summary>
	/// Collection of utility methods that wrap the mod.Calls available from AoMM.
	/// </summary>
	[Content(ConfigurationSystem.AllFlags)]
	public class AmuletOfManyMinionsApi : AssSystem
	{
		//GENERAL INFO - PLEASE READ THIS FIRST!
		//-----------------------
		//https://github.com/westphallm1/AoMM-Cross-Mod-Sample#readme
		//
		//This file is kept up-to-date to the latest AoMM release. You are encouraged to not edit this file, and when an update happens, copy&replace this file again.
		//Nothing will happen if AoMM updates and your mod doesn't, it's your choice to update it further
		//-----------------------

		//This is the version of the calls that are used for the mod.
		//If AoMM updates, it will keep working on the outdated calls, but new features might not be available
		internal static readonly Version apiVersion = new Version(0, 16, 1);

		internal static string versionString;

		private static Mod aommMod;

		internal static Mod AommMod
		{
			get
			{
				if (!ContentConfig.Instance.AommSupport) { return null; }

				if (aommMod == null && ModLoader.TryGetMod("AmuletOfManyMinions", out var mod))
				{
					aommMod = mod;
				}
				return aommMod;
			}
		}

		public static LocalizedText AoMMVersionText { get; private set; }

		public static LocalizedText ConcatenateTwoText { get; private set; }

		public static LocalizedText AppendAoMMVersion(LocalizedText text)
		{
			return ConcatenateTwoText.WithFormatArgs(text, AoMMVersionText);
		}

		public override void Load()
		{
			versionString = apiVersion.ToString();
			AoMMVersionText = Language.GetOrRegister(Mod.GetLocalizationKey($"Common.AoMMVersion"));
			ConcatenateTwoText = Language.GetOrRegister(Mod.GetLocalizationKey($"Common.ConcatenateTwo"));
		}

		public override void Unload()
		{
			aommMod = null;
			versionString = null;
		}

		#region Calls
		/// <summary>
		/// Get the entire <key, object> mapping of the projectile's cross-mod exposed state, if it has one.
		/// See IAoMMState interface below for the names and types of the exposed state variables.
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for</param>
		internal static Dictionary<string, object> GetState(ModProjectile proj)
		{
			return AommMod?.Call("GetState", versionString, proj) as Dictionary<string, object>;
		}

		/// <summary>
		/// Attempt to fill the projectile's cross-mod exposed state directly into a destination object.
		/// The returned object will contain all AoMM state variables automatically cast to the correct type 
		/// (see IAoMMState interface below).
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for.</param>
		/// <param name="destination">The object to populate the projectile's cross mod state into.</param>
		/// <returns>True if AoMM is enabled and the projectile has an AoMM state attached, false otherwise.</returns>
		internal static bool TryGetStateDirect(ModProjectile proj, out IAoMMState destination)
		{
			destination = new AoMMStateImpl();
			AommMod?.Call("GetStateDirect", versionString, proj, destination);
			return destination != null;
		}

		/// <summary>
		/// Quick, non-reflective getter for the cross-mod IsActive flag. See the IAoMMCombatPetParams interface for more details.
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for</param>
		internal static bool IsActive(ModProjectile proj)
		{
			return (bool)(AommMod?.Call("IsActive", versionString, proj) ?? false);
		}

		/// <summary>
		/// Quick, non-reflective getter for the cross-mod IsIdle flag. See the CrossModState interface for more details.
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for</param>
		internal static bool IsIdle(ModProjectile proj)
		{
			return (bool)(AommMod?.Call("IsIdle", versionString, proj) ?? false);
		}

		/// <summary>
		/// Quick, non-reflective getter for the cross-mod IsAttacking flag. See the CrossModState interface for more details.
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for</param>
		internal static bool IsAttacking(ModProjectile proj)
		{
			return (bool)(AommMod?.Call("IsAttacking", versionString, proj) ?? false);
		}

		/// <summary>
		/// Quick, non-reflective getter for the cross-mod IsPathfinding flag. See the CrossModState interface for more details.
		/// </summary>
		/// <param name="proj">The ModProjectile to access the state for</param>
		internal static bool IsPathfinding(ModProjectile proj)
		{
			return (bool)(AommMod?.Call("IsPathfinding", versionString, proj) ?? false);
		}

		/// <summary>
		/// Get the <key, object> mapping of the parameters used to control this projectile's
		/// cross-mod behavior. See IAoMMParams interface below for the names and types of these parameters.
		/// </summary>
		/// <param name="proj">The ModProjectile to access the behavior parameters for.</param>
		internal static Dictionary<string, object> GetParams(ModProjectile proj)
		{
			return AommMod?.Call("GetParams", versionString, proj) as Dictionary<string, object>;
		}

		/// <summary>
		/// Attempt to fill the projectile's cross-mod behavior parameters directly into a destination object.
		/// The returned object will contain all AoMM parameters automatically cast to the correct type 
		/// (see IAoMMParams interface below).
		/// </summary>
		/// <param name="proj">The ModProjectile to access the behavior parameters for.</param>
		/// <param name="destination">The object to populate the projectile's behavior parameters into.</param>
		/// <returns>True if AoMM is enabled and the projectile has AoMM params attached, false otherwise.</returns>
		internal static bool TryGetParamsDirect(ModProjectile proj, out IAoMMParams destination)
		{
			destination = new AoMMParamsImpl();
			AommMod?.Call("GetParamsDirect", versionString, proj, destination);
			return destination != null;
		}

		/// <summary>
		/// Update the parameters used to control this projectile's cross mod behavior by passing
		/// in a <key, object> mapping of new parameter values. See IAoMMParams interface below for the names and 
		/// types of these parameters.
		/// </summary>
		/// <param name="proj">The ModProjectile to update the behavior parameters for.</param>
		/// <param name="update">A dictionary containing new behavior parameter values.</param>
		internal static void UpdateParams(ModProjectile proj, Dictionary<string, object> update)
		{
			AommMod?.Call("UpdateParams", versionString, proj, update);
		}

		/// <summary>
		/// Update the parameters used to control this projectile's cross mod behavior by passing
		/// in an object that implements the correct parameter names and types. See IAoMMParams interface below for 
		/// the names and types of these parameters.
		/// </summary>
		/// <param name="proj">The ModProjectile to update the behavior parameters for.</param>
		/// <param name="update">An object containing new behavior parameter values.</param>
		internal static void UpdateParamsDirect(ModProjectile proj, IAoMMParams update)
		{
			AommMod?.Call("UpdateParamsDirect", versionString, proj, update);
		}

		/// <summary>
		/// For the following frame, do not apply AoMM's pre-calculated position and velocity changes 
		/// to the projectile in PostAI(). Used to temporarily override behavior in fully managed minion AIs
		///
		/// Note: This call resets the projectile's position and velocity to their values at the beginning of
		/// PreAI, so it should be called before any changes to velocity occur in AI().
		/// </summary>
		/// <param name="proj">The ModProjectile to release for this frame</param>
		internal static void ReleaseControl(ModProjectile proj)
		{
			AommMod?.Call("ReleaseControl", versionString, proj);
		}


		/// <summary>
		/// Register a basic cross mod combat pet. AoMM will run its state calculations for this minion every frame,
		/// and take over its position and velocity while the pathfinding node is present.
		/// The pet's movement speed and search range will automatically scale with the player's combat
		/// pet level.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// </param>
		internal static void RegisterPathfindingPet(ModProjectile proj, ModBuff buff)
		{
			AommMod?.Call("RegisterPathfindingPet", versionString, proj, buff);
		}

		/// <summary>
		/// Register a fully managed flying cross mod combat pet. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
		/// The pet's damage, movement speed, and search range will automatically scale with the player's combat
		/// pet level.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
		/// <param name="defaultIdle">
		/// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
		/// while not attacking enemies.
		/// </param>
		internal static void RegisterFlyingPet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true)
		{
			AommMod?.Call("RegisterFlyingPet", versionString, proj, buff, projType, defaultIdle);
		}

		/// <summary>
		/// Register a fully managed grounded cross mod combat pet. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a basic grounded minion (eg. the Pirate staff).
		/// The pet's damage, movement speed, and search range will automatically scale with the player's combat
		/// pet level.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
		/// <param name="defaultIdle">
		/// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
		/// while not attacking enemies.
		/// </param>
		internal static void RegisterGroundedPet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true)
		{
			AommMod?.Call("RegisterGroundedPet", versionString, proj, buff, projType, defaultIdle);
		}

		/// <summary>
		/// Register a fully managed slime-style cross mod combat pet. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a slime pet (eg. the Slime Prince).
		/// The pet's damage, movement speed, and search range will automatically scale with the player's combat
		/// pet level. 
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack.</param>
		/// <param name="defaultIdle">
		/// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
		/// while not attacking enemies.
		/// </param>
		internal static void RegisterSlimePet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true)
		{
			AommMod?.Call("RegisterSlimePet", versionString, proj, buff, projType, defaultIdle);
		}

		/// <summary>
		/// Register a fully managed worm-style cross mod combat pet. AoMM will take over this projectile's 
		/// AI every frame, and will cause it to behave like a worm pet (eg. the Eater of Worms).
		/// The pet's damage, movement speed, and search range will automatically scale with the player's combat
		/// pet level. Note that the worm AI is intended for melee attacks, and will not move smoothly if
		/// set to fire a projectile.
		/// </summary>
		/// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
		/// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
		/// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack.</param>
		/// <param name="defaultIdle">
		/// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
		/// while not attacking enemies.
		/// </param>
		/// <param name="wormLength">
		/// The approximate length of the worm. Mostly cosmetic, used to determine radius of idling animation.
		/// </param>
		internal static void RegisterWormPet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true, int wormLength = 64)
		{
			AommMod?.Call("RegisterWormPet", versionString, proj, buff, projType, defaultIdle, wormLength);
		}

		/// <summary>
		/// Get the combat pet level of a player directly. Most stats on managed combat pets
		/// scale automatically with the player's combat pet level. 
		/// Pet Levels are as follows: 
		/// 0. Base power, similar in power to the Finch Staff 
		/// 1. Gold ore tier, similar in power to the Flinx Staff 
		/// 2. Evil ore tier, similar in power to Hornet Staff 
		/// 3. Dungeon tier, similar in power to the Imp Staff 
		/// 4. Early-hardmode tier, similar in power to the Spider Staff 
		/// 5. Hallowed bar tier, similar in power to the Optic Staff 
		/// 6. Post-Plantera Dungeon tier, similar in power to the Xeno Staff 
		/// 7. Lunar Pillar tier, similar in power to the Stardust Cell Staff 
		/// 8. Post-Moonlord tier, slightly stronger than the strongest vanilla minions
		/// </summary>
		/// <param name="player">The player whose combat pet level should be retrieved</param>
		/// <returns>The combat pet level of that player, based on the strongest pet emblem in their inventory</returns>
		internal static int GetPetLevel(Player player)
		{
			return ((int?)AommMod?.Call("GetPetLevel", versionString, player)) ?? 0;
		}
		#endregion
	}

	#region Auxiliary classes and interfaces
	/// <summary>
	/// Interface containing the names and types of the variables in the AoMM state.
	/// An object that implements this interface can be populated directly with a projectile's
	/// current AoMM state using mod.Call("GetStateDirect", versionString, projectile, stateImpl).  
	/// Values in the AoMM state are all read-only. For AoMM variables that can be both read and
	/// updated, see IAoMMParams. State is only calculated while the cross-mod AI is active. When
	/// accessing the cross-mod state while cross-mod AI is not active, all values will be `default`.
	/// </summary>
	public interface IAoMMState
	{
		/// <summary>
		/// How quickly the minion should change directions while moving. Higher values lead to
		/// slower turning. Updated automatically for pets, set in the mod.Call for minions.
		/// </summary>
		int Inertia { get; set; }

		/// <summary>
		/// Whether AoMM expects the minion to be attacking an enemy on the current frame. If this
		/// is true, TargetNPC will be non null.
		/// </summary>
		bool IsAttacking { get; set; }

		/// <summary>
		/// Whether AoMM expects the minion to be idling on the current frame.
		/// </summary>
		bool IsIdle { get; set; }

		/// <summary>
		/// Whether AoMM expects the minion to be following the pathfinder on the current frame.
		/// </summary>
		bool IsPathfinding { get; set; }

		/// <summary>
		/// Whether this projectile is being treated as a combat pet.
		/// </summary>
		bool IsPet { get; set; }

		/// <summary>
		/// Max travel speed for the minion. Updated automatically for pets, set in the 
		/// mod.Call for minions.
		/// </summary>
		int MaxSpeed { get; set; }

		/// <summary>
		/// The position of the next bend in the pathfinding path, based on the minion's current
		/// position.
		/// </summary>
		Vector2? NextPathfindingTaret { get; set; }

		/// <summary>
		/// The position of the end of the pathfinding path.
		/// </summary>
		Vector2? PathfindingDestination { get; set; }

		/// <summary>
		/// The current combat pet level of the player the projectile belongs to.
		/// </summary>
		int PetLevel { get; set; }

		/// <summary>
		/// All possible NPC targets, ordered by proximity to the most relevant target.
		/// </summary>
		List<NPC> PossibleTargetNPCs { get; set; }

		/// <summary>
		/// The range (in pixels) over which the tactic enemy selection should search. Updated
		/// automatically for pets, set in the mod.Call for minions.
		/// </summary>
		int SearchRange { get; set; }

		/// <summary>
		/// The NPC selected as most relevant based on the minion's current tactic and search range.
		/// This value is set as soon as a hostile NPC enters the minion's line of sight within
		/// its search range, and unset when that NPC dies, or line of sight is broken for several
		/// consecutive frames.
		/// </summary>
		NPC TargetNPC { get; set; }

		/// <summary>
		/// For managed cross-mod AIs that fire a projectile, true whenever an enemy is present,
		/// and within a close enough range to fire a projectile at. Always false for non-managed
		/// cross-mod AIs, and for managed melee cross-mod AIs. Can be used to implement custom
		/// behavior on top of a managed cross-mod minion's attacking AI.
		/// </summary>
		bool IsInFiringRange { get; set; }

		/// <summary>
		/// For managed cross-mod AIs that fire a projectile, true whenever the cross-mod AI has fired
		/// a projectile on the current frame. Always false for non-managed cross-mod AIs, and for managed 
		/// melee cross-mod AIs. Can be used to implement custom behavior on top of a managed cross-mod 
		/// minion's attacking AI. 
		/// Note: Only set to true on the client that owns the projectile.
		/// </summary>
		bool ShouldFireThisFrame { get; set; }
	}

	/// <summary>
	/// Utility class for accessing the AoMM state directly via
	/// mod.Call("GetStateDirect", versionString, projectile, stateImpl).  
	/// </summary>
	public class AoMMStateImpl : IAoMMState
	{
		public int MaxSpeed { get; set; }
		public int Inertia { get; set; }
		public int SearchRange { get; set; }
		public Vector2? NextPathfindingTaret { get; set; }
		public Vector2? PathfindingDestination { get; set; }
		public NPC TargetNPC { get; set; }
		public List<NPC> PossibleTargetNPCs { get; set; }
		public bool IsPet { get; set; }
		public int PetLevel { get; set; }
		public bool IsPathfinding { get; set; }
		public bool IsAttacking { get; set; }
		public bool IsIdle { get; set; }
		public bool IsActive { get; set; }
		public bool IsInFiringRange { get; set; }
		public bool ShouldFireThisFrame { get; set; }
	}


	/// <summary>
	/// Interface containing the names and types of the parameters used to determine the 
	/// behavior of managed minions and combat pets. These parameters are initially set in
	/// the registration mod.Call("RegisterXPet",...).
	/// 
	/// An object that implements this interface can be populated directly with a projectile's
	/// current AoMM parameters using mod.Call("GetParamsDirect", versionString, projectile, paramsImpl).  
	/// 
	/// The AI parameters of an active projectile are all read/write, and can be updated to match an 
	/// object that implements this interface using mod.Call("UpdateParamsDirect", versionString, projectile, paramsImpl).  
	/// 
	/// An additional interface is provided below for parameters that are only relevant to minions,
	/// as they are updated automatically for combat pets based on the player's pet level.
	/// </summary>
	public interface IAoMMCombatPetParams
	{
		/// <summary>
		/// The projectile that the minion or pet fires. If null, the minion will use a
		/// melee attack.
		/// </summary>
		int? FiredProjectileId { get; set; }

		/// <summary>
		/// Whether this projectile should currently have cross-mod AI applied.
		/// By default, this flag is managed by AoMM and is set to true under the following conditions:
		/// - For pets, as long as the associated cross-mod buff is active
		/// - For minions, as long as the associated cross-mod buff is active, and the minion was
		///   spawned from an item that provides that buff
		/// For simple use cases (most pets, and most minions that consist of a single projectile),
		/// this flag should be left as its default value.
		/// For more complicated use cases, such as a minion that is spawned as a sub-projectile of 
		/// another minion, this flag must be set manually.
		/// Once this flag has been set manually at least once for a projectile, AoMM will stop updating 
		/// it automatically, and it will maintain its latest set value. 
		/// </summary>
		bool IsActive { get; set; }

		/// <summary>
		/// How quickly this combat pet should turn, compared to the default combat pet AI.
		/// Lower values lead to faster turning. For best results, should be in the range of
		/// 0.5f to 1.5f. Default 1f. Has no effect on regular minion AI.
		/// </summary>
		float InertiaScaleFactor { get; set; }

		/// <summary>
		/// How quickly this combat pet should move, compared to the default combat pet AI.
		/// Higher values lead to faster movement speed. For best results, should be in the 
		/// range of 0.75f to 1.25f. Default 1f. Has no effect on regular minion AI.
		/// </summary>
		float MaxSpeedScaleFactor { get; set; }

		/// <summary>
		/// How quickly this combat pet should fire projectiles, compared to the default 
		/// combat pet AI. Lower values lead to a higher rate of fire. For best results, 
		/// should be in the range of 0.5f to 1.5f. Default 1f. Has no effect on regular 
		/// minion AI.
		/// </summary>
		float AttackFramesScaleFactor { get; set; }

		/// <summary>
		/// How fast the projectiles launched by this combat pet should travel, compared
		/// to the default combat pet AI. Higher values lead to a higher launch velocity. 
		/// For best results, should be in the range of 0.5f to 2f. Default 1f. Has no effect 
		/// on regular minion AI.
		/// </summary>
		float LaunchVelocityScaleFactor { get; set; }

		/// <summary>
		/// If this minion fires a projectile, it will attempt to position itself `PreferredTargetDistance`
		/// pixels away from the NPC that it is attacking. Has no effect on melee minions.
		/// </summary>
		int PreferredTargetDistance { get; set; }

	}

	/// <summary>
	/// Interface containing the names and types of the parameters used to determine the 
	/// behavior of managed minions and combat pets. These parameters are initially set in
	/// the registration mod.Call("RegisterXMinion",...).
	/// 
	/// An object that implements this interface can be populated directly with a projectile's
	/// current AoMM parameters using mod.Call("GetParamsDirect", versionString, projectile, paramsImpl).  
	/// 
	/// The AI parameters of an active projectile can be updated to match an object that implements
	/// this interface using mod.Call("UpdateParamsDirect", versionString, projectile, paramsImpl).  
	///
	/// The values in this interface are updated automatically for combat pets after a single frame,
	/// so must be set via mod.Call("UpdateParamsDirect") every frame to maintain a persistent value.
	/// </summary>
	public interface IAoMMParams : IAoMMCombatPetParams
	{
		/// <summary>
		/// How quickly the minion should change directions while moving. Higher values lead to
		/// slower turning. Only applies to minions, updated automatically every frame for combat
		/// pets.
		/// </summary>
		int Inertia { get; set; }

		/// <summary>
		/// Max travel speed for the minion. Only applies to minions, updated automatically every 
		/// frame for combat pets.
		/// </summary>
		int MaxSpeed { get; set; }

		/// <summary>
		/// The range (in pixels) over which the tactic enemy selection should search. Only applies 
		/// to minions, updated automatically every frame for combat pets.
		/// </summary>
		int SearchRange { get; set; }

		/// <summary>
		/// The projectile firing rate for the minion, if that minion fires a projectile. Only
		/// applies to projectile-firing minions. The attack speed of melee minions is derived
		/// from their movement speed. Updated automatically every frame for combat pets.
		/// </summary>
		int AttackFrames { get; set; }

		/// <summary>
		/// How fast the projectiles launched by this combat pet should travel, compared
		/// to the default combat pet AI. 
		/// </summary>
		float LaunchVelocity { get; set; }
	}

	public class AoMMParamsImpl : IAoMMParams
	{
		public bool IsActive { get; set; }
		public int Inertia { get; set; }
		public int MaxSpeed { get; set; }
		public int SearchRange { get; set; }
		public int AttackFrames { get; set; }
		public int? FiredProjectileId { get; set; }
		public float InertiaScaleFactor { get; set; }
		public float MaxSpeedScaleFactor { get; set; }
		public float AttackFramesScaleFactor { get; set; }
		public float LaunchVelocity { get; set; }
		public float LaunchVelocityScaleFactor { get; set; }
		public int PreferredTargetDistance { get; set; }
	}
	#endregion
}
