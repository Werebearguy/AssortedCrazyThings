using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    class AssModPlayer : ModPlayer
    {
        public bool everburningCandleBuff;

        public bool everburningCursedCandleBuff;

        public bool everfrozenCandleBuff;

        public bool variable_debuff_04;

        public bool variable_debuff_05;

        public bool everburningShadowflameCandleBuff;

        public bool variable_debuff_07;

        public override void ResetEffects()
        {
            everburningCandleBuff = false;
            everburningCursedCandleBuff = false;
            everfrozenCandleBuff = false;
            variable_debuff_04 = false;
            variable_debuff_05 = false;
            everburningShadowflameCandleBuff = false;
            variable_debuff_07 = false;
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            NPC npc = new NPC();
            if (victim is NPC)
            {
                npc = (NPC)victim;
            }
            if (everburningCandleBuff)
            {
                npc.AddBuff(BuffID.OnFire, 120, true);
            }
            if (everburningCursedCandleBuff)
            {
                npc.AddBuff(BuffID.CursedInferno, 120, true);
            }
            if (everfrozenCandleBuff)
            {
                npc.AddBuff(BuffID.Frostburn, 120, true);
            }
            if (variable_debuff_04)
            {
                npc.AddBuff(BuffID.Ichor, 120, true);
            }
            if (variable_debuff_05)
            {
                npc.AddBuff(BuffID.Venom, 120, true);
            }
            if (everburningShadowflameCandleBuff)
            {
                npc.AddBuff(BuffID.ShadowFlame, 60, true);
            }
            if (variable_debuff_07)
            {
                npc.AddBuff(BuffID.Bleeding, 120, true);
            }
        }
    }
}
