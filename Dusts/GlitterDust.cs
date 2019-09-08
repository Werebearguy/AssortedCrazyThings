using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Dusts
{
    /// <summary>
    /// Clone of dust type 15/57/58
    /// </summary>
    public abstract class GlitterDustBase : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            //dust.velocity *= 0.4f;
            dust.noGravity = true;
            dust.noLight = true;
            //dust.scale *= 1.5f;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.99f;
            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White * ((255f - dust.alpha) / 255f);
        }
    }

    public class GlitterDust15 : GlitterDustBase { }
    public class GlitterDust57 : GlitterDustBase { }
    public class GlitterDust58 : GlitterDustBase { }
}
