using AssortedCrazyThings.Items.Accessories.Useful;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers
{
	[Content(ContentType.Bosses)]
	public class HarvesterWingsLayer : AssPlayerLayer
	{
		private Asset<Texture2D> wingAsset;
		private Asset<Texture2D> glowAsset;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				string common = "Items/Accessories/Useful/HarvesterWings_Wings";
				wingAsset = Mod.Assets.Request<Texture2D>(common + "_Proper");
				glowAsset = Mod.Assets.Request<Texture2D>(common + "_Glowmask");
			}
		}

		public override void Unload()
		{
			wingAsset = null;
			glowAsset = null;
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

			Asset<Texture2D> asset = wingAsset;

			Vector2 directions = drawPlayer.Directions;
			Vector2 offset = new Vector2(0f, 7f);

			//bonus bobbing
			Rectangle bobFrame = drawInfo.drawPlayer.bodyFrame;
			offset += Main.OffsetsPlayerHeadgear[bobFrame.Y / bobFrame.Height] * drawPlayer.gravDir;

			Vector2 position = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height / 2) + offset;

			int num11 = -5;
			int num12 = -7;
			int numFrames = HarvesterWings.NumFrames;

			Color color = drawInfo.colorArmorBody;

			position += new Vector2(num12 - 9, num11 + 2) * directions;
			position = position.Floor();
			int totalHeight = asset.Height();
			int totalWidth = asset.Width();
			Rectangle frame = new Rectangle(0, totalHeight / numFrames * drawPlayer.wingFrame, totalWidth, totalHeight / numFrames);
			Vector2 origin = new Vector2(totalWidth / 2, totalHeight / numFrames / 2);
			float bodyRotation = drawPlayer.bodyRotation;
			SpriteEffects playerEffect = drawInfo.playerEffect;
			Texture2D texture = asset.Value;
			DrawData data = new DrawData(texture, position, frame, color, (float)bodyRotation, origin, 1f, playerEffect, 0)
			{
				shader = drawInfo.cWings
			};
			drawInfo.DrawDataCache.Add(data);

			asset = glowAsset;
			texture = asset.Value;
			color = drawPlayer.GetImmuneAlpha(Color.White, drawInfo.shadow);
			data = new DrawData(texture, position, frame, color, (float)bodyRotation, origin, 1f, playerEffect, 0)
			{
				shader = drawInfo.cWings
			};
			drawInfo.DrawDataCache.Add(data);
		}
	}
}
