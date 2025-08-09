using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	/// <summary>
	/// Hides from bestiary, has default stats, drops, and same texture handle
	/// </summary>
	[Content(ContentType.HostileNPCs)]
	public abstract class DemonEyeRecolorBase : AssNPC
	{
		public virtual int TotalNumberOfThese => 0;

		public static LocalizedText CommonDisplayNameText { get; private set; }

		public override LocalizedText DisplayName => CommonDisplayNameText;

		public static HashSet<int> DemonEyes { get; private set; }

		public override void Load()
		{
			DemonEyes ??= new HashSet<int>();
		}

		public override void Unload()
		{
			DemonEyes = null;
		}

		public override void SetStaticDefaults()
		{
			CommonDisplayNameText ??= Mod.GetLocalization($"{LocalizationCategory}.DemonEyeRecolor.DisplayName");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DemonEye];
			NPCID.Sets.DemonEyes[NPC.type] = true;
			DemonEyes.Add(NPC.type);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true //Hides this NPC from the Bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
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

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			var vanillaDrops = Main.ItemDropsDB.GetRulesForNPCID(NPCID.DemonEye, false); // false is important here
			foreach (var dropRule in vanillaDrops)
			{
				npcLoot.Add(dropRule);
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / (float)NPC.lifeMax * 50f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 30; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2 * hit.HitDirection, -1f);
				}
			}
		}

		public ref float AiTexture => ref NPC.ai[3];

		public override bool PreAI()
		{
			if (AiTexture == 0 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient && TotalNumberOfThese > 0)
			{
				AiTexture = Main.rand.Next(TotalNumberOfThese);

				NPC.localAI[0] = 1;
				NPC.netUpdate = true;
			}

			return true;
		}
	}

	//Lazy solution: Count *all* demon eye variants towards regular demon eye. Proper would be to make separate bestiary entries for them just like vanilla
	[Content(ContentType.HostileNPCs)]
	public class DemonEyeRecolorBestiarySystem : AssSystem
	{
		public override void PostSetupContent()
		{
			foreach (var type in DemonEyeRecolorBase.DemonEyes)
			{
				ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[type] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCID.DemonEye];
			}
		}
	}
}
