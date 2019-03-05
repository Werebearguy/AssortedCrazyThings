using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;

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

        public override void NPCLoot(NPC npc)
        {
            //other pets
            if(npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior)
            {
                if (Main.rand.NextBool(100)) Item.NewItem(npc.getRect(), mod.ItemType<GobletItem>());
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

            if (npc.type == NPCID.Plantera)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active)
                    {
                        AssPlayer mPlayer = Main.player[i].GetModPlayer<AssPlayer>(mod);
                        mPlayer.planteraGitGudCounter = 0; //resets even when all but one player is dead and plantera is defeated
                    }
                }
            }

            //soul spawn from dead enemies

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
