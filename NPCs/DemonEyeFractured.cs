using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    /* This class holds three retextures of DemonEye, that only differ by their texture and their gore
     * First off, you need to declare the total number of retextures that you intend to use for this particular class
     * (private const int TotalNumberOfThese)
     * Then for organizations sake you "name" them by their number so you can see in the code what texture is what number
     * (FG is Fractured Green etc)
     * Then you need a "default" texture so cheatsheet shows a proper one, here just use the 0 one
     * 
     * Whereever you have something that isn't just easily adressed by the "number" of the NPC, use a switch case (like in HitEffects()).
     * Check WalkingTombstoneRegular or StrangeSlime for usage in NPCLoot()
     * 
     * Depending on what NPC you try to retexture, you might need to change the number that is in npc.ai[_] in "public float AiTexture"
     * (check via first spawning the vanilla NPC, enable misc -> show NPC info in ModdersToolkit, then looking at the four numbers in the
     * first row (npc.ai[0] to npc.ai[3]), and trying out different scenarios for the NPC depending on its behavior pattern (demon eyes
     * actually don't use any of those values, while for example Granite Elementals use all four (which makes this method of retexturing
     * not work)), trying out different environments (day/night, water/no water, player being unreachable, while attacking the player, etc)
     * and see if any of those four numbers never changes (stays 0). This one you can then use by index (from 0 to 3) as a variable to store
     * our texture "number"
     * 
     * for some obscure cases you might need to change the number in npc.LocalAI[_] inside PreAI() (also from 0 to 3)
     * 
     * Finally, name the textures appropriately with whatever you have in PreDraw()
     * (Texture2D texture = mod.GetTexture("NPCs/DemonEyeFractured_" + AiTexture);).Value
     */

    public class DemonEyeFractured : ModNPC
    {
        private const int TotalNumberOfThese = 3;

        /*FG = 0
        * FP = 1
        * FR = 2
        */
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeFractured_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Eye");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DemonEye];
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DemonEye);
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 18;
            NPC.defense = 2;
            NPC.lifeMax = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 2;
            AIType = NPCID.DemonEye;
            AnimationType = NPCID.DemonEye;
            Banner = Item.NPCtoBanner(NPCID.DemonEye);
            BannerItem = Item.BannerToItem(Banner);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !NPC.downedBoss1 ? 0f : SpawnCondition.OverworldNightMonster.Chance * 0.025f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Lens, chanceDenominator: 33));
            npcLoot.Add(ItemDropRule.Common(ItemID.BlackLens, chanceDenominator: 100));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Text here.")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                switch ((int)AiTexture)
                {
                    case 0:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 1:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    case 2:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeRedGore_0").Type, 1f);
                        break;
                    default:
                        break;
                }
            }
        }

        public float AiTexture
        {
            get
            {
                return NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
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
            Texture2D texture = Mod.GetTexture("NPCs/DemonEyeFractured_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, NPC.height / 3); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
