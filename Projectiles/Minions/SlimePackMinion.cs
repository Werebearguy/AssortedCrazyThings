using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions
{
    //check this file for more info vvvvvvvv
    public class SlimePackMinion : BabySlimeBase
    {
        public const int DefDamage = 36;

        private const byte TotalNumberOfThese = 33;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Minions/SlimePackMinions/SlimeMinion_0"; //use fixed texture
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Pack Minion");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            drawOffsetX = -8;
            drawOriginOffsetY = -2;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions and damage (if there is, defaults to 0)
            projectile.width = 34;
            projectile.height = 30;

            Damage = DefDamage;
        }

        public override bool PreAI()
        {
            AssPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<AssPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.slimePackMinion = false;
            }
            if (modPlayer.slimePackMinion)
            {
                projectile.timeLeft = 2;
            }

            //since default state is 0, textures will use [1 to TotalNumberOfThese] as indices, then it will substract 1 in pre-draw
            if (projectile.localAI[1] == 0 && Main.netMode != NetmodeID.Server)
            {
                projectile.localAI[1] = Main.rand.Next(1, TotalNumberOfThese + 1);
                projectile.netUpdate = true;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D image = mod.GetTexture("Projectiles/Minions/SlimePackMinions/SlimeMinion_" + (projectile.localAI[1] - 1));
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / 6
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number
            Vector2 stupidOffset = new Vector2(0f, + 1f + projectile.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f);
            Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;

            Color color = drawColor * ((255 - projectile.alpha) / 255f);
            spriteBatch.Draw(image, drawPos, bounds, color, projectile.rotation, bounds.Size() / 2, projectile.scale, effect, 0f);
            return false;
        }
    }
}
