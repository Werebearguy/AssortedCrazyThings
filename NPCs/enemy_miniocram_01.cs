using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_miniocram_01 : ModNPC
	{
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Spawn of Ocram");
				Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Corruptor];
			}
		public override void SetDefaults()
			{
				npc.width = 92;
				npc.height = 66;
				npc.damage = 95;
				npc.defense = 65;
				npc.lifeMax = 8500;
				npc.HitSound = SoundID.NPCHit14;
				npc.DeathSound = SoundID.NPCDeath1;
				npc.value = 0f;
				npc.knockBackResist = 0f;
				npc.aiStyle = 5;
				aiType = NPCID.Corruptor;
				animationType = NPCID.Corruptor;
				npc.noGravity = true;
				npc.noTileCollide = true;
			}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			{
				return !NPC.downedGolemBoss ? 0f :
				SpawnCondition.OverworldNightMonster.Chance * 0.005f;
			}
        public override void NPCLoot()
			{
				{
					Item.NewItem(npc.getRect(), ItemID.Emerald);
					if(Main.rand.Next(5) < 1) // a 2 in 7 chance
						Item.NewItem(npc.getRect(), mod.ItemType("pet_Ocram_01"));
				}
			}
		public override void HitEffect(int hitDirection, double damage)
			{
				{
					if (npc.life <= 0)
						{
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_miniocram_03"), 1f);
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_miniocram_02"), 1f);
							Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_miniocram_01"), 1f);
						}
				}
			}
	}
}
