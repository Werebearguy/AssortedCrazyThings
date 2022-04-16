using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)] //Tied to chunky/meatball which is hostile
	public abstract class ChunkysMeatballsEyeBase : AssNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DemonEye];
			Main.npcCatchable[NPC.type] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Position = new Vector2(0, -6f)
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 46;
			NPC.friendly = true;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 60;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 75f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.dontTakeDamage = true;
			AnimationType = NPCID.DemonEye;
		}

		public override void AI()
		{
			if (NPC.ai[0] == 0)
			{
				NPC.velocity.Y = -0.022f * 2f;
				NPC.netUpdate = true;
			}

			NPC.rotation = MathHelper.PiOver2;
			NPC.direction = 1;
			NPC.velocity.X = 0;
			NPC.ai[0]++;
			NPC.velocity.Y -= 0.022f * 1.5f; //0.022f * 2f;
			if (NPC.timeLeft > 80)
			{
				NPC.timeLeft = 80;
			}
		}
	}
}
