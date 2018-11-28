using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_eye_18 : ModNPC
	{
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Wandering Eye");
				Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.WanderingEye];
			}
		public override void SetDefaults()
			{
				npc.width = 38;
				npc.height = 46;
				npc.damage = 40;
				npc.defense = 20;
				npc.lifeMax = 300;
				npc.HitSound = SoundID.NPCHit1;
				npc.DeathSound = SoundID.NPCDeath1;
				npc.value = 75f;
				npc.knockBackResist = 0.8f;
				npc.aiStyle = 2;
				aiType = NPCID.WanderingEye;
				animationType = NPCID.WanderingEye;
            banner = Item.NPCtoBanner(NPCID.DemonEye);
            bannerItem = Item.BannerToItem(banner);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			{
				if(Main.hardMode == true)
				{
					return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
				}
				else
				{
					return SpawnCondition.OverworldNightMonster.Chance * 0f;
				}
			}
        public override void NPCLoot()
			{
				{
					if (Main.rand.NextBool(33))
							Item.NewItem(npc.getRect(), ItemID.Lens, 1);
					if (Main.rand.NextBool(100))
							Item.NewItem(npc.getRect(), ItemID.BlackLens, 1);
				}
			}
		public override void HitEffect(int hitDirection, double damage)
			{
				{
					if (npc.life <= 0)
						{
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_bigpurple"), 1f);
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_eye_bigpurple"), 1f);
						}
				}
			}
	}
}
