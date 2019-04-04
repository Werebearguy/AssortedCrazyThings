using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetMoonProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/PetMoonProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Personal Moon");
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 40;
            projectile.height = 40;
            projectile.alpha = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Vector3 lightVector = Vector3.One * 0.6f;
            float lightFactor = 1f;

            projectile.frame = Main.moonPhase;

            //0 is 1f, 4 is not drawn at all
            if (projectile.frame == 1 || projectile.frame == 7)
            {
                lightFactor = 0.85f;
            }
            else if (projectile.frame == 2 || projectile.frame == 6)
            {
                lightFactor = 0.7f;
            }
            else if (projectile.frame == 3 || projectile.frame == 5)
            {
                lightFactor = 0.55f;
            }

            //int texture = Main.moonType; //0, 1 and 2
            int texture = mPlayer.petMoonType;
            if (Main.pumpkinMoon)
            {
                texture = 3;
            }
            else if (Main.snowMoon)
            {
                texture = 4;
            }
            
            if (projectile.frame != 4) Lighting.AddLight(projectile.Center, lightVector * lightFactor);

            Texture2D image = mod.GetTexture("Projectiles/Pets/PetMoonProj_" + texture);

            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = (image.Bounds.Height / Main.projFrames[projectile.type]);
            bounds.Y = projectile.frame * bounds.Height;

            Vector2 stupidOffset = new Vector2(projectile.width / 2, (projectile.height - 18f));
            Vector2 drawPos = projectile.position - Main.screenPosition + stupidOffset;

            if (!Main.eclipse && projectile.frame != 4) spriteBatch.Draw(image, drawPos, bounds, Color.LightGray, 0f, bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetMoon = false;
            }
            if (modPlayer.PetMoon)
            {
                projectile.timeLeft = 2;

                AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, offsetX: 20f, offsetY: -32f);
            }
        }
    }
}
