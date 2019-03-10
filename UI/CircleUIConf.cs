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
        public int CircleAmount { get; private set; }
        // divider of spritesheet height (if needed)
        public int SpritesheetDivider { get; private set; }

        //mainly used for passing the projectile type atm
        public int AdditionalInfo { get; private set; }

        public CircleUIConf(int spritesheetDivider = 0, int additionalInfo = -1, List<Texture2D> textures = null, List<bool> unlocked = null, List<string> tooltips = null, List<string> toUnlock = null)
        {
            if (textures == null || textures.Count <= 0) throw new Exception("texturesArg has to be specified or has to contain at least one element");
            else CircleAmount = textures.Count;

            if (unlocked == null)
            {
                unlocked = new List<bool>();
                for (int i = 0; i < CircleAmount; i++)
                {
                    unlocked.Add(true);
                }
            }

            if (tooltips == null)
            {
                tooltips = new List<string>();
                for (int i = 0; i < CircleAmount; i++)
                {
                    tooltips.Add("");
                }
            }

            if (toUnlock == null)
            {
                toUnlock = new List<string>();
                for (int i = 0; i < CircleAmount; i++)
                {
                    toUnlock.Add("");
                }
            }

            if (CircleAmount != unlocked.Count ||
                CircleAmount != tooltips.Count ||
                CircleAmount != toUnlock.Count) throw new Exception("atleast one of the specified lists isn't the same length as texturesArg");

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

            return new CircleUIConf(Main.projFrames[type], type, l1, null, tooltips, null);
        }

        //here start the specific confs that are called in PostSetupContent
        //everhallowed

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

            List<bool> unlocked = new List<bool>()
            {
                true,                //      0
                NPC.downedMechBoss3, //skele 1
                NPC.downedMechBoss2, //twins 2
                NPC.downedMechBoss1, //destr 3
            };

            return new CircleUIConf(8, -1, textures, unlocked, tooltips, toUnlock);
        }

        //pets

        public static CircleUIConf LifeLikeMechFrogConf()
        {
            List<Texture2D> l1 = new List<Texture2D>() { AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrog"),
                                                         AssUtils.Instance.GetTexture("Projectiles/Pets/LifelikeMechanicalFrogCrown") };

            List<string> l3 = new List<string>() { "Default", "Crowned" };

            //no need for unlocked + toUnlock
            return new CircleUIConf(
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
            List<string> tooltips = new List<string>() { "Default", "Dragon" };

            return PetConf("CursedSkull", tooltips);
        }

        public static CircleUIConf YoungWyvernConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Mythical", "Arch", "Arch (Legacy)" };
            
            return PetConf("YoungWyvern", tooltips);
        }

        public static CircleUIConf PetFishronConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Sharkron", "Sharknado" };

            return PetConf("PetFishronProj", tooltips);
        }

        public static CircleUIConf PetMoonConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Orange", "Green" }; //only 0, 1, 2 registered, 3 and 4 are event related

            return PetConf("PetMoonProj", tooltips);
        }

        public static CircleUIConf YoungHarpyConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Eagle", "Raven", "Dove" };

            return PetConf("YoungHarpy", tooltips);
        }

        public static CircleUIConf AbeeminiationConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Snow Bee", "Oil Spill", "Missing Ingredients" };

            return PetConf("AbeeminationProj", tooltips);
        }

        public static CircleUIConf LilWrapsConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Dark", "Light", "Shadow", "Spectral" };

            return PetConf("LilWrapsProj", tooltips);
        }

        public static CircleUIConf VampireBatConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Werebat" };

            return PetConf("VampireBat", tooltips);
        }

        public static CircleUIConf PigronataConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Winter", "Autumn", "Spring", "Summer", "Halloween", "Christmas" };

            return PetConf("Pigronata", tooltips);
        }

        public static CircleUIConf QueenLarvaConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Prawn Larva", "Unexpected Seed", "Big Kid Larva", "Where's The Baby?" };

            return PetConf("QueenLarvaProj", tooltips);
        }

        public static CircleUIConf OceanSlimeConf()
        {
            List<string> tooltips = new List<string>() { "Default", "Stupid Hat", "Gnarly Grin", "Flipped Jelly" };

            return PetConf("OceanSlimeProj", tooltips);
        }

        //ALTERNATE
        //public static CircleUIConf ClassNameConf()
        //{
        //    List<string> tooltips = new List<string>() { "Default", "AltName1", "AltName2" };

        //    return PetConf("ClassNameProj", tooltips);
        //}
    }
}
