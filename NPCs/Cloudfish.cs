using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Cloudfish : ModNPC
    {
        public float scareRange = 200f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloudfish");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 36;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = -1; //custom
            AIType = NPCID.Goldfish;
            AnimationType = NPCID.Goldfish;
            NPC.noGravity = true;
            NPC.catchItem = ItemID.Cloudfish;
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneSkyHeight)
            {
                if (Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].LiquidAmount == 0)
                {
                    return 0f;
                }
                else if (
                   !WorldGen.SolidTile(spawnInfo.spawnTileX, spawnInfo.spawnTileY) &&
                   !WorldGen.SolidTile(spawnInfo.spawnTileX, spawnInfo.spawnTileY + 1) &&
                   !WorldGen.SolidTile(spawnInfo.spawnTileX, spawnInfo.spawnTileY + 2))
                {
                    return SpawnCondition.Sky.Chance * 4f; //0.05f before, 100f now because water check
                }
            }
            return 0f;
        }

        //public override int SpawnNPC(int tileX, int tileY)
        //{
        //    if (Main.tile[tileX, tileY].LiquidAmount == 0)
        //    {
        //        return 0;
        //    }
        //    else if (
        //       !WorldGen.SolidTile(tileX, tileY) &&
        //       !WorldGen.SolidTile(tileX, tileY + 1) &&
        //       !WorldGen.SolidTile(tileX, tileY + 2))
        //    {
        //        //actually spawn
        //        return base.SpawnNPC(tileX, tileY);
        //    }
        //    return 0;
        //}
        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Cloud, 10 + Main.rand.Next(20));
            if (Main.rand.NextBool(10))
                Item.NewItem(NPC.getRect(), ItemID.RainCloud, 10 + Main.rand.Next(20));
            if (Main.rand.NextBool(15))
                Item.NewItem(NPC.getRect(), ItemID.SnowCloudBlock, 10 + Main.rand.Next(20));
        }

        public override void AI()
        {
            AssAI.ModifiedGoldfishAI(NPC, 200f);
        }
    }
}
