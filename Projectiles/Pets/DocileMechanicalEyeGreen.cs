using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class DocileMechanicalEyeGreen : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Mechanical Eye");
            Main.projFrames[projectile.type] = 3;
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

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Main.NewText(Main.projectileTexture[projectile.type].Width);
            //Main.NewText(Main.projectileTexture[projectile.type].Height);

            Texture2D texture = mod.GetTexture("Glowmasks/DocileMechanicalEye_Glowmask");
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 3); //not animated so I don't care
            //frame.Y = (int)projectile.frameCounter % 60;
            //Main.NewText("counter " + projectile.frameCounter);
            //Main.NewText("frame " +  frame);
            //if (frame.Y > 0)
            //{
            //    frame.Y = 0;
            //}
            //frame.Y *= projectile.height;

            //Main.NewText(" " + Main.npcTexture[npc.type].Width + " " + Main.npcTexture[npc.type].Height);
            //Main.NewText(npc.frame);
            Vector2 stupidOffset = new Vector2(-7f, 0f);
            SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f);
            Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, new Rectangle?(frame), Color.White, projectile.rotation, frame.Size() / 2, projectile.scale, effect, 0f);
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.DocileMechanicalEyeGreen = false;
            }
            if (modPlayer.DocileMechanicalEyeGreen)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
