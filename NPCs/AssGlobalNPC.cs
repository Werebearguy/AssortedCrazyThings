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

        public override void NPCLoot(NPC npc)
        {
            //other pets
            if (npc.type == NPCID.Antlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.WalkingAntlion)
            {
                if (Main.rand.NextBool(100)) Item.NewItem(npc.getRect(), mod.ItemType<MiniAntlionItem>());
            }

            if (npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinWarrior)
            {
                if (Main.rand.NextBool(200)) Item.NewItem(npc.getRect(), mod.ItemType<GobletItem>());
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

            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<ObservingEyeItem>());
            }

            if (npc.type == NPCID.BrainofCthulhu)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<BrainofConfusion>());
            }

            if (npc.type == NPCID.QueenBee)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<QueenLarvaItem>());
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<SkeletronHandItem>());
            }

            if (npc.type == NPCID.WallofFlesh)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<WallFragmentItem>());
            }

            if (npc.type == NPCID.Retinazer && !NPC.AnyNPCs(NPCID.Spazmatism) || npc.type == NPCID.Spazmatism && !NPC.AnyNPCs(NPCID.Retinazer))
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<TinyTwinsItem>());
            }

            if (npc.type == NPCID.Golem)
            {
                if (Main.rand.NextBool(10)) Item.NewItem(npc.getRect(), mod.ItemType<PetGolemHeadItem>());
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

            GitgudData.Reset(npc);
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
