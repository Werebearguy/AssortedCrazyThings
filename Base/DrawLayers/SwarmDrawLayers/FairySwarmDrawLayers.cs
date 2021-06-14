using AssortedCrazyThings.Base.SwarmDraw;

namespace AssortedCrazyThings.Base.DrawLayers.SwarmDrawLayers
{
    public abstract class SwarmofCthulhuDrawLayer : SwarmDrawLayerBase
    {
        public override SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer)
        {
            return sdPlayer.swarmofCthulhuDrawSet;
        }
    }

    public class SwarmofCthulhuDrawLayer_Front : SwarmofCthulhuDrawLayer
    {
        public override bool Front => true;
    }

    public class SwarmofCthulhuDrawLayers_Back : SwarmofCthulhuDrawLayer
    {
        public override bool Front => false;
    }
}
