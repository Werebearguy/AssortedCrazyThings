using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    //Not ContentType.FriendlyNPCs, but separate toggle
    [Autoload]
    public abstract class CuteSlimeBaseNPC : AssNPC
    {
        public abstract string IngameName { get; }

        public abstract int CatchItem { get; }

        public abstract SpawnConditionType SpawnCondition { get; }

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
            NPCID.Sets.CountsAsCritter[NPC.type] = true;

            SafeSetStaticDefaults();
        }

        public virtual void SafeSetStaticDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            NPC.friendly = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.width = 28;
            NPC.height = 33;
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

            SafeSetDefaults();

            // Slime AI breaks with big enough height when it jumps against a low ceiling
            // then glitches into the ground
            if (NPC.scale > 1f)
            {
                NPC.height -= (int)((NPC.scale - 1f) * NPC.height);
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return null; //TODO NPC return true
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return !projectile.minion;
        }

        public virtual void SafeSetDefaults()
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

            SafeModifyNPCLoot(npcLoot);
        }

        public virtual void SafeModifyNPCLoot(NPCLoot npcLoot)
        {

        }

        public sealed override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return SafePreDraw(spriteBatch, screenPos, drawColor);
        }

        public virtual bool SafePreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }
    }
}
