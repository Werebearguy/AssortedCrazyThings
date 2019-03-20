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

        public const byte kingSlimeGitGudCounterMax = 5;
        public byte kingSlimeGitGudCounter = 0;
        public bool kingSlimeGitGud = false;

        public const byte planteraGitGudCounterMax = 5;
        public byte planteraGitGudCounter = 0;
        public bool planteraGitGud = false;

        public override void ResetEffects()
        {
            kingSlimeGitGud = false;
            planteraGitGud = false;
        }

        //no need for syncplayer because the server handles the item drop stuff

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"kingSlimeGitGudCounter", (byte)kingSlimeGitGudCounter},
                {"planteraGitGudCounter", (byte)planteraGitGudCounter},
            };
        }

        public override void Load(TagCompound tag)
        {
            kingSlimeGitGudCounter = tag.GetByte("kingSlimeGitGudCounter");
            planteraGitGudCounter = tag.GetByte("planteraGitGudCounter");
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if ((kingSlimeGitGud && proj.type == ProjectileID.SpikedSlimeSpike) ||
                (planteraGitGud && (proj.type == ProjectileID.ThornBall || proj.type == ProjectileID.SeedPlantera)))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if ((kingSlimeGitGud && (npc.type == NPCID.KingSlime || npc.type == NPCID.BlueSlime)) ||
               (planteraGitGud && (npc.type == NPCID.Plantera || npc.type == NPCID.PlanterasHook || npc.type == NPCID.PlanterasTentacle)))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (NPC.AnyNPCs(NPCID.KingSlime)) kingSlimeGitGudCounter++;
            if (NPC.AnyNPCs(NPCID.Plantera)) planteraGitGudCounter++;

            return true;
        }

        private void UpdateGitGud()
        {
            if (kingSlimeGitGudCounter >= kingSlimeGitGudCounterMax)
            {
                kingSlimeGitGudCounter = 0;
                if (!player.HasItem(mod.ItemType<SlimeInquisitionNotice>()) && !kingSlimeGitGud)
                {
                    Item.NewItem(player.getRect(), mod.ItemType<SlimeInquisitionNotice>());
                }
            }

            if (planteraGitGud) player.buffImmune[BuffID.Poisoned] = true;

            if (planteraGitGudCounter >= planteraGitGudCounterMax)
            {
                planteraGitGudCounter = 0;
                if (!player.HasItem(mod.ItemType<GreenThumb>()) && !planteraGitGud)
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
                packet.Write((byte)kingSlimeGitGudCounter);
                packet.Write((byte)planteraGitGudCounter);
                packet.Send();
            }
        }

        public override void PostUpdateEquips() //this actually only gets called when player is alive
        {
            UpdateGitGud();
        }
    }
}
