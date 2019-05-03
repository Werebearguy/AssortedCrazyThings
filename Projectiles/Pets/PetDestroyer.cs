using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class PetDestroyerBase : ModProjectile
    {
        public static int[] wormTypes;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Destroyer");
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
                modPlayer.PetDestroyer = false;
            }
            if (modPlayer.PetDestroyer)
            {
                projectile.timeLeft = 2;
            }

            if (projectile.type != mod.ProjectileType<PetDestroyerHead>())
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
            Texture2D texture2D34 = Main.projectileTexture[projectile.type];
            Rectangle drawRect = texture2D34.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
            Color color = projectile.GetAlpha(lightColor);
            Vector2 drawOrigin = drawRect.Size() / 2f;
            
            //alpha5.A /= 2;

            Main.spriteBatch.Draw(texture2D34, drawPos, drawRect, color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);

            return false;
        }
    }

    public class PetDestroyerHead : PetDestroyerBase { }

    public class PetDestroyerBody1 : PetDestroyerBase { }

    public class PetDestroyerBody2 : PetDestroyerBase { }

    public class PetDestroyerTail : PetDestroyerBase { }

    public class PetDestroyerProbe : ModProjectile
    {
        private const int LaserDamage = 8;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Destroyer Probe");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.aiStyle = -1;
            projectile.width = 16;
            projectile.height = 16;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetDestroyer = false;
            }
            if (modPlayer.PetDestroyer)
            {
                projectile.timeLeft = 2;
            }

            #region FindParent
            //set parent when spawned

            int parentType = projectile.whoAmI % 2 == 0 ? mod.ProjectileType<PetDestroyerHead>() : mod.ProjectileType<PetDestroyerTail>();
            if (projectile.ai[0] == 0f)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == parentType && projectile.owner == Main.projectile[i].owner)
                    {
                        //since technically index 0 also exists, shift it by 1 when found
                        projectile.ai[0] = i + 1;
                        projectile.netUpdate = true;
                        break;
                    }
                }
            }

            //if something goes wrong, abort mission
            if (projectile.ai[0] == 0f)
            {
                projectile.Kill();
                return;
            }
            Projectile parent = Main.projectile[(int)projectile.ai[0] - 1];
            #endregion

            //offsets to the drones
            Vector2 between = parent.Center - projectile.Center;
            float offsetX = (between.X < 0f).ToDirectionInt() * 60f;
            float offsetY = 60;
            
            AssAI.ZephyrfishAI(projectile, parent: parent, velocityFactor: 0.5f, random: true, swapSides: 1, offsetX: offsetX, offsetY: offsetY);
            
            projectile.ai[1]++;
            int targetIndex = AssAI.FindTarget(projectile, projectile.Center, range: 600f);
            if ((int)projectile.ai[1] % 60 == 0)
            {
                if (Main.myPlayer == projectile.owner)
                {
                    if (targetIndex != -1 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        if (projectile.ai[1] == 60) projectile.ai[1] += 60;
                        Vector2 position = projectile.Center;
                        Vector2 velocity = Main.npc[targetIndex].Center + Main.npc[targetIndex].velocity * 5f - projectile.Center;
                        velocity.SafeNormalize(default(Vector2));
                        velocity *= 7f;
                        Projectile.NewProjectile(position, velocity, ProjectileID.MiniRetinaLaser, LaserDamage, 2f, Main.myPlayer, 0f, 0f);
                        projectile.netUpdate = true;
                        parent.netUpdate = true;
                    }
                    else
                    {
                        if (projectile.ai[1] > 60)
                        {
                            projectile.ai[1] -= 60;
                            projectile.netUpdate = true;
                            parent.netUpdate = true;
                        }
                    }
                }
                projectile.ai[1] -= 60;
            }

            if(targetIndex != -1)
            {
                projectile.rotation = (Main.npc[targetIndex].Center - projectile.Center).ToRotation();
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation();
            }

            projectile.direction = projectile.spriteDirection = -1;
        }
    }
}
