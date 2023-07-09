using AssortedCrazyThings.Base.Data;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Chatter
{
	/// <summary>
	/// Provides hooks for events to spawn messages for. Unique per <see cref="ChatterType"/>. Can contain one or more <see cref="ChatterGenerator"/>
	/// </summary>
	[Autoload(false)] //Handled by configuration system
	public abstract class ChatterHandler : ModType
	{
		public ChatterType ChatterType { get; init; }

		//Used for maintaining/updating
		public abstract IEnumerable<ChatterGenerator> Generators { get; }

		// Inheriting classes need a parameterless constructor for autoloading
		public ChatterHandler(ChatterType chatterType)
		{
			ChatterType = chatterType;
		}

		protected sealed override void Register()
		{
			foreach (var gen in Generators)
			{
				foreach (var pair in gen.Chatters)
				{
					gen.RegisterMessageGroup(pair.Key, pair.Value);
				}
			}

			ChatterHandlerLoader.Register(this);
		}

		public sealed override void SetupContent()
		{
			SetStaticDefaults();
		}

		public void OnEnterWorld(Player player)
		{
			foreach (var gen in Generators)
			{
				gen.OnEnterWorld(player);
			}
		}

		public virtual void OnPlayerHurt(Player player, Entity entity, Player.HurtInfo hurtInfo)
		{

		}

		public virtual void OnArmorEquipped(Player player, EquipSnapshot equips, EquipSnapshot prevEquips)
		{

		}

		public virtual void OnItemSelected(Player player, int itemType, int prevItemType)
		{

		}

		public virtual void OnInvasionChanged(int invasionType, int prevInvasionType)
		{

		}

		public virtual void OnBloodMoonChanged(bool bloodMoon, bool prevBloodMoon)
		{

		}

		public virtual void OnSlainBoss(Player player, int type)
		{

		}

		public virtual void OnSpawnedBoss(NPC npc, float distSQ)
		{

		}

		public virtual void OnOOAStarts(int forHowLong)
		{

		}

		public virtual void OnOOANewWave(int forHowLong)
		{

		}
	}
}
