using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace AssortedCrazyThings.Base.SwarmDraw
{
    public class SwarmDrawSet
    {
        private bool firstTick = true;

        public bool Active { get; private set; }

        public int Count => Units.Count;

        public List<SwarmDrawUnit> Units { get; private set; }

        public SwarmDrawSet(List<SwarmDrawUnit> units)
        {
            Units = units;
        }

        public void Update(Vector2 center)
        {
            if (!Active)
            {
                return;
            }

            if (firstTick)
            {
                firstTick = false;

                foreach (var unit in Units)
                {
                    unit.OnSpawn();
                }
            }

            foreach (var unit in Units)
            {
                unit.Update(center);
            }
        }

        public void Activate()
        {
            Active = true;
            firstTick = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public List<DrawData> ToDrawDatas(PlayerDrawSet drawInfo, bool front)
        {
            var data = new List<DrawData>();
            foreach (var unit in Units)
            {
                data.AddRange(unit.ToDrawDatas(drawInfo, front));
            }
            return data;
        }

        public List<DrawData> TrailToDrawDatas(PlayerDrawSet drawInfo, bool front)
        {
            var data = new List<DrawData>();
            foreach (var unit in Units)
            {
                data.AddRange(unit.TrailToDrawDatas(drawInfo, front));
            }
            return data;
        }
    }
}
