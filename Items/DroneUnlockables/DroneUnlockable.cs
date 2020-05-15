using AssortedCrazyThings.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.DroneUnlockables
{
    public abstract class DroneUnlockable : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.rare = -11;
            item.width = 26;
            item.height = 24;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item4;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.sellPrice(silver: 50);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            string tooltip = DroneController.GetDroneData(UnlockedType).Name + " for the Drone Controller";
            if (!mPlayer.droneControllerUnlocked.HasFlag(UnlockedType))
            {
                tooltip = "Unlocks the " + tooltip;
            }
            else
            {
                tooltip = "Already unlocked " + tooltip;
            }
            tooltips.Add(new TooltipLine(mod, "Unlocks", tooltip));
        }

        public abstract DroneType UnlockedType { get; }

        public override bool CanUseItem(Player player)
        {
            if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI)
            {
                AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
                if (!mPlayer.droneControllerUnlocked.HasFlag(UnlockedType))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI)
            {
                player.GetModPlayer<AssPlayer>().droneControllerUnlocked |= UnlockedType;
                Main.NewText("Unlocked: " + DroneController.GetDroneData(UnlockedType).Name, CombatText.HealLife);
            }
            return true;
        }

        public override void AddRecipes()
        {
            DroneRecipe recipe = new DroneRecipe(mod, UnlockedType);
            recipe.AddIngredient(ModContent.ItemType<DroneParts>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class DroneRecipe : ModRecipe
    {
        public DroneType UnlockedType;

        public DroneRecipe(Mod mod, DroneType unlockedType) : base(mod)
        {
            UnlockedType = unlockedType;
        }

        public override bool RecipeAvailable()
        {
            return !Main.LocalPlayer.GetModPlayer<AssPlayer>().droneControllerUnlocked.HasFlag(UnlockedType);
        }
    }

    public class DroneUnlockableBasicLaserDrone : DroneUnlockable
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basic Laser Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.BasicLaser;
            }
        }
    }

    public class DroneUnlockableHeavyLaserDrone : DroneUnlockable
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavy Laser Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.HeavyLaser;
            }
        }
    }

    public class DroneUnlockableMissileDrone : DroneUnlockable
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missile Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.Missile;
            }
        }
    }

    public class DroneUnlockableHealingDrone : DroneUnlockable
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.Healing;
            }
        }
    }

    public class DroneUnlockableShieldDrone : DroneUnlockable
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.Shield;
            }
        }
    }
}
