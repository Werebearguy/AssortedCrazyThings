using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs.DropConditions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class WalkingTombstoneGolden : ModNPC
    {
        private const int TotalNumberOfThese = 5;

        /*
        * 0 = Tombstone
        * 1 = CrossGraveMarker
        * 2 = GraveMarker
        * 3 = Gravestone
        * 4 = Headstone
        */

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/WalkingTombstoneGolden_0"; //use fixed texture
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
            if (AssUtils.AssConfig.WalkingTombstones) return SpawnCondition.OverworldNight.Chance * 0.005f * 1f;
            else return 0f;
        }

        //public override void OnKill()
        //{
        //    int itemid = 0;

        //    if (NPC.Center == new Vector2(1000, 1000)) //RecipeBrowser fix
        //    {
        //        AiTexture = Main.rand.Next(5);
        //    }

        //    switch ((int)AiTexture)
        //    {
        //        case 0:
        //            itemid = ItemID.RichGravestone2;
        //            break;
        //        case 1:
        //            itemid = ItemID.RichGravestone1;
        //            break;
        //        case 2:
        //            itemid = ItemID.RichGravestone3;
        //            break;
        //        case 3:
        //            itemid = ItemID.RichGravestone4;
        //            break;
        //        case 4:
        //            itemid = ItemID.RichGravestone5;
        //            break;
        //        default:
        //            break;
        //    }

        //    Item.NewItem(NPC.getRect(), itemid);
        //}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Terrestrial crabs that have taken up a morbid and expensive form of camouflage.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 0), ItemID.RichGravestone2));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 1), ItemID.RichGravestone1));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 2), ItemID.RichGravestone3));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 3), ItemID.RichGravestone4));
            npcLoot.Add(ItemDropRule.ByCondition(new MatchAppearanceCondition(1, 3), ItemID.RichGravestone5));
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/WalkingTombstoneGolden_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
            Main.spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("WalkingTombstoneGore_01").Type, 1f);
            }
        }
    }
}
