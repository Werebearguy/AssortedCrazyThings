using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public abstract class CuteSlimeBaseNPC : ModNPC
    {
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
            //DropRandomItem(player.getRect());
        }

        public sealed override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            if (ShouldDropGel)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Gel));
            }

            MoreModifyNPCLoot(npcLoot);
        }

        public virtual void MoreModifyNPCLoot(NPCLoot npcLoot)
        {

        }

        public sealed override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return MorePreDraw(spriteBatch, drawColor);
        }

        public virtual bool MorePreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return true;
        }
    }
}
