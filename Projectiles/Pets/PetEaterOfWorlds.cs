using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class PetEaterofWorldsBase : ModProjectile
    {
        public static int[] wormTypes;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Eater of Worlds");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
            //ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true; //doesn't work for some reason with hide = true
            //ProjectileID.Sets.NeedsUUID[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            AssAI.StardustDragonSetDefaults(projectile, minion: false);
            projectile.alpha = 0;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetEaterofWorlds = false;
            }
            if (modPlayer.PetEaterofWorlds)
            {
                projectile.timeLeft = 2;
            }

            if (projectile.type != mod.ProjectileType<PetEaterofWorldsHead>())
            {
                AssAI.StardustDragonAI(projectile, wormTypes);
            }
            else
            {
                AssAI.BabyEaterAI(projectile);
                //float scaleFactor = MathHelper.Clamp(projectile.localAI[0], 0f, 50f);
                //projectile.scale = 1f + scaleFactor * 0.01f;

                projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
                projectile.direction = projectile.spriteDirection = (projectile.velocity.X > 0f).ToDirectionInt();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 drawPos = projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Texture2D texture = Main.projectileTexture[projectile.type];
            Rectangle drawRect = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
            Color color = projectile.GetAlpha(lightColor);
            Vector2 drawOrigin = drawRect.Size() / 2f;

            //alpha5.A /= 2;

            Main.spriteBatch.Draw(texture, drawPos, drawRect, color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);

            return false;
        }
    }

    public class PetEaterofWorldsHead : PetEaterofWorldsBase { }

    public class PetEaterofWorldsBody1 : PetEaterofWorldsBase { }

    public class PetEaterofWorldsBody2 : PetEaterofWorldsBase { }

    public class PetEaterofWorldsTail : PetEaterofWorldsBase { }
}
