using AssortedCrazyThings.Base.SwarmDraw;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers
{
	/// <summary>
	/// These layers have to get created manually, they are loaded in SwarmDrawSet
	/// </summary>
	[Autoload(false)] //AssPlayerLayer already has false, but just for clarity
	public sealed class SwarmDrawLayer : AssPlayerLayer
	{
		private readonly string name;
		public override string Name => name;

		/// <summary>
		/// No parameterless constructor needed since Autoload is false
		/// </summary>
		public SwarmDrawLayer(string name, bool front, Func<SwarmDrawPlayer, SwarmDrawSet> getDrawSet)
		{
			//Important to assign name based on the instance so they won't count as duplicates
			this.name = $"{name}_{(front ? "Front" : "Back")}";
			this.front = front;
			this.getDrawSet = getDrawSet;
		}

		public bool front;

		public Func<SwarmDrawPlayer, SwarmDrawSet> getDrawSet;

		public sealed override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (getDrawSet == null || drawInfo.shadow != 0f || drawPlayer.dead)
			{
				return false;
			}

			return getDrawSet.Invoke(drawPlayer.GetModPlayer<SwarmDrawPlayer>())?.Active ?? false;
		}

		public sealed override Position GetDefaultPosition()
		{
			return front ? new AfterParent(PlayerDrawLayers.BeetleBuff) : new BeforeParent(PlayerDrawLayers.JimsCloak);
		}

		protected sealed override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			var set = getDrawSet?.Invoke(drawInfo.drawPlayer.GetModPlayer<SwarmDrawPlayer>());

			if (set == null || !set.Active)
			{
				return;
			}

			//First draw the trails
			var datas = set.TrailToDrawDatas(drawInfo, front);
			drawInfo.DrawDataCache.AddRange(datas);

			//Then the actual thing
			datas = set.ToDrawDatas(drawInfo, front);
			drawInfo.DrawDataCache.AddRange(datas);
		}
	}
}
