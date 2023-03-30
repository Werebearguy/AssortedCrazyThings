using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;
using AssortedCrazyThings.Items.Weapons;

namespace AssortedCrazyThings.Base.DrawLayers
{
	//TODO convert this to generic item glowmask one + func with DrawLayerData
	[Content(ContentType.Bosses)]
	public sealed class BoneCleavingFangLayer : AssPlayerLayer
	{
		private static Asset<Texture2D> glowmask;

		public override void SetStaticDefaults()
		{
			glowmask = ModContent.Request<Texture2D>(ModContent.GetInstance<BoneCleavingFang>().Texture + "Glowmask");
		}

		public override void Unload()
		{
			glowmask = null;
		}

		public override Position GetDefaultPosition()
		{
			//replicate vanilla placement, HeldItem will be replaced
			//return new Multiple()
			//{
			//	{ new Between(PlayerDrawLayers.BalloonAcc, PlayerDrawLayers.Skin), drawinfo => drawinfo.weaponDrawOrder == WeaponDrawOrder.BehindBackArm },
			//	{ new Between(PlayerDrawLayers.SolarShield, PlayerDrawLayers.ArmOverItem), drawinfo => drawinfo.weaponDrawOrder == WeaponDrawOrder.BehindFrontArm },
			//	{ new Between(PlayerDrawLayers.BladedGlove, PlayerDrawLayers.ProjectileOverArm), drawinfo => drawinfo.weaponDrawOrder == WeaponDrawOrder.OverFrontArm }
			//};
			return new AfterParent(PlayerDrawLayers.HeldItem);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Item heldItem = drawInfo.heldItem;
			if (!BoneClearingFangPlayer.HoldingItem(heldItem))
			{
				return false;
			}
			bool usingItem = drawPlayer.itemAnimation > 0 && heldItem.useStyle != ItemUseStyleID.None;
			bool holdingSuitableItem = heldItem.holdStyle != 0 && !drawPlayer.pulley;

			if (!drawPlayer.CanVisuallyHoldItem(heldItem))
			{
				holdingSuitableItem = false;
			}

			if (drawInfo.shadow != 0f || drawPlayer.JustDroppedAnItem || drawPlayer.frozen || !(usingItem || holdingSuitableItem) || heldItem.type <= 0 || drawPlayer.dead || heldItem.noUseGraphic || drawPlayer.wet && heldItem.noWet)
			{
				return false;
			}

			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			//Copied crucial logic from vanilla since we hide the layer
			//if (drawInfo.drawPlayer.heldProj >= 0 && drawInfo.shadow == 0f && !drawInfo.heldProjOverHand)
			//{
			//	drawInfo.projectileDrawPosition = drawInfo.DrawDataCache.Count;
			//}
			//drawInfo.itemColor = Lighting.GetColor((int)(drawInfo.Position.X + drawInfo.drawPlayer.width * 0.5) / 16, (int)((drawInfo.Position.Y + drawInfo.drawPlayer.height * 0.5) / 16.0));

			var color = new Color(250, 250, 250, drawInfo.heldItem.alpha);
			BoneClearingFangPlayer modPlayer = drawPlayer.GetModPlayer<BoneClearingFangPlayer>();
			float alpha = modPlayer.TimerRatio;

			DrawWithTextureAndColor(drawInfo, glowmask.Value, color * alpha);
		}

		private static void DrawWithTextureAndColor(PlayerDrawSet drawInfo, Texture2D texture, Color? color = null)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Item heldItem = drawInfo.heldItem;
			int useStyle = heldItem.useStyle;
			float adjustedItemScale = drawPlayer.GetAdjustedItemScale(heldItem);
			Vector2 position = new Vector2((int)(drawInfo.ItemLocation.X - Main.screenPosition.X), (int)(drawInfo.ItemLocation.Y - Main.screenPosition.Y));
			Rectangle? sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);

			if (useStyle == ItemUseStyleID.Swing)
			{
				Vector2 origin = new Vector2(drawPlayer.direction == -1 ? texture.Width : 0, drawPlayer.gravDir == -1 ? 0 : texture.Height);
				DrawData drawData = new DrawData(texture, position, sourceRect, heldItem.GetAlpha(color ?? drawInfo.itemColor), drawPlayer.itemRotation, origin, adjustedItemScale, drawInfo.itemEffect, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}
