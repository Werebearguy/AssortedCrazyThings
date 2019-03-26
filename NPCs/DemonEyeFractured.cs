using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
     * (Texture2D texture = mod.GetTexture("NPCs/DemonEyeFractured_" + AiTexture);)
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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DemonEye];
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.DemonEye);
            npc.width = 32;
            npc.height = 32;
            npc.damage = 18;
            npc.defense = 2;
            npc.lifeMax = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 75f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 2;
            aiType = NPCID.DemonEye;
            animationType = NPCID.DemonEye;
            banner = Item.NPCtoBanner(NPCID.DemonEye);
            bannerItem = Item.BannerToItem(banner);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !NPC.downedBoss1 ? 0f : SpawnCondition.OverworldNightMonster.Chance * 0.025f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(33))
                Item.NewItem(npc.getRect(), ItemID.Lens, 1);
            if (Main.rand.NextBool(100))
                Item.NewItem(npc.getRect(), ItemID.BlackLens, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                switch ((int)AiTexture)
                {
                    case 0:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeGreenGore_0"), 1f);
                        break;
                    case 1:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyePurpleGore_0"), 1f);
                        break;
                    case 2:
                        Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DemonEyeRedGore_0"), 1f);
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
                return npc.ai[3];
            }
            set
            {
                npc.ai[3] = value;
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
            Texture2D texture = mod.GetTexture("NPCs/DemonEyeFractured_" + AiTexture);
            Vector2 stupidOffset = new Vector2(0f, npc.height / 3); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            return false;
        }
    }
}
