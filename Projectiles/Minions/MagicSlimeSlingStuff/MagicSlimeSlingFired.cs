using AssortedCrazyThings.Base;
using AssortedCrazyThings.Dusts;
using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff
{
    public class MagicSlimeSlingFired : ModProjectile
    {
        public byte ColorType = 0;
        public Color Color = default(Color);

        private void PreSync(Projectile proj)
        {
            if (proj.ModProjectile != null && proj.ModProjectile is MagicSlimeSlingMinionBase)
            {
                MagicSlimeSlingMinionBase minion = (MagicSlimeSlingMinionBase)proj.ModProjectile;
                minion.ColorType = ColorType;
                //ActualColor won't be synced, its assigned in send/recv 
                minion.Color = MagicSlimeSling.GetColor(minion.ColorType);
            }
        }

        public override string Texture
        {
            get
            {
                return "Terraria/Images/Item_" + ItemID.Gel;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Slime Sling Fired");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.DamageType = DamageClass.Summon;
            //Projectile.minion = true;
            Projectile.friendly = true;
            Projectile.minionSlots = 0f;
            Projectile.width = 16;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.alpha = 255;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Color == default(Color)) return lightColor;
            return lightColor.MultiplyRGB(Color) * ((255 - Projectile.alpha) / 255f) * 0.7f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)ColorType);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ColorType = reader.ReadByte();
            if (Color == default(Color)) Color = MagicSlimeSling.GetColor(ColorType);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.Center.X, (int)Projectile.Center.Y, SoundID.NPCDeath1.Style, 0.8f, 0.2f);
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 4, -Projectile.direction, -2f, 200, Color, 1f);
                dust.velocity *= 0.4f;
            }

            if (Projectile.active && Main.myPlayer == Projectile.owner)
            {
                int sum = 0;
                for (int i = 0; i < MagicSlimeSling.Types.Length; i++)
                {
                    sum += Main.LocalPlayer.ownedProjectileCounts[MagicSlimeSling.Types[i]];
                }
                if (sum < (2 + Projectile.GetOwner().maxMinions))
                {
                    int type = MagicSlimeSling.Types[ColorType];
                    Vector2 velo = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-4, -2));
                    velo += -Projectile.oldVelocity * 0.4f;
                    int index = AssUtils.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Top, velo, type, Projectile.damage, Projectile.knockBack, preSync: PreSync);
                    Main.projectile[index].originalDamage = Projectile.originalDamage;
                }
            }
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
                if (Projectile.alpha < 0) Projectile.alpha = 0;
            }

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            Projectile.velocity.Y += 0.15f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            if (Projectile.alpha > 200) return;

            //colored sparkles
            int dustType = Main.rand.Next(4);
            if (dustType == 0)
            {
                dustType = ModContent.DustType<GlitterDust15>();
            }
            else if (dustType == 1)
            {
                dustType = ModContent.DustType<GlitterDust57>();
            }
            else if (dustType == 2)
            {
                dustType = ModContent.DustType<GlitterDust58>();
            }
            else
            {
                return;
            }
            // 8f is the shootspeed of the weapon shooting this projectile
            if (Main.rand.NextFloat() < Projectile.velocity.Length() / 7f)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default(Color), 1.25f);
                dust.velocity *= 0.1f;
            }
        }
    }
}
