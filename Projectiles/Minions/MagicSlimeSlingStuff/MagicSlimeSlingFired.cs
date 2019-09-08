using AssortedCrazyThings.Base;
using AssortedCrazyThings.Dusts;
using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff
{
    public class MagicSlimeSlingFired : ModProjectile
    {
        public byte ColorType = 0;
        public Color Color = default(Color);

        private void PreSync(Projectile proj)
        {
            if (proj.modProjectile != null && proj.modProjectile is MagicSlimeSlingMinionBase)
            {
                MagicSlimeSlingMinionBase minion = (MagicSlimeSlingMinionBase)proj.modProjectile;
                minion.ColorType = ColorType;
                //ActualColor won't be synced, its assigned in send/recv 
                minion.Color = MagicSlimeSling.GetColor(minion.ColorType);
            }
        }

        public override string Texture
        {
            get
            {
                return "Terraria/Item_" + ItemID.Gel;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Slime Sling Fired");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.minionSlots = 0f;
            projectile.width = 16;
            projectile.height = 14;
            projectile.aiStyle = -1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Color == default(Color)) return lightColor;
            float average = lightColor.GetAverage();
            return Color * (average * (255 - projectile.alpha) / 65025f) * 0.7f;
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
            Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.NPCDeath1.Style, 0.8f, 0.2f);
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, -projectile.direction, -2f, 200, Color, 1f);
                dust.velocity *= 0.4f;
            }

            if (projectile.active && Main.myPlayer == projectile.owner)
            {
                int sum = 0;
                for (int i = 0; i < MagicSlimeSling.Types.Length; i++)
                {
                    sum += Main.LocalPlayer.ownedProjectileCounts[MagicSlimeSling.Types[i]];
                }
                if (sum < (2 + Main.LocalPlayer.maxMinions))
                {
                    int type = MagicSlimeSling.Types[ColorType];
                    Vector2 velo = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-4, -2));
                    velo += -projectile.oldVelocity * 0.4f;
                    AssUtils.NewProjectile(projectile.Top, velo, type, projectile.damage, projectile.knockBack, preSync: PreSync);
                }
            }
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 15;
                if (projectile.alpha < 0) projectile.alpha = 0;
            }

            projectile.rotation += projectile.velocity.X * 0.05f;

            projectile.velocity.Y += 0.15f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            //stars
            int dustType = Main.rand.Next(4);
            if (dustType == 0)
            {
                dustType = mod.DustType<GlitterDust15>();
            }
            else if (dustType == 1)
            {
                dustType = mod.DustType<GlitterDust57>();
            }
            else if(dustType == 2)
            {
                dustType = mod.DustType<GlitterDust58>();
            }
            else
            {
                return;
            }
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, default(Color), 1.25f);
            dust.velocity *= 0.1f;
        }
    }
}
