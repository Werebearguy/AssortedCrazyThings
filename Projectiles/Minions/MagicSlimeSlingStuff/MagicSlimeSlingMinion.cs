using AssortedCrazyThings.Dusts;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff
{
    public abstract class MagicSlimeSlingMinionBase : BabySlimeBase
    {
        private const int TimeLeft = 360;

        private const int PulsatingLimit = 30;

        private bool Increment = true;

        private bool Spawned = false;

        public byte ColorType = 0;

        public Color Color = default(Color);

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Minions/MagicSlimeSlingStuff/MagicSlimeSlingMinion";
            }
        }

        private int PulsatingCounter
        {
            get
            {
                return (int)projectile.localAI[1];
            }
            set
            {
                projectile.localAI[1] = value;
            }
        }

        private float PulsatingAlpha
        {
            get
            {
                //0.7f to 1f when full TimeLeft, drops down to 0.7f
                return 0.7f + ((float)PulsatingCounter / PulsatingLimit) * ((float)projectile.timeLeft / TimeLeft);
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Slime Sling Minion");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void MoreSetDefaults()
        {
            projectile.width = 24;
            projectile.height = 18;

            projectile.minion = true;
            customMinionSlots = 0f;
            projectile.timeLeft = TimeLeft;

            drawOriginOffsetY = 3;
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
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, -projectile.direction, -2f, 200, Color, 1f);
                dust.velocity *= 0.5f;
            }
            Main.PlaySound(SoundID.NPCDeath1.SoundId, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.NPCDeath1.Style, 0.7f, 0.2f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Color == default(Color)) return lightColor;
            Color color = lightColor.MultiplyRGB(Color) * PulsatingAlpha;
            if (color.A > 220) color.A = 220;
            return color;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Inflate(6, 6);
            hitbox.Y -= 6;
        }

        public override void PostAI()
        {
            if (!Spawned)
            {
                Spawned = true;
                Main.PlaySound(SoundID.Item9.SoundId, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item9.Style, 0.7f);
            }

            if (Increment)
            {
                PulsatingCounter++;
                if (PulsatingCounter >= PulsatingLimit) Increment = false;
            }
            else
            {
                PulsatingCounter--;
                if (PulsatingCounter <= 0) Increment = true;
            }

            if (projectile.frame > 1)
            {
                projectile.frame = 1;
            }

            if (projectile.ai[0] != 0)
            {
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

                if (Main.rand.NextFloat() < projectile.velocity.Length() / 7f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, dustType, 0f, 0f, 100, default(Color), 1.25f);
                    dust.velocity *= 0.1f;
                }
            }
        }
    }

    //Separate classes so projectile.usesIDStaticNPCImmunity works
    public class MagicSlimeSlingMinion1 : MagicSlimeSlingMinionBase { }

    public class MagicSlimeSlingMinion2 : MagicSlimeSlingMinionBase { }

    public class MagicSlimeSlingMinion3 : MagicSlimeSlingMinionBase { }
}
