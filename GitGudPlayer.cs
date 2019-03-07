using AssortedCrazyThings.Items.Fun;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
    public class GitGudPlayer : ModPlayer
    {
        public const int planteraGitGudCounterMax = 5;
        public byte planteraGitGudCounter = 0;
        public bool planteraGitGud = false;

        public override void ResetEffects()
        {
            planteraGitGud = false;
        }

        //no need for syncplayer because the server handles the item drop stuff

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"planteraGitGudCounter", (byte)planteraGitGudCounter},
            };
        }

        public override void Load(TagCompound tag)
        {
            planteraGitGudCounter = tag.GetByte("planteraGitGudCounter");
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (planteraGitGud && (proj.type == ProjectileID.ThornBall || proj.type == ProjectileID.SeedPlantera))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (planteraGitGud && (npc.type == NPCID.Plantera || npc.type == NPCID.PlanterasHook || npc.type == NPCID.PlanterasTentacle))
            {
                damage = (int)(damage * 0.85f);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (NPC.AnyNPCs(NPCID.Plantera)) planteraGitGudCounter++;

            return true;
        }

        private void UpdateGitGud()
        {
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
