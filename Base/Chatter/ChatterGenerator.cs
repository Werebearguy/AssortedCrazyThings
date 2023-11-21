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
		PlayerHurtByTrap,
		BossSpawn,
		BossDefeat,
		OOAStarts,
		OOANewWave,
		ArmorEquipped,
		ItemSelected,
		InvasionChanged,
		BloodMoonChanged,
		Dialogue,
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

		public Color Color { get; init; }

		public ChatterGenerator(string key, Color color)
		{
			Key = key;
			Color = color;

			MessageCooldownsBySource = new Dictionary<ChatterSource, Ref<float>>();
		}

		public Dictionary<ChatterSource, Ref<float>> MessageCooldownsBySource { get; init; }

		public void RegisterMessageGroup(ChatterSource source, ChatterMessageGroup messageGroup)
		{
			MessageCooldownsBySource[source] = new Ref<float>();
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

		public void PutMessageTypeOnCooldown(ChatterSource source, int? cooldownOverride = null, float factor = 1f)
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
			else
			{
				cd = ChatterSystem.SourceToCooldowns[source]();
			}

			cd = (int)(cd * factor);

			//Don't override long cooldown with short cooldown:
			if (cd > MessageCooldownsBySource[source].Value)
			{
				//AssUtils.Print($"g cd: {ChatterSystem.GlobalCooldownMax}, msg cd: {cd}");
				MessageCooldownsBySource[source] = new Ref<float>(cd);
			}
		}

		public bool MessageOnCooldown(ChatterSource source)
		{
			if (MessageCooldownsBySource.TryGetValue(source, out var cd))
			{
				return cd.Value > 0;
			}
			return false; //No cooldown: never on cooldown
		}

		/// <summary>
		/// Checks for cooldown aswell
		/// </summary>
		public bool TryCreate(ChatterSource source, Vector2 position, Vector2 velocity, IChatterParams param = null, int? cooldownOverride = null)
		{
			bool? ret = null;
			if (ChatterSystem.GlobalCooldown <= 0 && !MessageOnCooldown(source))
			{
				//Can return false if only conditionals in message group and no condition satisfied
				ret = Create(source, position, velocity, param, cooldownOverride);
			}

			//if (!ret.HasValue)
			//{
			//	if (ChatterSystem.GlobalCooldown > 0)
			//	{
			//		AssUtils.Print($"message on cooldown: g: {ChatterSystem.GlobalCooldown}");
			//	}
			//	if (MessageCooldownsBySource.TryGetValue(source, out var cd) && cd.Value > 0)
			//	{
			//		AssUtils.Print($"message on cooldown: msg: {cd.Value}");
			//	}
			//}
			//else if (ret.HasValue && !ret.Value)
			//{
			//	AssUtils.Print($"no messages to display");
			//}
			return ret ?? false;
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

		protected virtual void ModifyRequest(ChatterSource source, IChatterParams param, ref AdvancedPopupRequest request)
		{

		}

		public bool SpawnPreviousPopupText(ChatterSource source, IChatterParams param, Vector2 position, Vector2 velocity)
		{
			return SpawnPopupText(source, param, position, velocity, true);
		}

		protected bool SpawnPopupText(ChatterSource source, IChatterParams param, Vector2 position, Vector2 velocity, bool peekPrev = false)
		{
			if (!TryGetText(source, param, out string textForVariation, peekPrev))
			{
				return false;
			}

			AdvancedPopupRequest request = default;
			request.Text = textForVariation;
			//Eight symbols should equal to one second => 8 / 60 = 7.5
			float seconds = Math.Min(textForVariation.Length / 7.5f, 4);
			request.DurationInFrames = (int)(seconds * 60);
			request.Velocity = velocity;
			request.Color = Color;
			ModifyRequest(source, param, ref request);
			PopupText.NewText(request, position);
			return true;
		}

		private bool TryGetText(ChatterSource source, IChatterParams param, out string text, bool peekPrev = false)
		{
			text = string.Empty;
			if (Chatters.TryGetValue(source, out ChatterMessageGroup chatter) && chatter.TryChooseMessage(source, param, out var message, peekPrev))
			{
				text = message.ToString();
			}
			return !string.IsNullOrEmpty(text);
		}
	}
}
