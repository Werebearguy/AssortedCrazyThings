using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AssortedCrazyThings.UI
{
    public class CircleUIConf
    {
        public static List<int> TriggerList = new List<int>();

        public List<Texture2D> Textures { get; private set; }
        public List<bool> Unlocked { get; private set; } //all true if just selection
        public List<string> Tooltips { get; private set; } //atleast "", only shown when unlocked
        public List<string> ToUnlock { get; private set; } //atleast "", only shown when !unlocked

        // Amount of spawned circles
        public int circleAmount = -1;
        // divider of spritesheet height (if needed)
        public int spritesheetDivider = -1;

        //mainly used for passing the projectile type atm
        public int additionalInfo = -1;

        public CircleUIConf(int triggerItemType = 0, int spritesheetDividerArg = 0, int additionalInfoArg = -1, List<Texture2D> texturesArg = null, List<bool> unlockedArg = null, List<string> tooltipsArg = null, List<string> toUnlockArg = null)
        {
            if (triggerItemType == 0) throw new Exception("triggerItemType has to be specified as an item type (via 'mod.ItemType<ClassNameOfItem>()')");

            if (texturesArg == null || texturesArg.Count <= 0) throw new Exception("texturesArg has to be specified or has to contain at least one element");
            else circleAmount = texturesArg.Count;

            if (unlockedArg == null)
            {
                unlockedArg = new List<bool>();
                for (int i = 0; i < circleAmount; i++)
                {
                    unlockedArg.Add(true);
                }
            }

            if (tooltipsArg == null)
            {
                tooltipsArg = new List<string>();
                for (int i = 0; i < circleAmount; i++)
                {
                    tooltipsArg.Add("");
                }
            }

            if (toUnlockArg == null)
            {
                toUnlockArg = new List<string>();
                for (int i = 0; i < circleAmount; i++)
                {
                    toUnlockArg.Add("");
                }
            }

            if (circleAmount != unlockedArg.Count ||
                circleAmount != tooltipsArg.Count ||
                circleAmount != toUnlockArg.Count) throw new Exception("atleast one of the specified lists isn't the same length as texturesArg");

            spritesheetDivider = spritesheetDividerArg;
            additionalInfo = additionalInfoArg;

            Textures = new List<Texture2D>(texturesArg);
            Unlocked = new List<bool>(unlockedArg);
            Tooltips = new List<string>(tooltipsArg);
            ToUnlock = new List<string>(toUnlockArg);

            if (!TriggerList.Contains(triggerItemType)) TriggerList.Add(triggerItemType);
        }

        public static CircleUIConf DocileDemonEyeConf()
        {
            List<Texture2D> l1 = new List<Texture2D>();
            for (int i = 0; i < DocileDemonEyeProj.TotalNumberOfThese; i++)
            {
                l1.Add(AssUtils.Instance.GetTexture("Projectiles/Pets/DocileDemonEye_" + i));
            }
            
            List<string> l3 = new List<string>() { "Red", "Green", "Purple",
                "Red Fractured", "Green Fractured", "Purple Fractured",
                "Red Mechanical", "Green Mechanical", "Purple Mechanical",
                "Red Laser", "Green Laser", "Purple Laser" };

            //no need for unlocked + toUnlock
            int test2 = AssUtils.Instance.ProjectileType<DocileDemonEyeProj>();
            int test = Main.projFrames[test2];
            return new CircleUIConf(AssUtils.Instance.ItemType<VanitySelector>(),
                Main.projFrames[AssUtils.Instance.ProjectileType<DocileDemonEyeProj>()],
                AssUtils.Instance.ProjectileType<DocileDemonEyeProj>(),
                l1, null, l3, null);
        }

        public static CircleUIConf LifeLikeMechFrogConf()
        {
            List<Texture2D> l1 = new List<Texture2D>() { AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrog"),
                                                         AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrogCrown") };

            List<string> l3 = new List<string>() { "Regular", "Crowned"};

            //no need for unlocked + toUnlock
            return new CircleUIConf(AssUtils.Instance.ItemType<VanitySelector>(),
                Main.projFrames[AssUtils.Instance.ProjectileType<LifelikeMechanicalFrog>()],
                AssUtils.Instance.ProjectileType<LifelikeMechanicalFrog>(),
                l1, null, l3, null);
        }

        public static CircleUIConf CursedSkullConf()
        {
            List<Texture2D> l1 = new List<Texture2D>();
            for (int i = 0; i < CursedSkull.TotalNumberOfThese; i++)
            {
                l1.Add(AssUtils.Instance.GetTexture("Projectiles/Pets/CursedSkull_" + i));
            }

            List<string> l3 = new List<string>() { "Regular", "Dragon" };

            //no need for unlocked + toUnlock
            return new CircleUIConf(AssUtils.Instance.ItemType<VanitySelector>(),
                Main.projFrames[AssUtils.Instance.ProjectileType<CursedSkull>()],
                AssUtils.Instance.ProjectileType<CursedSkull>(),
                l1, null, l3, null);
        }

        public static CircleUIConf YoungWyvernConf()
        {
            List<Texture2D> l1 = new List<Texture2D>();
            for (int i = 0; i < YoungWyvern.TotalNumberOfThese; i++)
            {
                l1.Add(AssUtils.Instance.GetTexture("Projectiles/Pets/YoungWyvern_" + i));
            }

            List<string> l3 = new List<string>() { "Regular", "Mythical", "Arch", "Arch (Legacy)" };

            //no need for unlocked + toUnlock
            return new CircleUIConf(AssUtils.Instance.ItemType<VanitySelector>(),
                Main.projFrames[AssUtils.Instance.ProjectileType<YoungWyvern>()],
                AssUtils.Instance.ProjectileType<YoungWyvern>(),
                l1, null, l3, null);
        }
    }
}
