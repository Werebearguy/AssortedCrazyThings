using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class CuteSlimeRainbow : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 52;
            npc.scale = 1.2f;
            npc.friendly = true;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.3f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            Main.npcCatchable[mod.NPCType("CuteSlimeRainbow")] = true;
            npc.catchItem = (short)mod.ItemType("CuteSlimeRainbowNew");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ModConf.CuteSlimes) return SpawnCondition.OverworldDaySlime.Chance * 0.025f * 0.5f;
            else return 0f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.DiscoColor;
        }
    }
}
