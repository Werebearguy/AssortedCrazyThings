using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    //TODO textures
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
            if (proj.modProjectile != null && proj.modProjectile is MagicSlimeSlingFired)
            {
                MagicSlimeSlingFired fired = (MagicSlimeSlingFired)proj.modProjectile;
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
            item.width = 12;
            item.height = 24;
            item.summon = true;
            item.damage = 5;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.useTime = 35;
            item.useAnimation = 35;
            item.UseSound = SoundID.Item19;
            item.mana = 10;
            item.shootSpeed = 9f;
            item.shoot = ModContent.ProjectileType<MagicSlimeSlingFired>();
            item.rare = -11;
            item.value = Item.sellPrice(silver: 15);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel, 20);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UseStyle(Player player)
        {
            //using shortsword arm position but reset the movement/rotation stuff
            player.itemLocation.X = player.Center.X;
            player.itemLocation.Y = player.Center.Y + 6f;
            player.itemRotation = 0f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
            float magnitude = new Vector2(speedX, speedY).Length();
            speedY = -1f;
            speedX = magnitude * Math.Sign(speedX);

            //PreSync uses current mPlayer.nextMagicSlimeSlingMinion
            AssUtils.NewProjectile(position.X, position.Y - 6f, speedX, speedY, type, damage, knockBack, preSync: PreSync);

            //switch to next type
            mPlayer.nextMagicSlimeSlingMinion = (byte)((mPlayer.nextMagicSlimeSlingMinion + 1) % MagicSlimeSlingMinionTypes);
            return false;
        }
    }
}
