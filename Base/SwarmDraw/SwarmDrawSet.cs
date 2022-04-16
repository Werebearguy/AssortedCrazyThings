using AssortedCrazyThings.Base.DrawLayers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.SwarmDraw
{
	/// <summary>
	/// This class works as follows:
	/// Classes extending from it create a parameterless constructor which fullfills this classes constructor.
	/// The PlayerDrawLayers added here have to NOT be autoloaded, they'll get added through ILoadable.Load automatically.
	/// Those instances are fetched on the SwarmDrawPlayer and cloned (so players don't share data between eachother)
	/// </summary>
	[Autoload(false, Side = ModSide.Client)]
	public abstract class SwarmDrawSet : ILoadable, ICloneable
	{
		private bool firstTick = true;

		public bool Active { get; private set; }

		public int Count => Units.Count;

		public List<SwarmDrawUnit> Units { get; private set; }

		public List<SwarmDrawLayer> Layers { get; private set; }

		public SwarmDrawSet(string name, List<SwarmDrawUnit> units)
		{
			Units = units;

			Layers = new List<SwarmDrawLayer>()
			{
				new SwarmDrawLayer(name, true, GetDrawSet),
				new SwarmDrawLayer(name, false, GetDrawSet)
			};
		}

		void ILoadable.Load(Mod mod)
		{
			foreach (var layer in Layers)
			{
				mod.AddContent(layer);
			}
		}

		void ILoadable.Unload()
		{
			Layers?.Clear();
			Layers = null;
		}

		public abstract SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer);

		public object Clone()
		{
			var clone = (SwarmDrawSet)MemberwiseClone();

			//Need to reinitialize the list with cloned data
			clone.Units = new List<SwarmDrawUnit>(Units.Select(u => (SwarmDrawUnit)u.Clone()));
			return clone;
		}

		public void Update(Vector2 center)
		{
			if (!Active)
			{
				return;
			}

			if (firstTick)
			{
				firstTick = false;

				foreach (var unit in Units)
				{
					unit.OnSpawn();
				}
			}

			foreach (var unit in Units)
			{
				unit.Update(center);
			}
		}

		public void Activate()
		{
			Active = true;
			firstTick = true;
		}

		public void Deactivate()
		{
			Active = false;
		}

		public List<DrawData> ToDrawDatas(PlayerDrawSet drawInfo, bool front)
		{
			var data = new List<DrawData>();
			foreach (var unit in Units)
			{
				data.AddRange(unit.ToDrawDatas(drawInfo, front));
			}
			return data;
		}

		public List<DrawData> TrailToDrawDatas(PlayerDrawSet drawInfo, bool front)
		{
			var data = new List<DrawData>();
			foreach (var unit in Units)
			{
				data.AddRange(unit.TrailToDrawDatas(drawInfo, front));
			}
			return data;
		}

		public static T New<T>() where T : SwarmDrawSet
		{
			return (T)ModContent.GetInstance<T>().Clone();
		}
	}
}
