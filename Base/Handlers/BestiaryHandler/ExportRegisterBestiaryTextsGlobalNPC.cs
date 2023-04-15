using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Handlers.BestiaryHandler
{
	[Content(ContentType.FriendlyNPCs | ContentType.HostileNPCs | ContentType.Bosses, needsAllToFilterOut: true)]
	public class ExportRegisterBestiaryTextsGlobalNPC : AssGlobalNPC
	{
		public static readonly string Key = "BestiaryFlavorText";

		public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			//Filter only our NPCs
			if (npc.ModNPC?.Mod != Mod)
			{
				return;
			}

			//Filter only NPCs with a bestiary entry
			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(npc.type, out var offset) && offset.Hide)
			{
				return;
			}

			//EXPORT PART (you won't need this after you ran it once and deleted all manual FlavorTextBestiaryInfoElement creations)
			/*
			foreach (var item in bestiaryEntry.Info)
			{
				if (item is FlavorTextBestiaryInfoElement bestiaryInfo)
				{
					Type type = bestiaryInfo.GetType();
					var field = type.GetField("_key", BindingFlags.Instance | BindingFlags.NonPublic);
					var value = (string)field.GetValue(bestiaryInfo);
					npc.ModNPC.GetLocalization("BestiaryFlavorText", () => value);
					break;
				}
			}
			*/

			//REGISTER PART
			var text = npc.ModNPC.GetLocalization(Key, () => string.Empty);
			//In case you still have NPCs that appear in the bestiary, but without text, don't create an element
			if (text.ToString() != string.Empty)
			{
				bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement(text.Key));
			}
		}
	}
}
