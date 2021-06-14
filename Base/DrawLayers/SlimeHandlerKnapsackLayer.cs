using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers
{
    public class SlimeHandlerKnapsackLayer : PlayerDrawLayer
    {
        private Asset<Texture2D> knapsackTexture;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                knapsackTexture = Mod.GetTexture("Items/Weapons/SlimeHandlerKnapsack_Back");
            }
        }

        public override void Unload()
        {
            knapsackTexture?.Dispose();
            knapsackTexture = null;
        }

        public override bool GetDefaultVisiblity(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.HeldItem?.type == ModContent.ItemType<SlimeHandlerKnapsack>();
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Backpacks);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || drawPlayer.dead)
            {
                return;
            }

            if (drawPlayer.wings == 0 || drawPlayer.velocity.Y == 0f)
            {
                Texture2D texture = knapsackTexture.Value;

                Vector2 offset = new Vector2(0f, 8f);
                Vector2 position = drawInfo.Position - Main.screenPosition + drawPlayer.bodyPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height / 2) + new Vector2(0f, -4f) + offset;
                position = position.Floor();

                DrawData drawData = new DrawData(texture, position, drawPlayer.bodyFrame, drawInfo.colorArmorBody, drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }
}
