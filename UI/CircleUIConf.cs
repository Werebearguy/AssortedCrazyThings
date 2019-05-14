using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Pets;
using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.UI
{
    public class Temp
    {
        //Item it is used with
        public int TriggerItem { get; private set; }

        public Func<bool> Condition { get; private set; }

        //all these three things get called when their respective condition returns true
        //Holds data about what to draw
        public Func<CircleUIConf> UIConf { get; private set; }

        //assigns the CircleUI the selected thing
        public Func<int> OnUIStart { get; private set; }

        //Does things when the UI closes
        public Action OnUIEnd { get; private set; }

        //optional arguments
        //On leftclick
        public bool TriggerLeft { get; private set; }

        //if it needs saving, this will be the tag name for the tagCompound, saved as a byte thats returned from OnUIStart
        public string SavedName { get; private set; }

        public Temp(int triggerItem, Func<bool> condition, Func<CircleUIConf> uiConf, Func<int> onUIStart, Action onUIEnd, bool triggerLeft = true, string savedName = "")
        {
            TriggerItem = triggerItem;
            Condition = condition;
            UIConf = uiConf;
            OnUIStart = onUIStart;
            OnUIEnd = onUIEnd;
            TriggerLeft = triggerLeft;
            SavedName = savedName;
        }

        public static CircleUIConf PetConf(string name, List<string> tooltips)
        {
            //uses VanitySelector as the triggerItem
            //order of tooltips must be the same as the order of textures (0, 1, 2 etc)

            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 0; i < tooltips.Count; i++)
            {
                textures.Add(AssUtils.Instance.GetTexture("Projectiles/Pets/" + name + "_" + i));
            }

            int type = AssUtils.Instance.ProjectileType(name);

            return new CircleUIConf(Main.projFrames[type], type, textures: textures, tooltips: tooltips);
        }
    }
    public class CircleUIConf
    {
        public static List<int> TriggerListRight = new List<int>();
        public static List<int> TriggerListLeft = new List<int>();

        public List<Texture2D> Textures { get; private set; }
        public List<bool> Unlocked { get; private set; } //all true if just selection
        public List<string> Tooltips { get; private set; } //atleast "", only shown when unlocked
        public List<string> ToUnlock { get; private set; } //atleast "", only shown when !unlocked
        
        public int CircleAmount { get; private set; } //amount of spawned circles

        public int SpritesheetDivider { get; private set; }//divider of spritesheet height (if needed)

        public int AdditionalInfo { get; private set; }//mainly used for passing the projectile type atm

        public CircleUIConf(int spritesheetDivider = 0, int additionalInfo = -1, List<Texture2D> textures = null, List<bool> unlocked = null, List<string> tooltips = null, List<string> toUnlock = null)
        {
            if (textures == null || textures.Count <= 0) throw new Exception("texturesArg has to be specified or has to contain at least one element");
            else CircleAmount = textures.Count;

            if (unlocked == null) AssUtils.FillWithDefault(ref unlocked, true, CircleAmount);

            if (tooltips == null) AssUtils.FillWithDefault(ref tooltips, "", CircleAmount);

            if (toUnlock == null) AssUtils.FillWithDefault(ref toUnlock, "", CircleAmount);

            if (CircleAmount != unlocked.Count ||
                CircleAmount != tooltips.Count ||
                CircleAmount != toUnlock.Count) throw new Exception("atleast one of the specified lists isn't the same length as textures");

            SpritesheetDivider = spritesheetDivider;
            AdditionalInfo = additionalInfo;

            Textures = new List<Texture2D>(textures);
            Unlocked = new List<bool>(unlocked);
            Tooltips = new List<string>(tooltips);
            ToUnlock = new List<string>(toUnlock);
        }

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
}
