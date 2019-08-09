using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeIlluminant : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Illuminant Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
            NPCID.Sets.TrailingMode[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 52;
            //npc.friendly = true;
            npc.chaseable = false;
            npc.damage = 0;
            npc.defense = 2;
            npc.lifeMax = 20;
            npc.rarity = 1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            npc.alpha = 80;
            Main.npcCatchable[mod.NPCType("CuteSlimeIlluminant")] = true;
            npc.catchItem = (short)mod.ItemType("CuteSlimeIlluminantNew");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SlimePets.CuteSlimeSpawnChance(spawnInfo, SlimePets.SpawnConditionType.Hallow);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/CuteSlimes/CuteSlimeIlluminantAddition");
            SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f + npc.gfxOffY + 4f);

            //the higher the k, the older the position
            //Length is implicitely set in TrailCacheLength up there
            for (int k = npc.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = npc.GetAlpha(Color.White) * ((npc.oldPos.Length - k) / (1f * npc.oldPos.Length)) * ((255 - npc.alpha) / 255f) * 0.5f;
                color.A = (byte)(npc.alpha * ((npc.oldPos.Length - k) / npc.oldPos.Length));
                spriteBatch.Draw(texture, drawPos, npc.frame, color, npc.oldRot[k], npc.frame.Size() / 2, npc.scale, effect, 0f);
            }
        }
    }
}
