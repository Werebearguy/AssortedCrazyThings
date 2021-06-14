using AssortedCrazyThings.Base.SwarmDraw;

namespace AssortedCrazyThings.Base.DrawLayers.SwarmDrawLayers
{
    public abstract class FairySwarmDrawLayer : SwarmDrawLayerBase
    {
        public override SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer)
        {
            return sdPlayer.fairySwarmDrawSet;
        }
    }

    public class FairySwarmDrawLayer_Front : FairySwarmDrawLayer
    {
        public override bool Front => true;
    }

    public class FairySwarmDrawLayers_Back : FairySwarmDrawLayer
    {
        public override bool Front => false;
    }
}
