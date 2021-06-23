using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class AlienHornetProj : SimplePetProjBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alien Hornet");
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyHornet);
            AIType = ProjectileID.BabyHornet;
            Projectile.width = 38;
            Projectile.height = 36;
            Projectile.alpha = 0;
        }

        public override void PostDraw(Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("Projectiles/Pets/AlienHornetProj_Glowmask").Value;
            Vector2 drawPos = Projectile.position - Main.screenPosition;
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 4);
            frame.Y = Projectile.frameCounter % 60;
            if (frame.Y > 24)
            {
                frame.Y = 24;
            }
            frame.Y *= Projectile.height;
            Main.EntitySpriteDraw(texture, drawPos, frame, Color.White * 0.7f, 0f, Vector2.Zero, 1f, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.hornet = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.AlienHornet = false;
            }
            if (modPlayer.AlienHornet)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }
    }
}
