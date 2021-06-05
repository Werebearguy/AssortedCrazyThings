using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DemonEyeRegular : DemonEyeRecolorBase
    {
        public override int TotalNumberOfThese => 6;
        /*CG = 0
        * CP = 1
        * DG = 2
        * DP = 3
        * SG = 4
        * SP = 5
        */

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeRegular_0"; //use fixed texture
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                /*CG = 0
                * CP = 1
                * DG = 2
                * DP = 3
                * SG = 4
                * SP = 5
                */
                switch ((int)AiTexture)
                {
                    case 0:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeCataractGore").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 1:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeCataractGore").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    case 2:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeDilatedGore").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 3:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeDilatedGore").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    case 4:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeSleepyGore_0").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 5:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeSleepyGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    default:
                        break;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/DemonEyeRegular_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, 0f); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
