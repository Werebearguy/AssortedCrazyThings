using AssortedCrazyThings.Base.SwarmDraw;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers.SwarmDrawLayers
{
    public abstract class SwarmDrawLayerBase : PlayerDrawLayer
    {
        public abstract bool Front { get; }

        public abstract SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer);

        public sealed override bool GetDefaultVisiblity(PlayerDrawSet drawInfo)
        {
            return GetDrawSet(drawInfo.drawPlayer.GetModPlayer<SwarmDrawPlayer>())?.Active ?? false;
        }

        public sealed override Position GetDefaultPosition()
        {
            return Front ? new AfterParent(PlayerDrawLayers.BeetleBuff) : new BeforeParent(PlayerDrawLayers.JimsCloak);
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
            var datas = set.TrailToDrawDatas(drawInfo, Front);
            drawInfo.DrawDataCache.AddRange(datas);

            //Then the actual thing
            datas = set.ToDrawDatas(drawInfo, Front);
            drawInfo.DrawDataCache.AddRange(datas);
        }
    }
}
