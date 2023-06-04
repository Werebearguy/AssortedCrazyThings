using AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.NPCs.Bosses.Harvester
{
	[Content(ContentType.Bosses)]
	public class BabyHarvesterBuff : AssBuff
	{
		public const int FrameCount = 3;

		private static Asset<Texture2D> sheetAsset;

		private const string dummy = "REPLACEME";

		public static LocalizedText AbsorbedText { get; private set; }
		public static LocalizedText Tier1Text { get; private set; }
		public static LocalizedText Tier2Text { get; private set; }
		public static LocalizedText Tier3Text { get; private set; }

		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;

			Main.debuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;

			AbsorbedText = this.GetLocalization("Absorbed");
			Tier1Text = this.GetLocalization("Tier1");
			Tier2Text = this.GetLocalization("Tier2");
			Tier3Text = this.GetLocalization("Tier3");

			if (!Main.dedServ)
			{
				sheetAsset = ModContent.Request<Texture2D>(Texture + "_Sheet");
			}
		}

		public override void Unload()
		{
			sheetAsset = null;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			if (!TryGetBabyHarvester(out BabyHarvesterProj babyHarvester))
			{
				tip = tip.Replace(dummy, "");
				return;
			}

			int tier = babyHarvester.Tier;
			string toReplaceWith;
			if (tier == 1)
			{
				toReplaceWith = Tier1Text.ToString();
			}
			else if (tier == 2)
			{
				toReplaceWith = Tier2Text.ToString();
			}
			else
			{
				toReplaceWith = Tier3Text.ToString();
			}
			tip = tip.Replace(dummy, toReplaceWith);

			var numSouls = babyHarvester.SoulsEaten;
			if (numSouls > 0)
			{
				string amount = "" + babyHarvester.SoulsEaten;
				if (BabyHarvesterProj.TierToSoulsEaten.TryGetValue(BabyHarvesterProj.MaxTier, out var v))
				{
					amount += "/" + v;
				}

				tip += $"\n" + AbsorbedText.Format(amount);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
			if (!TryGetBabyHarvester(out BabyHarvesterProj babyHarvester))
			{
				return false;
			}

			Texture2D ourTexture = sheetAsset.Value;

			int frameY = babyHarvester.Tier - 1;
			Rectangle ourSourceRectangle = ourTexture.Frame(verticalFrames: FrameCount, frameY: frameY);

			drawParams.Texture = ourTexture;
			drawParams.SourceRectangle = ourSourceRectangle;

			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (TryGetBabyHarvester(out BabyHarvesterProj babyHarvester))
			{
				if (babyHarvester.PlayerOwner == player.whoAmI)
				{
					player.buffTime[buffIndex] = 2;
				}
			}
		}

		private static bool TryGetBabyHarvester(out BabyHarvesterProj babyHarvester)
		{
			babyHarvester = null;
			if (!BabyHarvesterHandler.TryFindBabyHarvester(out Projectile proj, out _))
			{
				return false;
			}

			if (!(proj.ModProjectile is BabyHarvesterProj babyHarvester2 && babyHarvester2.HasValidPlayerOwner))
			{
				return false;
			}

			babyHarvester = babyHarvester2;
			return true;
		}
	}
}
