using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class aaaHarvester2 : BaseHarvester
    {
        public const string typeName = "aaaHarvester2";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DemonEye];
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 5f; //takes 5 npc slots out of 200 when alive
            npc.width = 38;
            npc.height = 46;
            npc.damage = 11;
            npc.defense = 11;
            npc.lifeMax = 111;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1; //91;
            aiType = NPCID.Zombie; //91
            animationType = NPCID.DemonEye;
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
            jumpRange = 300; //also noclip detect range
            restrictedSoulSearch = false;
            soulsEaten = 0;
            stopTime = idleTime;
            aiTargetType = Target_Soul;
            target = 0;
            stuckTimer = 0;
            rndJump = 0;
            transformServer = false;
            transformTo = AssWorld.harvesterTypes[2];
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
            writer.Write((byte)stuckTimer);
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
            stuckTimer = reader.ReadByte();
            rndJump = reader.ReadByte();
            target = reader.ReadInt16();
            transformServer = reader.ReadBoolean();
            //Print("recv: " + AI_State + " " + soulsEaten.ToString() + " " + stuckTimer.ToString() + " " + rndJump.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void AI()
        {
            HarvesterAI(allowNoclip: !restrictedSoulSearch);
            if (transformServer) Transform(transformTo);
        }
    }
}
