using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
	//this class also contains the NPC classes at the very bottom
	[Content(ContentType.Bosses)]
	public abstract class DungeonSoulBase : AssNPC
	{
		protected int frameSpeed;
		protected float fadeAwayMax;
		public static int SoulActiveTime = NPC.activeTime * 5;

		public static int wid = 34; //24
		public static int hei = 38;

		public sealed override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Soul");
			Main.npcFrameCount[NPC.type] = 6;
			Main.npcCatchable[NPC.type] = true;

			//Hide both souls
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true //Hides this NPC from the Bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			NPC.width = wid;
			NPC.height = hei;
			NPC.npcSlots = 0f;
			NPC.chaseable = false;
			NPC.dontCountMe = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.dontTakeDamage = true;
			NPC.friendly = true;
			NPC.noGravity = true;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.scale = 1f;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 0f;
			NPC.aiStyle = -1;
			AIType = -1;
			AnimationType = -1;
			NPC.color = new Color(0, 0, 0, 50);
			NPC.timeLeft = SoulActiveTime;
			NPC.direction = 1;
			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{

		}

		public static readonly short offsetYPeriod = 120;

		public ref float AI_Local_Timer => ref NPC.localAI[0];

		public static void KillInstantly(NPC npc)
		{
			npc.life = 0;
			npc.active = false;
			npc.netUpdate = true;
		}

		public override bool CheckActive()
		{
			//manually decrease timeleft
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.LoopAnimation(frameHeight, frameSpeed);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			drawColor = NPC.GetAlpha(drawColor) * 0.99f; //1f is opaque
			drawColor.R = Math.Max(drawColor.R, (byte)200); //100 for dark
			drawColor.G = Math.Max(drawColor.G, (byte)200);
			drawColor.B = Math.Max(drawColor.B, (byte)200);

			Texture2D image = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Rectangle bounds = new Rectangle
			{
				X = 0,
				Y = NPC.frame.Y,
				Width = image.Bounds.Width,
				Height = image.Bounds.Height / Main.npcFrameCount[NPC.type]
			};

			float sinY = 0;
			AI_Local_Timer = AI_Local_Timer > offsetYPeriod ? 0 : AI_Local_Timer + 1;
			sinY = (float)((Math.Sin((AI_Local_Timer / offsetYPeriod) * MathHelper.TwoPi) - 1) * 10);

			if (NPC.timeLeft <= fadeAwayMax)
			{
				drawColor = NPC.GetAlpha(drawColor) * (NPC.timeLeft / (float)fadeAwayMax);
			}

			Vector2 stupidOffset = new Vector2(wid / 2, (hei - 10f) + sinY);

			SpriteEffects effects = SpriteEffects.None;

			spriteBatch.Draw(image, NPC.position - screenPos + stupidOffset, bounds, drawColor, NPC.rotation, bounds.Size() / 2, NPC.scale, effects, 0f);
		}

		public override void AI()
		{
			--NPC.timeLeft;
			if (NPC.timeLeft < 0)
			{
				KillInstantly(NPC);
			}

			NPC.scale = 1f;

			NPC.noGravity = true;
			NPC.noTileCollide = false;
			NPC.velocity *= 0.55f;
		}
	}

	//the one the harvester hunts for
	public class DungeonSoul : DungeonSoulBase
	{
		public override void SafeSetDefaults()
		{
			frameSpeed = 6;
			NPC.catchItem = (short)ModContent.ItemType<CaughtDungeonSoul>();

			fadeAwayMax = 240;
		}
	}

	//the one that gets converted into
	public class DungeonSoulFreed : DungeonSoulBase
	{
		public override void SafeSetDefaults()
		{
			frameSpeed = 4;
			NPC.catchItem = (short)ModContent.ItemType<CaughtDungeonSoulFreed>();

			NPC.timeLeft = 3600;
			fadeAwayMax = 3600;
		}

		public override bool PreAI()
		{
			//only if awakened
			if (NPC.ai[2] != 0)
			{
				AI_Local_Timer = NPC.ai[2];
				NPC.ai[2] = 0;
			}
			NPC.noTileCollide = false;
			NPC.velocity *= 0.95f;

			--NPC.timeLeft;
			if (NPC.timeLeft < 0)
			{
				KillInstantly(NPC);
			}
			return false;
		}
	}
}
