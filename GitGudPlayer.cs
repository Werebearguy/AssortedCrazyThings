using AssortedCrazyThings.Items.Gitgud;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
    public class GitGudPlayer : ModPlayer
    {
        //other places where adjustments are needed:
        //GitGudReset in AssGlobalNPC
        //HandlePacket in ACT, and the enum

        public const byte kingSlimeGitgudCounterMax = 5;
        public byte kingSlimeGitgudCounter = 0;
        public bool kingSlimeGitgud = false;

        public const byte eyeOfCthulhuGitgudCounterMax = 5;
        public byte eyeOfCthulhuGitgudCounter = 0;
        public bool eyeOfCthulhuGitgud = false;

        public const byte brainOfCthulhuGitgudCounterMax = 5;
        public byte brainOfCthulhuGitgudCounter = 0;
        public bool brainOfCthulhuGitgud = false;

        public const byte eaterOfWorldsGitgudCounterMax = 5;
        public byte eaterOfWorldsGitgudCounter = 0;
        public bool eaterOfWorldsGitgud = false;

        public const byte queenBeeGitgudCounterMax = 5;
        public byte queenBeeGitgudCounter = 0;
        public bool queenBeeGitgud = false;

        public const byte skeletronGitgudCounterMax = 5;
        public byte skeletronGitgudCounter = 0;
        public bool skeletronGitgud = false;

        public const byte wallOfFleshGitgudCounterMax = 5;
        public byte wallOfFleshGitgudCounter = 0;
        public bool wallOfFleshGitgud = false;

        public const byte planteraGitgudCounterMax = 5;
        public byte planteraGitgudCounter = 0;
        public bool planteraGitgud = false;

        public override void ResetEffects()
        {
            kingSlimeGitgud = false;
            eyeOfCthulhuGitgud = false;
            brainOfCthulhuGitgud = false;
            eaterOfWorldsGitgud = false;
            queenBeeGitgud = false;
            skeletronGitgud = false;
            wallOfFleshGitgud = false;
            planteraGitgud = false;
        }

        //no need for syncplayer because the server handles the item drop stuff

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"kingSlimeGitgudCounter", (byte)kingSlimeGitgudCounter},
                {"eyeOfCthulhuGitgudCounter", (byte)eyeOfCthulhuGitgudCounter},
                {"brainOfCthulhuGitgudCounter", (byte)brainOfCthulhuGitgudCounter},
                {"eaterOfWorldsGitgudCounter", (byte)eaterOfWorldsGitgudCounter},
                {"queenBeeGitgudCounter", (byte)queenBeeGitgudCounter},
                {"skeletronGitgudCounter", (byte)skeletronGitgudCounter},
                {"wallOfFleshGitgudCounter", (byte)wallOfFleshGitgudCounter},
                {"planteraGitGudCounter", (byte)planteraGitgudCounter}, //don't correct the string
            };
        }

        public override void Load(TagCompound tag)
        {
            kingSlimeGitgudCounter = tag.GetByte("kingSlimeGitgudCounter");
            eyeOfCthulhuGitgudCounter = tag.GetByte("eyeOfCthulhuGitgudCounter");
            brainOfCthulhuGitgudCounter = tag.GetByte("brainOfCthulhuGitgudCounter");
            eaterOfWorldsGitgudCounter = tag.GetByte("eaterOfWorldsGitgudCounter");
            queenBeeGitgudCounter = tag.GetByte("queenBeeGitgudCounter");
            skeletronGitgudCounter = tag.GetByte("skeletronGitgudCounter");
            wallOfFleshGitgudCounter = tag.GetByte("wallOfFleshGitgudCounter");
            planteraGitgudCounter = tag.GetByte("planteraGitGudCounter"); //don't correct the string
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if ((kingSlimeGitgud && proj.type == ProjectileID.SpikedSlimeSpike) ||
                /*(eyeOfCthulhuGitgud) ||*/
                /*(brainOfCthulhuGitgud) ||*/
                /*(eaterOfWorldsGitgud) ||*/
                (queenBeeGitgud && proj.type == ProjectileID.Stinger) ||
                (skeletronGitgud && proj.type == ProjectileID.Skull) ||
                (wallOfFleshGitgud && proj.type == ProjectileID.EyeLaser) ||
                (planteraGitgud && (proj.type == ProjectileID.ThornBall || proj.type == ProjectileID.SeedPlantera || proj.type == ProjectileID.PoisonSeedPlantera)))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if ((kingSlimeGitgud && (npc.type == NPCID.KingSlime || npc.type == NPCID.BlueSlime)) ||
                (eyeOfCthulhuGitgud && (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.ServantofCthulhu)) ||
                (brainOfCthulhuGitgud && (npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.Creeper)) ||
                (eaterOfWorldsGitgud && (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.VileSpit)) ||
                (queenBeeGitgud && (npc.type == NPCID.QueenBee || npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall)) ||
                (skeletronGitgud && (npc.type == NPCID.SkeletronHead || npc.type == NPCID.SkeletronHand)) ||
                (wallOfFleshGitgud && (npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye || npc.type == NPCID.TheHungry || npc.type == NPCID.TheHungryII)) ||
               (planteraGitgud && (npc.type == NPCID.Plantera || npc.type == NPCID.PlanterasHook || npc.type == NPCID.PlanterasTentacle)))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //to not call NPC.AnyNPCs() for every boss, do it manually
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active)
                {
                    if (Main.npc[k].type == NPCID.KingSlime) kingSlimeGitgudCounter++;
                    if (Main.npc[k].type == NPCID.EyeofCthulhu) eyeOfCthulhuGitgudCounter++;
                    if (Main.npc[k].type == NPCID.BrainofCthulhu) brainOfCthulhuGitgudCounter++;
                    if (Main.npc[k].type == NPCID.EaterofWorldsHead) eaterOfWorldsGitgudCounter++;
                    if (Main.npc[k].type == NPCID.QueenBee) queenBeeGitgudCounter++;
                    if (Main.npc[k].type == NPCID.SkeletronHead) skeletronGitgudCounter++;
                    if (Main.npc[k].type == NPCID.WallofFlesh) wallOfFleshGitgudCounter++;
                    if (Main.npc[k].type == NPCID.Plantera) planteraGitgudCounter++;
                }
            }

            return true;
        }

        private void UpdateGitGud()
        {
            if (kingSlimeGitgudCounter >= kingSlimeGitgudCounterMax)
            {
                kingSlimeGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<KingSlimeGitgud>()) && !kingSlimeGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<KingSlimeGitgud>());
                }
            }

            if (eyeOfCthulhuGitgudCounter >= eyeOfCthulhuGitgudCounterMax)
            {
                eyeOfCthulhuGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<EyeOfCthulhuGitgud>()) && !eyeOfCthulhuGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<EyeOfCthulhuGitgud>());
                }
            }

            if (brainOfCthulhuGitgud && NPC.AnyNPCs(NPCID.BrainofCthulhu)) player.buffImmune[BuffID.Slow] = true;

            if (brainOfCthulhuGitgudCounter >= brainOfCthulhuGitgudCounterMax)
            {
                brainOfCthulhuGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<BrainOfCthulhuGitgud>()) && !brainOfCthulhuGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<BrainOfCthulhuGitgud>());
                }
            }

            if (eaterOfWorldsGitgud && NPC.AnyNPCs(NPCID.EaterofWorldsHead)) player.buffImmune[BuffID.Weak] = true;

            if (eaterOfWorldsGitgudCounter >= eaterOfWorldsGitgudCounterMax)
            {
                eaterOfWorldsGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<EaterOfWorldsGitgud>()) && !eaterOfWorldsGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<EaterOfWorldsGitgud>());
                }
            }

            if (queenBeeGitgud && NPC.AnyNPCs(NPCID.QueenBee)) player.buffImmune[BuffID.Poisoned] = true;

            if (queenBeeGitgudCounter >= queenBeeGitgudCounterMax)
            {
                queenBeeGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<QueenBeeGitgud>()) && !queenBeeGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<QueenBeeGitgud>());
                }
            }

            if (skeletronGitgud && NPC.AnyNPCs(NPCID.SkeletronHead)) player.buffImmune[BuffID.Bleeding] = true;

            if (skeletronGitgudCounter >= skeletronGitgudCounterMax)
            {
                skeletronGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<SkeletronGitgud>()) && !skeletronGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<SkeletronGitgud>());
                }
            }

            if (wallOfFleshGitgudCounter >= wallOfFleshGitgudCounterMax)
            {
                wallOfFleshGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<WallOfFleshGitgud>()) && !wallOfFleshGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<WallOfFleshGitgud>());
                }
            }

            if (planteraGitgud && NPC.AnyNPCs(NPCID.Plantera)) player.buffImmune[BuffID.Poisoned] = true;

            if (planteraGitgudCounter >= planteraGitgudCounterMax)
            {
                planteraGitgudCounter = 0;
                if (!player.HasItem(mod.ItemType<GreenThumb>()) && !planteraGitgud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<GreenThumb>());
                }
            }

            //others
        }

        public override void OnEnterWorld(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //sent in OnEnterWorld to tell the server about the loaded values in tagcompound
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)AssMessageType.SendClientChangesGitGud);
                packet.Write((byte)player.whoAmI);
                packet.Write((byte)kingSlimeGitgudCounter);
                packet.Write((byte)eyeOfCthulhuGitgudCounter);
                packet.Write((byte)brainOfCthulhuGitgudCounter);
                packet.Write((byte)eaterOfWorldsGitgudCounter);
                packet.Write((byte)queenBeeGitgudCounter);
                packet.Write((byte)skeletronGitgudCounter);
                packet.Write((byte)wallOfFleshGitgudCounter);
                packet.Write((byte)planteraGitgudCounter);
                packet.Send();
            }
        }

        public override void PostUpdateEquips() //this actually only gets called when player is alive
        {
            UpdateGitGud();
        }
    }
}
