using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers
{
    public class HarvesterWingsLayer : PlayerDrawLayer
    {
        private Asset<Texture2D> wingTexture;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                wingTexture = Mod.GetTexture("Items/Accessories/Useful/HarvesterWings_Wings_Glowmask");
            }
        }

        public override void Unload()
        {
            wingTexture?.Dispose();
            wingTexture = null;
        }

        public override bool GetDefaultVisiblity(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.wings == Mod.GetEquipSlot("HarvesterWings", EquipType.Wings);
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Wings);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || drawPlayer.dead)
            {
                return;
            }

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
            DrawData data = new DrawData(texture, position.Floor(), new Rectangle(0, asset.Height() / numFrames * drawPlayer.wingFrame, asset.Width(), asset.Height() / numFrames), color, drawPlayer.bodyRotation, new Vector2(asset.Width() / 2, asset.Height() / numFrames / 2), 1f, drawInfo.playerEffect, 0)
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

            //Texture2D texture = mod.GetTexture("Items/Accessories/Useful/HarvesterWings_Wings_Glowmask").Value;
            //float drawX = (int)drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X;
            //float drawY = (int)drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y;

            //Vector2 stupidOffset = new Vector2(-9 * drawPlayer.direction + 0 * drawPlayer.direction, 2f * drawPlayer.gravDir + 0 * drawPlayer.gravDir);

            //DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY) + stupidOffset, new Rectangle(0, texture.Height / 4 * drawPlayer.wingFrame, texture.Width, texture.Height / 4), Color.White * ((255 - drawPlayer.immuneAlpha) / 255f), drawPlayer.bodyRotation, new Vector2(texture.Width / 2, texture.Height / 8), 1f, GetSpriteEffects(drawPlayer), 0)
            //{
            //    shader = drawInfo.wingShader
            //};
            //Main.playerDrawData.Add(drawData);

            //if (drawPlayer.velocity.Y != 0 && drawPlayer.wingFrame != 0)
            //{
            //    if (Main.rand.NextBool(3))
            //    {
            //        int dustOffset = -16 - drawPlayer.direction * 20;
            //        int dustIndex = Dust.NewDust(new Vector2(drawPlayer.position.X + (drawPlayer.width / 2) + dustOffset, drawPlayer.position.Y + (drawPlayer.height / 2) - 8f), 30, 26, 135, 0f, 0f, 0, default(Color), 1.5f);
            //        Dust dust = Main.dust[dustIndex];
            //        dust.noGravity = true;
            //        dust.noLight = true;
            //        dust.velocity *= 0.3f;
            //        if (Main.rand.NextBool(5))
            //        {
            //            dust.fadeIn = 1f;
            //        }
            //        dust.shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cWings, drawPlayer);
            //        Main.playerDrawDust.Add(dustIndex);
            //    }
            //}
        }
    }
}
