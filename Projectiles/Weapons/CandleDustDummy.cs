using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    //used in GlobalItem for creating candle dust

    public class CandleDustDummy : ModProjectile
    {
        private static readonly int LifeTime = 10;

        public override string Texture
        {
            get
            {
                return "Terraria/Images/Projectile_" + ProjectileID.WoodenArrowFriendly;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Candle Dust Dummy");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.timeLeft = LifeTime;
            Projectile.hide = true;
            //projectile.alpha = 255;
            Projectile.tileCollide = false;
        }

        //ai 0 is the timer until the arrow starts dropping (caps at 15)
        //ai 1 unused
        public override bool PreAI()
        {
            AssPlayer mPlayer = Projectile.GetOwner().GetModPlayer<AssPlayer>();

            bool[] buffs = new bool[] {
            mPlayer.everburningCandleBuff,
            mPlayer.everburningCursedCandleBuff,
            mPlayer.everfrozenCandleBuff,
            mPlayer.everburningShadowflameCandleBuff};

            int[] types = new int[] {
            6,
            75,
            59,
            62};

            Color[] colors = new Color[] {
            new Color(255, 255, 255),
            new Color(255, 255, 255),
            new Color(255, 255, 255),
            new Color(196, 0, 255)};

            //projectile.alpha = 255;

            //projectile.ai[0] = 0; //fly straight
            //kinda cheaty since the arrow AI makes itself only visible after a few ticks, so no need to make alpha
            //if timeLeft is only between 2 and 0

            int[] randomindex = new int[] { 0, 1, 2, 3 };

            int buffCount = 0;

            for (int t = 0; t < randomindex.Length; t++)
            {
                //shuffle indexes (purely visual so different dusts overlap)
                int tmp = randomindex[t];
                int r = Main.rand.Next(t, randomindex.Length);
                randomindex[t] = randomindex[r];
                randomindex[r] = tmp;

                //check number of enabled buffs (to reduce number of particles when spammed)
                if (buffs[t]) buffCount++;
            }

            for (int i = 0; i < randomindex.Length; i++)
            {
                if (buffs[randomindex[i]] && Projectile.ai[0] < 2)
                {
                    for (int k = 0; k < 10 - (int)(buffCount * 1.75f); k++) //spawn less dusts if more buffs active
                    {
                        if (Main.rand.NextFloat() < 0.8f)
                        {
                            float rand = Main.rand.NextFloat(0.7f, 1.3f);
                            Vector2 cm = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10));
                            Vector2 velo = cm * rand;
                            Dust dust = Dust.NewDustPerfect(Projectile.position, types[randomindex[i]], velo, 100 * (int)(Projectile.ai[0] + 1f), colors[randomindex[i]], 2.368421f);
                            dust.noGravity = true;
                            dust.noLight = true;
                        }
                    }
                }
            }

            return true;
        }
    }
}
