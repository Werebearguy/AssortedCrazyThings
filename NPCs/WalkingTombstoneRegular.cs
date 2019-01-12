using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class WalkingTombstoneRegular : ModNPC
    {
        private const int TotalNumberOfThese = 6;

        /*
        * 0 = Tombstone
        * 1 = CrossGraveMarker
        * 2 = GraveMarker
        * 3 = Gravestone
        * 4 = Headstone
        * 5 = Obelisk
        */

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/WalkingTombstoneRegular_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Walking Tombstone");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Crab];
        }

        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 46;
            npc.damage = 0;
            npc.defense = 10;
            npc.lifeMax = 40;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 3;
            aiType = NPCID.Crab;
            animationType = NPCID.Crab;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNight.Chance * 0.025f * 1f;
        }

        public override void NPCLoot()
        {
            int itemid = 0;

            switch ((int)AiTexture)
            {
                case 0:
                    itemid = ItemID.Tombstone;
                    break;
                case 1:
                    itemid = ItemID.CrossGraveMarker;
                    break;
                case 2:
                    itemid = ItemID.GraveMarker;
                    break;
                case 3:
                    itemid = ItemID.Gravestone;
                    break;
                case 4:
                    itemid = ItemID.Headstone;
                    break;
                case 5:
                    itemid = ItemID.Obelisk;
                    break;
                default:
                    break;
            }

            Item.NewItem(npc.getRect(), itemid);
        }

        public override void PostAI()
        {
            if (Main.dayTime)
            {
                if (npc.velocity.X > 0 || npc.velocity.X < 0)
                {
                    npc.velocity.X = 0;
                }
                npc.ai[0] = 1f;
                npc.direction = 1;
            }

            if (npc.velocity.Y < 0)
            {
                npc.velocity.Y = 0;
            }
        }

        public float AiTexture
        {
            get
            {
                return npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        public override bool PreAI()
        {
            if (AiTexture == 0 && npc.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AiTexture = Main.rand.Next(TotalNumberOfThese);

                npc.localAI[0] = 1;
                npc.netUpdate = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/WalkingTombstoneRegular_" + AiTexture);
            Vector2 stupidOffset = new Vector2(0f, 4f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, new Rectangle?(npc.frame), drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            return false;
        }
    }
}
