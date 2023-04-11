using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.NPCs
{
	[Autoload(false)]
	public class NeurotoxinBuff : AssBuff
	{
		public override string Texture => "AssortedCrazyThings/Buffs/NPCs/NeurotoxinBuff";

		public int DPS { get; init; }
		public int Tier { get; init; }

		public override string Name => $"{nameof(NeurotoxinBuff)}_{Tier}";

		public NeurotoxinBuff(int dps, int tier)
		{
			DPS = dps;
			Tier = tier;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<NeurotoxinGlobalNPC>().dps = DPS;
		}
	}

	//Load every tier as a separate buff to apply
	[Content(ContentType.AommSupport | ContentType.OtherPets)]
	public class NeurotoxinLoader : AssSystem
	{
		//See Api.GetPetLevel for progression reference
		public const int TierCount = 9;

		public override void OnModLoad()
		{
			for (int i = 0; i < TierCount; i++)
			{
				//Final tier will be 56
				Mod.AddContent(new NeurotoxinBuff((i + 1) * 6, i));
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.OtherPets)]
	public class NeurotoxinGlobalNPC : AssGlobalNPC
	{
		public override bool InstancePerEntity => true;

		public int dps = 0;

		public override void ResetEffects(NPC npc)
		{
			dps = 0;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (dps > 0)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}

				int doubledDps = dps * 2;
				npc.lifeRegen -= doubledDps;
				damage = Math.Max(1, doubledDps / 4); //mirrored roughly from vanilla, avoids lots of "1" popups when it runs out
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (dps > 0)
			{
				//Values stolen from npc.poisoned
				float r = 0.65f;
				float g = 1f;
				float b = 0.75f;
				float a = 1f;
				drawColor = NPC.buffColor(drawColor, r, g, b, a);
			}
		}
	}
}
