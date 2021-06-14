using AssortedCrazyThings.Base.SwarmDraw;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers.SwarmDrawLayers
{
    /// <summary>
    /// These layers have to get created manually, they are loaded in SwarmDrawSet
    /// </summary>
    [Autoload(false)]
    public abstract class SwarmDrawLayer : PlayerDrawLayer
    {
        /// <summary>
        /// Important to assign Name based on the instance so they won't count as duplicates
        /// </summary>
        public override string Name => $"{base.Name}_{(front ? "Front" : "Back")}";

        /// <summary>
        /// No parameterless constructor needed since Autoload is false
        /// </summary>
        public SwarmDrawLayer(bool front)
        {
            this.front = front;
        }

        public bool front;

        public abstract SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer);

        public sealed override bool GetDefaultVisiblity(PlayerDrawSet drawInfo)
        {
            return GetDrawSet(drawInfo.drawPlayer.GetModPlayer<SwarmDrawPlayer>())?.Active ?? false;
        }

        public sealed override Position GetDefaultPosition()
        {
            return front ? new AfterParent(PlayerDrawLayers.BeetleBuff) : new BeforeParent(PlayerDrawLayers.JimsCloak);
        }

        protected sealed override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || drawPlayer.dead)
            {
                return;
            }

            var set = GetDrawSet(drawPlayer.GetModPlayer<SwarmDrawPlayer>());

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
