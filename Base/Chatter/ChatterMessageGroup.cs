using AssortedCrazyThings.Base.Chatter.Conditions;
using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	/// <summary>
	/// Handles picking a suitable message
	/// </summary>
	public class ChatterMessageGroup
	{
		//Current iteration of message picking algorithm is an initial randomizer, then sequenced and looped around. Loop always progresses regardless of meeting condition or not
		//An alternative (better than the current one) would be to randomize the order initially, go through messages in order, then if arrived at the end, randomize a new order again

		/// <summary>
		/// Manages providing a message (regardless of condition)
		/// </summary>
		public class Pool
		{
			//each message that matches the same condition goes into the same pool
			public List<ChatterMessage> Messages { get; private set; }
			public int VariationIndex { get; private set; }

			public Pool()
			{
				Messages = new List<ChatterMessage>();
			}

			public int Count => Messages.Count;

			public void RandomizeVariation()
			{
				VariationIndex = Main.rand.Next(Count);
			}

			public void ProgressVariation()
			{
				VariationIndex = (VariationIndex + 1) % Count;
			}

			public ChatterMessage GetMessage()
			{
				var message = Messages[VariationIndex];
				ProgressVariation();
				return message;
			}
		}

		/// <summary>
		/// Sorted by order in first parameter of <see cref="ChatterMessageGroup(List{ChatterMessage}, Func{int})"/>, grouping messages with the same condition together, with no conditions being last priority
		/// </summary>
		public List<Pool> PoolsByPriority { get; init; }

		/// <summary>
		/// Unsorted list of messages
		/// </summary>
		public IReadOnlyList<ChatterMessage> Messages { get; init; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messages">Order matters: Lower index = Higher priority</param>
		public ChatterMessageGroup(List<ChatterMessage> messages)
		{
			Messages = messages.AsReadOnly();
			if (messages == null || messages.Count == 0)
			{
				throw new Exception($"{nameof(messages)} has to be non-null and contain atleast one element");
			}

			var poolPriorities = new Dictionary<Type, int>();
			var pools = new Dictionary<Type, Pool>();
			PoolsByPriority = new();
			var alwaysTruePool = new Pool();
			for (int i = 0; i < messages.Count; i++)
			{
				var message = messages[i];

				Type condType = message.Condition.GetType();
				if (condType == typeof(AlwaysTrueChatterCondition) || message.AddedToAlwaysTruePool)
				{
					alwaysTruePool.Messages.Add(message);
					continue;
				}

				//First message gets to register priority (0 highest)
				if (!poolPriorities.ContainsKey(condType))
				{
					poolPriorities[condType] = pools.Count;
					pools[condType] = new Pool();
				}

				pools[condType].Messages.Add(message);
			}

			var PoolsByPriorityArray = new Pool[pools.Count];
			foreach (var pool in pools)
			{
				PoolsByPriorityArray[poolPriorities[pool.Key]] = pool.Value;
			}

			PoolsByPriority = new List<Pool>(PoolsByPriorityArray);
			PoolsByPriority.Add(alwaysTruePool); //By definition last priority
		}

		/// <summary>
		/// Assigns <paramref name="message"/> to a message if conditions meet
		/// </summary>
		public bool TryChooseMessage(ChatterSource source, IChatterParams param, out ChatterMessage message)
		{
			message = null;

			foreach (var pool in PoolsByPriority)
			{
				for (int i = 0; i < pool.Count; i++)
				{
					//Try out all messages in the pool, so that if one message does not meet conditions, it checks for the next one
					ChatterMessage chatterMessage = pool.GetMessage();
					if (chatterMessage.Condition.IsTrue(source, param))
					{
						message = chatterMessage;
						return true;
					}
				}
			}

			return false;
		}
	}
}
