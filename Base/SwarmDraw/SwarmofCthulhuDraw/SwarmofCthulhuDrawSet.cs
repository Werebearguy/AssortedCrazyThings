using AssortedCrazyThings.Base.DrawLayers.SwarmDrawLayers;
using System.Collections.Generic;

namespace AssortedCrazyThings.Base.SwarmDraw.SwarmofCthulhuDraw
{
    public class SwarmofCthulhuDrawSet : SwarmDrawSet
    {
        public SwarmofCthulhuDrawSet() : base(new List<SwarmDrawUnit>()
            {
                new SwarmofCthulhuDrawUnit(),
                new SwarmofCthulhuDrawUnit(),
                new SwarmofCthulhuDrawUnit(),
            }, new SwarmofCthulhuDrawLayer(true), new SwarmofCthulhuDrawLayer(false))
        {

        }
    }
}
