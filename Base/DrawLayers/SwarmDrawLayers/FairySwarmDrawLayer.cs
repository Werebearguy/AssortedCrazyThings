using AssortedCrazyThings.Base.SwarmDraw;

namespace AssortedCrazyThings.Base.DrawLayers.SwarmDrawLayers
{
    public class FairySwarmDrawLayer : SwarmDrawLayer
    {
        public FairySwarmDrawLayer(bool front) : base(front)
        {

        }

        public override SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer)
        {
            return sdPlayer.fairySwarmDrawSet;
        }
    }
}
