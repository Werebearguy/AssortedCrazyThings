using System.Collections.Generic;

namespace AssortedCrazyThings.Base.SwarmDraw.SwarmofCthulhuDraw
{
    public class SwarmofCthulhuDrawSet : SwarmDrawSet
    {
        public SwarmofCthulhuDrawSet() : base("SwarmofCthulhuDrawSet", new List<SwarmDrawUnit>()
            {
                new SwarmofCthulhuDrawUnit(),
                new SwarmofCthulhuDrawUnit(),
                new SwarmofCthulhuDrawUnit(),
            })
        {

        }

        public override SwarmDrawSet GetDrawSet(SwarmDrawPlayer sdPlayer)
        {
            return sdPlayer.swarmofCthulhuDrawSet;
        }
    }
}
