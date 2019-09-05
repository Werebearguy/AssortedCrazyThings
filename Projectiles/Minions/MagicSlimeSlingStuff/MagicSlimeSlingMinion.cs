using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.MagicSlimeSlingStuff
{
    //TODO textures
    public abstract class MagicSlimeSlingMinionBase : BabySlimeBase
    {
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

        private const int TimeLeft = 360;

        private const int PulsatingLimit = 30;

        private bool Increment = true;

        private bool Spawned = false;

        public byte ColorType = 0;

        public Color Color = default(Color);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Slime Sling Minion");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void MoreSetDefaults()
        {
            projectile.width = 32;
            projectile.height = 30;
            projectile.scale = 0.5f;

            projectile.minion = true;
            customMinionSlots = 0f;
            projectile.timeLeft = TimeLeft;
            //drawOffsetX = -19;
            drawOriginOffsetX = 6;
            drawOriginOffsetY = -9;
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
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, -projectile.direction, -2f, 100, Color, 1f);
                dust.velocity *= 0.5f;
            }
            Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.NPCDeath1.Style, 0.8f, 0.2f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = lightColor * PulsatingAlpha;
            if (color.A > 200) color.A = 200;
            return color;
        }

        public override void PostAI()
        {
            if (!Spawned)
            {
                Spawned = true;
                Main.PlaySound(SoundID.Item9, projectile.Center);
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

            //int dustType = Main.rand.Next(20);
            //if (dustType == 0)
            //{
            //    dustType = mod.DustType<GlitterDust15>();
            //}
            //else if (dustType == 1)
            //{
            //    dustType = mod.DustType<GlitterDust57>();
            //}
            //else if (dustType == 2)
            //{
            //    dustType = mod.DustType<GlitterDust58>();
            //}
            //else
            //{
            //    return;
            //}
            //Dust dust = Dust.NewDustDirect(projectile.position + new Vector2(0f, 6f), projectile.width, projectile.height - 2, dustType, 0f, 0f, 100, default(Color), 0.8f);
            //dust.velocity *= 0.07f;
        }
    }

    //Separate classes so projectile.usesIDStaticNPCImmunity works
    public class MagicSlimeSlingMinion1 : MagicSlimeSlingMinionBase { }
    public class MagicSlimeSlingMinion2 : MagicSlimeSlingMinionBase { }
    public class MagicSlimeSlingMinion3 : MagicSlimeSlingMinionBase { }
}
