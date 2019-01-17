using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class DocileDemonEyeProj : ModProjectile
    {
        public const byte TotalNumberOfThese = 12;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/DocileDemonEye_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eater = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.DocileDemonEye = false;
            }
            if (modPlayer.DocileDemonEye)
            {
                projectile.timeLeft = 2;
            }
        }
    }


    //Handle everything else in DocileEyeProj.cs
    public class DocileEyeProj : GlobalProjectile
    {
        public DocileEyeProj()
        {

        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        private byte eyeType = 0;

        public byte CycleType()
        {
            eyeType++;
            if (eyeType >= DocileDemonEyeProj.TotalNumberOfThese) eyeType = 0;
            return eyeType;
        }

        public void SetEyeType(byte type)
        {
            eyeType = type;
        }

        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.type == mod.ProjectileType<DocileDemonEyeProj>())
            {
                SpriteEffects effects = SpriteEffects.None;
                Texture2D image = mod.GetTexture("Projectiles/Pets/DocileDemonEye_" + eyeType);
                Rectangle bounds = new Rectangle
                {
                    X = 0,
                    Y = projectile.frame,
                    Width = image.Bounds.Width,
                    Height = image.Bounds.Height / 2
                };
                bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

                Vector2 stupidOffset = new Vector2(projectile.width/2, projectile.height/2);

                spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

                return false;
            }
            return base.PreDraw(projectile, spriteBatch, lightColor);
        }
    }
}
