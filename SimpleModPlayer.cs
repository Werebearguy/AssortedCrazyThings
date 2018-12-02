using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    class SimpleModPlayer : ModPlayer
    {
        public bool everburningCandleBuff;

        public bool everburningCursedCandleBuff;

        public bool everfrozenCandleBuff;

        public bool variable_debuff_04;

        public bool variable_debuff_05;

        public bool everburningShadowflameCandleBuff;

        public bool variable_debuff_07;

        public override void ResetEffects()
        {
            everburningCandleBuff = false;
            everburningCursedCandleBuff = false;
            everfrozenCandleBuff = false;
            variable_debuff_04 = false;
            variable_debuff_05 = false;
            everburningShadowflameCandleBuff = false;
            variable_debuff_07 = false;
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            NPC npc = new NPC();
            if (victim is NPC)
            {
                npc = (NPC)victim;
            }
            if (everburningCandleBuff)
            {
                npc.AddBuff(BuffID.OnFire, 120, true);
            }
            if (everburningCursedCandleBuff)
            {
                npc.AddBuff(BuffID.CursedInferno, 120, true);
            }
            if (everfrozenCandleBuff)
            {
                npc.AddBuff(BuffID.Frostburn, 120, true);
            }
            if (variable_debuff_04)
            {
                npc.AddBuff(BuffID.Ichor, 120, true);
            }
            if (variable_debuff_05)
            {
                npc.AddBuff(BuffID.Venom, 120, true);
            }
            if (everburningShadowflameCandleBuff)
            {
                npc.AddBuff(BuffID.ShadowFlame, 60, true);
            }
            if (variable_debuff_07)
            {
                npc.AddBuff(BuffID.Bleeding, 120, true);
            }
        }

        private void SpawnMeleeDust(int type, Color color, Rectangle hitbox)
        {
            //6 is the default fire particle type
            int dustid = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, type, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, color, 2f);
            Main.dust[dustid].noGravity = true;
            Dust dust2 = Main.dust[dustid];
            dust2.velocity.X = dust2.velocity.X * 2f;
            Dust dust3 = Main.dust[dustid];
            dust3.velocity.Y = dust3.velocity.Y * 2f;
        }

        private void SpawnRangedDust(int type, Color color, float speed)
        {
            Vector2 cm = new Vector2(Main.MouseWorld.X - player.Center.X, Main.MouseWorld.Y - player.Center.Y);
            for (int k = 0; k < 10; k++)
            {
                if (Main.rand.NextFloat() < 0.8f)
                {
                    float randx = Main.rand.NextFloat(0.7f, 1.3f);
                    float randx2 = Main.rand.NextFloat(-1.5f, 1.5f);
                    float randy = Main.rand.NextFloat(0.7f, 1.3f);
                    float bobandy = Main.rand.NextFloat(-1.5f, 1.5f);
                    float velox = ((cm.X * speed * randx) / cm.Length()) + randx2; //first rand makes it so it has different velocity factor (how far it flies)
                    float veloy = ((cm.Y * speed * randy) / cm.Length()) + bobandy; //second rand is a kinda offset used mainly for when shooting vertically or horizontally
                    Vector2 velo = new Vector2(velox, veloy);
                    Vector2 pos = new Vector2(player.Center.X + velox * 1.2f, player.Center.Y + veloy * 1.2f);
                    Dust dust = Dust.NewDustPerfect(pos, type, velo, 0, color, 2.368421f);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
        }

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float speed = (float)Math.Sqrt((double)(speedX * speedX + speedY * speedY));
            if(speed < 10f)
            {
                speed = 10f;
            }

            if (everburningCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnRangedDust(6, color, speed);
            }
            if (everburningCursedCandleBuff)
            {
                Color color = new Color(196, 255, 0); //so it's light green and not dark green
                SpawnRangedDust(61, color, speed);
            }
            if (everfrozenCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnRangedDust(59, color, speed);
            }
            if (everburningShadowflameCandleBuff)
            {
                Color color = new Color(196, 0, 255);
                SpawnRangedDust(62, color, speed);
            }
            return true;
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (everburningCandleBuff)
            {
                Color color = new Color(255,255,255);
                SpawnMeleeDust(6, color, hitbox);
            }
            if (everburningCursedCandleBuff)
            {
                Color color = new Color(196, 255, 0);
                SpawnMeleeDust(61, color, hitbox);
            }
            if (everfrozenCandleBuff)
            {
                Color color = new Color(255, 255, 255);
                SpawnMeleeDust(59, color, hitbox);
            }
            if (everburningShadowflameCandleBuff)
            {
                Color color = new Color(196, 0, 255);
                SpawnMeleeDust(62, color, hitbox);
            }
            //type 64 is ichor
        }
    }
}
