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
	[Content(ContentType.CuteSlimes)]
	public abstract class CuteSlimeBaseNPC : AssNPC
	{
		public abstract int CatchItem { get; }

		public abstract SpawnConditionType SpawnCondition { get; }

		public abstract Color DustColor { get; }

		public virtual bool ShouldDropGel => true;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
			Main.npcCatchable[NPC.type] = true;
			NPCID.Sets.CountsAsCritter[NPC.type] = true; //Guide To Critter Companionship

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
		{
			NPC.friendly = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.width = 28;
			NPC.height = 33;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.rarity = 1;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 25f;
			NPC.aiStyle = 1;
			AIType = NPCID.ToxicSludge;
			AnimationType = NPCID.ToxicSludge;
			NPC.alpha = 75;
			NPC.catchItem = CatchItem;

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
			return player.CanBeHitByItemCritterLike(NPC);
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return projectile.CanBeHitByProjectileCritterLike(NPC);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			Color color = DustColor;
			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / NPC.lifeMax * 100f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, NPC.alpha, color);
				}
			}
			else
			{
				for (int i = 0; i < 30; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, 2 * hit.HitDirection, -2f, NPC.alpha, color);
				}
			}
		}

		public virtual void SafeSetDefaults()
		{

		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SlimePets.CuteSlimeSpawnChance(spawnInfo, SpawnCondition);
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
