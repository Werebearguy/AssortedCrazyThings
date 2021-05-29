using AssortedCrazyThings.Base;
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
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Crab];
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 46;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.Crab;
            AnimationType = NPCID.Crab;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (AssUtils.AssConfig.WalkingTombstones) return SpawnCondition.OverworldNight.Chance * 0.025f * 1f;
            else return 0f;
        }

        public override void OnKill()
        {
            int itemid = 0;

            if (NPC.Center == new Vector2(1000, 1000)) //RecipeBrowser fix
            {
                AiTexture = Main.rand.Next(6);
            }

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

            Item.NewItem(NPC.getRect(), itemid);
        }

        public override void PostAI()
        {
            if (Main.dayTime)
            {
                if (NPC.velocity.X > 0 || NPC.velocity.X < 0)
                {
                    NPC.velocity.X = 0;
                }
                NPC.ai[0] = 1f;
                NPC.direction = 1;
            }

            if (NPC.velocity.Y < 0)
            {
                NPC.velocity.Y = 0;
            }
        }

        public float AiTexture
        {
            get
            {
                return NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
            }
        }

        public override bool PreAI()
        {
            if (AiTexture == 0 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AiTexture = Main.rand.Next(TotalNumberOfThese);

                NPC.localAI[0] = 1;
                NPC.netUpdate = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/WalkingTombstoneRegular_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
            Main.spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/WalkingTombstoneGore_01").Type, 1f);
            }
        }
    }
}
