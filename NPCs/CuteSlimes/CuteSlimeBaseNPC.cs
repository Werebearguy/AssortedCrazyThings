using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
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
        public int Stack = -1;

        public bool DecidedOnRandomItem = false;

        public bool HasRandomItem
        {
            get
            {
                return RandomItem > -1 && Stack > 0;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(IngameName);
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
            Main.npcCatchable[NPC.type] = true;
            MoreSetStaticDefaults();
        }

        public virtual void MoreSetStaticDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            if (IsFriendly)
            {
                NPC.friendly = true;
                NPC.defense = 0;
                NPC.lifeMax = 5;
            }
            else
            {
                NPC.chaseable = false;
                NPC.defense = 2;
                NPC.lifeMax = 20;
            }
            NPC.width = 46;
            NPC.height = 32;
            NPC.damage = 0;
            NPC.rarity = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 25f;
            NPC.knockBackResist = 0.9f;
            NPC.aiStyle = 1;
            AIType = NPCID.ToxicSludge;
            AnimationType = NPCID.ToxicSludge;
            NPC.alpha = 75;
            NPC.catchItem = (short)CatchItem;

            MoreSetDefaults();

            // Slime AI breaks with big enough height when it jumps against a low ceiling
            // then glitches into the ground
            if (NPC.scale > 0.9f)
            {
                NPC.height -= (int)((NPC.scale - 0.9f) * NPC.height);
            }
        }

        public virtual void MoreSetDefaults()
        {

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SlimePets.CuteSlimeSpawnChance(spawnInfo, SpawnCondition);
        }

        public override void OnCatchNPC(Player player, Item item)
        {
            DropRandomItem(player.getRect());
            MoreNPCLoot(player.getRect());
        }

        public sealed override void OnKill()
        {
            if (ShouldDropGel) Item.NewItem(NPC.getRect(), ItemID.Gel);
            DropRandomItem(NPC.getRect());
            MoreNPCLoot(NPC.getRect());
        }

        public void DropRandomItem(Rectangle pos)
        {
            if (ShouldDropRandomItem && HasRandomItem && NPC.value > 0)
            {
                Item.NewItem(pos, RandomItem, Stack);
            }
        }

        public virtual void MoreNPCLoot(Rectangle pos)
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
            if (ShouldDropRandomItem && !DecidedOnRandomItem && Main.netMode != NetmodeID.MultiplayerClient && NPC.value > 0f)
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
                    int stack = 1;
                    if (choice == 8)
                    {
                        stack = Main.rand.Next(5, 11);
                    }
                    else if (choice == 166)
                    {
                        stack = Main.rand.Next(2, 7);
                    }
                    else if (choice == 965)
                    {
                        stack = Main.rand.Next(20, 46);
                    }
                    else if ((choice >= 11 && choice <= 14) || (choice >= 699 && choice <= 702))
                    {
                        stack = Main.rand.Next(3, 9);
                        if (Main.rand.Next(2) == 0)
                        {
                            stack += 5;
                        }
                    }
                    else if (choice == 71)
                    {
                        stack = Main.rand.Next(50, 100);
                    }
                    else if (choice == 72)
                    {
                        stack = Main.rand.Next(20, 100);
                    }
                    else if (choice == 73)
                    {
                        stack = Main.rand.Next(1, 3);
                    }
                    RandomItem = (short)choice;
                    Stack = stack;
                    NPC.netUpdate = true;
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
                float someX = 20f * NPC.scale;
                float someY = 16f * NPC.scale;
                ReLogic.Content.Asset<Texture2D> asset = Terraria.GameContent.TextureAssets.Item[type];
                float itemWidth = asset.Width();
                float itemHeight = asset.Height();
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
                float yOff = 3f + DrawOffsetY;
                int frameNumber = NPC.frame.Y / (Terraria.GameContent.TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type]);
                //xOff += frameNumber;
                yOff += frameNumber * 2; //bobbing
                if (NPC.scale < 0.9 && frameNumber == 2) yOff -= frameNumber * 2;
                //xOff *= scale;
                if (NPC.scale > 1) yOff -= NPC.scale * 3;
                if (NPC.scale < 0.9) yOff -= NPC.scale * 10;
                float rotation = 0f;
                Main.spriteBatch.Draw(asset.Value, new Vector2(NPC.Center.X - Main.screenPosition.X + xOff, NPC.Center.Y - Main.screenPosition.Y + NPC.gfxOffY + yOff), null, drawColor, rotation, asset.Size() / 2, scale, SpriteEffects.None, 0f);

            }
            return MorePreDraw(spriteBatch, drawColor);
        }

        public virtual bool MorePreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return true;
        }
    }
}
