using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class aaaHarvester1 : BaseHarvester
    {
        public const string typeName = "aaaHarvester1";

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/StoneSoldier"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ArmedZombie];
        }

        public override void SetDefaults()
        {
            npc.scale = 1f;
            npc.npcSlots = 5f; //takes 5 npc slots out of 200 when alive
            npc.width = 38;
            npc.height = 46;
            npc.damage = 1;
            npc.defense = 1;
            npc.lifeMax = 11;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.aiStyle = -1; //91;
            aiType = NPCID.Zombie; //91
            animationType = NPCID.ArmedZombie;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = false;

            maxVeloScale = 1.3f; //2f default
            maxAccScale = 0.04f; //0.07f default
            stuckTime = 6; //*30 for ticks, *0.5 for seconds
            afterEatTime = 60;
            eatTime = EatTimeConst - 5;
            idleTime = IdleTimeConst;
            hungerTime = 3600; //AI_Timer
            maxSoulsEaten = 3;
            jumpRange = 100;//also noclip detect range //100 for restricted v
            restrictedSoulSearch = true;
            soulsEaten = 0;
            stopTime = idleTime;
            aiTargetType = Target_Soul;
            target = 0;
            stuckTimer = 0;
            rndJump = 0;
            transformServer = false;
            transformTo = AssWorld.harvesterTypes[1];
        }
    
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //from server to client
            writer.Write((bool)aiTargetType);
            writer.Write((byte)soulsEaten);
            //writer.Write((byte)stuckTimer);
            writer.Write((byte)rndJump);
            writer.Write((short)target);
            writer.Write((bool)transformServer);
            //Print("send: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //from client to server
            aiTargetType = reader.ReadBoolean();
            soulsEaten = reader.ReadByte();
            //stuckTimer = reader.ReadByte();
            rndJump = reader.ReadByte();
            target = reader.ReadInt16();
            transformServer = reader.ReadBoolean();
            //Print("recv: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool shouldSpawn = true;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) != -1)
                {
                    shouldSpawn = false;
                    break;
                }
            }

            if (spawnInfo.player.ZoneDungeon && shouldSpawn)
            {
                //only spawns when in dungeon and when no other is alive atm
                return 0.04f;
            }
            else
            {
                return 0f;
            }
        }

        public override void AI()
        {
            HarvesterAI(allowNoclip: !restrictedSoulSearch);
            if (transformServer) Transform(transformTo);
        }
    }
}
