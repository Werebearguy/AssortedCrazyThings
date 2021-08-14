using AssortedCrazyThings.Items.Accessories.Useful;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers
{
    [Content(ContentType.Bosses)]
    public class HarvesterWingsLayer : AssPlayerLayer
    {
        private Asset<Texture2D> wingTexture;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                wingTexture = Mod.Assets.Request<Texture2D>("Items/Accessories/Useful/HarvesterWings_Wings_Glowmask");
            }
        }

        public override void Unload()
        {
            wingTexture = null;
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.dead)
            {
                return false;
            }

            return drawInfo.drawPlayer.wings == Mod.GetEquipSlot(nameof(HarvesterWings), EquipType.Wings);
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Wings);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            Asset<Texture2D> asset = wingTexture;
            Texture2D texture = asset.Value;

            Vector2 directions = drawPlayer.Directions;
            Vector2 offset = new Vector2(0f, 7f);
            Vector2 position = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height / 2) + offset;

            int num11 = 0;
            int num12 = 0;
            int numFrames = 4;

            Color color = /*drawInfo.colorArmorBody*/ drawPlayer.GetImmuneAlpha(Color.White, drawInfo.shadow);

            position += new Vector2(num12 - 9, num11 + 2) * directions;
            position = position.Floor();
            Rectangle frame = new Rectangle(0, asset.Height() / numFrames * drawPlayer.wingFrame, asset.Width(), asset.Height() / numFrames);
            DrawData data = new DrawData(texture, position.Floor(), frame, color, drawPlayer.bodyRotation, new Vector2(asset.Width() / 2, asset.Height() / numFrames / 2), 1f, drawInfo.playerEffect, 0)
            {
                shader = drawInfo.cWings
            };
            drawInfo.DrawDataCache.Add(data);

            if (drawPlayer.velocity.Y != 0 && drawPlayer.wingFrame != 0)
            {
                if (Main.rand.NextBool(3))
                {
                    int dustOffset = -16 - drawPlayer.direction * 20;
                    int dustIndex = Dust.NewDust(new Vector2(drawPlayer.position.X + (drawPlayer.width / 2) + dustOffset, drawPlayer.position.Y + (drawPlayer.height / 2) - 8f), 30, 26, 135, 0f, 0f, 0, default(Color), 1.5f);
                    Dust dust = Main.dust[dustIndex];
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.velocity *= 0.3f;
                    if (Main.rand.NextBool(5))
                    {
                        dust.fadeIn = 1f;
                    }
                    dust.shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cWings, drawPlayer);
                    drawInfo.DustCache.Add(dustIndex);
                }
            }
        }
    }
}
