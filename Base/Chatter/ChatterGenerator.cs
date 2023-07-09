using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	public enum ChatterType
	{
		GoblinUnderling,
	}

	public enum ChatterSource
	{
		Idle,
		FirstSummon,
		Attacking,
		PlayerHurt,
		BossSpawn,
		BossDefeat,
		OOAStarts,
		OOANewWave,
		ArmorEquipped,
		ItemSelected,
		InvasionChanged,
		BloodMoonChanged,
	}

	/// <summary>
	/// Contains all messages, manages cooldowns and spawning
	/// </summary>
	public class ChatterGenerator
	{
		//Does not necessarily cover all ChatterSource
		public Dictionary<ChatterSource, ChatterMessageGroup> Chatters { get; init; }

		/// <summary>
		/// Unique key ("internal name")
		/// </summary>
		public string Key { get; init; }

		public ChatterGenerator(string key)
		{
			Key = key;

			MessageCooldownsBySource = new Dictionary<ChatterSource, Ref<float>>();
			SourceToCooldowns = new Dictionary<ChatterSource, Func<int>>();
		}

		public Dictionary<ChatterSource, Ref<float>> MessageCooldownsBySource { get; init; }
		public Dictionary<ChatterSource, Func<int>> SourceToCooldowns { get; init; }

		public void RegisterMessageGroup(ChatterSource source, ChatterMessageGroup messageGroup)
		{
			MessageCooldownsBySource[source] = new Ref<float>();
			SourceToCooldowns[source] = messageGroup.Cooldown;
		}

		public void OnEnterWorld(Player player)
		{
			foreach (var chatter in Chatters.Values)
			{
				foreach (var pool in chatter.PoolsByPriority)
				{
					pool.RandomizeVariation();
				}
			}

			//No immediate message on spawn
			PutMessageTypeOnCooldown(ChatterSource.Idle);
		}

		public void UpdateCooldowns(float reduceAmount)
		{
			foreach (var pair in MessageCooldownsBySource)
			{
				var cd = pair.Value;
				if (cd.Value > 0)
				{
					cd.Value -= reduceAmount;
					if (cd.Value < 0)
					{
						cd.Value = 0;
					}
				}
			}
		}

		public void PutMessageTypeOnCooldown(ChatterSource source, int? cooldownOverride = null)
		{
			if (!MessageCooldownsBySource.ContainsKey(source))
			{
				//Only add cooldown if key exists
				return;
			}

			int cd = 0;
			if (cooldownOverride.HasValue)
			{
				cd = cooldownOverride.Value;
			}
			else if (Chatters.TryGetValue(source, out ChatterMessageGroup group))
			{
				cd = group.Cooldown();
			}

			MessageCooldownsBySource[source] = new Ref<float>(cd);
		}

		/// <summary>
		/// Checks for cooldown aswell
		/// </summary>
		public bool TryCreate(ChatterSource source, Vector2 position, Vector2 velocity, IChatterParams param = null, int? cooldownOverride = null)
		{
			if (ChatterSystem.GlobalCooldown <= 0 && MessageCooldownsBySource.TryGetValue(source, out var cd) && cd.Value <= 0)
			{
				Create(source, position, velocity, param, cooldownOverride);
				return true;
			}

			return false;
		}

		protected bool Create(ChatterSource source, Vector2 position, Vector2 velocity, IChatterParams param = null, int? cooldownOverride = null)
		{
			param ??= new DefaultChatterParams();
			if (SpawnPopupText(source, param, position, velocity))
			{
				ChatterSystem.SetGlobalCooldown();
				PutMessageTypeOnCooldown(source, cooldownOverride);
				return true;
			}

			return false;
		}

		protected bool SpawnPopupText(ChatterSource source, IChatterParams param, Vector2 position, Vector2 velocity)
		{
			if (!TryGetText(source, param, out string textForVariation))
			{
				return false;
			}

			AdvancedPopupRequest request = default;
			request.Text = textForVariation;
			//Eight symbols should equal to one second => 8 / 60 = 7.5
			float seconds = Math.Min(textForVariation.Length / 7.5f, 4);
			request.DurationInFrames = (int)(seconds * 60);
			request.Velocity = velocity;
			request.Color = new Color(125, 217, 124);
			PopupText.NewText(request, position);
			return true;
		}

		private bool TryGetText(ChatterSource source, IChatterParams param, out string text)
		{
			text = string.Empty;
			if (Chatters.TryGetValue(source, out ChatterMessageGroup chatter) && chatter.TryChooseMessage(source, param, out var message))
			{
				text = message.ToString();
			}
			return !string.IsNullOrEmpty(text);
		}
	}
}
