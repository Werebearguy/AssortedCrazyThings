using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.NPCs
{
	public class AssGlobalNPC : GlobalNPC
	{
        public bool shouldSoulDrop = false;

		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

        public override void ResetEffects(NPC npc)
        {
            shouldSoulDrop = false;
        }

        private void ResetCounter(ref byte counter)
        {
            counter = 0;
        }

        private void GitGudReset(int type)
        {
            GitGudType gitGudType = GitGudType.None;

            //Single and Server only
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active)
                {
                    //resets even when all but one player is dead and boss is defeated
                    GitGudPlayer gPlayer = Main.player[i].GetModPlayer<GitGudPlayer>(mod);
                    if (type == NPCID.KingSlime)
                    {
                        gitGudType = GitGudType.KingSlime;
                        gPlayer.kingSlimeGitGudCounter = 0;
                    }
                    else if (type == NPCID.Plantera)
                    {
                        gitGudType = GitGudType.Plantera;
                        gPlayer.planteraGitGudCounter = 0; 
                    }
                }
            }

            if (Main.netMode == NetmodeID.Server && gitGudType != GitGudType.None)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)AssMessageType.ResetGitGud);
                packet.Write((byte)gitGudType);
                packet.Send(); //send to all clients
            }
        }

        public override void NPCLoot(NPC npc)
        {
            //other pets
            if (npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior)
            {
                if (Main.rand.NextBool(100)) Item.NewItem(npc.getRect(), mod.ItemType<GobletItem>());
            }

            if (npc.type == NPCID.DarkMummy || npc.type == NPCID.LightMummy || npc.type == NPCID.Mummy)
            {
                if (Main.rand.NextBool(100)) Item.NewItem(npc.getRect(), mod.ItemType<LilWrapsItem>());
            }

            if (npc.type == NPCID.RainbowSlime)
            {
                if (Main.rand.NextBool(2)) Item.NewItem(npc.getRect(), mod.ItemType<RainbowSlimeItem>());
            }

            if (npc.type == NPCID.IlluminantSlime)
            {
                if (Main.rand.NextBool(100)) Item.NewItem(npc.getRect(), mod.ItemType<IlluminantSlimeItem>());
            }

            //boss pets

            if (npc.type == NPCID.KingSlime)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PrinceSlimeItem>());
            }

            if (npc.type == NPCID.QueenBee)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<QueenLarvaItem>());
            }

            if (npc.type == NPCID.WallofFlesh)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<WallFragmentItem>());
            }

            if (npc.type == NPCID.DukeFishron)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetFishronItem>());
            }

            //soul spawn from dead enemies while harvester alive

            if (shouldSoulDrop)
            {
                if (npc.type != mod.NPCType<DungeonSoul>())
                {
                    int soulType = mod.NPCType<DungeonSoul>();

                    //NewNPC starts looking for the first !active from 0 to 200
                    int soulID = NPC.NewNPC((int)npc.position.X + DungeonSoulBase.wid / 2, (int)npc.position.Y + DungeonSoulBase.hei / 2, soulType); //Spawn coords are actually the tile where its supposed to spawn on
                    Main.npc[soulID].timeLeft = DungeonSoulBase.SoulActiveTime;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(23, -1, -1, null, soulID);
                    }
                }
            }

            //RecipeBrowser fixes (not actual drops)

            if (npc.type == NPCID.TheDestroyer && npc.Center == new Vector2(1000, 1000))
            {
                Item.NewItem(npc.getRect(), mod.ItemType<DroneParts>());
            }

            GitGudReset(npc.type);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.PartyGirl && NPC.downedSlimeKing)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType<SlimeBeaconItem>());
                nextSlot++;
            }
        }
    }
}
