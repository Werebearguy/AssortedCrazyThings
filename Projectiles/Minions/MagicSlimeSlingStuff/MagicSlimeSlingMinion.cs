using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

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
            drawOffsetX = -19;
            drawOriginOffsetX = -1;
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
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, -projectile.direction, -2f, 200, Color, 1f);
                dust.velocity *= 0.5f;
            }
            Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.NPCDeath1.Style, 0.8f, 0.2f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Color == default(Color)) return lightColor;
            float average = lightColor.GetAverage();
            Color color = Color * PulsatingAlpha * (average / 255f);
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
            //because projectile.scale breaks it
            if (projectile.spriteDirection == -1)
            {
                drawOriginOffsetX = -1;
            }
            else
            {
                drawOriginOffsetX = 3;
            }
        }
    }

    //Separate classes so projectile.usesIDStaticNPCImmunity works
    public class MagicSlimeSlingMinion1 : MagicSlimeSlingMinionBase { }
    public class MagicSlimeSlingMinion2 : MagicSlimeSlingMinionBase { }
    public class MagicSlimeSlingMinion3 : MagicSlimeSlingMinionBase { }
}
