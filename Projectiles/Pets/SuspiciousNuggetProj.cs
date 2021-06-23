using AssortedCrazyThings.Base;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class SuspiciousNuggetProj : SimplePetProjBase
    {
        private int frame2Counter = 0;
        private int frame2 = 0;
        private float rot2 = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Suspicious Nugget");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyGrinch);
            Projectile.width = 22;
            Projectile.height = 22;
            AIType = ProjectileID.BabyRedPanda;
            DrawOriginOffsetY = -4;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.grinch = false; // Relic from AIType

            GetFrame();

            return true;
        }

        public bool InAir => Projectile.ai[0] != 0f;

        private void GetFrame()
        {
            if (!InAir) //not flying
            {
                rot2 = 0;
                if (Projectile.velocity.Y == 0f)
                {
                    if (Projectile.velocity.X == 0f)
                    {
                        frame2 = 0;
                        frame2Counter = 0;
                    }
                    else if (Projectile.velocity.X < -0.8f || Projectile.velocity.X > 0.8f)
                    {
                        frame2Counter += (int)Math.Abs(Projectile.velocity.X);
                        frame2Counter++;
                        if (frame2Counter > 12) //6
                        {
                            frame2++;
                            frame2Counter = 0;
                        }
                        if (frame2 > 6) //frame 1 to 6 is running
                        {
                            frame2 = 1;
                        }
                    }
                    else
                    {
                        frame2 = 0; //frame 0 is idle
                        frame2Counter = 0;
                    }
                }
                else if (Projectile.velocity.Y != 0f)
                {
                    frame2Counter = 0;
                    frame2 = 4; //frame 4 is jumping
                }
            }
            else //flying
            {
                rot2 += -Projectile.spriteDirection * 0.1f;
                frame2Counter = 0;
                frame2 = 7; //frame 7 is flying
            }
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.SuspiciousNugget = false;
            }
            if (modPlayer.SuspiciousNugget)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            Projectile.frame = frame2;
            Projectile.rotation = rot2;
        }
    }
}
