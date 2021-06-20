using System.Collections.Generic;

namespace AssortedCrazyThings.Base.SwarmDraw.FairySwarmDraw
{
    public class FairySwarmDrawSet : SwarmDrawSet
    {
        public FairySwarmDrawSet() : base("FairySwarmDrawSet", new List<SwarmDrawUnit>()
            {
                new FairySwarmDrawUnit(0),
                new FairySwarmDrawUnit(1),
                new FairySwarmDrawUnit(2),
                new FairySwarmDrawUnit(0),
                new FairySwarmDrawUnit(1),
                new FairySwarmDrawUnit(2)
            })
        {

        }

        public override SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer)
        {
            return sdPlayer.fairySwarmDrawSet;
        }
    }
}
