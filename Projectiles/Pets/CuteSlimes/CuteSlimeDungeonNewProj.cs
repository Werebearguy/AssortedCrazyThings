using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeDungeonNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Dungeon Slime");
            Main.projFrames[Projectile.type] = 10;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -14; //-16
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PetLizard);
            Projectile.width = Projwidth; //64 because of wings
            Projectile.height = Projheight;
            AIType = ProjectileID.PetLizard;
            Projectile.scale = 1.2f;
            Projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.CuteSlimeDungeonNew = false;
            }
            if (modPlayer.CuteSlimeDungeonNew)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool MorePreDrawBaseSprite(Color lightColor, bool useNoHair)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = Mod.GetTexture("Projectiles/Pets/CuteSlimes/CuteSlimeDungeonNewProjAddition").Value;
            Rectangle frameLocal = new Rectangle(0, frame2 * image.Height / 10, image.Width, image.Height / 10);
            Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, 10f + Projectile.gfxOffY);
            Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effects, 0f);
            return true;
        }
    }
}
