using AssortedCrazyThings.Base.SwarmDraw;

namespace AssortedCrazyThings.Base.DrawLayers.SwarmDrawLayers
{
    public class SwarmofCthulhuDrawLayer : SwarmDrawLayer
    {
        public SwarmofCthulhuDrawLayer(bool front) : base(front)
        {

        }

        public override SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer)
        {
            return sdPlayer.swarmofCthulhuDrawSet;
        }
    }
}
