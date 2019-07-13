using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;
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
        /// If the UI even opens if the condition is met
        /// </summary>
        public Func<bool> Condition { get; private set; }

        //all these three things get called when their respective condition returns true
        /// <summary>
        /// Holds data about what to draw
        /// </summary>
        public Func<CircleUIConf> UIConf { get; private set; }

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

        /// <summary>
        /// If it needs saving (has to be added manually to related hooks for saving/loading)
        /// </summary>
        public bool NeedsSaving { get; private set; }

        public CircleUIHandler(int triggerItem, Func<bool> condition, Func<CircleUIConf> uiConf, Func<int> onUIStart, Action onUIEnd, bool triggerLeft = true, bool needsSaving = false)
        {
            TriggerItem = triggerItem;
            Condition = condition;
            UIConf = uiConf;
            OnUIStart = onUIStart;
            OnUIEnd = onUIEnd;
            TriggerLeft = triggerLeft;
            NeedsSaving = needsSaving;
        }

        /// <summary>
        /// creates a simplified conf specific for pets
        /// </summary>
        public static CircleUIConf PetConf(string name, List<string> tooltips)
        {
            //uses VanitySelector as the triggerItem
            //order of tooltips must be the same as the order of textures (0, 1, 2 etc)

            List<string> textureNames = new List<string>();
            for (int i = 0; i < tooltips.Count; i++)
            {
                textureNames.Add(AssUtils.Instance.Name + "/Projectiles/Pets/" + name + "_" + i);
            }

            int type = AssUtils.Instance.ProjectileType(name);

            return new CircleUIConf(Main.projFrames[type], type, textureNames: textureNames, tooltips: tooltips);
        }

        /// <summary>
        /// Add trigger item for the UI to check for when holding the item type
        /// </summary>
        public static void AddItemAsTrigger(int triggerItemType, bool triggerLeft = true)
        {
            if (triggerLeft)
            {
                if (!TriggerListLeft.Contains(triggerItemType)) TriggerListLeft.Add(triggerItemType);
            }
            else
            {
                if (!TriggerListRight.Contains(triggerItemType)) TriggerListRight.Add(triggerItemType);
            }
        }
    }

    /// <summary>
    /// Contains data for displaying the Circle UI
    /// </summary>
    public class CircleUIConf
    {
        public List<string> TextureNames { get; private set; }
        public List<bool> Unlocked { get; private set; } //all true if just selection
        public List<string> Tooltips { get; private set; } //atleast "", only shown when unlocked
        public List<string> ToUnlock { get; private set; } //atleast "", only shown when !unlocked

        public int CircleAmount { get; private set; } //amount of spawned circles

        public int SpritesheetDivider { get; private set; } //divider of spritesheet height (if needed)

        public int AdditionalInfo { get; private set; } //mainly used for passing the projectile type atm

        public CircleUIConf(int spritesheetDivider = 0, int additionalInfo = -1, List<string> textureNames = null, List<bool> unlocked = null, List<string> tooltips = null, List<string> toUnlock = null)
        {
            if (textureNames == null || textureNames.Count <= 0) throw new Exception("'textureNames' has to be specified or has to contain at least one element");
            else CircleAmount = textureNames.Count;

            //Test if textures exist
            foreach (string texture in textureNames)
            {
                if (ModContent.GetTexture(texture) == null) throw new Exception("'texture' " + texture + " doesn't exist. Is it spelled correctly?");
            }

            if (unlocked == null) AssUtils.FillWithDefault(ref unlocked, true, CircleAmount);

            if (tooltips == null) AssUtils.FillWithDefault(ref tooltips, "", CircleAmount);

            if (toUnlock == null) AssUtils.FillWithDefault(ref toUnlock, "", CircleAmount);

            if (CircleAmount != unlocked.Count ||
                CircleAmount != tooltips.Count ||
                CircleAmount != toUnlock.Count) throw new Exception("Atleast one of the specified lists isn't the same length as textureNames");

            SpritesheetDivider = spritesheetDivider;
            AdditionalInfo = additionalInfo;

            TextureNames = new List<string>(textureNames);
            Unlocked = new List<bool>(unlocked);
            Tooltips = new List<string>(tooltips);
            ToUnlock = new List<string>(toUnlock);
        }
    }
}
