using System.IO;
using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public abstract class CuteSlimeBaseNPC : ModNPC
    {
        private static readonly int RandomItemChance = 4;

        public abstract string IngameName { get; }

        public abstract int CatchItem { get; }

        public abstract SpawnConditionType SpawnCondition { get; }

        public virtual bool IsFriendly
        {
            get
            {
                return true;
            }
        }

        public virtual bool ShouldDropGel
        {
            get
            {
                return true;
            }
        }

        public virtual bool ShouldDropRandomItem
        {
            get
            {
                return true;
            }
        }

        public short RandomItem = -1;

        public bool DecidedOnRandomItem = false;

        public bool HasRandomItem
        {
            get
            {
                return RandomItem > -1;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(IngameName);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
            Main.npcCatchable[npc.type] = true;
            MoreSetStaticDefaults();
        }

        public virtual void MoreSetStaticDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            if (IsFriendly)
            {
                npc.friendly = true;
                npc.defense = 0;
                npc.lifeMax = 5;
            }
            else
            {
                npc.chaseable = false;
                npc.defense = 2;
                npc.lifeMax = 20;
            }
            npc.width = 46;
            npc.height = 52;
            npc.damage = 0;
            npc.rarity = 1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.9f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            npc.alpha = 75;
            npc.catchItem = (short)CatchItem;

            MoreSetDefaults();

            // Slime AI breaks with big enough height when it jumps against a low ceiling
            // then glitches into the ground
            if (npc.scale > 0.9f)
            {
                npc.height -= (int)((npc.scale - 0.9f) * npc.height);
            }
        }

        public virtual void MoreSetDefaults()
        {

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SlimePets.CuteSlimeSpawnChance(spawnInfo, SpawnCondition);
        }

        public sealed override void NPCLoot()
        {
            if (ShouldDropGel) Item.NewItem(npc.getRect(), ItemID.Gel);
            if (ShouldDropRandomItem && HasRandomItem && npc.value > 0)
            {
                // Copied from vanilla and adjusted
                int type = RandomItem;
                if (type > 0)
                {
                    int stack = 1;
                    if (type == 8)
                    {
                        stack = Main.rand.Next(5, 11);
                    }
                    else if (type == 166)
                    {
                        stack = Main.rand.Next(2, 7);
                    }
                    else if (type == 965)
                    {
                        stack = Main.rand.Next(20, 46);
                    }
                    else if ((type >= 11 && type <= 14) || (type >= 699 && type <= 702))
                    {
                        stack = Main.rand.Next(3, 9);
                        if (Main.rand.Next(2) == 0)
                        {
                            stack += 5;
                        }
                    }
                    else if (type == 71)
                    {
                        stack = Main.rand.Next(50, 100);
                    }
                    else if (type == 72)
                    {
                        stack = Main.rand.Next(20, 100);
                    }
                    else if (type == 73)
                    {
                        stack = Main.rand.Next(1, 3);
                    }
                    Item.NewItem(npc.getRect(), type, stack);
                }
            }
            MoreNPCLoot();
        }

        public virtual void MoreNPCLoot()
        {

        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((short)RandomItem);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            RandomItem = reader.ReadInt16();
        }

        public override bool PreAI()
        {
            if (ShouldDropRandomItem && !DecidedOnRandomItem && Main.netMode != NetmodeID.MultiplayerClient && npc.value > 0f)
            {
                // Copied from vanilla and adjusted
                DecidedOnRandomItem = true;
                if (Main.rand.Next(RandomItemChance) == 0)
                {
                    int choice = Main.rand.Next(4);
                    if (choice == 0)
                    {
                        choice = Main.rand.Next(7);
                        if (choice == 0)
                        {
                            choice = 290;
                        }
                        else if (choice == 1)
                        {
                            choice = 292;
                        }
                        else if (choice == 2)
                        {
                            choice = 296;
                        }
                        else if (choice == 3)
                        {
                            choice = 2322;
                        }
                        else if (Main.netMode != NetmodeID.SinglePlayer && Main.rand.Next(2) == 0)
                        {
                            choice = 2997;
                        }
                        else
                        {
                            choice = 2350;
                        }
                    }
                    else if (choice == 1)
                    {
                        choice = Main.rand.Next(3); //4
                        if (choice == 0)
                        {
                            choice = 8;
                        }
                        else if (choice == 1)
                        {
                            choice = 166;
                        }
                        else if (choice == 2)
                        {
                            choice = 965;
                        }
                        //else
                        //{
                        //    choice = 58;
                        //}
                    }
                    else if (choice == 2)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            choice = Main.rand.Next(11, 15);
                        }
                        else
                        {
                            choice = Main.rand.Next(699, 703);
                        }
                    }
                    else
                    {
                        choice = Main.rand.Next(3);
                        if (choice == 0)
                        {
                            choice = 71;
                        }
                        else if (choice == 1)
                        {
                            choice = 72;
                        }
                        else
                        {
                            choice = 73;
                        }
                    }
                    RandomItem = (short)choice;
                    npc.netUpdate = true;
                }
            }
            return true;
        }

        public sealed override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (HasRandomItem)
            {
                // Copied from vanilla and adjusted
                int type = RandomItem;
                float scale = 1;
                float someX = 20f * npc.scale;
                float someY = 16f * npc.scale;
                float itemWidth = Main.itemTexture[type].Width;
                float itemHeight = Main.itemTexture[type].Height;
                if (itemWidth > someX)
                {
                    scale *= someX / itemWidth;
                    //itemWidth *= scale;
                    itemHeight *= scale;
                }
                if (itemHeight > someY)
                {
                    scale *= someY / itemHeight;
                    //itemWidth *= scale;
                    //itemHeight *= scale;
                }
                //float xOff = -1f;
                //float yOff = 1f;
                float xOff = -2f;
                float yOff = 14f;
                int frameNumber = npc.frame.Y / (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]);
                //xOff += frameNumber;
                yOff += frameNumber * 2;
                if (npc.scale < 0.9 && frameNumber == 2) yOff -= frameNumber * 2;
                //xOff *= scale;
                if (npc.scale > 1) yOff -= npc.scale * 3;
                if (npc.scale < 0.9) yOff -= npc.scale * 10;
                float rotation = 0f;
                spriteBatch.Draw(Main.itemTexture[type], new Vector2(npc.Center.X - Main.screenPosition.X + xOff, npc.Center.Y - Main.screenPosition.Y + npc.gfxOffY + yOff), null, drawColor, rotation, Main.itemTexture[type].Size() / 2, scale, SpriteEffects.None, 0f);

            }
            return MorePreDraw(spriteBatch, drawColor);
        }

        public virtual bool MorePreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return true;
        }
    }
}
