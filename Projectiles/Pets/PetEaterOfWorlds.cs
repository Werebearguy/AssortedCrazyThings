using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class PetEaterofWorldsBase : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Eater of Worlds");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.NeedsUUID[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            AssAI.StardustDragonSetDefaults(projectile, minion: false);
            if (projectile.type == mod.ProjectileType<PetEaterofWorldsHead>())
            {
                projectile.alpha = 0;
            }
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
                AssAI.StardustDragonAI(projectile, new int[]
                {
                mod.ProjectileType<PetEaterofWorldsHead>(),
                mod.ProjectileType<PetEaterofWorldsBody1>(),
                mod.ProjectileType<PetEaterofWorldsBody2>(),
                mod.ProjectileType<PetEaterofWorldsTail>(),
                },
                minion: false);
            }
            else
            {
                AssAI.BabyEaterAI(projectile);
                projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
                projectile.direction = projectile.spriteDirection = (projectile.velocity.X > 0f).ToDirectionInt();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 vector66 = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Texture2D texture2D34 = Main.projectileTexture[projectile.type];
            Rectangle rectangle17 = texture2D34.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
            Color alpha5 = projectile.GetAlpha(lightColor);
            Vector2 origin11 = rectangle17.Size() / 2f;
            
            alpha5.A /= 2;

            Main.spriteBatch.Draw(texture2D34, vector66, new Microsoft.Xna.Framework.Rectangle?(rectangle17), alpha5, projectile.rotation, origin11, projectile.scale, effects, 0f);

            return false;
        }
    }

public class PetEaterofWorldsHead : PetEaterofWorldsBase
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.StardustDragon1;
            }
        }
    }

    public class PetEaterofWorldsBody1 : PetEaterofWorldsBase
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.StardustDragon2;
            }
        }
    }

    public class PetEaterofWorldsBody2 : PetEaterofWorldsBase
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.StardustDragon3;
            }
        }
    }

    public class PetEaterofWorldsTail : PetEaterofWorldsBase
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Projectile_" + ProjectileID.StardustDragon4;
            }
        }
    }
}
