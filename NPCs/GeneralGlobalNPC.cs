using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.DroneUnlockables;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    [Autoload]
    public class GeneralGlobalNPC : AssGlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            //TODO convert this to a drop rule
            if (npc.type == NPCID.TheDestroyer)
            {
                AssUtils.DropItemInstanced(npc, npc.Center, npc.Size, ModContent.ItemType<DroneParts>(),
                    condition: delegate (NPC n, Player player)
                    {
                        return !DroneController.AllUnlocked(player);
                    });
            }

            GitgudData.Reset(npc);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.PartyGirl && NPC.downedSlimeKing)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<SlimeBeaconItem>());
                nextSlot++;
            }
        }

        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (AConfigurationConfig.Instance.OtherPets && Main.rand.NextBool(4))
            {
                shop[nextSlot] = ModContent.ItemType<SuspiciousNuggetItem>();
                nextSlot++;
            }
        }
    }
}
