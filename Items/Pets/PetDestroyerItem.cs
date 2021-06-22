using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.DroppedPets)]
    public class PetDestroyerItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<PetDestroyerHead>();

        public override int BuffType => ModContent.BuffType<PetDestroyerBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destroyer's Core");
            Tooltip.SetDefault("Summons a tiny Destroyer and two tiny Probes to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
        }

        public static void Spawn(Player player, int buffIndex = -1, Item item = null)
        {
            if (Main.myPlayer != player.whoAmI)
            {
                //Clientside only
                return;
            }

            IProjectileSource source;
            if (buffIndex > -1)
            {
                source = player.GetProjectileSource_Buff(buffIndex);
            }
            else if (item != null)
            {
                source = player.GetProjectileSource_Item(item);
            }
            else
            {
                return;
            }

            int probe = ModContent.ProjectileType<PetDestroyerProbe>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && (Array.IndexOf(PetDestroyerBase.wormTypes, proj.type) > -1 || proj.type == probe))
                {
                    proj.Kill();
                }
            }

            //prevIndex stuff only needed for when replacing/summoning the minion segments individually

            int index = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, player.direction, -player.gravDir, ModContent.ProjectileType<PetDestroyerHead>(), 0, 0f, player.whoAmI, 0f, 0f);
            int prevIndex = index;
            int off = PetDestroyerBase.DISTANCE_BETWEEN_SEGMENTS;

            for (int i = 1; i <= PetDestroyerBase.NUMBER_OF_BODY_SEGMENTS; i++)
            {
                index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerBody1>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                off += PetDestroyerBase.DISTANCE_BETWEEN_SEGMENTS;
                index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerBody2>(), 0, 0f, player.whoAmI, index, 0f);
                Main.projectile[prevIndex].localAI[1] = index;
                prevIndex = index;
                off += PetDestroyerBase.DISTANCE_BETWEEN_SEGMENTS;
            }
            index = Projectile.NewProjectile(source, player.Center.X - off * player.direction, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PetDestroyerTail>(), 0, 0f, player.whoAmI, index, 0f);
            Main.projectile[prevIndex].localAI[1] = index;

            //spawn probes
            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, probe, 0, 0f, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, probe, 0, 0f, player.whoAmI, 0f, ((player.whoAmI + 1) * 13) % PetDestroyerProbe.AttackDelay);
        }
    }
}
