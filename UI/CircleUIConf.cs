using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.UI
{
    public class CircleUIConf
    {
        public static List<int> TriggerListRight = new List<int>();
        public static List<int> TriggerListLeft = new List<int>();

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

        public CircleUIConf(int triggerItemType = 0, bool triggerLeft = true, int spritesheetDividerArg = 0, int additionalInfoArg = -1, List<Texture2D> texturesArg = null, List<bool> unlockedArg = null, List<string> tooltipsArg = null, List<string> toUnlockArg = null)
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

            if (triggerLeft)
            {
                if (!TriggerListLeft.Contains(triggerItemType)) TriggerListLeft.Add(triggerItemType);
            }
            else
            {
                if (!TriggerListRight.Contains(triggerItemType)) TriggerListRight.Add(triggerItemType);
            }
        }

        private static CircleUIConf PetConf(string name, List<string> tooltips)
        {
            //uses VanitySelector as the triggerItem
            //order of tooltips must be the same as the order of textures (0, 1 2 etc)

            List<Texture2D> l1 = new List<Texture2D>();
            for (int i = 0; i < tooltips.Count; i++)
            {
                l1.Add(AssUtils.Instance.GetTexture("Projectiles/Pets/" + name + "_" + i));
            }

            int type = AssUtils.Instance.ProjectileType(name);

            return new CircleUIConf(AssUtils.Instance.ItemType<VanitySelector>(), true, Main.projFrames[type], type, l1, null, tooltips, null);
        }

        //here start the specific confs that are called in PostSetupContent

        public static CircleUIConf LifeLikeMechFrogConf()
        {
            List<Texture2D> l1 = new List<Texture2D>() { AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrog"),
                                                         AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrogCrown") };

            List<string> l3 = new List<string>() { "Regular", "Crowned" };

            //no need for unlocked + toUnlock
            return new CircleUIConf(AssUtils.Instance.ItemType<VanitySelector>(),
                true,
                Main.projFrames[AssUtils.Instance.ProjectileType<LifelikeMechanicalFrog>()],
                AssUtils.Instance.ProjectileType<LifelikeMechanicalFrog>(),
                l1, null, l3, null);
        }

        public static CircleUIConf DocileDemonEyeConf()
        {
            List<string> tooltips = new List<string>() { "Red", "Green", "Purple",
                "Red Fractured", "Green Fractured", "Purple Fractured",
                "Red Mechanical", "Green Mechanical", "Purple Mechanical",
                "Red Laser", "Green Laser", "Purple Laser" };

            return PetConf("DocileDemonEyeProj", tooltips);
        }

        public static CircleUIConf CursedSkullConf()
        {
            List<string> tooltips = new List<string>() { "Regular", "Dragon" };

            return PetConf("CursedSkull", tooltips);
        }

        public static CircleUIConf YoungWyvernConf()
        {
            List<string> tooltips = new List<string>() { "Regular", "Mythical", "Arch", "Arch (Legacy)" };
            
            return PetConf("YoungWyvern", tooltips);
        }

        public static CircleUIConf PetFishronConf()
        {
            List<string> tooltips = new List<string>() { "Regular", "Sharkron", "Sharknado" };

            return PetConf("PetFishronProj", tooltips);
        }

        public static CircleUIConf EverhallowedLanternConf()
        {
            List<Texture2D> textures = new List<Texture2D>();
            List<string> tooltips = new List<string>();
            List<string> toUnlock = new List<string>();
            for (int soulType = 0; soulType < 4; soulType++)
            {
                var stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(soulType, fromUI: true);
                var tempSoulType = (CompanionDungeonSoulMinionBase.SoulType)stats.SoulType;
                string tooltip = tempSoulType.ToString()
                    + "\nBase Damage: " + stats.Damage
                    + "\nBase Knockback: " + stats.Knockback
                    + "\n" + stats.Description;
                textures.Add(Main.projectileTexture[stats.Type]);
                tooltips.Add(tooltip);
                toUnlock.Add(stats.ToUnlock);
            }

            List <bool> unlocked = new List<bool>()
            {
                true,                //      0
                NPC.downedMechBoss3, //skele 1
                NPC.downedMechBoss2, //twins 2
                NPC.downedMechBoss1, //destr 3
            };
            
            return new CircleUIConf(AssUtils.Instance.ItemType<EverhallowedLantern>(), false, 8, -1, textures, unlocked, tooltips, toUnlock);
        }
    }
}
