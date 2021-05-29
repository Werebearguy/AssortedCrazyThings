using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class MagicSlimeSling : ModItem
    {
        public static Color GetColor(byte c)
        {
            //, 100 Alpha
            switch (c)
            {
                case 0:
                    //blue
                    return new Color(0, 80, 255);
                case 1:
                    //green
                    return new Color(0, 220, 40);
                default:
                    //ed
                    return new Color(255, 30, 0);
            }
        }

        private void PreSync(Projectile proj)
        {
            if (proj.ModProjectile != null && proj.ModProjectile is MagicSlimeSlingFired)
            {
                MagicSlimeSlingFired fired = (MagicSlimeSlingFired)proj.ModProjectile;
                fired.ColorType = proj.GetOwner().GetModPlayer<AssPlayer>().nextMagicSlimeSlingMinion;
                //Color won't be synced, its assigned in send/recv 
                fired.Color = GetColor(fired.ColorType);
            }
        }

        public const byte MagicSlimeSlingMinionTypes = 3;

        public static int[] Types => new int[]
        {
            ModContent.ProjectileType<MagicSlimeSlingMinion1>(),
            ModContent.ProjectileType<MagicSlimeSlingMinion2>(),
            ModContent.ProjectileType<MagicSlimeSlingMinion3>()
        };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Slime Sling");
            Tooltip.SetDefault("Shoots magic gel that turns into slime minions on hit");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 24;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 5;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.UseSound = SoundID.Item19;
            Item.mana = 10;
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<MagicSlimeSlingFired>();
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 15);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Gel, 20).AddIngredient(ItemID.FallenStar, 3).AddTile(TileID.WorkBenches).Register();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            //using shortsword arm position but reset the movement/rotation stuff
            player.itemLocation.X = player.Center.X;
            player.itemLocation.Y = player.Center.Y + 6f;
            player.itemRotation = 0f;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            float magnitude = velocity.Length();
            velocity.Y = -1f;
            velocity.X = magnitude * Math.Sign(velocity.X);

            //PreSync uses current mPlayer.nextMagicSlimeSlingMinion
            int index = AssUtils.NewProjectile(source, position.X, position.Y - 6f, velocity.X, velocity.Y, type, damage, knockback, preSync: PreSync);
            Main.projectile[index].originalDamage = damage;

            //switch to next type
            mPlayer.nextMagicSlimeSlingMinion = (byte)((mPlayer.nextMagicSlimeSlingMinion + 1) % MagicSlimeSlingMinionTypes);
            return false;
        }
    }
}
