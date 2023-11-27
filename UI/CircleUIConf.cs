using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.UI
{
	/// <summary>
	/// Contains Information about the data and the actions necessary to open/close/act upon the data in the Circle UI
	/// 
	/// </summary>
	/// <seealso cref="CircleUI"/>
	public class CircleUIHandler
	{
		/// <summary>
		/// List that contains the trigger items (when using right click) from all CircleUIHandlers combined
		/// (makes the check easier on the CPU each tick since most Handlers share the trigger item)
		/// </summary>
		public static List<int> TriggerListRight = new List<int>();
		/// <summary>
		/// List that contains the trigger items (when using left click) from all CircleUIHandlers combined
		/// (makes the check easier on the CPU each tick since most Handlers share the trigger item)
		/// </summary>
		public static List<int> TriggerListLeft = new List<int>();

		/// <summary>
		/// Item it is used with
		/// </summary>
		public int TriggerItem { get; private set; }

		/// <summary>
		/// If the UI even opens when the condition is met
		/// </summary>
		public Func<bool> Condition { get; private set; }

		//all these three things get called when their respective condition returns true
		//gets also called after loading, to initialize localization
		/// <summary>
		/// Holds data about what to draw
		/// </summary>
		public Func<bool, CircleUIConf> UIConf { get; private set; }

		/// <summary>
		/// Assigns the CircleUI the selected thing
		/// </summary>
		public Func<int> OnUIStart { get; private set; }

		/// <summary>
		/// Does things when the UI closes
		/// </summary>
		public Action OnUIEnd { get; private set; }

		//optional arguments
		/// <summary>
		/// If the UI opens on left click (false if right click)
		/// </summary>
		public bool TriggerLeft { get; private set; }

		public CircleUIHandler(int triggerItem, Func<bool> condition, Func<bool, CircleUIConf> uiConf, Func<int> onUIStart, Action onUIEnd, bool triggerLeft = true)
		{
			TriggerItem = triggerItem;
			Condition = condition;
			UIConf = uiConf;
			OnUIStart = onUIStart;
			OnUIEnd = onUIEnd;
			TriggerLeft = triggerLeft;
		}

		/// <summary>
		/// creates a simplified conf specific for pets
		/// </summary>
		public static CircleUIConf PetConf(string name, List<string> tooltips, Vector2 drawOffset = default)
		{
			//uses VanitySelector as the triggerItem
			//order of tooltips must be the same as the order of textures (0, 1, 2 etc)

			List<Asset<Texture2D>> assets = new List<Asset<Texture2D>>();
			for (int i = 0; i < tooltips.Count; i++)
			{
				assets.Add(AssUtils.Instance.Assets.Request<Texture2D>("Projectiles/Pets/" + name + "_" + i));
			}

			int type = AssUtils.Instance.Find<ModProjectile>(name).Type;

			return new CircleUIConf($"Projectiles.{name}", Main.projFrames[type], type, assets: assets, tooltips: tooltips, drawOffset: drawOffset);
		}

		public static CircleUIConf PetConf(int type, List<string> tooltips, Vector2 drawOffset = default)
		{
			return PetConf(ModContent.GetModProjectile(type).Name, tooltips, drawOffset);
		}

		/// <summary>
		/// Add trigger item for the UI to check for when holding the item type
		/// </summary>
		public void AddTriggers()
		{
			int triggerItemType = TriggerItem;
			var list = TriggerLeft ? TriggerListLeft : TriggerListRight;
			if (!list.Contains(triggerItemType))
			{
				list.Add(triggerItemType);
			}
		}
	}

	/// <summary>
	/// Contains data for displaying the Circle UI
	/// </summary>
	public class CircleUIConf
	{
		public List<Asset<Texture2D>> Assets { get; private set; }
		public List<bool> Unlocked { get; private set; } //all true if just selection
		public List<LocalizedText> Tooltips { get; private set; } //atleast LocalizedText.Empty, only shown when unlocked
		public List<LocalizedText> ToUnlock { get; private set; } //atleast LocalizedText.Empty, only shown when !unlocked
		public Vector2 DrawOffset { get; private set; } //if the preview is off from the selection, offsets it

		public int CircleAmount { get; private set; } //amount of spawned circles

		public int SpritesheetDivider { get; private set; } //divider of spritesheet height (if needed)

		public int AdditionalInfo { get; private set; } //mainly used for passing the projectile type atm

		public CircleUIConf(int spritesheetDivider = 0, int additionalInfo = -1, List<Asset<Texture2D>> assets = null, List<bool> unlocked = null, List<LocalizedText> tooltips = null, List<LocalizedText> toUnlock = null, Vector2 drawOffset = default)
		{
			if (assets == null || assets.Count <= 0) throw new Exception($"'{nameof(assets)}' has to be specified or has to contain at least one element");
			else CircleAmount = assets.Count;

			if (unlocked == null) AssUtils.FillWithDefault(ref unlocked, true, CircleAmount);

			//Test if textures exist
			//foreach (string texture in textureNames)
			//{
			//    if (ModContent.Request<Texture2D>(texture) == null) throw new Exception("'texture' " + texture + " doesn't exist. Is it spelled correctly?");
			//}

			if (tooltips == null) AssUtils.FillWithDefault(ref tooltips, LocalizedText.Empty, CircleAmount);
			if (toUnlock == null) AssUtils.FillWithDefault(ref toUnlock, LocalizedText.Empty, CircleAmount);

			ConstructorHelper(spritesheetDivider, additionalInfo, assets, unlocked, tooltips, toUnlock, drawOffset);
		}

		public CircleUIConf(string key, int spritesheetDivider = 0, int additionalInfo = -1, List<Asset<Texture2D>> assets = null, List<bool> unlocked = null, List<string> tooltips = null, List<string> toUnlock = null, Vector2 drawOffset = default)
		{
			if (assets == null || assets.Count <= 0) throw new Exception($"'{nameof(assets)}' has to be specified or has to contain at least one element");
			else CircleAmount = assets.Count;

			if (unlocked == null) AssUtils.FillWithDefault(ref unlocked, true, CircleAmount);

			//Test if textures exist
			//foreach (string texture in textureNames)
			//{
			//    if (ModContent.Request<Texture2D>(texture) == null) throw new Exception("'texture' " + texture + " doesn't exist. Is it spelled correctly?");
			//}

			List<LocalizedText> tooltipTexts, toUnlockTexts;
			FillLists(key, tooltips, toUnlock, out tooltipTexts, out toUnlockTexts);

			ConstructorHelper(spritesheetDivider, additionalInfo, assets, unlocked, tooltipTexts, toUnlockTexts, drawOffset);
		}

		private void ConstructorHelper(int spritesheetDivider, int additionalInfo, List<Asset<Texture2D>> assets, List<bool> unlocked, List<LocalizedText> tooltips, List<LocalizedText> toUnlock, Vector2 drawOffset)
		{
			if (CircleAmount != unlocked.Count ||
				CircleAmount != toUnlock.Count ||
				CircleAmount != tooltips.Count) throw new Exception($"Atleast one of the specified lists isn't the same length as '{nameof(assets)}'");

			SpritesheetDivider = spritesheetDivider;
			AdditionalInfo = additionalInfo;

			Assets = new List<Asset<Texture2D>>(assets);
			Unlocked = new List<bool>(unlocked);
			Tooltips = new List<LocalizedText>(tooltips);
			ToUnlock = new List<LocalizedText>(toUnlock);
			DrawOffset = drawOffset;
		}

		private void FillLists(string key, List<string> tooltips, List<string> toUnlock, out List<LocalizedText> tooltipTexts, out List<LocalizedText> toUnlockTexts)
		{
			tooltipTexts = null;
			toUnlockTexts = null;
			if (tooltips == null) AssUtils.FillWithDefault(ref tooltipTexts, LocalizedText.Empty, CircleAmount);
			else
			{
				tooltipTexts = new List<LocalizedText>();
				foreach (var variation in tooltips)
				{
					tooltipTexts.Add(variation != "" ? AssUtils.Instance.GetLocalization($"{key}.{variation}.Tooltip", () => variation) : LocalizedText.Empty);
				}
			}

			if (toUnlock == null) AssUtils.FillWithDefault(ref toUnlockTexts, LocalizedText.Empty, CircleAmount);
			else
			{
				toUnlockTexts = new List<LocalizedText>();
				foreach (var variation in toUnlock)
				{
					toUnlockTexts.Add(variation != "" ? AssUtils.Instance.GetLocalization($"{key}.{variation}.ToUnlock", () => variation) : LocalizedText.Empty);
				}
			}
		}
	}
}
