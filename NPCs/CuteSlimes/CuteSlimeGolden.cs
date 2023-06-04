using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
	public class CuteSlimeGolden : CuteSlimeBaseNPC
	{
		public override int CatchItem
		{
			get
			{
				return ModContent.ItemType<CuteSlimeGoldenItem>();
			}
		}

		public override SpawnConditionType SpawnCondition
		{
			get
			{
				return SpawnConditionType.None;
			}
		}

		public override bool CannotTransformIntoShimmerOrRareVariants => false;

		public override Color DustColor => Color.Transparent;

		public override void HitEffect(NPC.HitInfo hit)
		{
			//No base call
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			var entitySource = NPC.GetSource_Death();

			int count = 7;
			float scale = 1.1f;
			int type = 10;
			Color color = default(Color);
			if (NPC.life <= 0)
			{
				scale = 1.5f;
				count = 40;
				for (int i = 0; i < 8; i++)
				{
					Gore gore = Gore.NewGoreDirect(entitySource, new Vector2(NPC.position.X, NPC.Center.Y - 10f), Vector2.Zero, 1218);
					gore.velocity = new Vector2(Main.rand.Next(1, 10) * 0.3f * 2.5f * hit.HitDirection, 0f - (3f + Main.rand.Next(4) * 0.3f));
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					Gore gore = Gore.NewGoreDirect(entitySource, new Vector2(NPC.position.X, NPC.Center.Y - 10f), Vector2.Zero, 1218);
					gore.velocity = new Vector2(Main.rand.Next(1, 10) * 0.3f * 2f * hit.HitDirection, 0f - (2.5f + Main.rand.Next(4) * 0.3f));
				}
			}

			for (int k = 0; k < count; k++)
			{
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, type, 2 * hit.HitDirection, -1f, 80, color, scale);
				if (!Main.rand.NextBool(3))
				{
					dust.noGravity = true;
				}
			}
		}

		public override void SafeSetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true //Hides this NPC from the Bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;
		}

		public override void SafeSetDefaults()
		{
			NPC.scale = 0.9f;
			NPC.alpha = 0;
		}

		public override void DrawEffects(ref Color drawColor)
		{
			Color color = new Color(204, 181, 72, 255);
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), color.R / 255f * 1.1f, color.G / 255f * 1.1f, color.B / 255f * 1.1f);
			if (NPC.velocity.LengthSquared() > 1f || !Main.rand.NextBool(3))
			{
				int offset = 4;
				Vector2 vector = NPC.position + new Vector2(-offset, -offset);
				int width = NPC.width + offset * 2;
				int height = NPC.height + offset * 2;
				Dust dust = Dust.NewDustDirect(vector, width, height, 246);
				dust.noGravity = true;
				dust.noLightEmittence = true;
				dust.velocity *= 0.2f;
				dust.scale = 1.3f;
			}
		}
	}
}
