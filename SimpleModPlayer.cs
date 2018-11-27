using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01
{
	class SimpleModPlayer : ModPlayer
	{
		public bool variable_debuff_01;
		public bool variable_debuff_02;
		public bool variable_debuff_03;
		public bool variable_debuff_04;
		public bool variable_debuff_05;
		public bool variable_debuff_06;
		public bool variable_debuff_07;
		
			public override void ResetEffects()
				{
					variable_debuff_01 = false;
					variable_debuff_02 = false;
					variable_debuff_03 = false;
					variable_debuff_04 = false;
					variable_debuff_05 = false;
					variable_debuff_06 = false;
					variable_debuff_07 = false;
				}
			public override void OnHitAnything(float x, float y, Entity victim)
				{
					NPC npc = new NPC();
					if (victim is NPC)
					{
						npc = (NPC)victim;
					}
					if(variable_debuff_01)
					{
					npc.AddBuff(BuffID.OnFire, 120, true);
					}
					if(variable_debuff_02)
					{
					npc.AddBuff(BuffID.CursedInferno, 120, true);
					}
					if(variable_debuff_03)
					{
					npc.AddBuff(BuffID.Frostburn, 120, true);
					}
					if(variable_debuff_04)
					{
					npc.AddBuff(BuffID.Ichor, 120, true);
					}
					if(variable_debuff_05)
					{
					npc.AddBuff(BuffID.Venom, 120, true);
					}
					if(variable_debuff_06)
					{
					npc.AddBuff(BuffID.ShadowFlame, 60, true);
					}
					if(variable_debuff_07)
					{
					npc.AddBuff(BuffID.Bleeding, 120, true);
					}
				}
	}
}
